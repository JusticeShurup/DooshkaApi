using BLL.UserLogic.Commands;
using BLL.UserLogic.DTOs;
using DAL.Entities;
using DAL.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.UserLogic.Handlers
{
    public class ChangeNameCommandHandler : IRequestHandler<ChangeNameCommand, UserAccountDTO>
    {

        private readonly IRepository<User> _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChangeNameCommandHandler(IHttpContextAccessor httpContextAccessor, IRepository<User> userRepository) 
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            
        }

        public async Task<UserAccountDTO> Handle(ChangeNameCommand request, CancellationToken cancellationToken)
        {
            User user = (User)_httpContextAccessor.HttpContext.Items["User"];

            user.Name = request.NewName;

            await _userRepository.UpdateAsync(user);

            return new UserAccountDTO() { Email = user.Email, Name = user.Name };
        }
    }
}
