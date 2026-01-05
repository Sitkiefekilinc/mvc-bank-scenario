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
                IslemYonu islemYonu = 0; 


                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var hesaplar = await _context.Hesaplar
                    .Where(x=>x.UserId==userId).ToListAsync();

                var islemler = new List<IslemlerViewModel>();

                foreach (var item in hesaplar)
                {
                         islemler = await _context.Islemler
                        .Where(x => x.GonderenHesapId == item.HesapId)
                        .Select(x => new IslemlerViewModel
                        {
                            Yon = IslemYonu.Giden,
                            Tutar = x.IslemMiktari,
                            KullaniciAdi = x.GonderenBankaHesabi.User.UserName,
                            KarsiKullaniciAdi = x.AliciBankaHesabi.User.UserName,
                            HesapAdi = x.GonderenBankaHesabi.AccountName,
                            Islemtarihi = x.IslemTarihi
                        }).ToListAsync();
                    var gelenIslem = await _context.Islemler
                        .Where(x => x.AliciHesapId == item.HesapId)
                        .Select(x => new IslemlerViewModel
                        {
                            Yon = IslemYonu.Gelen,
                            Tutar = x.IslemMiktari,
                            KullaniciAdi = x.AliciBankaHesabi.User.UserName,
                            KarsiKullaniciAdi = x.GonderenBankaHesabi.User.UserName,
                            HesapAdi = x.AliciBankaHesabi.AccountName,
                            Islemtarihi = x.IslemTarihi
                        }).ToListAsync();
                    islemler.AddRange(gelenIslem);
                }


                var model = new HesapOzetiViewModel
                {
                    Hesaplar = hesaplar,
                    Islemler = islemler.OrderByDescending(x => x.Islemtarihi).Take(10).ToList(),
                    ToplamBakiye = hesaplar.Sum(x => x.Balance)
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
