using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects.Account
{
    public class UserListDto
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string NationalCode { get; set; }
        public string Description { get; set; }
        public long? Mobile { get; set; }
        public bool IsActive { get; set; }
        public List<string> Roles { get; set; }


    }
}
