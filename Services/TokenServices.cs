using API.Entities;
using API.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
//ALL THIS DOES IS RECIEVE A USER AND RETURN A TOKEN
namespace API.Services
{
    public class TokenServices : ITokenService
    {
        //Symmetric encryption is a type of encryption where one key is used to both encrypt and decrypt
        //key - used to sign the jwt and verify the signature
        private readonly SymmetricSecurityKey _key;
        public TokenServices(IConfiguration config )
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        public string CreateToken(AppUser user)
        {
            //How to specify the claims for the jwt
            var claims = new List<Claim>
            {
                //For now only the username as the NameID 
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.NameId, user.UserName)
            };

            //Assignining the verify signature credential part
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            //What goes inside our token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            //Create token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //Return token to whoever needs it
            return tokenHandler.WriteToken(token);

        }
    }
}
