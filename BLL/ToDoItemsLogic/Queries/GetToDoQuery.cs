using BLL.ToDoItemsLogic.DTOs;
using DAL.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ToDoItemsLogic.Queries
{
    public record GetToDoQuery : IRequest<List<CreatedToDoItemDTO>>;
}
