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
            //ViewBag.BakiyelerToplami = await _context.Users.SumAsync(u => u.UserBalance);
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

                    var hesapFaker = new Faker<BankAccounts>("tr")
                        .RuleFor(h => h.Id, f => Guid.NewGuid())
                        .RuleFor(h => h.UserId, user.UserId)
                        .RuleFor(h => h.AccountNumbers, f => f.Random.ReplaceNumbers("##########"))
                        .RuleFor(h => h.Iban, f => "TR" + f.Random.ReplaceNumbers("########################"))
                        .RuleFor(h => h.Balance, f => Math.Round(f.Finance.Amount(1000, 100000), 2))
                        .RuleFor(h => h.CurrencyCode, "TRY")
                        .RuleFor(h => h.CreatedDate, f => f.Date.Past(2));
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
    }
}
