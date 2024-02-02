using BLL.UserLogic.Commands;
using BLL.UserLogic.Queries;
using DAL.Entities;
using DAL.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserAccountController : Controller
    {
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> ChangeName(ISender sender, [FromBody] ChangeNameCommand command)
        {
            var result = await sender.Send(command);

            return Ok(result);
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> ChangePassword(ISender sender, [FromBody] ChangePasswordCommand command)
        {
            await sender.Send(command);

            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<int> GetCompletedTaskStatistic(ISender sender)
        {
            var result = await sender.Send(new GetCompletedTaskStatisticQuery());

            return result;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<int> GetIncompletedTaskStatistic(ISender sender)
        {
            var result = await sender.Send(new GetIncompletedTaskStatisticQuery());

            return result;
        }

    }
}
