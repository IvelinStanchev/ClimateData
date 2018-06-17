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

namespace MeteoApp.Controllers
{
    public class HomeController : Controller
    {
        public const string STATION_DOES_NOT_EXIST = "Station does not exist.";

        public IActionResult Index()
        {
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
