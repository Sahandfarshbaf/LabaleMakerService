using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.Commodity
{
    public class CommodityFullInfoDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public long CommodityId { get; set; }
        public long? TrackingCode { get; set; }
        public long? MunufacturerPrice { get; set; }
        public long? ConsumerPrice { get; set; }
        public string? CreateUserName { get; set; }
        public long? CreateDate { get; set; }
    }
}
