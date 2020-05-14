using System;
using System.Net;
using System.Threading.Tasks;
using BackingServices.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using QuotingAPI.Exceptions;
using BusinessLogic.Exceptions;

namespace QuotingAPI.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExcepHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExcepHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                Console.WriteLine("This is the Exception Middleware");
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleError(httpContext, ex);
            }
        }

        private static Task HandleError(HttpContext httpContext, Exception ex)
        {
            int httpStatusCode;
            string messageToShow;
            if (ex is QuoteLogicException)
            {
                httpStatusCode = (int)HttpStatusCode.ServiceUnavailable;
                messageToShow = ex.Message;
            }
            else if (ex is BackingServiceException)
            {
                httpStatusCode = (int)HttpStatusCode.ServiceUnavailable;
                messageToShow = ex.Message;
            }
            else if (ex is DatabaseException)
            {
                httpStatusCode = (int)HttpStatusCode.ServiceUnavailable;
                messageToShow = ex.Message;
            }
            else
            {
                httpStatusCode = (int)HttpStatusCode.InternalServerError;
                messageToShow = "The server occurs an unexpected error.";
            }

            var errorModel = new
            {
                status = httpStatusCode,
                message = messageToShow
            };

            // httpContext.Response.StatusCode = httpStatusCode;
            return httpContext.Response.WriteAsync(JsonConvert.SerializeObject(errorModel));
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExcepHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExcepHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExcepHandlerMiddleware>();
        }
    }
}
