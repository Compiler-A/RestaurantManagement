using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;

namespace APILayer
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); 
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int statusCode;
            string message;

            switch (exception)
            {
                case ArgumentOutOfRangeException argOutOfRange:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = argOutOfRange.Message;
                    break;

                case ArgumentNullException argNullEx:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = argNullEx.Message;
                    break;

                case ArgumentException argEx:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = argEx.Message;
                    break;

                case KeyNotFoundException keyEx:
                    statusCode = StatusCodes.Status404NotFound;
                    message = keyEx.Message;
                    break;

                case InvalidOperationException invOpEx:
                    statusCode = StatusCodes.Status409Conflict;
                    message = invOpEx.Message;
                    break;

                case UnauthorizedAccessException unAuthEx:
                    statusCode = StatusCodes.Status403Forbidden;
                    message = unAuthEx.Message;
                    break;

                case FormatException formatEx:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = formatEx.Message;
                    break;

                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    message = _env.IsDevelopment()
                        ? exception.Message
                        : "Internal server error.";
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var responseType = typeof(object); 
            var response = new
            {
                statusCode = statusCode,
                message = message,
                data = (object?)null
            };

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }
}