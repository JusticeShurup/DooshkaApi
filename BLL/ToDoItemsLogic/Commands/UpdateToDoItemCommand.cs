using BLL.ToDoItemsLogic.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ToDoItemsLogic.Commands
{
    public class UpdateToDoItemCommand : IRequest<UpdatedToDoItemDTO>
    {
        public required Guid Id { get; set; }
        
        public required string Title { get; set; }
        
        public required string Description { get; set; } = string.Empty;

        public required string CompletionDate { get; set; }
    }
}
