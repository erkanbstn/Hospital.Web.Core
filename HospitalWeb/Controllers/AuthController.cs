using HospitalWeb.Models;
using HospitalWeb.Models.Domain;
using HospitalWeb.Models.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HospitalWeb.Controllers
{
    [AllowAnonymous] // Bu controller'a anonim kullanıcılar da erişebilir
    public class AuthController : Controller
    {
        private readonly AppDbContext _appDbContext;

        // Constructor: DbContext'in enjekte edilmesi
        public AuthController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // GET: Kullanıcı Giriş Sayfası
        public IActionResult GirisYap()
        {
            return View();
        }

        // POST: Kullanıcı Giriş İşlemi (Hasta & Doktor)
        [HttpPost]
        public async Task<IActionResult> GirisYap(LoginDTO loginDto)
        {
            // Kullanıcı (Hasta veya Doktor) veritabanında var mı kontrol et
            var hasta = await _appDbContext.Hastalar.FirstOrDefaultAsync(h => h.TC == loginDto.TC);
            var doktor = await _appDbContext.Doktorlar.FirstOrDefaultAsync(d => d.TC == loginDto.TC);

            object user = hasta ?? (object)doktor; // Eğer hasta varsa onu, yoksa doktoru al
            string role = hasta != null ? "Hasta" : doktor != null ? "Doktor" : null; // Kullanıcının rolü

            // Kullanıcı bulunamazsa hata mesajı göster
            if (user == null)
            {
                ViewBag.Message = "TC veya şifre hatalı!";
                return View();
            }

            // Şifre doğrulaması
            string password = hasta != null ? hasta.Sifre : doktor.Sifre;
            if (loginDto.Sifre != password)
            {
                ViewBag.Message = "TC veya şifre hatalı!";
                return View();
            }

            // Kullanıcıya ait claim'ler oluşturuluyor
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginDto.TC), // Kullanıcı TC
                new Claim(ClaimTypes.Role, role), // Kullanıcı Rolü (Hasta veya Doktor)
            };

            // ClaimsIdentity nesnesi oluşturuluyor
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // ClaimsPrincipal oluşturuluyor ve kullanıcı çerezle doğrulanıyor
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            // Kullanıcı Hasta ise Randevu sayfasına, Doktor ise Reçete sayfasına yönlendirilir
            if (role == "Hasta")
                return RedirectToAction("RandevuAl", "Hasta"); // Hastayı randevu sayfasına yönlendir
            else
                return RedirectToAction("ReceteYaz", "Doktor"); // Doktoru reçete yazma sayfasına yönlendir
        }

        // Kullanıcı çıkış işlemi
        public async Task<IActionResult> CikisYap()
        {
            // Kullanıcıyı oturumdan çıkarmak
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // Login sayfasına yönlendir
            return RedirectToAction("GirisYap", "Auth");
        }

        // GET: Kullanıcı Kayıt Sayfası
        public IActionResult KayitOl()
        {
            // Poliklinik verilerini veri tabanından çek
            var poliklinikler = _appDbContext.Poliklinikler.ToList();

            // Poliklinikleri ViewBag ile View'a aktar
            ViewBag.Poliklinikler = poliklinikler;

            return View();
        }

        // POST: Kullanıcı Kayıt İşlemi (Hasta & Doktor)
        [HttpPost]
        public async Task<IActionResult> KayitOl(RegisterDTO model)
        {
            // TC zaten kayıtlı mı kontrol et
            var existingUser = await _appDbContext.Hastalar
                .AnyAsync(h => h.TC == model.TC) ||
                await _appDbContext.Doktorlar.AnyAsync(d => d.TC == model.TC);

            if (existingUser)
            {
                ViewBag.Message = "Bu TC kimlik numarası zaten kayıtlı!";
                return View();
            }

            if (!string.IsNullOrEmpty(model.SertifikaNo))
            {
                // Doktor Kaydı
                var doktor = new Doktor
                {
                    TC = model.TC,
                    Isim = model.Isim,
                    Sifre = model.Sifre,
                    SertifikaNo = model.SertifikaNo,
                    PoliklinikId = model.PoliklinikId // Poliklinik seçimi
                };
                _appDbContext.Doktorlar.Add(doktor);
            }
            else
            {
                // Hasta Kaydı
                var hasta = new Hasta
                {
                    TC = model.TC,
                    Isim = model.Isim,
                    Sifre = model.Sifre
                };
                _appDbContext.Hastalar.Add(hasta);
            }

            await _appDbContext.SaveChangesAsync();
            ViewBag.Message = "Kayıt başarılı, lütfen giriş yapın!";
            return RedirectToAction("GirisYap");
        }

    }
}