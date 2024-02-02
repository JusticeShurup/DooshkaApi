using BLL.Exceptions;
using BLL.ToDoItemsLogic.Commands;
using BLL.ToDoItemsLogic.DTOs;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ToDoItemsLogic.Handlers
{
    public class UpdateToDoItemCommandHandler : IRequestHandler<UpdateToDoItemCommand, UpdatedToDoItemDTO>
    {
        private readonly IRepository<ToDoItem> _toDoItemRepository;

        public UpdateToDoItemCommandHandler(IRepository<ToDoItem> toDoItemRepository)
        {
            _toDoItemRepository = toDoItemRepository;
        }


        public async Task<UpdatedToDoItemDTO> Handle(UpdateToDoItemCommand request, CancellationToken cancellationToken)
        {
            ToDoItem? currentItem = _toDoItemRepository.Find(x => x.Id == request.Id);
        
            if (currentItem == null)
            {
                throw new NotFoundException("ToDoItem not found");
            }

            ToDoItem? parentItem = currentItem.ParentItemId != null ? _toDoItemRepository.Find(x => x.Id == (Guid)currentItem.ParentItemId) : null;

            if (DateOnly.Parse(request.CompletionDate) < currentItem.CreatedDate)
            {
                throw new BadRequestException("Completion date is bad");
            }

            if (parentItem != null && DateOnly.Parse(request.CompletionDate) > parentItem.CompletionDate)
            {
                throw new BadRequestException("The CompletionDate cannot be later than the MainItem CompletionDate");
            } 

            List<ToDoItem> subItems = (List<ToDoItem>)await _toDoItemRepository.FindAll(x => x.ParentItemId == currentItem.Id);

            if (subItems != null)
            {
                foreach (ToDoItem subItem in subItems)
                {
                    subItem.CompletionDate = DateOnly.Parse(request.CompletionDate);
                }
            }

            currentItem.Title = request.Title;
            currentItem.Description = request.Description;
            currentItem.CompletionDate = DateOnly.Parse(request.CompletionDate);


            return new UpdatedToDoItemDTO()
            {
                Id = currentItem.Id,
                Title = currentItem.Title,
                Description = currentItem.Description,
                CompletionDate = (DateOnly)currentItem.CompletionDate!,
                CreatedDate = currentItem.CreatedDate,
                SubItems = (List<UpdatedToDoItemDTO>)subItems.Select(x => new UpdatedToDoItemDTO() 
                { 
                    Id = x.Id, 
                    Title = x.Title,
                    Description = x.Description, 
                    CompletionDate = (DateOnly)currentItem.CompletionDate!, 
                    CreatedDate = currentItem.CreatedDate, 
                    SubItems = new List<UpdatedToDoItemDTO>() 
                }).ToList()
            };


        }
    }
}
