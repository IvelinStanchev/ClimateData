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
                new Station{ID = 1, Name="София"},
                new Station{ID = 2, Name="Видин"},
                new Station{ID = 3, Name="Монтана"},
                new Station{ID = 4, Name="Враца"},
                new Station{ID = 5, Name="Плевен"},
                new Station{ID = 6, Name="В. Търново"},
                new Station{ID = 7, Name="Русе"},
                new Station{ID = 8, Name="Разград"},
                new Station{ID = 9, Name="Добрич"},
                new Station{ID = 10, Name="Варна"},
                new Station{ID = 11, Name="Бургас"},
                new Station{ID = 12, Name="Сливен"},
                new Station{ID = 13, Name="Кърджали"},
                new Station{ID = 14, Name="Пловдив"},
                new Station{ID = 15, Name="Благоевград"},
                new Station{ID = 16, Name="Сандански"},
                new Station{ID = 17, Name="Кюстендил"}

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


            var daysData = new DayData[]
            {
                CreateDayData(1, new DateTime(2018, 6, 14), 30, 0.2M, 2.4M, 0),
                CreateDayData(1, new DateTime(2018, 6, 13), 28, 2.0M, 4.8M, 0),
                CreateDayData(1, new DateTime(2018, 6, 12), 29, 0.0M, 4.0M, 0),
            };
            foreach (DayData eachData in daysData)
            {
                context.DaysData.Add(eachData);
            }          
            context.SaveChanges();

        }
       
        private static DayData CreateDayData(int stationID, DateTime date, decimal temperature, decimal precipitation, decimal wind, int numberOfThunders)
        {
            return new DayData
            {
                StationID = stationID,
                Date = date,
                Temperature = temperature,
                Precipitation = precipitation,
                Wind = wind,
                NumberOfThunders = numberOfThunders
            };
        }

    }
}

