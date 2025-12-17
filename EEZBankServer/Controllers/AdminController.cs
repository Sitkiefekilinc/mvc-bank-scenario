using Bogus;
using EEZBankServer.EfCore;
using EEZBankServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace EEZBankServer.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserDbContext _context;
        public AdminController(UserDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult FakeVeriUret()
        {
            try
            {
                var userFaker = new Faker<UserAccountInfos>("tr")
                    .RuleFor(u => u.UserId, f => Guid.NewGuid())
                    .RuleFor(u => u.UserName, f => f.Name.FirstName())
                    .RuleFor(u => u.UserSurname, f => f.Name.LastName())
                    .RuleFor(u => u.UserEmail, (f, u) => f.Internet.Email(u.UserName, u.UserSurname).ToLower())
                    .RuleFor(u => u.UserPassword, f => f.Internet.Password(8))
                    .RuleFor(u => u.UserPasswordAgain, (f, u) => u.UserPassword) 
                    .RuleFor(u => u.UserBalance, f => f.Finance.Amount(500, 100000))
                    .RuleFor(u => u.UserIban, f => f.Finance.Iban(false,"TR"))
                    .RuleFor(u => u.UserPhoneNumber, f => f.Phone.PhoneNumber("0532#######"))
                    .RuleFor(u => u.TcKimlikNo, f => f.Random.ReplaceNumbers("###########"))
                    .RuleFor(u => u.UserBirthDate, f => f.Date.Past(40, DateTime.Now.AddYears(-18)))
                    .RuleFor(u => u.Adress, f => f.Address.FullAddress())
                    .RuleFor(u => u.HasTheAgreementBeenAccepted, true)
                    .RuleFor(u => u.UserType, f => f.PickRandom<UserTypes>());

                var fakeUsers = userFaker.Generate(100);

                foreach (var user in fakeUsers)
                {
                    _context.Users.Add(user);

                    if (user.UserType == UserTypes.Kurumsal)
                    {
                        var kurumsalFaker = new Faker<KurumsalKullaniciModel>("tr")
                            .RuleFor(k => k.UserId, user.UserId)
                            .RuleFor(k => k.CorporateName, f => f.Company.CompanyName() + " " + f.Company.CompanySuffix())
                            .RuleFor(k => k.CorporateType, f => f.PickRandom<CorporateTypes>())
                            .RuleFor(k => k.TaxNumber, f => f.Random.ReplaceNumbers("##########"))
                            .RuleFor(k => k.CorporateAddress, f => f.Address.FullAddress())
                            .RuleFor(k => k.AuthorizedPersonsTask, f => f.Name.JobTitle());

                        _context.KurumsalKullaniciBilgileri.Add(kurumsalFaker.Generate());
                    }
                    else if (user.UserType == UserTypes.Ticari)
                    {
                        var ticariFaker = new Faker<TicariKullaniciModel>("tr")
                            .RuleFor(t => t.UserId, user.UserId) 
                            .RuleFor(t => t.CompanyName, f => f.Name.FullName() + " Ltd. Şti.")
                            .RuleFor(t => t.CompanyEmail, f => f.Internet.Email());

                        _context.TicariKullaniciBilgileri.Add(ticariFaker.Generate());
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
