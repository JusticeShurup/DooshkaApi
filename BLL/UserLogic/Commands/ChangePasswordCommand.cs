using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.UserLogic.Commands
{
    public class ChangePasswordCommand : IRequest
    {
        public required string Password { get; set; }
        public required string NewPassword { get; set; }
    }
}
