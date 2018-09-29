using System.Net;
using System.Text;
using FluentFTP;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NurbolFtp.Models;

namespace NurbolFtp.Controllers
{
    public class HomeController : Controller
    {
        public static object locker = new object();
        private readonly IHostingEnvironment _appEnvironment;
        private readonly IConfiguration _configuration;

        public HomeController(IHostingEnvironment appEnvironment, IConfiguration configuration)
        {
            _appEnvironment = appEnvironment;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new WpLink());
        }

        [HttpPost]
        public IActionResult Index(WpLink model)
        {
            lock (locker)
            {
                if (!ModelState.IsValid)
                    return View(model);
                model.Phone = model.Phone
                    .Replace(" ", string.Empty)
                    .Replace("(", string.Empty)
                    .Replace(")", string.Empty)
                    .Replace("-", string.Empty);

                string password = _configuration["Ftp:Pass"];
                if (model.Password != password)
                {
                    ModelState.AddModelError(string.Empty, "Неправильный пароль");
                    return View(model);
                }

                WebClient client = new WebClient();
                string link = "http://whatspp.kz/" + model.Name;
                string url = "";
                if (model.Theme == 0)
                    url = Request.IsHttps ? "https://" : "http://" + Request.Host.Value + "/home/whatsapp?name=" + model.Name + "&phone=" + model.Phone + "&picture=" + model.File.FileName + "&text=" + model.Text + "&theme=" + model.Theme;
                else
                    url = Request.IsHttps ? "https://" : "http://" + Request.Host.Value + "/home/whatsapp?name=" + model.Name + "&phone=" + model.Phone + "&text=" + model.Text + "&theme=" + model.Theme;

                string html = client.DownloadString(url);

                using (var ftpClient = new FtpClient(_configuration["Ftp:Host"]))
                {
                    ftpClient.Credentials = new NetworkCredential(_configuration["Ftp:Username"], _configuration["Ftp:Password"]);
                    ftpClient.Upload(Encoding.UTF8.GetBytes(html), $"/httpdocs/{model.Name}/index.html", FtpExists.Overwrite, true);

                    if (model.File != null)
                    {
                        var pictureBuffer = new byte[model.File.Length];
                        using (var stream = model.File.OpenReadStream())
                        {
                            stream.Read(pictureBuffer, 0, (int)model.File.Length - 1);
                        }

                        ftpClient.Upload(pictureBuffer, $"/httpdocs/{model.Name}/{model.File.FileName}", FtpExists.Overwrite, true);
                    }
                }

                TempData["wpLink"] = link;
                return RedirectToActionPermanent(nameof(Index));
            }
        }
        
        public IActionResult Whatsapp(WpLink model)
        {
            if (model.Theme == 0)
                return View("Picture", model);
            else
                return View("Text", model);
        }
    }
}
