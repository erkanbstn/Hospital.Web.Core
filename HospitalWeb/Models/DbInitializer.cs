using HospitalWeb.Models.Domain;

namespace HospitalWeb.Models
{
    // Veritabanı başlangıç verilerini eklemek için kullanılan sınıf
    public static class DbInitializer
    {
        // Veritabanına başlangıç verilerini ekler
        public static void Seed(AppDbContext context)
        {
            // Eğer veritabanında zaten veri varsa işlemi durdur
            if (context.Doktorlar.Any() || context.Hastalar.Any() || context.Poliklinikler.Any() ||
                context.Randevular.Any() || context.Receteler.Any())
            {
                return;
            }

            // Poliklinik Ekleme
            var poliklinikler = new List<Poliklinik>
            {
                new Poliklinik { Ad = "Dahiliye" },
                new Poliklinik { Ad = "Kardiyoloji" },
                new Poliklinik { Ad = "Ortopedi" }
            };
            context.Poliklinikler.AddRange(poliklinikler);
            context.SaveChanges();

            // Doktorları ekleme
            var doktorlar = new List<Doktor>
            {
                new Doktor { TC = "11111111111", Isim = "Ali Yılmaz", Uzmanlik = "Dahiliye", Sifre = "1234", PoliklinikId = poliklinikler[0].Id, SertifikaNo = "SER-" + Guid.NewGuid().ToString().Substring(0, 4) },
                new Doktor { TC = "22222222222", Isim = "Ayşe Kara", Uzmanlik = "Kardiyoloji", Sifre = "1234", PoliklinikId = poliklinikler[1].Id, SertifikaNo = "SER-" + Guid.NewGuid().ToString().Substring(0, 4) },
                new Doktor { TC = "33333333333", Isim = "Mehmet Demir", Uzmanlik = "Ortopedi", Sifre = "1234", PoliklinikId = poliklinikler[2].Id, SertifikaNo = "SER-" + Guid.NewGuid().ToString().Substring(0, 4) },
                new Doktor { TC = "44444444444", Isim = "Zeynep Çelik", Uzmanlik = "Dahiliye", Sifre = "1234", PoliklinikId = poliklinikler[0].Id, SertifikaNo = "SER-" + Guid.NewGuid().ToString().Substring(0, 4) },
                new Doktor { TC = "55555555555", Isim = "Burak Şahin", Uzmanlik = "Kardiyoloji", Sifre = "1234", PoliklinikId = poliklinikler[1].Id, SertifikaNo = "SER-" + Guid.NewGuid().ToString().Substring(0, 4) }
            };

            context.Doktorlar.AddRange(doktorlar);
            context.SaveChanges();

            // Hastaları ekleme
            var hastalar = Enumerable.Range(1, 9).Select(i => new Hasta
            {
                TC = $"1000000000{i}",
                Isim = $"HastaAd HastaSoyad{i}",
                Sifre = $"1234",
            }).ToList();
            context.Hastalar.AddRange(hastalar);
            context.SaveChanges();

            var random = new Random();

            // Randevular ekleme
            var randevular = new List<Randevu>();
            for (int i = 0; i < 10; i++)
            {
                var randomHasta = hastalar[random.Next(hastalar.Count)];
                var randomDoktor = doktorlar[random.Next(doktorlar.Count)];
                var poliklinik = poliklinikler.First(p => p.Id == randomDoktor.PoliklinikId);

                randevular.Add(new Randevu
                {
                    HastaTC = randomHasta.TC,
                    DoktorTC = randomDoktor.TC,
                    PoliklinikId = poliklinik.Id,
                    Tarih = DateTime.Now.AddDays(random.Next(1, 30)) // 1 ile 30 gün arasında rastgele bir tarih
                });
            }
            context.Randevular.AddRange(randevular);
            context.SaveChanges();

            // Reçeteler ekleme
            var receteler = new List<Recete>();
            for (int i = 0; i < 5; i++)
            {
                var randomHasta = hastalar[random.Next(hastalar.Count)];
                var randomDoktor = doktorlar[random.Next(doktorlar.Count)];

                receteler.Add(new Recete
                {
                    HastaTC = randomHasta.TC,
                    DoktorTC = randomDoktor.TC,
                    Teshis = $"Teşhis {i + 1}",
                    IlacAdi = $"İlaç {i + 1}"
                });
            }
            context.Receteler.AddRange(receteler);
            context.SaveChanges();
        }

    }
}
