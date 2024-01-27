using BLL.ToDoItemsLogic.DTOs;
using BLL.ToDoItemsLogic.Queries;
using DAL.Entities;
using DAL.Interfaces;
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
    public class GetToDoQueryHandler : IRequestHandler<GetToDoQuery, List<CreatedToDoItemDTO>>
    {
        private readonly IRepository<ToDoItem> _toDoItemRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IHttpContextAccessor _httpContext;

        public GetToDoQueryHandler(IRepository<ToDoItem> toDoItemRepository, IRepository<User> userRepository, IHttpContextAccessor httpContext) 
        {
            _toDoItemRepository = toDoItemRepository;
            _userRepository = userRepository;
            _httpContext = httpContext;
        }

        public async Task<List<CreatedToDoItemDTO>> Handle(GetToDoQuery request, CancellationToken cancellationToken)
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

            List<ToDoItem> allToDoItems = _toDoItemRepository.GetAll().ToList();

            var list = new List<CreatedToDoItemDTO>();

            if (user.ToDoItems != null)
            {
                foreach (ToDoItem toDoItem in user.ToDoItems)
                {
                    var subItemsList = new List<CreatedToDoItemDTO>();
                    
                    if (toDoItem.SubItems != null)
                    { 
                        foreach (ToDoItem subItem in toDoItem.SubItems!)
                        {
                            subItemsList.Add(new CreatedToDoItemDTO
                            {
                                Id = subItem.Id,
                                Title = subItem.Title,
                                Description = subItem.Description,
                                CompletionTime = subItem.CompletionTime,
                                CreatedTime = subItem.CreatedTime,
                                Status = subItem.Status,
                                SubItems = null
                            });
                        }
                    }

                    if (toDoItem.ParentItemId == null)
                    {
                        list.Add(new CreatedToDoItemDTO
                        {
                            Id = toDoItem.Id,
                            Title = toDoItem.Title,
                            Description = toDoItem.Description,
                            CompletionTime = toDoItem.CompletionTime,
                            CreatedTime = toDoItem.CreatedTime,
                            Status = toDoItem.Status,
                            SubItems = subItemsList
                        });
                    }
                }
            }
            else
            {
                foreach (ToDoItem toDoItem in allToDoItems)
                {
                    
                    if (toDoItem.User == user && toDoItem.ParentItemId == null)
                    {
                        var subItemsList = new List<CreatedToDoItemDTO>();

                        if (toDoItem.SubItems == null)
                        {
                            foreach (ToDoItem subItem in toDoItem.SubItems!)
                            {
                                subItemsList.Add(new CreatedToDoItemDTO
                                {
                                    Id = toDoItem.Id,
                                    Title = toDoItem.Title,
                                    Description = toDoItem.Description,
                                    CompletionTime = toDoItem.CompletionTime,
                                    CreatedTime = toDoItem.CreatedTime,
                                    Status = toDoItem.Status,
                                    SubItems = null
                                });
                            }
                        }

                        list.Add(new CreatedToDoItemDTO
                        {
                            Id = toDoItem.Id,
                            Title = toDoItem.Title,
                            Description = toDoItem.Description,
                            CompletionTime = toDoItem.CompletionTime,
                            CreatedTime = toDoItem.CreatedTime,
                            Status = toDoItem.Status,
                            SubItems = subItemsList
                        });
                    }
                }
            }

            return await Task.FromResult(list);
        }
    }
}
