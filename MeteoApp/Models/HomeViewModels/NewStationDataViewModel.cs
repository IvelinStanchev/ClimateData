using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeteoApp.Models.HomeViewModels
{
    public class NewStationDataViewModel
    {
        public IEnumerable<SelectListItem> StationNames { get; set; }


        [DataType(DataType.Date), Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public decimal Temperature { get; set; }
        public decimal Precipitation { get; set; }
        public decimal Wind { get; set; }
        public int ThunderCount { get; set; }
        public string StationName { get; set; }
    }
}
