using MeteoApp.Data;
using MeteoApp.Data.Models;
using MeteoApp.Data.Models.Base;
using MeteoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeteoApp.Services
{
    class MainService
    {
        MeteoDataDBContext dbContext = new MeteoDataDBContext();

        /*private IQueryable<IPeriod> ApplyRangeRestriction(IQueryable<IPeriod> oldDataSet, 
            DateTime from, DateTime to)
        {
            return oldDataSet.Where(set => 
            (set.From <= from && from <= set.To) ||
            (from <= set.From && set.From <= to));
        }*/

        public ICollection<StationAvailabilityPeriod> GetAvailablePeriodsForStationRange(DateTime from, DateTime to, Station station)
        {
            return dbContext.StationsAvailabilityPeriods
                .Where(sa => sa.Station.Id == station.Id)
                .Where(sa => (sa.From <= from && from <= sa.To) || (from <= sa.From && sa.From <= to))
                .ToList();
        }

        public ICollection<MeteoReport> GetMeteoReportData(DateTime from, DateTime to,
            decimal normTemperature, decimal normPrecipitation)
        {
            // getting all stations that are for the selected period
            var stations = dbContext.Stations.Where(x => x.StationAvailabilityPeriods.Any(period =>
           (period.From <= from && from <= period.To) || (from <= period.From && period.From <= to)));

            List<MeteoReport> reportList = new List<MeteoReport>();

            // get days data for that stations for from/to
            foreach (var station in stations)
            {
                var daysData = dbContext.DaysData
                    .Where(dd => dd.StationId == station.Id)
                    .Where(x => from <= x.Date && x.Date <= to)
                    .ToList();

                decimal averageTemperature = daysData.Average(x => x.Temperature);
                DayWeatherData dayMaxTemperature = daysData.Aggregate((agg, next) => next.Temperature > agg.Temperature ? next : agg);
                DayWeatherData dayMinTemperature = daysData.Aggregate((agg, next) => next.Temperature < agg.Temperature ? next : agg);
                decimal precipitationSum = daysData.Sum(x => x.Precipitation);
                DayWeatherData dayMaxPrecipitation = daysData.Aggregate((agg, next) => next.Precipitation > agg.Precipitation ? next : agg);

                reportList.Add(new MeteoReport(
                    station.Id,
                    averageTemperature,
                    Math.Abs(averageTemperature - normTemperature),
                    dayMaxTemperature.Temperature,
                    dayMaxTemperature.Date,
                    dayMinTemperature.Temperature,
                    dayMinTemperature.Date,
                    precipitationSum,
                    Math.Abs(precipitationSum - normPrecipitation),
                    dayMaxPrecipitation.Precipitation,
                    dayMaxPrecipitation.Date,
                    daysData.Where(x => x.Precipitation >= 1M).Count(),
                    daysData.Where(x => x.Precipitation >= 10M).Count(),
                    daysData.Where(x => x.Wind >= 14M).Count(),
                    daysData.Count(x => x.ThunderCount > 0)
                    ));
            }

            return reportList;
        }

        public MeteoReport GetMeteoReportDataGlobally(DateTime from, DateTime to,
            decimal normTemperature, decimal normPrecipitation)
        {
            ICollection<MeteoReport> reportForStations = GetMeteoReportData(from, to, normTemperature, normPrecipitation);

            var groupedStationWeights = dbContext.StationsWeights
                .Where(x => reportForStations.Any(y => y.StationId == x.StationId))
                .Where(x => (x.From <= from && from <= x.To) || (from <= x.From && x.From <= to))
                .GroupBy(x => x.Station.Name);

            IDictionary<MeteoReport, decimal> reportWeight = new Dictionary<MeteoReport, decimal>();// maps report with weight

            foreach (var meteoReport in reportForStations)
            {
                reportWeight[meteoReport] = 0;
            }

            foreach (var stationWeightGroup in groupedStationWeights)
            {
                StationWeight weightToAdd = stationWeightGroup
                    .OrderByDescending(x => x.AddedOn)
                    .FirstOrDefault();

                var report = reportWeight.Keys.Where(x => x.StationId == weightToAdd.StationId).FirstOrDefault();

                if (report != null)
                {
                    reportWeight[report] = weightToAdd.Weight;
                }
            }

            decimal averageTemperature = reportWeight.Sum(x => x.Key.TemperatureAvg * x.Value);
            MeteoReport reportMaxTemperature = reportWeight.Aggregate((agg, next) => next.Key.TemperatureMax > agg.Key.TemperatureMax ? next : agg).Key;
            MeteoReport reportMinTemperature = reportWeight.Aggregate((agg, next) => next.Key.TemperatureMin < agg.Key.TemperatureMin ? next : agg).Key;
            decimal precipitationSum = reportWeight.Sum(x => x.Key.PrecipitationSum * x.Value);
            MeteoReport reportMaxPrecipitation = reportWeight.Aggregate((agg, next) => next.Key.PrecipitationMax > agg.Key.PrecipitationMax ? next : agg).Key;

            return new MeteoReport(
                    -1,
                    averageTemperature,
                    Math.Abs(averageTemperature - normTemperature),
                    reportMaxTemperature.TemperatureMax,
                    reportMaxTemperature.TemperatureMaxDate,
                    reportMinTemperature.TemperatureMin,
                    reportMinTemperature.TemperatureMinDate,
                    precipitationSum,
                    Math.Abs(precipitationSum - normPrecipitation),
                    reportMaxPrecipitation.PrecipitationMax,
                    reportMaxPrecipitation.PrecipitationMaxDate,
                    reportWeight.Max(x => x.Key.DayCountPrecipitationBiggerThan1),
                    reportWeight.Max(x => x.Key.DayCountPrecipitationBiggerThan10),
                    reportWeight.Max(x => x.Key.DayCountWindBiggerThan14),
                    reportWeight.Max(x => x.Key.DayCountThunders)
                );
        }
    }
}