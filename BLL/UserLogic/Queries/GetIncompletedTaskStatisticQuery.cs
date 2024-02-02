using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.UserLogic.Queries
{
    public class GetIncompletedTaskStatisticQuery : IRequest<int>
    {
    }
}
