using System;
using MeteoApp.Data.Models.Base;

namespace MeteoApp.Data.Models
{
    public class StationWeight : DbModel
    {
        public decimal Weight { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public virtual int StationId { get; set; }
        public virtual Station Station { get; set; }
    }
}
