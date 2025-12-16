using System.Diagnostics;
using EEZBankServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace EEZBankServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Hizmetler()
        {
            return View();
        }
        public IActionResult Hakkýmýzda()
        {
            return View();
        }
        public IActionResult GirisYap()
        {
            return View();
        }
        public IActionResult MusteriOl()
        {
            return View();
        }
        public IActionResult BireyselMusteriOl()
        {
            return View();
        }
        public IActionResult KurumsalMusteriOl()
        {
            return View();
        }
        public IActionResult TicariMusteriOl()
        {
            return View();
        }
        public IActionResult Login(UserAccountInfos userAccountInfos)
        {
            
            return View(userAccountInfos);
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
