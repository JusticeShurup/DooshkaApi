using BLL.UserLogic.Commands;
using BLL.UserLogic.DTOS;
using DAL.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Register(ISender sender, RegisterCommand command)
        {
            var result = await sender.Send(command);

            if (result == null)
            {
                return BadRequest("User can't be created");
            }

            return new JsonResult(result) { StatusCode = StatusCodes.Status201Created};
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Login(ISender sender, LoginCommand command)
        {
            UserDTO result;

            try
            {
                result = await sender.Send(command);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to login {ex.Message}");
            }
            

            return Ok(result);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Refresh(ISender sender, RefreshAccessTokenCommand command)
        {
            RefreshResponseDTO result;

            try
            {
                result = await sender.Send(command);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to refresh {ex.Message}");
            }


            return new JsonResult(result) { StatusCode = StatusCodes.Status200OK};
        }

    }
}
