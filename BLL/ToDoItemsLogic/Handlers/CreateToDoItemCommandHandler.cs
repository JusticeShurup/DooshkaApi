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
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ToDoItemsLogic.Handlers
{
    public class CreateToDoItemCommandHandler : IRequestHandler<CreateToDoItemCommand, CreatedToDoItemDTO>
    {
        private readonly IRepository<ToDoItem> _toDoItemRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IHttpContextAccessor _httpContext;

        public CreateToDoItemCommandHandler(IRepository<ToDoItem> toDoItemRepository, IRepository<User> userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _toDoItemRepository = toDoItemRepository;
            _userRepository = userRepository;
            _httpContext = httpContextAccessor;
        }

        public async Task<CreatedToDoItemDTO> Handle(CreateToDoItemCommand request, CancellationToken cancellationToken)
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

            var mainItem = request.MainItem;

            ToDoItem parentItem = null;

            if (mainItem.ParentId != null)
            {
                parentItem = await _toDoItemRepository.FindByIdAsync(mainItem.ParentId.Value);
                if (parentItem == null)
                {
                    throw new Exception("Item under parent id doesn't exists");
                }
            }


            ToDoItem itemForCreate = new ToDoItem()
            {
                Title = mainItem.Title,
                Description = mainItem.Description,
                CompletionTime = mainItem.CompletionTime,
                CreatedTime = DateTime.UtcNow,
                User = user,
                UserId = user.Id,
                ParentItemId = mainItem.ParentId,
                Status = ToDoItemStatusType.NotStarted,
            };

            await _toDoItemRepository.CreateAsync(itemForCreate);

            if (parentItem != null)
            {
                if (parentItem.SubItems == null)
                {
                    parentItem.SubItems = new() { itemForCreate };
                }
                else
                {
                    parentItem.SubItems.Add(itemForCreate);
                }

                await _toDoItemRepository.UpdateAsync(parentItem);
            }



            List<CreatedToDoItemDTO>? subItemsList = null;

            if (request.SubItems != null)
            {
                subItemsList = new();
                foreach (var subItem in request.SubItems)
                {
                    var subItemToDo = new ToDoItem()
                    {
                        Title = subItem.Title,
                        Description = subItem.Description,
                        CompletionTime = subItem.CompletionTime,
                        CreatedTime = itemForCreate.CreatedTime,
                        ParentItemId = itemForCreate.Id,
                        User = user,
                        UserId = user.Id,
                        Status = ToDoItemStatusType.NotStarted
                    };

                    await _toDoItemRepository.CreateAsync(subItemToDo);

                    var subItemToDoDTO = new CreatedToDoItemDTO()
                    {
                        Id = subItemToDo.Id,
                        Title = subItem.Title,
                        Description = subItem.Description,
                        CompletionTime = subItem.CompletionTime,
                        CreatedTime = itemForCreate.CreatedTime,
                        Status = ToDoItemStatusType.NotStarted
                    };

                    subItemsList.Add(subItemToDoDTO);
                }
            }

            return new CreatedToDoItemDTO()
            {
                Id = itemForCreate.Id,
                Title = itemForCreate.Title,
                Description = itemForCreate.Description,
                CompletionTime = itemForCreate.CompletionTime,
                CreatedTime = itemForCreate.CreatedTime,
                Status = ToDoItemStatusType.NotStarted,
                SubItems = subItemsList
            };

        }
    }
}
