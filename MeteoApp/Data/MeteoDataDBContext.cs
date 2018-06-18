using MeteoApp.Data.Models;
using MeteoApp.Data.Models.Base;
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

        public DbSet<Station> Stations { get; set; }
        public DbSet<DayWeatherData> DaysData { get; set; }
        public DbSet<StationAvailabilityPeriod> StationsAvailabilityPeriods { get; set; }
        public DbSet<StationWeight> StationsWeights { get; set; }

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

        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges();
        }

        private void ApplyAuditInfoRules()
        {
            foreach (var entry in
                this.ChangeTracker.Entries()
                    .Where(
                        e =>
                        e.Entity is DbModel && ((e.State == EntityState.Added) || (e.State == EntityState.Modified))))
            {
                var entity = (DbModel)entry.Entity;
                if (entry.State == EntityState.Added)
                {
                    entity.AddedOn = DateTime.Now;
                }
                else
                {
                    entity.ChangedOn = DateTime.Now;
                }
            }
        }

    }
}
