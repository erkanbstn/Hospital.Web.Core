using HospitalWeb.Models.Domain;
using HospitalWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace HospitalWeb.Controllers
{
    [Authorize(Roles = "Doktor")]
    public class DoktorController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public DoktorController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // Profil Sayfası
        [HttpGet]
        public async Task<IActionResult> Profil()
        {
            // Giriş yapan doktor tc sini çek
            var doktor = await _appDbContext.Doktorlar
                                    .FirstOrDefaultAsync(h => h.TC == User.Identity.Name);

            // Doktoru View'a gönder
            return View(doktor);
        }

        [HttpPost] // Post metodunda profil güncelleme işlemi
        public async Task<IActionResult> ProfilGuncelle(Doktor doktor)
        {
            _appDbContext.Doktorlar.Update(doktor);
            await _appDbContext.SaveChangesAsync();

            return Ok("Profil bilgileriniz başarıyla güncellendi.");
        }

        // Poliklinikler Sayfası
        [HttpGet]
        public async Task<IActionResult> Poliklinikler()
        {
            // Poliklinikleri veritabanından çek
            var poliklinikler = await _appDbContext.Poliklinikler.ToListAsync();

            // Poliklinikleri View'a gönder
            return View(poliklinikler);
        }

        // Randevularım Sayfası
        [HttpGet]
        public async Task<IActionResult> Randevularim()
        {
            // Giriş yapan doktor tc sini çek
            var hasta = await _appDbContext.Doktorlar
                                    .FirstOrDefaultAsync(h => h.TC == User.Identity.Name);

            // Randevuları veritabanından çek
            var randevularım = await _appDbContext.Randevular.Include(x => x.Hasta).Where(x => x.DoktorId == hasta.Id).ToListAsync();

            // Randevuları View'a gönder
            return View(randevularım);
        }

        // Hastalar sayfası
        [HttpGet]
        public async Task<IActionResult> Hastalar()
        {
            var hastalar = await _appDbContext.Hastalar
                                              .ToListAsync();

            // Hastaları View'a gönder
            return View(hastalar);
        }

        [HttpGet]
        public async Task<IActionResult> ReceteOlustur()
        {
            var hastalar = await _appDbContext.Hastalar.ToListAsync();
            return View(hastalar);
        }

        // Reçete Gir
        [HttpPost]
        public async Task<IActionResult> ReceteOlustur(int hastaId, string ilacAdi, string teshis)
        {
            // Giriş yapan doktoru al
            var doktor = await _appDbContext.Doktorlar
                                            .FirstOrDefaultAsync(d => d.TC == User.Identity.Name);

            // Hastayı al
            var hasta = await _appDbContext.Hastalar
                                           .FirstOrDefaultAsync(h => h.Id == hastaId);

            // Yeni reçete oluştur
            var yeniRecete = new Recete
            {
                HastaId = hasta.Id,
                DoktorId = doktor.Id,
                IlacAdi = ilacAdi,
                Teshis = teshis,
                CreatedAt = DateTime.Now
            };

            // Veritabanına ekle
            _appDbContext.Receteler.Add(yeniRecete);
            await _appDbContext.SaveChangesAsync();

            return Ok("Reçete başarıyla eklendi.");
        }

        // Recetelerim Sayfası
        [HttpGet]
        public async Task<IActionResult> Recetelerim()
        {
            // Giriş yapan doktor tc sini çek
            var doktor = await _appDbContext.Doktorlar
                                    .FirstOrDefaultAsync(h => h.TC == User.Identity.Name);

            // Reçeteleri ve ilişkili Doktor verisini veritabanından çek
            var recetelerim = await _appDbContext.Receteler
                .Where(x => x.DoktorId == doktor.Id)
                .Include(r => r.Hasta)  // Hasta bilgilerini de yükle
                .ToListAsync();

            // Reçeteleri View'a gönder
            return View(recetelerim);
        }

        // Poliklinikler için doktorları getirir
        public async Task<IActionResult> DoktorlariGetir(int poliklinikId)
        {
            var doktorlar = await _appDbContext.Doktorlar
                .Where(d => d.PoliklinikId == poliklinikId)
                .ToListAsync();

            return PartialView("~/Views/Shared/DoktorlarPartial.cshtml", doktorlar);
        }
    }
}
