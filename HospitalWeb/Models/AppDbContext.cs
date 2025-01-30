using HospitalWeb.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace HospitalWeb.Models
{
    // Uygulamanın veritabanı bağlamını temsil eden sınıf
    public class AppDbContext : DbContext
    {
        // DbContextOptions parametresi ile yapılandırma sağlanır
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Veritabanındaki tabloları DbSet olarak tanımlar
        public DbSet<Doktor> Doktorlar { get; set; }
        public DbSet<Hasta> Hastalar { get; set; }
        public DbSet<Poliklinik> Poliklinikler { get; set; }
        public DbSet<Randevu> Randevular { get; set; }
        public DbSet<Recete> Receteler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tablo yapılandırmalarını özelleştir
            modelBuilder.Entity<Randevu>()
                .ToTable(tb => tb.UseSqlOutputClause(false)); // Kullanıcı tablosu için SQL çıkışlarını devre dışı bırak

            modelBuilder.Entity<Doktor>()
                .ToTable(tb => tb.UseSqlOutputClause(false)); // Koltuk tablosu için SQL çıkışlarını devre dışı bırak

            modelBuilder.Entity<Hasta>()
                .ToTable(tb => tb.UseSqlOutputClause(false)); // Bilet tablosu için SQL çıkışlarını devre dışı bırak

            modelBuilder.Entity<Poliklinik>()
                .ToTable(tb => tb.UseSqlOutputClause(false)); // Uçuş tablosu için SQL çıkışlarını devre dışı bırak

            modelBuilder.Entity<Recete>()
                .ToTable(tb => tb.UseSqlOutputClause(false)); // Uçuş tablosu için SQL çıkışlarını devre dışı bırak

            // Doktor ve Randevu arasındaki ilişki (1-N)
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Doktor)  // Her randevu bir doktora aittir
                .WithMany(d => d.Randevular)  // Bir doktorun birden fazla randevusu olabilir
                .HasForeignKey(r => r.DoktorTC)  // Randevu tablosundaki DoktorTC alanı, Doktor tablosuna referans verir
                .OnDelete(DeleteBehavior.Restrict); // Doktor silindiğinde ona bağlı randevular silinmez

            // Hasta ve Randevu arasındaki ilişki (1-N)
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Hasta)  // Her randevu bir hastaya aittir
                .WithMany(h => h.Randevular)  // Bir hastanın birden fazla randevusu olabilir
                .HasForeignKey(r => r.HastaTC)  // Randevu tablosundaki HastaTC alanı, Hasta tablosuna referans verir
                .OnDelete(DeleteBehavior.Restrict); // Hasta silindiğinde ona bağlı randevular silinmez

            // Poliklinik ve Randevu arasındaki ilişki (1-N)
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Poliklinik)  // Her randevu bir polikliniğe aittir
                .WithMany(p => p.Randevular)  // Bir poliklinikte birden fazla randevu olabilir
                .HasForeignKey(r => r.PoliklinikId)  // Randevu tablosundaki PoliklinikId alanı, Poliklinik tablosuna referans verir
                .OnDelete(DeleteBehavior.Restrict); // Poliklinik silindiğinde ona bağlı randevular silinmez

            // Doktor ve Reçete arasındaki ilişki (1-N)
            modelBuilder.Entity<Recete>()
                .HasOne(re => re.Doktor)  // Her reçete bir doktora aittir
                .WithMany(d => d.Receteler)  // Bir doktorun birden fazla reçetesi olabilir
                .HasForeignKey(re => re.DoktorTC)  // Recete tablosundaki DoktorTC alanı, Doktor tablosuna referans verir
                .OnDelete(DeleteBehavior.Restrict); // Doktor silindiğinde reçeteler silinmez

            // Hasta ve Reçete arasındaki ilişki (1-N)
            modelBuilder.Entity<Recete>()
                .HasOne(re => re.Hasta)  // Her reçete bir hastaya aittir
                .WithMany(h => h.Receteler)  // Bir hastanın birden fazla reçetesi olabilir
                .HasForeignKey(re => re.HastaTC)  // Recete tablosundaki HastaTC alanı, Hasta tablosuna referans verir
                .OnDelete(DeleteBehavior.Restrict); // Hasta silindiğinde reçeteler silinmez

            // Doktor ve Poliklinik arasındaki ilişki (1-N)
            modelBuilder.Entity<Doktor>()
                .HasOne(d => d.Poliklinik)  // Her doktor bir polikliniğe bağlıdır
                .WithMany(p => p.Doktorlar)  // Bir poliklinikte birden fazla doktor olabilir
                .HasForeignKey(d => d.PoliklinikId)  // Doktor tablosundaki PoliklinikId alanı, Poliklinik tablosuna referans verir
                .OnDelete(DeleteBehavior.Restrict); // Poliklinik silindiğinde doktorlar silinmez

            base.OnModelCreating(modelBuilder);
        }

        // Veritabanının oluşturulmasını sağlar (Eğer veritabanı mevcut değilse)
        public void EnsureDatabaseCreated()
        {
            this.Database.EnsureCreated(); // Veritabanını oluşturur, eğer yoksa
        }
    }
}
