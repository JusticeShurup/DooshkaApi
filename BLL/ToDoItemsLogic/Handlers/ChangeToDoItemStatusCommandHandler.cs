﻿using BLL.ToDoItemsLogic.Commands;
using BLL.ToDoItemsLogic.DTOs;
using DAL.Entities;
using DAL.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ToDoItemsLogic.Handlers
{
    public class ChangeToDoItemStatusCommandHandler : IRequestHandler<ChangeToDoItemStatusCommand>
    {
        private readonly IRepository<ToDoItem> _toDoItemRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IHttpContextAccessor _httpContext;

        public ChangeToDoItemStatusCommandHandler(IRepository<ToDoItem> toDoItemRepository, IRepository<User> userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _toDoItemRepository = toDoItemRepository;
            _userRepository = userRepository;
            _httpContext = httpContextAccessor;
        }

        public async Task Handle(ChangeToDoItemStatusCommand request, CancellationToken cancellationToken)
        {
            ToDoItem? toDoItem;
            
            toDoItem = await _toDoItemRepository.FindByIdAsync(request.ToDoItemId);
            
            if (toDoItem == null)
            {
                throw new Exception("ToDoItem doesn't found");
            }

            List<ToDoItem> subItems = _toDoItemRepository.GetAllByCondition(p => p.ParentItemId == toDoItem.Id).Result.ToList();

            toDoItem.Status = (ToDoItemStatusType)Enum.Parse(typeof(ToDoItemStatusType), request.Status.ToString());
        
            await _toDoItemRepository.UpdateAsync(toDoItem);

            foreach (ToDoItem subItem in subItems)
            {
                subItem.Status = toDoItem.Status;
                await _toDoItemRepository.UpdateAsync(subItem);
            }

                    
        }
    }
}