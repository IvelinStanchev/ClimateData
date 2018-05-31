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
                new Station{ID = 1, Name="София",Weight = 5.92M},
                new Station{ID = 2, Name="Видин",Weight = 5.88M},
                new Station{ID = 3, Name="Монтана",Weight = 5.88M},
                new Station{ID = 4, Name="Враца",Weight = 5.88M},
                new Station{ID = 5, Name="Плевен",Weight = 5.88M},
                new Station{ID = 6, Name="В. Търново",Weight = 5.88M},
                new Station{ID = 7, Name="Русе",Weight = 5.88M},
                new Station{ID = 8, Name="Разград",Weight = 5.88M},
                new Station{ID = 9, Name="Добрич",Weight = 5.88M},
                new Station{ID = 10, Name="Варна",Weight = 5.88M},
                new Station{ID = 11, Name="Бургас",Weight = 5.88M},
                new Station{ID = 12, Name="Сливен",Weight = 5.88M},
                new Station{ID = 13, Name="Кърджали",Weight = 5.88M},
                new Station{ID = 14, Name="Пловдив",Weight = 5.88M},
                new Station{ID = 15, Name="Благоевград",Weight = 5.88M},
                new Station{ID = 16, Name="Сандански",Weight = 5.88M},
                new Station{ID = 17, Name="Кюстендил",Weight = 5.88M}

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


            var statdata = new StationData[]
            {
                CreateStationData(1,2017,3,9,4.2M,23.1M,24,-2.0M,28,62,162,13,13,8,2,0,0),
                CreateStationData(2,2017,3,9.5M,3.8M,25.6M,30,-3.4M,4,38,85,19,11,6,1,4,1),
                CreateStationData(3,2017,3,10.1M,4.4M,24.1M,30,-0.6M,4,52,128,24,11,7,2,6,0),
                CreateStationData(4,2017,3,10.6M,4.9M,22.8M,30,0.0M,27,62,106,27,11,8,2,3,0),
                CreateStationData(5,2017,3,10.3M,4.1M,22.6M,22,-0.4M,27,62,162,20,14,8,2,3,0),
                CreateStationData(6,2017,3,10.0M,3.9M,24.2M,21,-1.2M,28,59,114,22,14,6,3,2,0),
                CreateStationData(7,2017,3,10.4M,3.8M,25.1M,24,0.2M,27,59,127,22,14,7,3,12,0),
                CreateStationData(8,2017,3,8.2M,3.3M,23.2M,24,-0.1M,14,59,165,26,14,7,2,4,0),
                CreateStationData(9,2017,3,7.3M,3.2M,23.3M,29,-0.9M,4,52,163,24,14,7,2,3,0),
                CreateStationData(10,2017,3,8.0M,2.6M,20.0M,30,1.0M,18,68,201,38,14,5,3,2,0),
                CreateStationData(11,2017,3,8.4M,2.3M,20.8M,30,0.8M,18,44,113,19,14,7,2,5,0),
                CreateStationData(12,2017,3,9.1M,2.9M,23.4M,30,-0.4M,18,37,111,17,8,5,1,8,1),
                CreateStationData(13,2017,3,9.5M,2.9M,23.3M,21,-1.4M,28,66,125,16,27,8,4,6,2),
                CreateStationData(14,2017,3,9.8M,3.0M,24.0M,20,-2.0M,18,52,129,12,9,7,2,0,0),
                CreateStationData(15,2017,3,10.1M,3.1M,26.2M,21,-1.6M,28,30,73,9,2,5,0,2,1),
                CreateStationData(16,2017,3,11.9M,3.5M,25.9M,21,-1.2M,28,33,87,8,8,7,0,1,3),
                CreateStationData(17,2017,3,9.1M,3.1M,25.6M,24,-4.2M,18,15,37,4,13,6,0,0,1),
            };
            foreach (StationData sd in statdata)
            {
                context.StationDatas.Add(sd);
            }          
            context.SaveChanges();

        }
       
        private static StationData CreateStationData(int StationID, int Year, int Month, decimal TmpAvg, decimal TmpDelta,
              decimal TmpMax, int TmpMaxDate, decimal TmpMin, int TmpMinDate, decimal PrecipSum, decimal PrecipQn, decimal PrecipMax, int PrecipMaxDate, int NumDaysPrecipAboveOne,
        int NumDaysPrecipAboveTen, int NumDaysWindAboveFourteen, int NumDaysTunder)
        {
            return new StationData
            {
                StationID = StationID,
                Year = Year,
                Month = Month,
                TmpAvg = TmpAvg,
                TmpDelta = TmpDelta,
                TmpMax = TmpMax,
                TmpMaxDate = TmpMaxDate,
                TmpMin = TmpMin,
                TmpMinDate = TmpMinDate,
                PrecipSum = PrecipSum,
                PrecipQn = PrecipQn,
                PrecipMax = PrecipMax,
                PrecipMaxDate = PrecipMaxDate,
                NumDaysPrecipAboveOne = NumDaysPrecipAboveOne,
                NumDaysPrecipAboveTen = NumDaysPrecipAboveTen,
                NumDaysWindAboveFourteen = NumDaysWindAboveFourteen,
                NumDaysTunder = NumDaysTunder
            };
        }

    }
}

