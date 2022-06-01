using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects.Account
{
    public class LoginDto
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string LoginIp { get; set; }
        public string LoginOs { get; set; }
        public string LoginBrowser { get; set; }
       

    }
}
