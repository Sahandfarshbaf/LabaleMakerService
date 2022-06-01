using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects.Account
{
    public class LoginResult
    {
        public List<RoleDto> RoleList { get; set; }
        public Guid UserLoginID { get; set; }
    }
}
