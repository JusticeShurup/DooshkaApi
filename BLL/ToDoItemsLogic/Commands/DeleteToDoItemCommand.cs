using BLL.ToDoItemsLogic.DTOs;
using MediatR;

namespace BLL.ToDoItemsLogic.Commands
{
    public class DeleteToDoItemCommand : IRequest<DeleteToDoItemResponseDTO>
    {
        public Guid Id;

    }
}
