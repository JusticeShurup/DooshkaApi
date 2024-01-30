using BLL.UserLogic.DTOS;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.UserLogic.Commands
{
    public class RefreshAccessTokenCommand : IRequest<RefreshResponseDTO>
    {
    }
}
