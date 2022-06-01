using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class Commodity
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public long CommodityId { get; set; }
        public long? TrackingCode { get; set; }
        public long? MunufacturerPrice { get; set; }
        public long? ConsumerPrice { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual User? Cuser { get; set; }
        public virtual User? DaUser { get; set; }
        public virtual User? Duser { get; set; }
        public virtual User? Muser { get; set; }
    }
}
