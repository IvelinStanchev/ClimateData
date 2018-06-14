using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeteoApp.Models
{
    public class DayData
    {
        public int ID { get; set; }
        public int StationID { get; set; }
        public Station Station { get; set; }
        public DateTime Date { get; set; }
        public decimal Temperature { get; set; }
        public decimal Precipitation { get; set; }
        public decimal Wind { get; set; }
        public int NumberOfThunders { get; set; }
    }
}
