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

            List<ToDoItem> allMainToDoItems = _toDoItemRepository.GetAllByCondition(p => p.UserId == user.Id && p.ParentItemId == null).Result.ToList();

            var list = new List<CreatedToDoItemDTO>();

            foreach (ToDoItem toDoItem in allMainToDoItems)
            {
                var subItemsList = new List<CreatedToDoItemDTO>();

                List<ToDoItem> subItems = _toDoItemRepository.GetAllByCondition(p => p.ParentItemId == toDoItem.Id).Result.ToList();

                foreach (var subItem in subItems)
                {
                    subItemsList.Add(new CreatedToDoItemDTO
                    {
                        Id = subItem.Id,
                        Title = subItem.Title,
                        Description = subItem.Description,
                        CompletionTime = subItem.CompletionTime,
                        CreatedTime = subItem.CreatedTime,
                        Status = subItem.Status
                    });
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
            
           

            return await Task.FromResult(list);
        }
    }
}
