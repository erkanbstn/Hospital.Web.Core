using HospitalWeb.Models;
using HospitalWeb.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalWeb.Controllers
{
    public class HastaController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public HastaController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // Doktorlar Sayfası
        public async Task<IActionResult> Doktorlar()
        {
            // Doktorları veritabanından çek
            var doktorlar = await _appDbContext.Doktorlar.ToListAsync();

            // Doktorları View'a gönder
            return View(doktorlar);
        }

        // Poliklinikler Sayfası
        public async Task<IActionResult> Poliklinikler()
        {
            // Poliklinikleri veritabanından çek
            var poliklinikler = await _appDbContext.Poliklinikler.ToListAsync();

            // Poliklinikleri View'a gönder
            return View(poliklinikler);
        }

        // Randevularım Sayfası
        public async Task<IActionResult> Randevularim()
        {
            // Giriş yapan hasta tc sini çek
            var hasta = await _appDbContext.Hastalar
                                    .FirstOrDefaultAsync(h => h.TC == User.Identity.Name);

            // Randevuları veritabanından çek
            var randevularım = await _appDbContext.Randevular.Where(x => x.HastaId == hasta.Id).ToListAsync();

            // Randevuları View'a gönder
            return View(randevularım);
        }

        // Profil Sayfası
        public async Task<IActionResult> Profil()
        {
            // Giriş yapan hasta tc sini çek
            var hasta = await _appDbContext.Hastalar
                                    .FirstOrDefaultAsync(h => h.TC == User.Identity.Name);

            // Hastayı View'a gönder
            return View(hasta);
        }

        [HttpPost] // Post metodunda profil güncelleme işlemi
        public async Task<IActionResult> Profil(Hasta hasta)
        {
            _appDbContext.Hastalar.Update(hasta);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Profil"); // Profil güncellenince kullanıcıyı Profil sayfasına yönlendir
        }

        // Recetelerim Sayfası
        public async Task<IActionResult> Recetelerim()
        {
            // Giriş yapan hasta tc sini çek
            var hasta = await _appDbContext.Hastalar
                                    .FirstOrDefaultAsync(h => h.TC == User.Identity.Name);

            // Reçeteleri veritabanından çek
            var recetelerim = await _appDbContext.Receteler.Where(x => x.HastaId == hasta.Id).ToListAsync();

            // Reçeteleri View'a gönder
            return View(recetelerim);
        }
    }
}