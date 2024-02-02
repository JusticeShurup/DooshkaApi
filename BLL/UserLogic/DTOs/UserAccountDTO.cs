using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.UserLogic.DTOs
{
    public class UserAccountDTO
    {
        public required string Email { get; set; }
        public required string Name { get; set; }

    }
}
