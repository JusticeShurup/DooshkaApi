using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ToDoItemsLogic.DTOs
{
    public class CreatedToDoItemDTO
    {
        [Required]
        public required Guid Id { get; set; }

        [Required]
        public required string Title { get; set; }

        public string? Description { get; set; }

        [EnumDataType(typeof(ToDoItemStatusType))]
        public ToDoItemStatusType Status { get; set; }

        public required DateTime CreatedTime { get; set; }

        public DateTime? CompletionTime { get; set; }

        public List<CreatedToDoItemDTO>? SubItems { get; set; }
        

    }
}
