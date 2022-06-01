using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.Account
{
    public class RoleListDto
    {
        public long Id { get; set; }
        public long? Pid { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int UserCount { get; set; }
    }
}
