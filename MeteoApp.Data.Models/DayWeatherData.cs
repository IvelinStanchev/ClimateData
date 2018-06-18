using System;
using MeteoApp.Data.Models.Base;

namespace MeteoApp.Data.Models
{
    public class DayWeatherData : DbModel
    {
        public DateTime Date { get; set; }

        public decimal Temperature { get; set; }
        public decimal Precipitation { get; set; }
        public decimal Wind { get; set; }
        public int ThunderCount { get; set; }
        
        public virtual int StationId { get; set; }
        public virtual Station Station { get; set; }
    }
}
