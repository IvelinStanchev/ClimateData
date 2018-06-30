using ChartJSCore.Models;
using MeteoApp.Data;
using MeteoApp.Data.Models;
using MeteoApp.Data.Models.Base;
using MeteoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeteoApp.Services
{
    public class MainService
    {
        MeteoDataDBContext dbContext = new MeteoDataDBContext();

        private LineDataset GetChartDataset(string labelId, List<double> data)
        {
            string datasetLabel = "";

            switch (labelId)
            {
                case "0":
                    datasetLabel = "Мин. температура";
                    break;
                case "1":
                    datasetLabel = "Макс. температура";
                    break;
                case "2":
                    datasetLabel = "Валежи";
                    break;
                case "3":
                    datasetLabel = "Гръмотевици";
                    break;
                default:
                    break;
            }

            return new LineDataset()
            {
                Label = datasetLabel,
                Data = data,
                Fill = "true",
                LineTension = 0.1,
                BackgroundColor = "rgba(75, 192, 192, 0.4)",
                BorderColor = "rgba(75,192,192,1)",
                BorderCapStyle = "butt",
                BorderDash = new List<int> { },
                BorderDashOffset = 0.0,
                BorderJoinStyle = "miter",
                PointBorderColor = new List<string>() { "rgba(75,192,192,1)" },
                PointBackgroundColor = new List<string>() { "#fff" },
                PointBorderWidth = new List<int> { 1 },
                PointHoverRadius = new List<int> { 5 },
                PointHoverBackgroundColor = new List<string>() { "rgba(75,192,192,1)" },
                PointHoverBorderColor = new List<string>() { "rgba(220,220,220,1)" },
                PointHoverBorderWidth = new List<int> { 2 },
                PointRadius = new List<int> { 1 },
                PointHitRadius = new List<int> { 10 },
                SpanGaps = false
            };
        }

        private List<DateTime> GetSplitIntervalsStartDates(DateTime from, DateTime to)
        {
            var result = new List<DateTime>();
            int parts = 5;

            var timespan = to.Subtract(from);
            int daysPerPart = timespan.Days / parts;

            for (int i = 0; i < parts; i++)
            {
                result.Add(from.AddDays(daysPerPart * i));
            }

            return result;
        }

        private List<double> GetSplitData(string optionType, string stationName, DateTime from, DateTime to)
        {
            var result = new List<double>();
            var dates = this.GetSplitIntervalsStartDates(from, to);

            for (int i = 0; i < dates.Count - 1; i++)
            {
                var reportResult = this.GetMeteoReportData(dates[i], dates[i + 1], 0, 5)
                    .FirstOrDefault(x => x.StationName.ToLower() == stationName.ToLower());

                if (reportResult == null)
                {
                    result.Add(5 * i);
                    continue;
                }

                switch (optionType)
                {
                    case "0":
                        result.Add((double)reportResult.TemperatureMin);
                        break;
                    case "1":
                        result.Add((double)reportResult.TemperatureMax);
                        break;
                    case "2":
                        result.Add((double)reportResult.PrecipitationSum);
                        break;
                    case "3":
                        result.Add((double)reportResult.DayCountThunders);
                        break;
                    default:
                        result.Add((double)reportResult.TemperatureMax);
                        break;
                }
            }

            return result;
        }

        public Chart GetChartData(string optionType, string stationName, DateTime from, DateTime to)
        {
            var chart = new Chart();
            chart.Type = "line";
            var splitData = this.GetSplitData(optionType, stationName, from, to);

            ChartJSCore.Models.Data data = new ChartJSCore.Models.Data();
            data.Labels = splitData.Select(x => "").ToList();

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(this.GetChartDataset(optionType, splitData));
            chart.Data = data;

            return chart;
        }



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
                    station.Name,
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

            IDictionary<MeteoReport, decimal> reportWeightTemp = new Dictionary<MeteoReport, decimal>();// maps report with weight

            foreach (var meteoReport in reportForStations)
            {
                reportWeightTemp.Add(meteoReport, 0);
                //reportWeightTemp[meteoReport] = 0;
            }

            decimal reportWeightsSum = 0;

            foreach (var stationWeightGroup in groupedStationWeights)
            {
                StationWeight weightToAdd = stationWeightGroup
                    .OrderByDescending(x => x.AddedOn)
                    .FirstOrDefault();

                var report = reportWeightTemp.Keys.Where(x => x.StationId == weightToAdd.StationId).FirstOrDefault();

                if (report != null)
                {
                    reportWeightTemp[report] = weightToAdd.Weight;
                    reportWeightsSum += weightToAdd.Weight;
                }
            }

            IDictionary<MeteoReport, decimal> reportWeight = new Dictionary<MeteoReport, decimal>();

            foreach (var pair in reportWeightTemp)
            {
                reportWeight.Add(pair.Key, pair.Value / reportWeightsSum);
                //reportWeight[pair.Key] = pair.Value / reportWeightsSum;
            }

            decimal averageTemperature = reportWeight.Sum(x => x.Key.TemperatureAvg * x.Value);

            //MeteoReport reportMaxTemperature = reportWeight.Aggregate((agg, next) => next.Key.TemperatureMax > agg.Key.TemperatureMax ? next : agg).Key;
            //MeteoReport reportMinTemperature = reportWeight.Aggregate((agg, next) => next.Key.TemperatureMin < agg.Key.TemperatureMin ? next : agg).Key);

            decimal reportMaxTemperature = decimal.MinValue;
            DateTime reportMaxTemperatureDate = DateTime.Now;
            decimal reportMinTemperature = decimal.MaxValue;
            DateTime reportMinTemperatureDate = DateTime.Now;
            decimal reportMaxPrecipitation = decimal.MinValue;
            DateTime reportMaxPrecipitationDate = DateTime.Now;

            foreach (var item in reportWeight)
            {
                var actualItem = item.Key;

                if (actualItem.TemperatureMax > reportMaxTemperature)
                {
                    reportMaxTemperature = actualItem.TemperatureMax;
                    reportMaxTemperatureDate = actualItem.TemperatureMaxDate;
                }

                if (actualItem.TemperatureMin < reportMinTemperature)
                {
                    reportMinTemperature = actualItem.TemperatureMin;
                    reportMinTemperatureDate = actualItem.TemperatureMinDate;
                }

                if (actualItem.PrecipitationMax > reportMaxPrecipitation)
                {
                    reportMaxPrecipitation = actualItem.PrecipitationMax;
                    reportMaxPrecipitationDate = actualItem.PrecipitationMaxDate;
                }
            }

            decimal precipitationSum = reportWeight.Sum(x => x.Key.PrecipitationSum * x.Value);

            return new MeteoReport(
                    -1,
                    "Global",
                    averageTemperature,
                    Math.Abs(averageTemperature - normTemperature),
                    reportMaxTemperature,
                    reportMaxTemperatureDate,
                    reportMinTemperature,
                    reportMinTemperatureDate,
                    precipitationSum,
                    Math.Abs(precipitationSum - normPrecipitation),
                    reportMaxPrecipitation,
                    reportMaxPrecipitationDate,
                    reportWeight.Count > 0
                        ? reportWeight.Max(x => x.Key.DayCountPrecipitationBiggerThan1)
                        : 0,
                    reportWeight.Count > 0
                        ? reportWeight.Max(x => x.Key.DayCountPrecipitationBiggerThan10)
                        : 0,
                    reportWeight.Count > 0
                        ? reportWeight.Max(x => x.Key.DayCountWindBiggerThan14)
                        : 0,
                    reportWeight.Count > 0
                        ? reportWeight.Max(x => x.Key.DayCountThunders)
                        : 0
                );
        }
    }
}