using System.Collections.Generic;
using MeteoApp.Data.Models.Base;

namespace MeteoApp.Data.Models
{
    public class Station : DbModel
    {
        public string Name { get; set; }

        public virtual ICollection<DayWeatherData> StationData { get; set; }
        public virtual ICollection<StationWeight> StationWeights { get; set; }
        public virtual ICollection<StationAvailabilityPeriod> StationAvailabilityPeriods { get; set; }

    }
}
