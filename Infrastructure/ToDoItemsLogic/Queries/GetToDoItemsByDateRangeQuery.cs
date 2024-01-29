using BLL.ToDoItemsLogic.DTOs;
using MediatR;

namespace BLL.ToDoItemsLogic.Queries
{
    public class GetToDoItemsByDateRangeQuery : IRequest<List<CreatedToDoItemDTO>>
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

    }
}
