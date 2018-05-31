using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeteoApp.Models
{
    public class StationData
    {
        public int ID { get; set; }
        public int StationID { get; set; }
        public Station Station { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TmpAvg { get; set; }
        public decimal TmpDelta { get; set; }
        public decimal TmpMax { get; set; }
        public int TmpMaxDate { get; set; }
        public decimal TmpMin { get; set; }
        public int TmpMinDate { get; set; }
        public decimal PrecipSum { get; set; }
        public decimal PrecipQn { get; set; }
        public decimal PrecipMax { get; set; }
        public int PrecipMaxDate { get; set; }
        public int NumDaysPrecipAboveOne { get; set; }
        public int NumDaysPrecipAboveTen { get; set; }
        public int NumDaysWindAboveFourteen { get; set; }
        public int NumDaysTunder { get; set; }
    }
}
