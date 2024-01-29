using BLL.ToDoItemsLogic.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ToDoItemsLogic.Queries
{
    public class GetToDoItemsByDateQuery : IRequest<List<CreatedToDoItemDTO>>
    {
        public DateOnly Date { get; set; }
    }
}
