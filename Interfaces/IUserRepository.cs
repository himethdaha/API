using API.DTOs;
using API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        //Let the user update their profile
        void Update(AppUser user);

        //Save all changes
        Task<bool> SaveAllAsync();

        //Task that returns IEnumerable of AppUser
        Task <IEnumerable<AppUser>> GetUsersAsync();

        //Getting a user by ID
        Task<AppUser> GetUserByIdAsync(int id);

        //Getting a user by UserName
        Task<AppUser> GetUserByUsernameAsync(string username);

        //Return a list of MemberDtos
        Task<IEnumerable<MemberDto>> GetMembersAsync();

        //Return MemberDto
        Task<MemberDto> GetMemberAsync(string username);

    }
}
