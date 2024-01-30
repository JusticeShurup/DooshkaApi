using BLL.Exceptions;
using BLL.UserLogic.Commands;
using BLL.UserLogic.DTOS;
using DAL.Entities;
using DAL.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.UserLogic.Handlers
{
    public class LoginRequestHandler : IRequestHandler<LoginCommand, UserDTO>
    {
        private readonly IRepository<User> _repository;
        private readonly IPasswordHasher<User> _passwordHasher;


        public LoginRequestHandler(IRepository<User> repository, IPasswordHasher<User> passwordHasher)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
        }


        public async Task<UserDTO> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new BadRequestException("User doesn't exists");
            }

            if (_passwordHasher.VerifyHashedPassword(user, user.Password, request.Password) == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Password invalid");
            }

            return new UserDTO {Id = user.Id, Email = user.Email, AccessToken = "", RefreshToken = user.RefreshToken };
        }
    }
}
