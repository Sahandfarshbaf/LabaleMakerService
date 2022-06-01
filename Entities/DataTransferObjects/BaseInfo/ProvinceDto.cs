using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.BaseInfo
{
    public class ProvinceDto
    {
        public int Id { get; set; }
        public long? CountryId { get; set; }
        public string CountryTitle { get; set; }
        public string Title { get; set; }
        public string CodeMapper { get; set; }
        public bool? IsActive { get; set; }
    }
}
