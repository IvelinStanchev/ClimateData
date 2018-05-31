using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeteoApp.Models
{
    public class Station
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Weight { get; set; }
        public ICollection<StationData> StationDatas { get; set; }
    }
}
