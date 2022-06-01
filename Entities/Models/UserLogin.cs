using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class UserLogin
    {
        public Guid Id { get; set; }
        public long? UserId { get; set; }
        public long? LoginDate { get; set; }
        public string? LoginIp { get; set; }
        public string? LoginOs { get; set; }
        public string? LoginBrowser { get; set; }
        /// <summary>
        /// 1 = Success
        /// 2 = Failed
        /// </summary>
        public int? LoginType { get; set; }
        public string? LoginPassword { get; set; }
        public long? Cdate { get; set; }
        public long? Ddate { get; set; }

        public virtual User? User { get; set; }
    }
}
