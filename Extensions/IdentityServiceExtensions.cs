using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        //Static - no need to instantiate class
        //IServiceCollection -  What we want to return, what we are extending
        //AddIdentityServices - Name of the method
        //this IServiceCollection -  To use IServiceCollection
        //IConfiguration - To use DbContext
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            //For authentication middleware
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    //Configuring the token parameters to be validated
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //Server will sign the token and need to validate the token to check if its correct
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"])),
                        //Additional Flags
                        //Issuer is our API server
                        ValidateIssuer = false,
                        //Audience is the Angular application
                        ValidateAudience = false
                    };
                });

            return services;
        }
    }
}
