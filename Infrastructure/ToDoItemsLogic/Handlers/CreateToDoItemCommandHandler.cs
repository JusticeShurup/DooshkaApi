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


            ToDoItem item = new()
            {
                Title = mainItem.Title,
                Description = mainItem.Description,
                Status = ToDoItemStatusType.NotStarted,
                CreatedTime = DateTime.UtcNow,
                CompletionTime = mainItem.CompletionTime,
                ParentItemId = mainItem.ParentId,
                User = user,
                UserId = user.Id,
            };

            List<ToDoItem>? subItems = null; 

            if (request.SubItems != null)
            {
                subItems = new();
                foreach (var subItemDTO in request.SubItems)
                {
                    var subItem = new ToDoItem
                    {
                        Title = subItemDTO.Title,
                        Description = subItemDTO.Description,
                        CreatedTime = item.CreatedTime,
                        CompletionTime = subItemDTO.CompletionTime,
                        User = user,
                        UserId = user.Id,
                        ParentItemId = item.Id
                    };
                    subItems.Add(subItem);
                }
            }

            item.SubItems = subItems;

            await _toDoItemRepository.CreateAsync(item);

            if (subItems != null)
            {
                foreach (var subItem in item.SubItems)
                {
                    await _toDoItemRepository.CreateAsync(subItem);
                }
            }


            List<CreatedToDoItemDTO> createdToDoItemDTOs = new List<CreatedToDoItemDTO>();

            foreach (var subItem in request.SubItems)
            {
                createdToDoItemDTOs.Add(new CreatedToDoItemDTO
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    Status = item.Status,
                    CreatedTime = item.CreatedTime,
                    CompletionTime = item.CompletionTime
                });
                
            }

            return new CreatedToDoItemDTO 
            {
                Id = item.Id, 
                Title = item.Title, 
                Description = item.Description,
                Status = item.Status, 
                CreatedTime = item.CreatedTime, 
                CompletionTime = item.CompletionTime, 
                SubItems = createdToDoItemDTOs
            };
        }
    }
}
