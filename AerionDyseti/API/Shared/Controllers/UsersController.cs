using AerionDyseti.API.Shared.Models;
using AerionDyseti.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AerionDyseti.API.Shared.Auth;
using Microsoft.AspNetCore.Authorization;

namespace AerionDyseti.API.Shared.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Controller to handle User Management, such as registration and fetching an access token.
    /// </summary>
    [Produces("application/json")]
    [Route("api/Accounts")]
    public class UsersController : Controller
    {
        private readonly JwtManager jwtManager;
        private readonly SignInManager<AerionDysetiUser> signInManager;
        private readonly UserManager<AerionDysetiUser> userManager;

        /// <summary>
        /// Creates an instance of the UsersController object.
        /// </summary>
        /// <param name="jwtManager">The JWT Management object to be injected into this controller.</param>
        /// <param name="userManager">The Identity User Manager to be injected into this controller.</param>
        /// <param name="signInManager">The Identity Sign-In Manager to be injected into this controller.</param>
        public UsersController(IOptions<JwtManager> jwtManager, UserManager<AerionDysetiUser> userManager, SignInManager<AerionDysetiUser> signInManager)
        {
            this.jwtManager = jwtManager.Value;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        /// <summary>
        /// Registers a new user, adding them to the Database.
        /// </summary>
        /// <param name="registerRequest">A request object containing the information needed to create a new user.</param>
        /// <returns>An ActionResult corresponding to the HTTP Status Code for the given transaction.</returns>
        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new AerionDysetiUser
            {
                UserName = registerRequest.MesNumber,
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                CreationDate = DateTime.UtcNow,
                ApprovalDate = null
            };

            var result = await userManager.CreateAsync(user, registerRequest.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new ApiErrorResponse { Errors = result.Errors.Select(err => new ApiError(err.Code, err.Description)).ToList() } );
            }

            // Create the JWT Token and return
            return Ok(new SuccessResponse());

        }

        /// <summary>
        /// Approve a user to use the API.
        /// </summary>
        /// <param name="email">Email of the user to approve for use in the API.</param>
        /// <returns>An ActionResult corresponding to the HTTP Status Code for the given transaction.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        [Route("Approve")]
        [ProducesResponseType(typeof(SuccessResponse), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<IActionResult> ApproveUser([FromBody][Required] string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiErrorResponse { Errors = new List<ApiError> { new ApiError() } });
            }

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest(new ApiErrorResponse { Errors = new List<ApiError> { new ApiError("UnknownUser", "The provided email does not match any accounts.") } });
            }

            user.ApprovalDate = DateTime.UtcNow;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(new ApiErrorResponse { Errors = result.Errors.Select(e => new ApiError(e.Code, e.Description)).ToList() });
            }

            return Ok(new SuccessResponse());
        }

        /// <summary>
        /// Logs the provided user in, returning an Access token which can be used to connect to the API.
        /// </summary>
        /// <param name="tokenRequest">Request for a token, including the username and password.</param>
        /// <returns>An ActionResult corresponding to the HTTP Status Code for the given transaction.</returns>
        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<IActionResult> Login([FromBody] TokenRequest tokenRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiErrorResponse { Errors = new List<ApiError> { new ApiError() } });
            }

            var user = await userManager.FindByEmailAsync(tokenRequest.Email);
            if (user == null)
            {
                return BadRequest(new ApiErrorResponse { Errors = new List<ApiError> { new ApiError("UnknownUser", "The provided email does not match any accounts.") } });
            }
            if (user.ApprovalDate == null)
            {
                return BadRequest(new ApiErrorResponse { Errors = new List<ApiError> { new ApiError("UnapprovedUser", "The provided email is not yet approved to use this system.") } });
            }

            var result = await signInManager.CheckPasswordSignIn(user, tokenRequest.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new ApiErrorResponse { Errors = new List<ApiError> { new ApiError("BadPassword", "The provided password was incorrect.") } });
            }

            await signInManager.SignIn(user);
            var userClaims = await userManager.GetClaimsAsync(user);

            // Create the JWT Token and return
            return Ok(jwtManager.GenerateTokenResponse(user, userClaims));
        }

        /// <summary>
        /// Updates whether the given email has the Admin Claim or not.
        /// </summary>
        /// <param name="adminRequest">Request DTO including the email to change, and whether the user should be an Admin or not.</param>
        /// <returns>An ActionResult corresponding to the HTTP Status Code for the given transaction.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        [Route("Admin")]
        [ProducesResponseType(typeof(SuccessResponse), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<IActionResult> UpdateAdmin([FromBody] PostAdminRequest adminRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiErrorResponse { Errors = new List<ApiError> { new ApiError() } });
            }

            var user = await userManager.FindByEmailAsync(adminRequest.Email);
            if (user == null)
            {
                return BadRequest(new ApiErrorResponse { Errors = new List<ApiError> { new ApiError("UnknownUser", "The provided email does not match any accounts.") } });
            }

            // If Admin is true, add as an admin. Else, remove as an admin.
            var result = (adminRequest.Admin) ? await userManager.AddClaimAsync(user, new AdminClaim()): await userManager.RemoveClaimAsync(user, new AdminClaim());
            if (!result.Succeeded)
            {
                return BadRequest(new ApiErrorResponse
                {
                    Errors = result.Errors.Select(e => new ApiError(e.Code, e.Description)).ToList()
                });
            }

            return Ok(new SuccessResponse());

        }

        /// <summary>
        /// Fetches the list of emails attached to users who are Admins.
        /// </summary>
        /// <returns>An ActionResult corresponding to the HTTP Status Code for the given transaction.</returns>
        [Authorize]
        [HttpGet]
        [Route("Admin")]
        [ProducesResponseType(typeof(GetAdminsResponse), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<IActionResult> GetAdminList()
        {
            var users = await userManager.GetUsersForClaimAsync(new AdminClaim());
            if (users == null)
            {
                return BadRequest(new ApiErrorResponse { Errors = new List<ApiError> { new ApiError() } });
            }

            return Ok(new GetAdminsResponse { Admins = users.Select(u => u.Email).ToList() });
        }








    }
}