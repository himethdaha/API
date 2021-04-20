using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;
        public BuggyController(DataContext context)
        {
            _context = context;
        }

        //auth - root parameter
        [Authorize]
        [HttpGet ("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _context.Users.Find(-1);

            if(thing == null)
            {
                return NotFound();
            }

            else
            {
                return Ok(thing);
            }
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
           
                /*Find() - Tries to find an entity with a given primary key value
            which is (-1) here.*/
                var thing = _context.Users.Find(-1);

                //generate an exception
                var thingToReturn = thing.ToString();

                return thingToReturn;           
           
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("Not a good request");
        }

        
    }
}
