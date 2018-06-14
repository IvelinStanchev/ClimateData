using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeteoApp.Models
{
    public class StationAvailability
    {
        public int ID { get; set; }
        public Station Station { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public decimal Weight { get; set; }
    }
}
