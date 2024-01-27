using BLL.ToDoItemsLogic.Commands;
using BLL.ToDoItemsLogic.DTOs;
using BLL.ToDoItemsLogic.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemController : Controller
    {

        [HttpPut("[action]")]
        public async Task<IActionResult> Create(ISender sender, CreateToDoItemCommand command)
        {
            var result = await sender.Send(command);

            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Get(ISender sender, GetToDoQuery command)
        {
            var result = await sender.Send(command);

            return Ok(result);
        }
    }
}
