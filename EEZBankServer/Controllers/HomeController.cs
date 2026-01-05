using System.Diagnostics;
using System.Security.Claims;
using EEZBankServer.EfCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EEZBankServer.Models.ViewModel;


namespace EEZBankServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated )
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var model = new TransferIndexViewModel
                {
                    Hesaplar = await _context.Hesaplar
                        .Where(x => x.UserId == userId)
                        .ToListAsync(),

                    SonIslemler = await _context.Islemler
                    .Where(x => x.GonderenBankaHesabi.UserId == userId || x.AliciBankaHesabi != null && x.AliciBankaHesabi.UserId == userId)
                    .OrderByDescending(x => x.IslemTarihi)
                    .Take(5)
                    .Include(x => x.AliciBankaHesabi)
                    .ThenInclude(x => x.User)
                    .Include(x => x.GonderenBankaHesabi)
                    .ToListAsync(),
                };

                return View(model);
            }
            else
            {
                return View();
            }
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
