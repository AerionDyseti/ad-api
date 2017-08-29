using AerionDyseti.Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AerionDyseti.Auth.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly IConfiguration config;
        private readonly UserManager<AerionDysetiUser> userManager;
        private readonly SignInManager<AerionDysetiUser> signInManager;

        public TokenController(IConfiguration config, UserManager<AerionDysetiUser> userManager)
        {
            this.config = config;
            this.userManager = userManager;
        }

        [HttpPost("")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] TokenRequest authUserRequest)
        {
            var user = await userManager.FindByEmailAsync(authUserRequest.Email);

            if (user != null)
            {
                var checkPwd = await signInManager.CheckPasswordSignInAsync(user, authUserRequest.Password, false);
                if (checkPwd.Succeeded)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Tokens:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(config["Tokens:Issuer"],
                    config["Tokens:Issuer"],
                    claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                }
            }

            return BadRequest("Could not create token");
        }
    }
}
