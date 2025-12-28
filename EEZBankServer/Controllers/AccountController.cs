using EEZBankServer.EfCore;
using EEZBankServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EEZBankServer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserDbContext _context;
        public AccountController(UserDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserEmail == model.Email);
            if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.UserPassword))
            {

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Surname, user.UserSurname),
                    new Claim(ClaimTypes.Email, user.UserEmail),
                    new Claim("UserType", user.UserType.ToString()),
                    new Claim("FullName", user.UserName +" " + user.UserSurname)
                };

                var claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = model.RememberMe };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "E-posta veya şifre hatalı!");
            return View(model);


        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (model.UserType == UserTypes.Bireysel)
            {
                ModelState.Remove(nameof(model.CorporateName));
                ModelState.Remove(nameof(model.CorporateAddress));
                ModelState.Remove(nameof(model.TaxNumber));
                ModelState.Remove(nameof(model.CompanyName));
                ModelState.Remove(nameof(model.CompanyEmail));
                ModelState.Remove(nameof(model.AuthorizedPersonsTask));
            }
            else if (model.UserType == UserTypes.Kurumsal)
            {
                ModelState.Remove(nameof(model.CompanyName));
                ModelState.Remove(nameof(model.CompanyEmail));
            }
            else if (model.UserType == UserTypes.Ticari)
            {
                ModelState.Remove(nameof(model.CorporateName));
                ModelState.Remove(nameof(model.CorporateAddress));
                ModelState.Remove(nameof(model.TaxNumber));
                ModelState.Remove(nameof(model.AuthorizedPersonsTask));
            }

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Lütfen formdaki eksikleri giderin." });
            }

            if (_context.Users.Any(u => u.UserEmail == model.UserEmail))
            {
                return Json(new { success = false, message = "Bu e-posta adresi zaten kayıtlı!" });
            }

            using var dbTransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = new UserAccountInfos
                {
                    UserName = model.UserName,
                    UserSurname = model.UserSurname,
                    UserEmail = model.UserEmail,
                    UserPassword = BCrypt.Net.BCrypt.HashPassword(model.UserPassword),
                    UserPhoneNumber = model.UserPhoneNumber,
                    TcKimlikNo = model.TcKimlikNo,
                    UserBirthDate = model.UserBirthDate,
                    Adress = model.Adress,
                    UserType = model.UserType,
                    IsActive = true,
                    HasTheAgreementBeenAccepted = model.HasTheAgreementBeenAccepted
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                if (model.UserType == UserTypes.Kurumsal)
                {
                    var kurumsal = new KurumsalKullaniciModel
                    {
                        UserId = user.UserId,
                        CorporateName = model.CorporateName,
                        CorporateType = model.CorporateType,
                        TaxNumber = model.TaxNumber,
                        CorporateAddress = model.CorporateAddress,
                        AuthorizedPersonsTask = model.AuthorizedPersonsTask
                    };
                    await _context.KurumsalKullaniciBilgileri.AddAsync(kurumsal);
                }
                else if (model.UserType == UserTypes.Ticari)
                {
                    var ticari = new TicariKullaniciModel
                    {
                        UserId = user.UserId,
                        CompanyName = model.CompanyName,
                        CompanyEmail = model.CompanyEmail
                    };
                    await _context.TicariKullaniciBilgileri.AddAsync(ticari);
                }

                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();

                return Json(new { success = true, message = "EEZ Bank'a hoş geldiniz! Kaydınız başarıyla tamamlandı." });
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync();
                return Json(new { success = false, message = "Teknik bir hata oluştu: " + ex.Message });
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null) return RedirectToAction("Login");

            var userId = Guid.Parse(userIdString);

            var model = new ProfileViewModel();
            model.UserAccountInfos = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (model.UserAccountInfos == null) return NotFound();
            model.Hesaplar = await _context.Hesaplar
                .Where(a => a.UserId == userId)
                .ToListAsync();

            return View(model); 
        }

        [HttpGet]
        [Authorize]
        public IActionResult HesapOlustur()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> HesapOlustur(HesapOluşturViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Lütfen tüm alanları eksiksiz doldurunuz." });
            }
            try
            {
               var yeniHesap = new BankAccounts
                {
                   UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                   AccountNumbers = Random.Shared.NextInt64(1000000000, 10000000000).ToString(),
                   Iban = "TR" + Guid.NewGuid().ToString().Substring(0, 24),
                   AccountName = model.AccountName,
                   CurrencyCode = model.CurrencyCode,
                   Balance = 0.0m,
                };

                await _context.Hesaplar.AddAsync(yeniHesap);
                await _context.SaveChangesAsync();

                return Json(new { success = true , message = "Vadesiz hesap başarıyla oluşturulmuştur."});

            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Teknik bir hata oluştu: " + ex.Message });
            }

        }

    }
}
