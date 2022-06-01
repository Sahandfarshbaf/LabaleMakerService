using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects.Account
{
    public class SetPasswordDto
    {
        public long UserId { get; set; }
        public int Code { get; set; }
        public string Password { get; set; }
    }
}
