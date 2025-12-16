using Microsoft.AspNetCore.Mvc;

namespace EEZBankServer.Controllers
{
    public class LocationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
