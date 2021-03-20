using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
       
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto _registerDto)
        {
            if(await UserExists(_registerDto.Username))
            {
                return BadRequest("User Name is taken");
            }

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = _registerDto.Username.ToLower(),
                PaswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(_registerDto.Password)),
                PaswordSalt = hmac.Key
            };

            //This ain't adding to the database yet.
            //Just tracking from the entityframework
            _context.Users.Add(user);
            //This part calls the database and save the user to the database
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto _loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == _loginDto.Username);

          
            if (user == null)
            {
                return Unauthorized("Invalid User Name");
            }


            //Checking computed hash of the password using passwordsalt
            using var hmac = new HMACSHA512(user.PaswordSalt);

            //Working out the hash for the password cointained in the LoginDto
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(_loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PaswordHash[i])
                {
                    return Unauthorized("Invalid Password");

                }
            }

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };



        }

        private async Task<bool> UserExists(string username)
        {
            //AnySync - checks if Any usernames in the table macthes the username parameter in te function
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

       
    }
}
