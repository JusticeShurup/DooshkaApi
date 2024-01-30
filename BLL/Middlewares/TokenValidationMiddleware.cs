using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;

namespace BLL.Middlewares
{
    public class TokenValidationMiddleware : IMiddleware
    {
        private readonly IRepository<User> _userRepository;

        public TokenValidationMiddleware([FromServices] IRepository<User> userRepository) 
        {
            _userRepository = userRepository;
        }


        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Headers["Authorization"].FirstOrDefault() == null)
            {
                await next.Invoke(context);
                return;
            }

            var tokenString = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            JwtSecurityToken token = new JwtSecurityToken(tokenString);

            string? email = token.Payload.Claims.FirstOrDefault(x => x.Type.ToString() == ClaimTypes.Email)?.Value;

            if (email == null)
            {
                throw new Exception("Logical error");
            }

            User? user = await _userRepository.FindByEmailAsync(email);

            if (user == null)
            {
                throw new Exception("User didn't not found");
            }

            context.Items.Add("User", user);

            await next.Invoke(context);
        }

    }
}
