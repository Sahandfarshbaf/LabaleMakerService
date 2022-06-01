using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class UserActivationCode
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? CodeType { get; set; }
        public long? Code { get; set; }
        public long? EndDateTime { get; set; }

        public virtual User? User { get; set; }
    }
}
