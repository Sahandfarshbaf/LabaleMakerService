using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.BaseInfo
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public long? CountryId { get; set; }
        public string CountryTitle { get; set; }
        public int? ProvinceId { get; set; }
        public string ProvinceTitle { get; set; }
        public bool? IsCapital { get; set; }
        public bool? IsActive { get; set; }
        public string CodeMapper { get; set; }
    }
}
