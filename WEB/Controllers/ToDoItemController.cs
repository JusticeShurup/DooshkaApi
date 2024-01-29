using BLL.ToDoItemsLogic.Commands;
using BLL.ToDoItemsLogic.DTOs;
using BLL.ToDoItemsLogic.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Web.Controllers
{
    [EnableCors("AllowAll")]
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

        [HttpGet("[action]")]
        public async Task<IActionResult> Get(ISender sender)
        {
            List<CreatedToDoItemDTO> result;
            try
            {
                result = await sender.Send(new GetToDoQuery());
            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteItemById(ISender sender, [FromQuery] Guid id)
        {
            DeleteToDoItemResponseDTO result;
            try
            {
               result = await sender.Send(new DeleteToDoItemCommand() { Id = id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetToDoItemsByCompletionDate(ISender sender, [FromQuery] string dateAsString)
        {
            List<CreatedToDoItemDTO> result;
            try
            {
                result = await sender.Send(new GetToDoItemsByDateQuery() { Date = DateOnly.Parse(dateAsString)});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetToDoItemsByDateRange(ISender sender, [FromQuery] string startDateAsString, [FromQuery] string endDateAsString)
        {
            List<CreatedToDoItemDTO> result;
            try
            {
                result = await sender.Send(new GetToDoItemsByDateRangeQuery() { StartDate = DateOnly.Parse(startDateAsString), EndDate = DateOnly.Parse(endDateAsString) });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ChangeToDoItemStatus(ISender sender, [FromQuery] Guid id, [FromQuery] int status)
        {
            try
            {
                await sender.Send(new ChangeToDoItemStatusCommand() { ToDoItemId = id, Status = status});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
