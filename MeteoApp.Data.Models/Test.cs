using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoApp.Data.Models
{
    /// <summary>
    /// Example class containing possible queries to the different data sets in order to get the data from DB.
    /// Still, needs to be divided in separated classes.
    /// </summary>
    public class Test
    {
        // These should be db data sets
        private ICollection<StationWeight> stationWeights;
        private ICollection<DayWeatherData> weatherData;
        private ICollection<Station> stations;
        private ICollection<StationAvailabilityPeriod> availabilityPeriods;

        public Test()
        {
            // These should be IQueryable, as db evals need to happen only when .ToList or similar gets called on them
            // Otherwise R.I.P. performace
            
            var stationWeights = new List<StationWeight>();
            var weatherData = new List<DayWeatherData>();
            var stations = new List<Station>();
            var availabilityPeriods = new List<StationAvailabilityPeriod>();
        }

        public ICollection<StationAvailabilityPeriod> GetAvailablePeriodsForRange(DateTime start, DateTime end)
        {
            return this.availabilityPeriods
                .Where(x => x.From >= start && x.To <= end)
                .ToList();
        }

        public ICollection<StationWeight> GetStationWeightsForPeriod(DateTime start, DateTime end, ICollection<StationAvailabilityPeriod> availPeriods)
        {
            var result = new List<StationWeight>();

            var groupedStationWeights = this.stationWeights
                .Where(x => availabilityPeriods.Any(y => y.Station.Name == x.Station.Name))
                .Where(x => x.From >= start && x.To <= end)
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
            return this.weatherData
                .Where(x => x.Station.Id == stationWeight.Station.Id)
                .Where(x => x.Date >= stationWeight.From && x.Date <= stationWeight.To)
                .ToList();
        }
    }
}
