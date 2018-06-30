using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MeteoApp.Models;
using Microsoft.AspNetCore.Authorization;
using MeteoApp.Models.HomeViewModels;
using MeteoApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using MeteoApp.Services;
using ChartJSCore.Models;
using MeteoApp.Models.AdminViewModels;
using MeteoApp.Data.Models;
using MeteoApp.Common;

namespace MeteoApp.Controllers
{
    public class HomeController : Controller
    {
        public const string STATION_DOES_NOT_EXIST = "Station does not exist.";

        [HttpGet]
        public IActionResult Index()
        {
            var meteoData = new MeteoDataDBContext();

            var stationNames = meteoData.Stations.Select(x => 
                    new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Name
                    }                
            );

            SelectListItem[] weatherDataTypes =
            {
                new SelectListItem { Text = "Мин. температура", Value = "0" },
                new SelectListItem { Text = "Макс. температура", Value = "1" },
                new SelectListItem { Text = "Валежи", Value = "2" },
                new SelectListItem { Text = "Гръмотевици", Value = "3" }
            };


            var viewModel = new GraphicalStationDataViewModel
            {
                From = DateTime.Now,
                To = DateTime.Now,
                StationNames = stationNames,
                WeatherDataTypeNames = weatherDataTypes
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Index(GraphicalStationDataViewModel stationDataQuery)
        {
            var meteoService = new MainService();

            ViewData["chart"] = meteoService.GetChartData(
                stationDataQuery.WeatherDataTypeName,
                stationDataQuery.StationName,
                DateTime.Now,
                DateTime.Now
            );

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult GetStationsReportForPeriod(DatePeriodModel datePeriod)
        {
            ICollection<MeteoReport> reports = new MainService().GetMeteoReportData(
                datePeriod.StartDate,
                datePeriod.EndDate,
                Constants.MONTHLY_TEMPERATURE_NORM,
                Constants.MONTHLY_PRECIPITATION_NORM);

            return PartialView("ReportDataView", reports);
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetStationsReportForPeriod()
        {
            return View("GetStationsReportForPeriodView");
        }

        [HttpPost]
        [Authorize]
        public IActionResult GetGlobalReport(DatePeriodModel datePeriod)
        {
            MeteoReport report = new MainService().GetMeteoReportDataGlobally(
                datePeriod.StartDate,
                datePeriod.EndDate,
                Constants.MONTHLY_TEMPERATURE_NORM,
                Constants.MONTHLY_PRECIPITATION_NORM);

            List<MeteoReport> reports = new List<MeteoReport>();
            reports.Add(report);

            return PartialView("ReportDataView", reports);
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetGlobalReport()
        {
            return View("GetGlobalReportView");
        }

        [Authorize]
        public IActionResult About()
        {         
            ViewData["Message"] = $"Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddStationData()
        {
            var meteoData = new MeteoDataDBContext();

            var stationNames = meteoData
                .Stations
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name
                });

            var viewModel = new NewStationDataViewModel
            {
                Date = DateTime.Now,
                StationNames = stationNames
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddStationData(NewStationDataViewModel stationData)
        {
            var meteoData = new MeteoDataDBContext();

            var station = meteoData
                .Stations
                .FirstOrDefault(x => x.Name.ToLower() == stationData.StationName.ToLower());

            if (station == null)
            {
                ModelState.AddModelError(string.Empty, STATION_DOES_NOT_EXIST);
                stationData.StationNames = meteoData
                    .Stations
                    .Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Name
                    });

                return View(stationData);
            }

            // TODO: validate those values
            var stationDataToAdd = new DayWeatherData
            {
                Station = station,
                Date = stationData.Date,
                Precipitation = stationData.Precipitation,
                Temperature = stationData.Temperature,
                ThunderCount = stationData.ThunderCount,
                Wind = stationData.Wind                
            };

            meteoData.DaysData.Add(stationDataToAdd);
            meteoData.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
