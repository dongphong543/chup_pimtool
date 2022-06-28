//using LoggerService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using PIMBackend.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PIMBackend.Errors;

namespace PIMBackend.ErrorHandler
{
    public static class GlobalExceptionHandler
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app) 
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        if (contextFeature.Error.GetType() == typeof(UpdateConflictException))
                        {
                            context.Response.StatusCode = StatusCodes.Status409Conflict;
                        }

                        await context.Response.WriteAsync(new ErrorDetails()
                        {   
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.GetType().ToString()
                        }.ToString());
                    }
                });
            });
        }
    }
}
