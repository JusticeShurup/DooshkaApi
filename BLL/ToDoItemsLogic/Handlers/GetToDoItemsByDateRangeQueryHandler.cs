using BLL.ToDoItemsLogic.DTOs;
using BLL.ToDoItemsLogic.Queries;
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
    public class GetToDoItemsByDateRangeQueryHandler : IRequestHandler<GetToDoItemsByDateRangeQuery, List<CreatedToDoItemDTO>>
    {
        private readonly IRepository<ToDoItem> _toDoItemRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IHttpContextAccessor _httpContext;


        public GetToDoItemsByDateRangeQueryHandler(IRepository<User> userRepository, IRepository<ToDoItem> toDoItemRepository, IHttpContextAccessor httpContext)
        {
            _userRepository = userRepository;
            _toDoItemRepository = toDoItemRepository;
            _httpContext = httpContext;

        }

        public async Task<List<CreatedToDoItemDTO>> Handle(GetToDoItemsByDateRangeQuery request, CancellationToken cancellationToken)
        {
            User user = (User)_httpContext.HttpContext.Items["User"];


            DateOnly startDate = new DateOnly(request.StartDate.Year, request.StartDate.Month, request.StartDate.Day); // начало дня
            DateOnly endDate = new DateOnly(request.EndDate.Year, request.EndDate.Month, request.EndDate.Day + 1);

            var result = await _toDoItemRepository.GetAllByCondition(p => p.CompletionTime >= startDate && p.CompletionTime <= endDate);

            List<ToDoItem> allToDoItems = result.ToList();

            var list = new List<CreatedToDoItemDTO>();

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
                                Status = toDoItem.Status
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

            return list;
        }
    }
}
