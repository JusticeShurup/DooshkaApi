using BLL.UserLogic.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.UserLogic.Commands
{
    public class ChangeNameCommand : IRequest<UserAccountDTO>
    {
        public required string NewName { get; set; }

    }
}
