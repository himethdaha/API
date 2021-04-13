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
        //Method to register a new user
        //Asynchronous task because we're conencting with the database
        /*When the HttpRequest is post. We send data in the body of the request
         which is sent to our api*/
        //When we send soemthing in the body of a request it should be sent out as an object to recieve the properties
        [HttpPost("register")]
        //public async Task<ActionResult<AppUser>> Register(string username, string password)
        //Before JWT
        //public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
        public async Task<ActionResult<UserDto>> Register(RegisterDto _registerDto)
        {
            //First check if the user already exists
            if (await UserExists(_registerDto.Username))
            {
                return BadRequest("User Name is taken");
            }

            //Instantiating the hashing algorithm
            //using - calls the dispose method of the class
            //Any class that uses the dispose method implements the Idsisposable interface
            using var hmac = new HMACSHA512();

            //Create new user
            var user = new AppUser
            {
                UserName = _registerDto.Username.ToLower(),
                /*Encoding - because our password is a string and it needs to be parsed into
                a byte array*/
                PaswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(_registerDto.Password)),
                /*HMACSHA512 class provides a random key when instantiated for the 1st time
                and that can be used for the Salt*/
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

        //Method to login a new user
        [HttpPost("login")]
        //Before adding the JWT
        //public async Task<ActionResult<AppUser>> Login(LoginDto loginDto)
        public async Task<ActionResult<UserDto>> Login(LoginDto _loginDto)
        {
            //First thing is to get the user from the database
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == _loginDto.Username);

          
            if (user == null)
            {
                return Unauthorized("Invalid User Name");
            }


            //Next check for the Password
            //Calculate the computed hash for their password using the password salt
            /*HMACSHA512 key is going to be the PasswordSalt. Cos we will get the same computed hash
             of the password becuase we're giving it the same key that was used when the password was hashed
            in the fisrt place*/
            using var hmac = new HMACSHA512(user.PaswordSalt);

            //Then, we need to workout the password hash that's contained in the loginDto
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(_loginDto.Password));

            //Since PasswordHash is a byte[] we need to loop over
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

        //Helper method to check if username exists
        private async Task<bool> UserExists(string username)
        {
            //AnySync - checks if Any usernames in the table macthes the username parameter in the function
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

       
    }
}
