using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }

        //IEumerable lets me iterate over a collection of a specified type
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return  users;
        }

        //controller/api/id
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user;
        }
            
    }
}
