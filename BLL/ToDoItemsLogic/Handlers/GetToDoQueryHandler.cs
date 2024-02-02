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
        private readonly IHttpContextAccessor _httpContext;

        public GetToDoQueryHandler(IRepository<ToDoItem> toDoItemRepository, IHttpContextAccessor httpContext) 
        {
            _toDoItemRepository = toDoItemRepository;
            _httpContext = httpContext;
        }

        public async Task<List<CreatedToDoItemDTO>> Handle(GetToDoQuery request, CancellationToken cancellationToken)
        {
            User user = (User)_httpContext.HttpContext.Items["User"];

            List<ToDoItem> allMainToDoItems = _toDoItemRepository.FindAll(p => p.UserId == user.Id && p.ParentItemId == null).Result.ToList();

            var list = new List<CreatedToDoItemDTO>();

            foreach (ToDoItem toDoItem in allMainToDoItems)
            {
                var subItemsList = new List<CreatedToDoItemDTO>();

                List<ToDoItem> subItems = _toDoItemRepository.FindAll(p => p.ParentItemId == toDoItem.Id).Result.ToList();

                foreach (var subItem in subItems)
                {
                    subItemsList.Add(new CreatedToDoItemDTO
                    {
                        Id = subItem.Id,
                        Title = subItem.Title,
                        Description = subItem.Description,
                        CompletionDate = subItem.CompletionDate,
                        CreatedDate = subItem.CreatedDate,
                        Status = subItem.Status
                    });
                }

                list.Add(new CreatedToDoItemDTO
                {
                    Id = toDoItem.Id,
                    Title = toDoItem.Title,
                    Description = toDoItem.Description,
                    CompletionDate = toDoItem.CompletionDate,
                    CreatedDate = toDoItem.CreatedDate,
                    Status = toDoItem.Status,
                    SubItems = subItemsList
                });
            }
            
           

            return await Task.FromResult(list);
        }
    }
}
