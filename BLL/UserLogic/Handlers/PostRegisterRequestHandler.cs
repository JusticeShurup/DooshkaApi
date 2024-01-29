using BLL.UserLogic.Commands;
using BLL.UserLogic.DTOS;
using DAL.Entities;
using DAL.Interfaces;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.UserLogic.Handlers
{
    public class PostRegisterRequestHandler : IRequestPostProcessor<RegisterCommand, UserDTO>
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<User> _repository;

        public PostRegisterRequestHandler(IConfiguration configuration, IRepository<User> repository)
        {
            _configuration = configuration;
            _repository = repository;
        }

        public async Task Process(RegisterCommand request, UserDTO response, CancellationToken cancellationToken)
        {
            if (response == null)
            {
                return;
            }

            List<Claim> claims = new() { new Claim(ClaimTypes.Email, request.Email) };


            response.AccessToken = CreateJwtAccessToken(claims);
            response.RefreshToken = CreateJwtRefreshToken(claims);

            var user = await _repository.FindByEmailAsync(request.Email);
            user!.RefreshToken = response.RefreshToken;

            await _repository.UpdateAsync(user);

        }

        private string CreateJwtAccessToken(List<Claim> claims)
        {
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtToken:Issuer"]!,
                audience: _configuration["JwtToken:Audience"]!,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddSeconds(double.Parse(_configuration["JwtToken:AccessTokenLifetimeSeconds"]!)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtToken:SecretKey"]!)), SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token)!;
        }

        private string CreateJwtRefreshToken(List<Claim> claims)
        {
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtToken:Issuer"]!,
                audience: _configuration["JwtToken:Audience"]!,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JwtToken:RefreshTokenLifetimeDays"]!)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtToken:SecretKey"]!)), SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token)!;
        }
    }
}
