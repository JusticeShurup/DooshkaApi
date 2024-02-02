using BLL.AuthLogic.Commands;
using DAL.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

using DAL.Entities;
using BLL.AuthLogic.DTOS;
using BLL.Exceptions;

namespace BLL.AuthLogic.Handlers
{
    public class RegisterRequestHandler : IRequestHandler<RegisterCommand, UserDTO>
    {
        private readonly IRepository<User> _repository;
        private readonly IPasswordHasher<User> _passwordHasher;


        public RegisterRequestHandler(IRepository<User> repository, IPasswordHasher<User> passwordHasher)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
        }


        public async Task<UserDTO> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            User user = new User { Email = request.Email, Password = request.Password, Name = request.Name };

            user.Password = _passwordHasher.HashPassword(user, request.Password);


            var existUser = _repository.Find(x => x.Id == user.Id);
            if (existUser != null)
            {
                throw new BadRequestException("User already exist");
            }

            await _repository.CreateAsync(user);


            UserDTO userDTO = new() { Id = user.Id, Email = user.Email, Name = user.Name, AccessToken = "", RefreshToken = "" };


            return await Task.FromResult(userDTO);
        }
    }
}
