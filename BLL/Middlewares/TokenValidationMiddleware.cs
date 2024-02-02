using BLL.Exceptions;
using DAL.Entities;
using DAL.Interfaces;
using MediatR;
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
        private readonly IRepository<RevokedToken> _revokedTokenRepository;


        public TokenValidationMiddleware([FromServices] IRepository<User> userRepository, [FromServices] IRepository<RevokedToken> revokedTokenRepository) 
        {
            _userRepository = userRepository;
            _revokedTokenRepository = revokedTokenRepository;
        }


        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Headers["Authorization"].FirstOrDefault() == null)
            {
                await next.Invoke(context);
                return;
            }

            string tokenString = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;

            JwtSecurityToken? token = null;
            try
            {
                token = new JwtSecurityToken(tokenString);
            }
            catch(Exception)
            {
                throw new UnauthorizedException("");
            }

            var result =_revokedTokenRepository.Find(x => x.Token == tokenString);

            if (result != null)
            {
                throw new BadRequestException("Token revoked");
            }

            string? email = token.Payload.Claims.FirstOrDefault(x => x.Type.ToString() == ClaimTypes.Email)?.Value;

            if (email == null)
            {
                throw new Exception("Logical error");
            }

            User? user = _userRepository.Find(x => x.Email == email);

            if (user == null)
            {
                throw new Exception("User didn't not found");
            }

            context.Items.Add("User", user);

            await next.Invoke(context);
        }

    }
}
