﻿using BLL.UserLogic.Queries;
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
    public class GetIncompletedTaskStatisticQueryHandler : IRequestHandler<GetIncompletedTaskStatisticQuery, int>
    {
        private readonly IRepository<ToDoItem> _toDoItemRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetIncompletedTaskStatisticQueryHandler(IRepository<ToDoItem> toDoItemRepository, IHttpContextAccessor httpContextAccessor)
        {
            _toDoItemRepository = toDoItemRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> Handle(GetIncompletedTaskStatisticQuery request, CancellationToken cancellationToken)
        {
            User user = (User)_httpContextAccessor.HttpContext.Items["User"];

            List<ToDoItem> result = _toDoItemRepository.FindAll(x => x.UserId == user.Id && x.Status == ToDoItemStatusType.NotStarted).Result.ToList();

            return result.Count;
        }
    }
}
