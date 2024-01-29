using BLL.ToDoItemsLogic.Commands;
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
    public class CompleteToDoItemCommandHandler : IRequestHandler<CompleteToDoItemCommand>
    {
        private readonly IRepository<ToDoItem> _toDoItemRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IHttpContextAccessor _httpContext;

        public CompleteToDoItemCommandHandler(IRepository<ToDoItem> toDoItemRepository, IRepository<User> userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _toDoItemRepository = toDoItemRepository;
            _userRepository = userRepository;
            _httpContext = httpContextAccessor;
        }

        public async Task Handle(CompleteToDoItemCommand request, CancellationToken cancellationToken)
        {
            ToDoItem? toDoItem;
            
            toDoItem = await _toDoItemRepository.FindByIdAsync(request.ToDoItemId);
            
            if (toDoItem == null)
            {
                throw new Exception("ToDoItem doesn't found");
            }

            List<ToDoItem> subItems = _toDoItemRepository.GetAllByCondition(p => p.ParentItemId == toDoItem.Id).Result.ToList();

            toDoItem.Status = ToDoItemStatusType.Done;
        
            await _toDoItemRepository.UpdateAsync(toDoItem);

            foreach (ToDoItem subItem in subItems)
            {
                subItem.Status = ToDoItemStatusType.Done;
                await _toDoItemRepository.UpdateAsync(subItem);
            }

                    
        }
    }
}
