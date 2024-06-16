using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Security.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, JwtValidator jwtValidator)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var accountId = await jwtValidator.ValidateTokenAsync(token);
            //if (accountId != null)
            //{
            //    // attach account to context on successful jwt validation
            //    context.Items["Account"] = await dataContext.Accounts.FindAsync(accountId.Value);
            //}

            await _next(context);
        }
    }
}