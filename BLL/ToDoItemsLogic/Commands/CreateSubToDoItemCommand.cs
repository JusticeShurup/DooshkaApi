using BLL.ToDoItemsLogic.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ToDoItemsLogic.Commands
{
    public class CreateSubToDoItemCommand : IRequest<CreatedToDoItemDTO>
    {
        [Required]
        public required string Title { get; set; }

        public string Description { get; set; } = string.Empty;

        public required string CompletionDate { get; set; }

        public required Guid ParentId { get; set; }
    }

}
