using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace ECommorceWeb.Controllers
{
    public class LoginController : Controller
    {
        ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public IActionResult Index()
        {
            // Eğer kullanıcı zaten oturum açmışsa, anasayfaya yönlendir
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string username, string password, bool isAdmin)
        {
            // Burada isAdmin parametresini sabit true yapıyorsunuz; gerçek uygulamada bunu kaldırabilirsiniz
            isAdmin = true;

            // Kullanıcı listesini çekiyoruz
            var loginList = _loginService.GetList();

            // Kullanıcı adı ve şifre ile eşleşen kullanıcıyı buluyoruz
            var user = loginList.Data.FirstOrDefault(c => c.UserName == username && c.PasswordHash == password);

            if (user != null)
            {
                // Kullanıcı doğrulandıysa, claim'leri oluşturuyoruz
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username), // Kullanıcı adı claim olarak ekleniyor
                    new Claim("IsAdmin", user.IsAdmin), // Kullanıcının admin olup olmadığı claim olarak ekleniyor,
                    new Claim(ClaimTypes.Role , user.IsAdmin),// claimrole isadmin bool değerini string olarak atıyoruz
                     new Claim(ClaimTypes.NameIdentifier, user.ApplicationUserId.ToString()),
    // Diğer claim'ler
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    // Authentication özelliklerini yapılandırabilirsiniz
                };

                // Kullanıcıyı giriş yapmış olarak işaretliyoruz
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);
                // Ana sayfaya yönlendiriyoruz
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Giriş başarısızsa, hata mesajı gösteriyoruz
                TempData["Message"] = "Geçersiz kullanıcı adı veya şifre.";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
