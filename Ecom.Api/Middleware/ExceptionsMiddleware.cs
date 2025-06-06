using Ecom.Api.Helper;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace Ecom.Api.Middleware
{
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _RateLimitWindow = TimeSpan.FromSeconds(30);
        public ExceptionsMiddleware(RequestDelegate next, IHostEnvironment environment, IMemoryCache cache)
        {
            _next = next;
            _environment = environment;
            _cache = cache;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {

                ApplySecurity(context);
                if (IsRequestAllowed(context) == false)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";
                    var response = new ApiExceptions((int)HttpStatusCode.TooManyRequests,
                        "Rate limit exceeded. Try again later.");

                    await context.Response.WriteAsJsonAsync(response);

                }
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                
                var response =_environment.IsDevelopment() ? 
                    new ApiExceptions((int)HttpStatusCode.InternalServerError,
                    ex.Message, ex.StackTrace)
                    : new ApiExceptions((int)HttpStatusCode.InternalServerError,
                    ex.Message);
                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }   

        private bool IsRequestAllowed(HttpContext context)
        {
            var ip=context.Connection.RemoteIpAddress.ToString();
            var cachKay = $"Rate:{ip}";
            var dateNow = DateTime.Now;

            var (timesTemp,count)=_cache.GetOrCreate(cachKay, entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = _RateLimitWindow;
                    return (timesTemp: dateNow, count: 0);
                });

            if (dateNow - timesTemp<_RateLimitWindow)
            {
                if (count >= 30)
                {
                    return false;
                }
                _cache.Set(cachKay, (timesTemp, count + 1),_RateLimitWindow);
            }
            else
            {
                _cache.Set(cachKay, (timesTemp, count), _RateLimitWindow);
            }
            return true;
        }

        private void ApplySecurity(HttpContext context)
        {
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["X-XSS-Protection"] = "1;mode=block";
            context.Response.Headers["X-Frame-Options"] = "DENY";
        }
    }
}
