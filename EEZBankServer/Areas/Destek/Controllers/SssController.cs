using Microsoft.AspNetCore.Mvc;

namespace EEZBankServer.Areas.Destek.Controllers
{
    [Area("Destek")]
    public class SssController : Controller
    {
        [Route("Destek/SSS")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
