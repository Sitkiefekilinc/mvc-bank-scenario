using Microsoft.AspNetCore.Mvc;

namespace EEZBankServer.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GirisYapIslem()
        {
            return View();
        }
        public IActionResult TicaretSicilKayit()
        {
            return View();
        }
        public IActionResult BireyselKayit()
        {
            return View();
        }
        public IActionResult KurumsalKayit()
        {
            return View();
        }

    }
}
