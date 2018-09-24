using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NurbolFtp.Models;

namespace NurbolFtp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Save(WpLink model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            WebClient client = new WebClient();
            string url = Request.IsHttps ? "https://" : "http://" +  Request.Host.Value + "/home/whatsapp?name=" + model.Name + "&phone=" + model.Phone + "&picture=" + model.Picture + "&text=" + model.Text + "&theme=" + model.Theme;
            string html = client.DownloadString(url);
            return Ok(new { html });
        }

        public IActionResult Whatsapp(WpLink model)
        {
            if (model.Theme == 0)
                return View("Picture", model);
            else
                return View("Text", model);
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
