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
        [HttpGet]
        public async Task<IActionResult> Doktorlar()
        {
            // Doktorları veritabanından çek
            var doktorlar = await _appDbContext.Doktorlar.Include(x => x.Poliklinik).ToListAsync();

            // Doktorları View'a gönder
            return View(doktorlar);
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
            // Giriş yapan hasta tc sini çek
            var hasta = await _appDbContext.Hastalar
                                    .FirstOrDefaultAsync(h => h.TC == User.Identity.Name);

            // Randevuları veritabanından çek
            var randevularım = await _appDbContext.Randevular.Include(x => x.Doktor).ThenInclude(y => y.Poliklinik).Where(x => x.HastaId == hasta.Id).ToListAsync();

            // Randevuları View'a gönder
            return View(randevularım);
        }
        // Randevu iptal metodu
        [HttpPost]
        public async Task<IActionResult> IptalEt(int id)
        {
            var randevu = await _appDbContext.Randevular.FindAsync(id);
            if (randevu == null)
            {
                return Json(new { success = false, message = "Randevu bulunamadı." });
            }

            // Geçerli randevu iptal ediliyor
            randevu.Status = false;
            _appDbContext.Update(randevu);
            await _appDbContext.SaveChangesAsync();

            return Json(new { success = true, message = "Randevu başarıyla iptal edildi." });
        }

        // Randevu Al Sayfası
        [HttpGet]
        public IActionResult RandevuAl()
        {
            return View();
        }

        // 1️ Seçilen tarihte aktif olan poliklinikleri getir
        [HttpPost]
        public async Task<IActionResult> GetPolikliniklerByDate(DateTime selectedDate)
        {
            var poliklinikler = await _appDbContext.Poliklinikler.ToListAsync();
            string options = "<option value=''>Poliklinik Seçiniz</option>";

            foreach (var pol in poliklinikler)
            {
                options += $"<option value='{pol.Id}'>{pol.Ad}</option>";
            }

            return Content(options, "text/html");
        }

        // 2️ Seçilen polikliniğe ait doktorları getir
        [HttpPost]
        public IActionResult GetDoktorlarByPoliklinik(int poliklinikId)
        {
            var doktorlar = _appDbContext.Doktorlar
                .Where(d => d.PoliklinikId == poliklinikId)
                .Select(d => new { d.Id, d.Isim })
                .ToList();

            string options = "<option value=''>Doktor Seçiniz</option>";
            foreach (var dr in doktorlar)
            {
                options += $"<option value='{dr.Id}'>{dr.Isim}</option>";
            }

            return Content(options, "text/html");
        }

        // 3️ Seçilen doktora randevu oluştur
        [HttpPost]
        public async Task<IActionResult> RandevuOlustur(DateTime selectedDate, int doktorId, int poliklinikId)
        {
            // Giriş yapan hasta tc sini çek
            var hasta = await _appDbContext.Hastalar
                                    .FirstOrDefaultAsync(h => h.TC == User.Identity.Name);

            var yeniRandevu = new Randevu
            {
                HastaId = hasta.Id,
                DoktorId = doktorId,
                Tarih = selectedDate,
                PoliklinikId = poliklinikId,
            };

            _appDbContext.Randevular.Add(yeniRandevu);
            await _appDbContext.SaveChangesAsync();

            return Ok("Randevunuz başarıyla oluşturuldu.");
        }

        // Profil Sayfası
        [HttpGet]
        public async Task<IActionResult> Profil()
        {
            // Giriş yapan hasta tc sini çek
            var hasta = await _appDbContext.Hastalar
                                    .FirstOrDefaultAsync(h => h.TC == User.Identity.Name);

            // Hastayı View'a gönder
            return View(hasta);
        }

        [HttpPost] // Post metodunda profil güncelleme işlemi
        public async Task<IActionResult> ProfilGuncelle(Hasta hasta)
        {
            _appDbContext.Hastalar.Update(hasta);
            await _appDbContext.SaveChangesAsync();

            return Ok("Profil bilgileriniz başarıyla güncellendi.");
        }

        // Recetelerim Sayfası
        [HttpGet]
        public async Task<IActionResult> Recetelerim()
        {
            // Giriş yapan hasta tc sini çek
            var hasta = await _appDbContext.Hastalar
                                    .FirstOrDefaultAsync(h => h.TC == User.Identity.Name);

            // Reçeteleri ve ilişkili Doktor verisini veritabanından çek
            var recetelerim = await _appDbContext.Receteler
                .Where(x => x.HastaId == hasta.Id)
                .Include(r => r.Doktor)  // Doktor bilgilerini de yükle
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