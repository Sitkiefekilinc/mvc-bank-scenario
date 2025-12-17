using EEZBankServer.EfCore;
using EEZBankServer.Models;
using Microsoft.AspNetCore.Authentication;
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
                    new Claim(ClaimTypes.Email, user.UserEmail),
                    new Claim("UserType", user.UserType.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims);
                var authProperties = new AuthenticationProperties { IsPersistent = model.RememberMe };

                await HttpContext.SignInAsync(
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
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Lütfen formdaki eksikleri giderin." });
            }
            if (_context.Users.Any(u => u.UserEmail == model.UserEmail))
            {
                return Json(new { success = false, message = "Bu e-posta adresi zaten kayıtlı!" });
            }

            try
            {
                var user = new UserAccountInfos
                {
                    UserName = model.UserName,
                    UserSurname = model.UserSurname,
                    UserEmail = model.UserEmail,
                    UserPassword = BCrypt.Net.BCrypt.HashPassword(model.UserPassword),
                    UserBalance = 0,
                    UserIban = "TR" + Guid.NewGuid().ToString().Substring(0, 24).ToUpper(),
                    UserPhoneNumber = model.UserPhoneNumber,
                    TcKimlikNo = model.TcKimlikNo,
                    UserBirthDate = model.UserBirthDate,
                    Adress = model.Adress,
                    UserType = model.UserType,
                    IsActive = true,
                    HasTheAgreementBeenAccepted = true
                };
                _context.Users.Add(user);

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
                    _context.KurumsalKullaniciBilgileri.Add(kurumsal);
                }
                else if (model.UserType == UserTypes.Ticari)
                {
                    var ticari = new TicariKullaniciModel
                    {
                        UserId = user.UserId,
                        CompanyName = model.CompanyName,
                        CompanyEmail = model.CompanyEmail
                    };
                    _context.TicariKullaniciBilgileri.Add(ticari);
                }
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "EEZ Bank'a hoş geldiniz! Kaydınız başarıyla tamamlandı." });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Kayıt sırasında teknik bir hata oluştu." });
            }


        }

    }
}
