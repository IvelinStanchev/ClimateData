using System;
using System.ComponentModel.DataAnnotations;

namespace MeteoApp.Models.AdminViewModels
{
    public class DatePeriodModel
    {
        [DataType(DataType.Date), Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date), Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
    }
}
