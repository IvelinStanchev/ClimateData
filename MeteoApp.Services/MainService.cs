using MeteoApp.Data;
using MeteoApp.Data.Models;
using MeteoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeteoApp.Services
{
    class MainService
    {
        MeteoDataDBContext dbContext = new MeteoDataDBContext();
        
        private bool IsInRange(DateTime from, DateTime to, DateTime periodFrom, DateTime periodTo)
        {
            return (periodFrom <= from && from <= periodTo) ||
                (from <= periodFrom && periodFrom <= to);
        }

        public List<Station> GetStationsForPeriod(DateTime from, DateTime to)
        {
            var stations = dbContext.StationsAvailabilityPeriods
                .Where(sa => IsInRange(from, to, sa.From, sa.To))
                .GroupBy(pair => pair.Station.Name)
                .Join(dbContext.Stations,
                sa => sa.Key,
                station => station.Name,
                (sa, station) => new { StationAvailability = sa, Station = station })
                .Select(group => group.Station)
                .ToList();

            return stations;
        }

        public ICollection<StationAvailabilityPeriod> GetAvailablePeriodsForStationRange(DateTime from, DateTime to, Station station)
        {
            return dbContext.StationsAvailabilityPeriods
                .Where(sa => sa.Station.Id == station.Id)
                .Where(sa => IsInRange(from, to, sa.From, sa.To))
                .ToList();
        }

        public ICollection<StationWeight> GetStationWeightsForPeriod(DateTime from, DateTime to, ICollection<StationAvailabilityPeriod> availPeriods)
        {
            var result = new List<StationWeight>();

            var groupedStationWeights = dbContext.StationsWeights
                .Where(x => availPeriods.Any(y => y.Station.Name == x.Station.Name))
                .Where(x => IsInRange(from, to, x.From, x.To))
                .GroupBy(x => x.Station.Name);

            foreach (var stationWeightGroup in groupedStationWeights)
            {
                StationWeight weightToAdd = stationWeightGroup
                    .OrderByDescending(x => x.AddedOn)
                    .FirstOrDefault();

                if (weightToAdd != null)
                {
                    result.Add(weightToAdd);
                }
            }

            return result;
        }

        public ICollection<DayWeatherData> GetWeatherDataFromWeight(StationWeight stationWeight)
        {
            return dbContext.DaysData
                .Where(x => x.Station.Id == stationWeight.Station.Id)
                .Where(x => stationWeight.From <= x.Date && x.Date <= stationWeight.To)
                .ToList();
        }
        
        
        public ICollection<MeteoReport> GetMeteoReportData(DateTime reportDateFrom, DateTime reportDateTo, 
            decimal normTemperature, decimal normPrecipitation)
        {
            // getting all stations that are for the selected period
            var periodsForEachStation =
                dbContext.StationsAvailabilityPeriods
                .Where(sa => IsInRange(reportDateFrom, reportDateTo, sa.From, sa.To))
                .GroupBy(sa => sa.Station.Name);

            List<MeteoReport> reportList = new List<MeteoReport>();

            // get days data for that stations for from/to
            foreach (var periods in periodsForEachStation)
            {
                int stationId = periods.FirstOrDefault().StationId;
                var daysData = dbContext.DaysData
                    .Where(dd => dd.StationId == stationId)
                    .Where(x => reportDateFrom <= x.Date && x.Date <= reportDateTo)
                    .ToList();

                decimal averageTemperature = daysData.Average(x => x.Temperature);
                DayWeatherData dayMaxTemperature = daysData.Aggregate((agg, next) => next.Temperature > agg.Temperature ? next : agg);
                DayWeatherData dayMinTemperature = daysData.Aggregate((agg, next) => next.Temperature < agg.Temperature ? next : agg);
                decimal precipitationSum = daysData.Sum(x => x.Precipitation);
                DayWeatherData dayMaxPrecipitation = daysData.Aggregate((agg, next) => next.Precipitation > agg.Precipitation ? next : agg);

                reportList.Add(new MeteoReport(
                    stationId,
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

        public MeteoReport GetMeteoReportDataGlobally(DateTime reportDateFrom, DateTime reportDateTo,
            decimal normTemperature, decimal normPrecipitation)
        {
            ICollection<MeteoReport> reportForStations = GetMeteoReportData(reportDateFrom, reportDateTo, normTemperature, normPrecipitation);

            var groupedStationWeights = dbContext.StationsWeights
                .Where(x => reportForStations.Any(y => y.StationId == x.StationId))
                .Where(x => IsInRange(reportDateFrom, reportDateTo, x.From, x.To))
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
