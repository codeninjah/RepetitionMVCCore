using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Repetition.Interfaces;
using Repetition.Models;
using Repetition.Resources;

namespace Repetition.Controllers
{
    public class HomeController : Controller
    {
		private ITimeProvider timeProvider;
		private readonly IStringLocalizer _Localizer;
		public HomeController(ITimeProvider _timeProvider, IStringLocalizerFactory factory)
		{
			timeProvider = _timeProvider;
			_Localizer = factory.Create(typeof(SharedResources));
		}

		public IActionResult Index()
		{
			//följande rads viewbag åkallas i Views/Home/Index
			ViewBag.Time = timeProvider.Now.ToString();
			return View();
		}

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

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
    }
}
