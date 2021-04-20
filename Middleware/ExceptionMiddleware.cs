using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        //RequestDelegate - What's coming up next in the middleware pipeline
        //ILogger - logout our exception into the terminal so it won't swallow it
        //IHostEnvironment - To check if we in prod or dev

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;

        }

        //Required method
        //When in middleware we have access to http request coming in
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                //Step 1 - get the context and pass it to the next piece of middleware
                /*This middleware sits at the very top of our middleware and anything below this
                 will invoke _next at some point and if any gets an exception they will throw the exeption
                up till something that can handle the exception and then cos our exeption middleware is at the 
                top of our tree then we will catch the excpeiton inside here*/
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                //Write out the exception to the response
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                : new ApiException(context.Response.StatusCode, "Internal Server Error!");

                //if (response == _env.IsDevelopment())
                //{
                //    new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString());
                //}

                //else
                //{
                //    new ApiException(context.Response.StatusCode, "Internal Server Error!");
                //}

                //Sending back response in JSON
                //Ensures our response goes back as a normal formatted Json response in CamelCase
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                //Changing response into Json CamelCase
                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
               
            }
        }
    }
}
