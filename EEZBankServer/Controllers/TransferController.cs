using EEZBankServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EEZBankServer.EfCore;
using Microsoft.AspNetCore.Authorization;
using EEZBankServer.Models.ViewModel;

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
                Hesaplar = await _context.Hesaplar
                    .Where(x => x.UserId == userId)
                    .ToListAsync(),

                SonIslemler = await _context.Islemler
                .Where(x => x.GonderenBankaHesabi.UserId == userId || x.AliciBankaHesabi != null && x.AliciBankaHesabi.UserId == userId)
                .OrderByDescending(x => x.IslemTarihi)
                .Take(5)
                .Include(x => x.AliciBankaHesabi)
                .ThenInclude(x=> x.User)
                .Include(x => x.GonderenBankaHesabi)
                .ToListAsync(),
            };

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Eft()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

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
                    Tur = IslemTuru.EFT
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

        [HttpGet]
        public IActionResult IslemGecmisi()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetIslemGecmisi()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();  
                var length = Request.Form["length"].FirstOrDefault(); 
                var searchValue = Request.Form["search[value]"].FirstOrDefault(); 
                var sortColumnIndex = Request.Form["order[0][column]"].FirstOrDefault(); 
                var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 10;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var query = _context.Islemler
                    .Include(x => x.AliciBankaHesabi).ThenInclude(h => h.User)
                    .Include(x => x.GonderenBankaHesabi).ThenInclude(h => h.User)
                    .AsQueryable();

                query = query.Where(x =>
                    (x.GonderenBankaHesabi.UserId == userId) ||
                    (x.AliciBankaHesabi != null && x.AliciBankaHesabi.UserId == userId)
                );

                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(x =>
                        (x.Aciklama != null && x.Aciklama.Contains(searchValue)) ||
                        (x.IslemMiktari.ToString().Contains(searchValue)) ||
                        (x.AliciBankaHesabi != null && x.AliciBankaHesabi.User.UserName.Contains(searchValue)) ||
                        (x.GonderenBankaHesabi.User.UserName.Contains(searchValue))
                    );
                }

                var recordsTotal = await query.CountAsync();

                switch (sortColumnIndex)
                {
                    case "1": 
                    case "2": 
                        query = sortDirection == "asc" ? query.OrderBy(x => x.IslemMiktari) : query.OrderByDescending(x => x.IslemMiktari);
                        break;
                    case "0":
                    default:
                        query = sortDirection == "asc" ? query.OrderBy(x => x.IslemTarihi) : query.OrderByDescending(x => x.IslemTarihi);
                        break;
                }

                var rawData = await query
                    .Skip(skip)
                    .Take(pageSize)
                    .ToListAsync();

 
                var data = rawData.Select(x => {

                    bool gonderenBenim = x.GonderenBankaHesabi.UserId == userId;
                    string karsiTaraf = "Bilinmiyor";
                    string islemTuruBadge = "";


                    if (x.Tur == IslemTuru.Odeme)
                    {
                        karsiTaraf = x.Aciklama ?? "Fatura";
                        islemTuruBadge = "Fatura";
                    }
                    else
                    {
                        if (gonderenBenim)
                            karsiTaraf = x.AliciBankaHesabi?.User != null
                                ? $"{x.AliciBankaHesabi.User.UserName} {x.AliciBankaHesabi.User.UserSurname}"
                                : (x.Aciklama ?? "Bilinmeyen");
                        else
                            karsiTaraf = x.GonderenBankaHesabi?.User != null
                                ? $"{x.GonderenBankaHesabi.User.UserName} {x.GonderenBankaHesabi.User.UserSurname}"
                                : "Gönderen Bilinmiyor";

                        islemTuruBadge = "Transfer";
                    }

                    return new
                    {
                        Tarih = x.IslemTarihi.ToString("dd.MM.yyyy HH:mm"),
                        KarsiTaraf = karsiTaraf,
                        Aciklama = x.Tur == IslemTuru.Odeme ? "Kurum Ödemesi" : x.Aciklama,
                        Tutar = x.IslemMiktari.ToString("N2"),
                        Yon = gonderenBenim ? "out" : "in", 
                        Tur = islemTuruBadge
                    };
                });

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, data = "" });
            }
        }
    }
}
