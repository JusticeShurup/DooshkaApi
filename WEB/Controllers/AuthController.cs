using BLL.UserLogic.Commands;
using BLL.UserLogic.DTOS;
using DAL.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{

    [EnableCors("AllowAll")]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Register(ISender sender, RegisterCommand command)
        {
            UserDTO result = await sender.Send(command);

            return new JsonResult(result) { StatusCode = StatusCodes.Status201Created };
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Login(ISender sender, LoginCommand command)
        {
            UserDTO result = await sender.Send(command);   

            return Ok(result);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Refresh(ISender sender, RefreshAccessTokenCommand command)
        {
            RefreshResponseDTO result;

            result = await sender.Send(command);


            return new JsonResult(result) { StatusCode = StatusCodes.Status200OK};
        }

        [Authorize]
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Logout(ISender sender, LogoutCommand command)
        {
            LogoutResponseDTO result;

            result = await sender.Send(command);
            

            return new JsonResult(result) { StatusCode = StatusCodes.Status200OK };
        }

    }
}
