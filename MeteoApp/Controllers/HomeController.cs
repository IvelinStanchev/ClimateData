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
using MeteoApp.Data.Models;
using ChartJSCore.Models;

namespace MeteoApp.Controllers
{
    public class HomeController : Controller
    {
        public const string STATION_DOES_NOT_EXIST = "Station does not exist.";

        public IActionResult Index()
        {
            Chart chart = new Chart();

            chart.Type = "line";

            ChartJSCore.Models.Data data = new ChartJSCore.Models.Data();
            data.Labels = new List<string>() { "January", "February", "March", "April", "May", "June", "July" };

            LineDataset dataset = new LineDataset()
            {
                Label = "My First dataset",
                Data = new List<double>() { 65, 59, 80, 81, 56, 55, 40 },
                Fill = "false",
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

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset);

            chart.Data = data;

            ViewData["chart"] = chart;

            return View();
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
