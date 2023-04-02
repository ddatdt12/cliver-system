using CliverSystem.Core.Contracts;

namespace CliverSystem.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUnitOfWork unit)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                var userId = unit.Auth.ValidateToken(token);
                if (userId != null)
                {
                    // attach user to context on successful jwt validation
                    context.Items["User"] = await unit.Users.FindById(userId);
                }
            }
            await _next(context);
        }
    }
}
