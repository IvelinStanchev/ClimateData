using MeteoApp.Data.Models;
using MeteoApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeteoApp.Data
{
    public class DBMeteoInitializer
    {
        public static void Initialize(MeteoDataDBContext context)
        {
            context.Database.EnsureCreated();
            if (context.Stations.Any())
            {
                return; // DB has been seeded
            }
            var stations = new Station[]
            {
                new Station{Id = 1, Name="София"},
                new Station{Id = 2, Name="Видин"},
                new Station{Id = 3, Name="Монтана"},
                new Station{Id = 4, Name="Враца"},
                new Station{Id = 5, Name="Плевен"},
                new Station{Id = 6, Name="В. Търново"},
                new Station{Id = 7, Name="Русе"},
                new Station{Id = 8, Name="Разград"},
                new Station{Id = 9, Name="Добрич"},
                new Station{Id = 10, Name="Варна"},
                new Station{Id = 11, Name="Бургас"},
                new Station{Id = 12, Name="Сливен"},
                new Station{Id = 13, Name="Кърджали"},
                new Station{Id = 14, Name="Пловдив"},
                new Station{Id = 15, Name="Благоевград"},
                new Station{Id = 16, Name="Сандански"},
                new Station{Id = 17, Name="Кюстендил"}

            };
            foreach (Station st in stations)
            {
                context.Stations.Add(st);
            }
            context.Database.OpenConnection();
            try
            {
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Stations ON");
                context.SaveChanges();
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Stations OFF");
            }
            finally
            {
                context.Database.CloseConnection();
            }


            var daysData = new DayWeatherData[]
            {
                CreateDayData(1, new DateTime(2018, 6, 14), 30, 0.2M, 2.4M, 0),
                CreateDayData(1, new DateTime(2018, 6, 13), 28, 2.0M, 4.8M, 0),
                CreateDayData(1, new DateTime(2018, 6, 12), 29, 0.0M, 4.0M, 0),
            };
            foreach (DayWeatherData eachData in daysData)
            {
                context.DaysData.Add(eachData);
            }          
            context.SaveChanges();

        }
       
        private static DayWeatherData CreateDayData(int stationID, DateTime date, decimal temperature, decimal precipitation, decimal wind, int numberOfThunders)
        {
            return new DayWeatherData
            {
                StationId = stationID,
                Date = date,
                Temperature = temperature,
                Precipitation = precipitation,
                Wind = wind,
                ThunderCount = numberOfThunders
            };
        }

    }
}

