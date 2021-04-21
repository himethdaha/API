using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using API.DTOs;

namespace API.Controllers
{
 
    [Authorize]
    public class UsersController : BaseApiController
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        /*BEFORE ADDING THE IUSERREPOSITORY INTERFACE WITH USERREPOSITORY CLASS*/
        //private readonly DataContext _context;
        //public UsersController(DataContext context)
        //{
        //    _context = context;
        //}

        /*AFTER ADDING THE USERREPOSITORY INTERFACE WITH USERREPOSITORY CLASS*/
        private readonly IUserRepository _userRepository;
        private readonly AutoMapper.IMapper _mapper;

        public UsersController(IUserRepository userRepository, AutoMapper.IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        //IEumerable lets me iterate over a collection of a specified type
        [HttpGet]
        //returns all users 
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();
            //map to MemberDto
            //users - source object
            return Ok(users);
        }

        //controller/api/id
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username);


        }
            
    }
}
