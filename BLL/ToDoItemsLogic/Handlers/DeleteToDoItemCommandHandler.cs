using BLL.Exceptions;
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
            User user = (User)_httpContext.HttpContext.Items["User"];

            var toDoItem = _toDoItemRepository.Find(x => x.Id == request.Id);
            
            if (toDoItem == null )
            {
                throw new NotFoundException("ToDoItem doesn't exists");
            }

            if (toDoItem.UserId != user.Id)
            {
                throw new BadRequestException("This ToDoItem isn't user own");
            }

            var subToDoItems = await _toDoItemRepository.FindAll(p => p.ParentItemId == toDoItem.Id);

            foreach ( var subItem in subToDoItems )
            {
                await _toDoItemRepository.DeleteAsync(subItem);
            }

            await _toDoItemRepository.DeleteAsync(toDoItem);

            return new DeleteToDoItemResponseDTO() { Response = "All deleted successffully"}; 



        }
    }
}
