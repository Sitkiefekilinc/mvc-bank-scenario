using Microsoft.AspNetCore.Mvc;

namespace EEZBankServer.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
