using BLL.Exceptions;
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
    public class CreateSubToDoItemCommandHandler : IRequestHandler<CreateSubToDoItemCommand, CreatedToDoItemDTO>
    {
        private readonly IRepository<ToDoItem> _toDoItemRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IHttpContextAccessor _httpContext;

        public CreateSubToDoItemCommandHandler(IRepository<ToDoItem> toDoItemRepository, IRepository<User> userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _toDoItemRepository = toDoItemRepository;
            _userRepository = userRepository;
            _httpContext = httpContextAccessor;
        }

        public async Task<CreatedToDoItemDTO> Handle(CreateSubToDoItemCommand request, CancellationToken cancellationToken)
        {
            User user = (User)_httpContext.HttpContext.Items["User"];




            ToDoItem? parentItem = _toDoItemRepository.Find(x => x.Id == request.ParentId);
            if (parentItem == null)
            {
                throw new BadRequestException("Item under parent id doesn't exist");
            }

            ToDoItem itemForCreate = new ToDoItem()
            {
                Title = request.Title,
                Description = request.Description,
                CompletionDate = request.CompletionDate == null ? null : DateOnly.Parse(request.CompletionDate),
                CreatedDate = DateOnly.FromDateTime(DateTime.Now),
                User = user,
                UserId = user.Id,
                ParentItemId = request.ParentId,
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

            return new CreatedToDoItemDTO()
            {
                Id = itemForCreate.Id,
                Title = itemForCreate.Title,
                Description = itemForCreate.Description,
                CompletionDate = itemForCreate.CompletionDate,
                CreatedDate = itemForCreate.CreatedDate,
                Status = ToDoItemStatusType.NotStarted
            };

        }
    }
}
