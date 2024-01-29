using BLL.UserLogic.Commands;
using BLL.UserLogic.DTOS;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.UserLogic.Handlers
{
    public class LogoutRequestHandler : IRequestHandler<LogoutCommand, LogoutResponseDTO>
    {
        public Task<LogoutResponseDTO> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult( new LogoutResponseDTO { Response = "Successfully logout" });
        }
    }
}
