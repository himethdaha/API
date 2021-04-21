using API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            //Checking if there are any users in the database
            if (await context.Users.AnyAsync())
            {
                return;
            }

            //Reading the data in UserSeedData.json
            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");

            //breaking the data from userdata into a list of type AppUser
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            //Reading each user and adding them to the database
            foreach(var user in users)
            {
                //Giving them passwords
                using var hmac = new HMACSHA512();

                //Populating AppUser UserName with json UserName
                user.UserName = user.UserName.ToLower();
                user.PaswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                user.PaswordSalt = hmac.Key;

                //Tracking users
                context.Users.Add(user);
            }

            //Adding users to the database
            await context.SaveChangesAsync();
        }
      
    }
}
