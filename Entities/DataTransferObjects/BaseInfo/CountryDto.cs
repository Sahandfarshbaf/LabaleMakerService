using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.BaseInfo
{
    public class CountryDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string AreaCode { get; set; }
        public long? CodeMapper { get; set; }
    }
}
