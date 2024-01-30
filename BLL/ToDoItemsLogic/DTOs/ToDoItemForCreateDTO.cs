using BLL.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ToDoItemsLogic.DTOs
{
    public class ToDoItemForCreateDTO
    {
        [Required]
        public required string Title { get; set; }

        public string? Description { get; set; }

        public string? CompletionTime { get; set; }

        public Guid? ParentId { get; set; }

    }
}
