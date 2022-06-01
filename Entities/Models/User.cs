using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class User
    {
        public User()
        {
            CommodityCusers = new HashSet<Commodity>();
            CommodityDaUsers = new HashSet<Commodity>();
            CommodityDusers = new HashSet<Commodity>();
            CommodityMusers = new HashSet<Commodity>();
            UserActivationCodes = new HashSet<UserActivationCode>();
            UserLogins = new HashSet<UserLogin>();
            UserRoles = new HashSet<UserRole>();
        }

        public long Id { get; set; }
        public int? LoginTryCount { get; set; }
        public string? Hpassword { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? NationalCode { get; set; }
        public long? Mobile { get; set; }
        public string? Description { get; set; }
        public string? Email { get; set; }
        public long? Cdate { get; set; }
        public long? Ddate { get; set; }
        public long? DaDate { get; set; }

        public virtual ICollection<Commodity> CommodityCusers { get; set; }
        public virtual ICollection<Commodity> CommodityDaUsers { get; set; }
        public virtual ICollection<Commodity> CommodityDusers { get; set; }
        public virtual ICollection<Commodity> CommodityMusers { get; set; }
        public virtual ICollection<UserActivationCode> UserActivationCodes { get; set; }
        public virtual ICollection<UserLogin> UserLogins { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
