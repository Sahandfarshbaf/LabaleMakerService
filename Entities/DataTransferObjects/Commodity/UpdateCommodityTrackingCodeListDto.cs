using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.Commodity
{
    public class UpdateCommodityTrackingCodeListDto
    {
        public long CommodityId { get; set; }
        public long TrackingCode { get; set; }
        public bool IsSuccessFull { get; set; }
        public string ErrorDescription { get; set; }

    }
}
