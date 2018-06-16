using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoApp.Data.Models
{
    public class MeteoReport
    {
        public MeteoReport(int stationId, decimal temperatureAvg, decimal temperatureDeviation, decimal temperatureMax, 
            DateTime temperatureMaxDate, decimal temperatureMin, DateTime temperatureMinDate, 
            decimal precipitationSum, decimal precipitationAgainstNorm, decimal precipitationMax, 
            DateTime precipitationMaxDate, int dayCountPrecipitationBiggerThan1, int dayCountPrecipitationBiggerThan10, 
            int dayCountWindBiggerThan14, int dayCountThunders)
        {
            StationId = stationId;
            TemperatureAvg = temperatureAvg;
            TemperatureDeviation = temperatureDeviation;
            TemperatureMax = temperatureMax;
            TemperatureMaxDate = temperatureMaxDate;
            TemperatureMin = temperatureMin;
            TemperatureMinDate = temperatureMinDate;
            PrecipitationSum = precipitationSum;
            PrecipitationAgainstNorm = precipitationAgainstNorm;
            PrecipitationMax = precipitationMax;
            PrecipitationMaxDate = precipitationMaxDate;
            DayCountPrecipitationBiggerThan1 = dayCountPrecipitationBiggerThan1;
            DayCountPrecipitationBiggerThan10 = dayCountPrecipitationBiggerThan10;
            DayCountWindBiggerThan14 = dayCountWindBiggerThan14;
            DayCountThunders = dayCountThunders;
        }

        public int StationId { get; set; }
        public decimal TemperatureAvg { get; set; }
        public decimal TemperatureDeviation { get; set; }
        public decimal TemperatureMax { get; set; }
        public DateTime TemperatureMaxDate { get; set; }
        public decimal TemperatureMin { get; set; }
        public DateTime TemperatureMinDate { get; set; }
        public decimal PrecipitationSum { get; set; }
        public decimal PrecipitationAgainstNorm { get; set; }
        public decimal PrecipitationMax { get; set; }
        public DateTime PrecipitationMaxDate { get; set; }
        public int DayCountPrecipitationBiggerThan1 { get; set; }
        public int DayCountPrecipitationBiggerThan10 { get; set; }
        public int DayCountWindBiggerThan14 { get; set; }
        public int DayCountThunders { get; set; }
    }
}
