using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AerionDyseti.API.Shared.Models;
using AerionDyseti.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace AerionDyseti
{
    public class JwtManager
    {
        public string Audience;
        public string Issuer;
        public string Secret;


        public TokenResponse GenerateTokenResponse(AerionDysetiUser user, IList<Claim> userClaims)
        {
            var signingCreds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret)),
                SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Iss, Issuer),
                new Claim(JwtRegisteredClaimNames.Aud, Audience),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Nbf, DateTime.UtcNow.ToUnixTimestamp()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToUnixTimestamp()),
                new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddHours(5).ToUnixTimestamp())
            };

            claims.AddRange(userClaims);

            var token = new JwtSecurityToken(claims: claims, signingCredentials: signingCreds);

            return new TokenResponse {Token = new JwtSecurityTokenHandler().WriteToken(token), Expiration = token.ValidTo};
        }
    }
}