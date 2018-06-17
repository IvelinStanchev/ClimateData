using MeteoApp.Data.Models;
using MeteoApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("MeteoDataDBContext");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public DbSet<Station> Stations { get; set; }
        public DbSet<DayWeatherData> DaysData { get; set; }
        public DbSet<StationAvailabilityPeriod> StationsAvailabilityPeriods { get; set; }
        public DbSet<StationWeight> StationsWeights { get; set; }

    }
}
