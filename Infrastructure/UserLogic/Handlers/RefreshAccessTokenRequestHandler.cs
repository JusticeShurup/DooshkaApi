using BLL.UserLogic.Commands;
using BLL.UserLogic.DTOS;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using MediatR;
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

namespace BLL.UserLogic.Handlers
{
    public class RefreshAccessTokenRequestHandler : IRequestHandler<RefreshAccessTokenCommand, RefreshResponseDTO>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IConfiguration _configuration;


        public RefreshAccessTokenRequestHandler(IRepository<User> repository, IConfiguration configuration)
        {
            _userRepository = repository;
            _configuration = configuration;
        }


        public async Task<RefreshResponseDTO> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
        {
            JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(request.RefreshToken);


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


            var claims = new List<Claim> { new Claim(ClaimTypes.Email, user.Email) };

            var response = new RefreshResponseDTO { AccessToken = CreateJwtAccessToken(claims) };

            return response;
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
    }
}
