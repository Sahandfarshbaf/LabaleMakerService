using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class Role
    {
        public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public long Id { get; set; }
        public string? Name { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? Ddate { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
