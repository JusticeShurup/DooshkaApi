using BLL.Exceptions;
using BLL.UserLogic.Commands;
using DAL.Entities;
using DAL.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.UserLogic.Handlers
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordHasher<User> _passwordHasher;

         
        public ChangePasswordCommandHandler(IRepository<User> userRepository, IHttpContextAccessor contextAccessor, IPasswordHasher<User> passwordHasher) 
        {
            _userRepository = userRepository;
            _httpContextAccessor = contextAccessor;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            User user = (User)_httpContextAccessor.HttpContext.Items["User"];

            if (_passwordHasher.VerifyHashedPassword(user, user.Password, request.Password) == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Password incorrect");
            }

            user.Password = _passwordHasher.HashPassword(user, request.NewPassword);

            await _userRepository.UpdateAsync(user);
        }
    }
}
