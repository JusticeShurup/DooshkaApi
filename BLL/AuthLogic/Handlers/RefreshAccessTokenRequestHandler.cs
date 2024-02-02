using BLL.Exceptions;
using BLL.AuthLogic.Commands;
using BLL.AuthLogic.DTOS;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AuthLogic.Handlers
{
    public class RefreshAccessTokenRequestHandler : IRequestHandler<RefreshAccessTokenCommand, RefreshResponseDTO>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;


        public RefreshAccessTokenRequestHandler(IRepository<User> repository, IConfiguration configuration, IHttpContextAccessor httpContext)
        {
            _userRepository = repository;
            _configuration = configuration;
            _httpContext = httpContext;
        }


        public async Task<RefreshResponseDTO> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
        {
            User user = (User)_httpContext.HttpContext.Items["User"];

            if (user.RefreshToken != _httpContext.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last())
            {
                throw new BadRequestException("You need to login again");
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Email, user.Email) };

            var response = new RefreshResponseDTO { AccessToken = CreateJwtAccessToken(claims) };

            return response;
        }


        private string CreateJwtAccessToken(List<Claim> claims)
        {
            var token = new JwtSecurityToken
                (
                issuer: _configuration["JwtToken:Issuer"]!,
                audience: _configuration["JwtToken:Audience"]!,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddSeconds(double.Parse(_configuration["JwtToken:AccessTokenLifetimeSeconds"]!)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtToken:SecretKey"]!)), SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token)!;
        }
    }
}
