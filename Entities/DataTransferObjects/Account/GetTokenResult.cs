using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects.Account
{
   public class GetTokenResult
    {
        public string Token { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
    }
}
