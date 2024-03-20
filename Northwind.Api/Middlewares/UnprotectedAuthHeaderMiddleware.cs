using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Northwind.Api.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class UnprotectedAuthHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        readonly IDataProtector _dataProtector;

        public UnprotectedAuthHeaderMiddleware(RequestDelegate next, IDataProtectionProvider dataProtectorProvider)
        {
            _next = next;
            _dataProtector = dataProtectorProvider.CreateProtector("tse");
        }

        public Task Invoke(HttpContext httpContext)
        {
            var authHeader = httpContext.Request.Headers.Authorization;
            if(string.IsNullOrEmpty(authHeader))
                return _next(httpContext);

            var  jwtToken = _dataProtector.Unprotect(authHeader);

            httpContext.Request.Headers.Authorization = $"Bearer {jwtToken}"; 

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class UnprotectedAuthHeaderExtensions
    {
        public static IApplicationBuilder UseUnprotectedAuthHeader(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UnprotectedAuthHeaderMiddleware>();
        }
    }
}
