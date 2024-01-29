using BLL.ToDoItemsLogic.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ToDoItemsLogic.Commands
{
    public class CompleteToDoItemCommand : IRequest
    {
        public Guid ToDoItemId { get; set; }
    }
}
