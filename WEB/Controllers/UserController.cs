using DAL.Entities;
using DAL.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        private readonly IRepository<User> _repository;
            

        public UserController(IRepository<User> repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task ChangeName(ISender sender, [FromQuery] string newName)
        {
            
        }

        [HttpPost]
        [Route("[action]")]
        public async Task ChangePassword(ISender sender, [FromBody] string newPassword)
        {

        }
    }
}
