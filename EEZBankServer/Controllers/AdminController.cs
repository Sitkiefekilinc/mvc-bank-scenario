using Bogus;
using EEZBankServer.EfCore;
using EEZBankServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EEZBankServer.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        private readonly UserDbContext _context;
        public AdminController(UserDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.KullaniciSayisi = await _context.Users.CountAsync();
            ViewBag.BakiyelerToplami = await _context.Hesaplar.SumAsync(u => u.Balance);
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> FakeVeriUret()
        {
            try
            {
                var userFaker = new Faker<UserAccountInfos>("tr")
                    .RuleFor(u => u.UserId, f => Guid.NewGuid())
                    .RuleFor(u => u.UserName, f => f.Name.FirstName())
                    .RuleFor(u => u.UserSurname, f => f.Name.LastName())
                    .RuleFor(u => u.UserEmail, (f, u) => f.Internet.Email(u.UserName, u.UserSurname).ToLower())
                    .RuleFor(u => u.UserPassword, f => 
                    { string rawPassword = f.Internet.Password(8);
                        return BCrypt.Net.BCrypt.HashPassword(rawPassword);
                    })
                    .RuleFor(u => u.UserPasswordAgain, (f, u) => u.UserPassword) 
                    .RuleFor(u => u.UserPhoneNumber, f => f.Phone.PhoneNumber("0532#######"))
                    .RuleFor(u => u.TcKimlikNo, f => f.Random.ReplaceNumbers("###########"))
                    .RuleFor(u => u.UserBirthDate, f => f.Date.Past(40, DateTime.Now.AddYears(-18)))
                    .RuleFor(u => u.Adress, f => f.Address.FullAddress())
                    .RuleFor(u => u.HasTheAgreementBeenAccepted, true)
                    .RuleFor(u => u.UserType, f => f.PickRandom(UserTypes.Bireysel, UserTypes.Kurumsal, UserTypes.Ticari ));

                var fakeUsers = userFaker.Generate(100);

                foreach (var user in fakeUsers)
                {
                    await _context.Users.AddAsync(user);

                    var hesapFaker = new Faker<BankAccountsModel>("tr")
                        .RuleFor(h => h.UserId, user.UserId)
                        .RuleFor(h => h.AccountNumbers, f => f.Random.ReplaceNumbers("##########"))
                        .RuleFor(h => h.Iban, f => "TR" + f.Random.ReplaceNumbers("########################"))
                        .RuleFor(h => h.Balance, f => Math.Round(f.Finance.Amount(1000, 100000), 2))
                        .RuleFor(h => h.CurrencyCode, "TRY")
                        .RuleFor(h => h.CreatedDate, f => f.Date.Past(2));
                    await _context.Hesaplar.AddAsync(hesapFaker.Generate());

                    if (user.UserType == UserTypes.Kurumsal)
                    {
                        var kurumsalFaker = new Faker<KurumsalKullaniciModel>("tr")
                            .RuleFor(k => k.UserId, user.UserId)
                            .RuleFor(k => k.CorporateName, f => f.Company.CompanyName() + " " + f.Company.CompanySuffix())
                            .RuleFor(k => k.CorporateType, f => f.PickRandom<CorporateTypes>())
                            .RuleFor(k => k.TaxNumber, f => f.Random.ReplaceNumbers("##########"))
                            .RuleFor(k => k.CorporateAddress, f => f.Address.FullAddress())
                            .RuleFor(k => k.AuthorizedPersonsTask, f => f.Name.JobTitle());
                        
                      await _context.KurumsalKullaniciBilgileri.AddAsync(kurumsalFaker.Generate());
                    }
                    else if (user.UserType == UserTypes.Ticari)
                    {
                        var ticariFaker = new Faker<TicariKullaniciModel>("tr")
                            .RuleFor(t => t.UserId, user.UserId) 
                            .RuleFor(t => t.CompanyName, f => f.Name.FullName() + " Ltd. Şti.")
                            .RuleFor(t => t.CompanyEmail, f => f.Internet.Email());

                        await _context.TicariKullaniciBilgileri.AddAsync(ticariFaker.Generate());
                    }
                }

                _context.SaveChanges();
                return Json(new { success = true, message = "100 Adet karma kullanıcı (Bireysel/Kurumsal/Ticari) başarıyla oluşturuldu." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }

        }

        [HttpGet]
        public IActionResult KullaniciListesi()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetKullaniciListesi()
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

                var query = _context.Users.AsQueryable();
                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(u =>
                        u.UserName.Contains(searchValue) ||
                        u.UserSurname.Contains(searchValue) ||
                        u.UserEmail.Contains(searchValue) ||
                        u.TcKimlikNo.Contains(searchValue)
                    );
                }

                var recordsTotal = await query.CountAsync();

                switch (sortColumnIndex)
                {
                    case "0": 
                        query = sortDirection == "asc" ? query.OrderBy(u => u.UserName) : query.OrderByDescending(u => u.UserName);
                        break;
                    case "1": 
                        query = sortDirection == "asc" ? query.OrderBy(u => u.UserEmail) : query.OrderByDescending(u => u.UserEmail);
                        break;

                    case "2": 
                        query = sortDirection == "asc" ? query.OrderBy(u => u.UserPhoneNumber) : query.OrderByDescending(u => u.UserPhoneNumber);
                        break;

                    case "3":
                        query = sortDirection == "asc" ? query.OrderBy(u => u.TcKimlikNo) : query.OrderByDescending(u => u.TcKimlikNo);
                        break;

                    case "4": 
                        query = sortDirection == "asc" ? query.OrderBy(u => u.UserType) : query.OrderByDescending(u => u.UserType);
                        break;
                    case "5": 
                        query = sortDirection == "asc" ? query.OrderBy(u => u.UserCreatedAt) : query.OrderByDescending(u => u.UserCreatedAt);
                        break;
                    case "6": 
                        query = sortDirection == "asc" ? query.OrderBy(u => u.UserCreatedAt) : query.OrderByDescending(u => u.UserCreatedAt);
                        break;
                    default: 
                        query = query.OrderByDescending(u => u.UserCreatedAt);
                        break;
                }

                var rawData = await query
                    .Skip(skip)
                    .Take(pageSize)
                    .ToListAsync();
                var data = rawData.Select(u => new
                {
                    id = u.UserId, 
                    adSoyad = $"{u.UserName} {u.UserSurname}",
                    email = u.UserEmail,
                    telefon = u.UserPhoneNumber, 
                    tcNo = u.TcKimlikNo,
                    tip = u.UserType.ToString(),
                    durum = u.IsActive,
                    tarih = u.UserCreatedAt.ToString("dd.MM.yyyy")
                });

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, data = "" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> KullaniciSil(Guid id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return Json(new { success = false, message = "Kullanıcı bulunamadı." });
                }

                bool hesapVarMi = await _context.Hesaplar.AnyAsync(x => x.UserId == id);
                if (hesapVarMi)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Bu kullanıcının aktif banka hesapları veya işlem geçmişi var. Silmek yerine 'Pasif' duruma getirmelisiniz."
                    });
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Kullanıcı başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Silme işlemi sırasında hata oluştu: " + ex.Message });
            }
        }
    }
}
