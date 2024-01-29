using BLL.ToDoItemsLogic.Commands;
using BLL.ToDoItemsLogic.DTOs;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ToDoItemsLogic.Handlers
{
    public class DeleteToDoItemCommandHandler : IRequestHandler<DeleteToDoItemCommand, DeleteToDoItemResponseDTO>
    {
        private readonly IRepository<ToDoItem> _toDoItemRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IHttpContextAccessor _httpContext;

        public DeleteToDoItemCommandHandler(IRepository<ToDoItem> toDoItemRepository, IRepository<User> userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _toDoItemRepository = toDoItemRepository;
            _userRepository = userRepository;
            _httpContext = httpContextAccessor;

        }

        public async Task<DeleteToDoItemResponseDTO> Handle(DeleteToDoItemCommand request, CancellationToken cancellationToken)
        {
            var tokenString = _httpContext.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

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


            var toDoItem = await _toDoItemRepository.FindByIdAsync(request.Id);
            
            if (toDoItem == null )
            {
                throw new Exception("ToDoItem doesn't exists");
            }

            if (toDoItem.UserId != user.Id)
            {
                throw new Exception("It's not yours");
            }

            var subToDoItems = await _toDoItemRepository.GetAllByCondition(p => p.ParentItemId == toDoItem.Id);

            foreach ( var subItem in subToDoItems )
            {
                await _toDoItemRepository.DeleteByIdAsync(subItem.Id);
            }

            await _toDoItemRepository.DeleteByIdAsync(toDoItem.Id);

            return new DeleteToDoItemResponseDTO() { Response = "All deleted successffully"}; 



        }
    }
}
