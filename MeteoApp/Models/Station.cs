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
        public ICollection<DayData> StationDatas { get; set; }
        public ICollection<StationWeight> StationWeights { get; set; }
        public ICollection<StationAvailability> StationAvailabilityPeriods { get; set; }
    }
}
