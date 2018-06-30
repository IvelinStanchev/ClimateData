using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeteoApp.Models.HomeViewModels
{
    public class GraphicalStationDataViewModel
    {
        [DataType(DataType.Date), Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM}", ApplyFormatInEditMode = true)]
        public DateTime From { get; set; }

        [DataType(DataType.Date), Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM}", ApplyFormatInEditMode = true)]
        public DateTime To { get; set; }

        public IEnumerable<SelectListItem> StationNames { get; set; }
        public IEnumerable<SelectListItem> WeatherDataTypeNames { get; set; }

        public string StationName { get; set; }
        public string WeatherDataTypeName { get; set; }
    }
}
