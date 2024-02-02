using BLL.AuthLogic.Commands;
using BLL.AuthLogic.DTOS;
using BLL.Exceptions;
using DAL.Entities;
using DAL.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using System.IdentityModel.Tokens.Jwt;

namespace BLL.AuthLogic.Handlers
{
    public class LogoutRequestHandler : IRequestHandler<LogoutCommand>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<RevokedToken> _revokedTokenRepository;
        private readonly IHttpContextAccessor _httpContext;


        public LogoutRequestHandler(IRepository<User> userRepository, IRepository<RevokedToken> revokedTokenRepository, IHttpContextAccessor httpContext) 
        {
            _userRepository = userRepository;
            _revokedTokenRepository = revokedTokenRepository;
            _httpContext = httpContext;
        }

        public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            User user = (User)_httpContext.HttpContext.Items["User"]!;

            string accessToken = _httpContext.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            
            try
            {
                await _revokedTokenRepository.CreateAsync(new RevokedToken() { Token = accessToken, RevokedAt = DateTime.UtcNow });
                await _revokedTokenRepository.CreateAsync(new RevokedToken() { Token = user.RefreshToken!, RevokedAt = DateTime.UtcNow});
            } catch (Exception ex)
            {

            }


            user.RefreshToken = "";

            await _userRepository.UpdateAsync(user);
        }
    }
}
