using EEZBankServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EEZBankServer.EfCore;
using Microsoft.AspNetCore.Authorization;

namespace EEZBankServer.Controllers
{
    [Authorize]
    public class TransferController : Controller
    {
        private readonly UserDbContext _context;

        public TransferController(UserDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new TransferIndexViewModel
            {
                Hesaplar = await _context.Hesaplar.Where(x => x.UserId == userId).ToListAsync(),

                SonIslemler = await _context.Islemler
                    .Where(x => x.GonderenBankaHesabi.UserId == userId &&
                               (x.Tur == IslemTuru.EFT || x.Tur == IslemTuru.Odeme))
                    .OrderByDescending(x => x.IslemTarihi)
                    .Take(5)
                    .Include(x => x.AliciBankaHesabi) 
                    .ToListAsync()
            };

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Eft()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Kullanıcının hesaplarını çekip ViewModel'e koyuyoruz
            var model = new TransferEftViewModel
            {
                KullaniciHesaplari = await _context.Hesaplar
                    .Where(x => x.UserId == userId)
                    .ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eft(TransferEftViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Json(new { success = false, message = errors });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var gonderenHesap = await _context.Hesaplar
                    .FirstOrDefaultAsync(x => x.HesapId== model.GonderenHesapId && x.UserId == userId);

                if (gonderenHesap == null)
                    return Json(new { success = false, message = "Gönderen hesap bulunamadı veya size ait değil." });

                if (gonderenHesap.Balance < model.Tutar)
                    return Json(new { success = false, message = "Bakiyeniz bu işlem için yetersiz." });

                string temizIban = model.AliciIban.Replace(" ", "").Trim();

                var aliciHesap = await _context.Hesaplar.FirstOrDefaultAsync(x => x.Iban.Replace(" ", "") == temizIban);

                if (aliciHesap == null)
                {
                    return Json(new { success = false, message = "Alıcı IBAN sistemimizde kayıtlı değil. (Şu an sadece banka içi transfer yapılabilir)" });
                }

                if (gonderenHesap.Iban == aliciHesap.Iban)
                    return Json(new { success = false, message = "Kendi hesabınıza aynı hesaptan para gönderemezsiniz." });

                gonderenHesap.Balance -= model.Tutar; 
                aliciHesap.Balance += model.Tutar;    

                var islemKaydi = new IslemlerModel
                {
                    IslemId = Guid.NewGuid(),
                    GonderenHesapId = gonderenHesap.HesapId,
                    AliciHesapId = aliciHesap.HesapId,
                    IslemMiktari = model.Tutar,
                    Aciklama = string.IsNullOrEmpty(model.Aciklama) ? $"{model.AliciAdSoyad} kişisine transfer" : model.Aciklama,
                    IslemTarihi = DateTime.Now,
                    Tur = IslemTuru.Havale
                };

                await _context.Islemler.AddAsync(islemKaydi);

                _context.Hesaplar.Update(gonderenHesap);
                _context.Hesaplar.Update(aliciHesap);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Json(new { success = true, message = $"{model.Tutar:N2} TL başarıyla gönderildi." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Json(new { success = false, message = "Transfer sırasında bir hata oluştu: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> FaturaOde()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var model = new TransferFaturaOdeViewModel
            {
                KullaniciHesaplari = await _context.Hesaplar
                    .Where(x => x.UserId == userId)
                    .ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> FaturaOde(TransferFaturaOdeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Json(new { success = false, message = errors });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var gonderenHesap = await _context.Hesaplar
                    .FirstOrDefaultAsync(x => x.HesapId == model.GonderenHesapId && x.UserId == userId);

                if (gonderenHesap == null)
                    return Json(new { success = false, message = "Hesap bulunamadı." });

                if (gonderenHesap.Balance < model.Tutar)
                    return Json(new { success = false, message = "Fatura ödemesi için bakiyeniz yetersiz." });

                gonderenHesap.Balance -= model.Tutar;

                var islemKaydi = new IslemlerModel
                {
                    IslemId = Guid.NewGuid(),
                    GonderenHesapId = gonderenHesap.HesapId,
                    AliciHesapId = null, 
                    IslemMiktari = model.Tutar,
                    IslemTarihi = DateTime.Now,
                    Tur = IslemTuru.Odeme,
                    Aciklama = $"Fatura Ödeme: {model.KurumAdi} (Abone: {model.AboneNo})"
                };

                await _context.Islemler.AddAsync(islemKaydi);
                _context.Hesaplar.Update(gonderenHesap);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Json(new { success = true, message = $"{model.KurumAdi} faturanız başarıyla ödendi." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Json(new { success = false, message = "Ödeme sırasında hata: " + ex.Message });
            }

        }
    }
}
