using System;
using System.Linq;
using System.Threading.Tasks;
using AerionDyseti.API.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AerionDyseti.API.Shared.Controllers
{
    [Produces("application/json")]
    [Route("api/Debug")]
    public class DebugController : Controller
    {
        private readonly UserManager<AerionDysetiUser> userManager;

        public DebugController(UserManager<AerionDysetiUser> userManager)
        {
            this.userManager = userManager;
        }


        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        [Route("ClearUsers")]
        [ProducesResponseType(typeof(SuccessResponse), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<IActionResult> ClearUsersAsync()
        {
            var users = userManager.Users.Where(u => !u.Email.Equals("whiteside.mes@gmail.com", StringComparison.OrdinalIgnoreCase));

            foreach (var aerionDysetiUser in users)
            {
                var result = await userManager.DeleteAsync(aerionDysetiUser);
                if (!result.Succeeded)
                {
                    return BadRequest(new ApiErrorResponse { Errors = result.Errors.Select(e => new ApiError(e.Code, e.Description)).ToList() });
                }
            }

            return Ok(new SuccessResponse());
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        [Route("CheckAdmin")]
        [ProducesResponseType(typeof(SuccessResponse), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public IActionResult CheckAdmin()
        {
            return Ok(new SuccessResponse());
        }

        [Authorize]
        [HttpGet]
        [Route("CheckLoggedIn")]
        [ProducesResponseType(typeof(SuccessResponse), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public IActionResult CheckLoggedIn()
        {
            return Ok(new SuccessResponse());
        }

        [HttpGet]
        [Route("CheckPublic")]
        [ProducesResponseType(typeof(SuccessResponse), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public IActionResult CheckPublic()
        {
            return Ok(new SuccessResponse());
        }
    }
}