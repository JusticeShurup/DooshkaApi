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
    public class CreateToDoItemCommand : IRequest<CreatedToDoItemDTO>
    {
        [Required]
        public required ToDoItemForCreateDTO MainItem { get; set; }

        public List<ToDoItemForCreateDTO>? SubItems { get; set; }


    }
}
