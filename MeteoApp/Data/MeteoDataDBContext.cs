using MeteoApp.Data.Models;
using MeteoApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeteoApp.Data
{
    public class MeteoDataDBContext : DbContext
    {
        public MeteoDataDBContext()
        {
        }

        public MeteoDataDBContext(DbContextOptions<MeteoDataDBContext> options) : base(options)
        {

        }

        public DbSet<Station> Stations { get; set; }
        public DbSet<DayWeatherData> DaysData { get; set; }
        public DbSet<StationAvailabilityPeriod> StationsAvailabilityPeriods { get; set; }
        public DbSet<StationWeight> StationsWeights { get; set; }

    }
}
