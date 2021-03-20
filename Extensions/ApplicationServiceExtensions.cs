using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        //Static - no need to instantiate class
        //IServiceCollection -  What we want to return, what we are extending
        //AddApplicationServices - Name of the method
        //this IServiceCollection -  To use IServiceCollection
        //IConfiguration - To use DbContext
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ITokenService, TokenServices>();
            services.AddDbContext<DataContext>(optionsBuilder
           => optionsBuilder.UseMySql(config.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
