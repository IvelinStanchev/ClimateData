using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeteoApp.Models.AdminViewModels
{
    public class StationWeightViewModel
    {
        public int StationId { get; set; }
        public string StationName { get; set; }
        public decimal StationWeight { get; set; }
    }
}
