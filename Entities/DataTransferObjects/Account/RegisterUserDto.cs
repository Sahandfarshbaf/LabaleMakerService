using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects.Account
{
    public class RegisterUserDto
    {
        public string NationalCode { get; set; }
        public long MobileNo { get; set; }

    }
}
