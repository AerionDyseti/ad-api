using AerionDyseti.Auth.Models;
using AerionDyseti.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AerionDyseti.Auth.Controllers
{
    [Produces("application/json")]
    [Route("api/Accounts")]
    public class AccountController : Controller
    {
        private readonly UserManager<AerionDysetiUser> userManager;
        private readonly SignInManager<AerionDysetiUser> signInManager;
        private readonly JwtSettings jwtSettings;

        public AccountController(IOptions<JwtSettings> jwtSettings, UserManager<AerionDysetiUser> userManager, SignInManager<AerionDysetiUser> signInManager)
        {
            this.jwtSettings = jwtSettings.Value;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (ModelState.IsValid)
            {

                var user = new AerionDysetiUser
                {
                    UserName = registerRequest.MesNumber,
                    Email = registerRequest.Email,
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName
                };

                var result = await userManager.CreateAsync(user, registerRequest.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignIn(user);

                    // Create the JWT Token and return
                    var token = new JwtSecurityToken(
                        claims: new Claim[]
                        {
                            new Claim(JwtRegisteredClaimNames.Iss, jwtSettings.Issuer),
                            new Claim(JwtRegisteredClaimNames.Aud, jwtSettings.Audience),
                            new Claim(JwtRegisteredClaimNames.Jti, user.Id),
                            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Nbf, DateTime.UtcNow.ToUnixTimestamp()),
                            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToUnixTimestamp()),
                            new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddHours(5).ToUnixTimestamp())
                        },
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)), SecurityAlgorithms.HmacSha256)
                    );

                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), expiration = token.ValidTo });
                }
                else
                {
                    // Return Errors result if we did not succeed in registering a new account.
                    return BadRequest(new { errors = result.Errors.Select(err => new { code = err.Code, description =  err.Description }) });
                }
                
            }

            return BadRequest(new { errors = new[] { new { code = "UnknownError", description =  "An unknown error occurred." } } });

        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] TokenRequest tokenRequest)
        {
            if (ModelState.IsValid)
            {

                var user = await userManager.FindByEmailAsync(tokenRequest.Email);

                if (user == null)
                {
                    return BadRequest(new { errors = new[] { new { code = "UnknownUser", description = "The provided email does not match any account." } } });
                }

                var result = await signInManager.CheckPasswordSignIn(user, tokenRequest.Password);

                if (!result.Succeeded)
                {
                    return BadRequest(new { errors = new[] { new { code = "BadPassword", description = "The provided password was incorrect." } } });
                }
                await signInManager.SignIn(user);

                // Create the JWT Token and return
                var token = new JwtSecurityToken(
                    claims: new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Iss, jwtSettings.Issuer),
                        new Claim(JwtRegisteredClaimNames.Aud, jwtSettings.Audience),
                        new Claim(JwtRegisteredClaimNames.Jti, user.Id),
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Nbf, DateTime.UtcNow.ToUnixTimestamp(), "integer"),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToUnixTimestamp()),
                        new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddHours(5).ToUnixTimestamp())
                    },
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)), SecurityAlgorithms.HmacSha256)
                );

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), expiration = token.ValidTo });

            }

            return BadRequest(new { errors = new[] { new { code = "UnknownError", description = "An unknown error occurred." } } });

        }


    }

}
