using System.ComponentModel.DataAnnotations;

namespace HospitalWeb.Models.Domain
{
    public class Doktor : BaseEntity
    {
        public string? TC { get; set; } // Doktor TC
        public string? Isim { get; set; } // Doktor Adı Soyadı
        public string? Uzmanlik { get; set; } // Doktor Uzmanlığı
        public string? Sifre { get; set; } // Doktor Giriş Şifresi
        public int? PoliklinikId { get; set; } // Doktorun bağlı olduğu poliklinik
        public string? SertifikaNo { get; set; } // Doktorun sertifika numarası
        public Poliklinik? Poliklinik { get; set; }
        public ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
        public ICollection<Recete> Receteler { get; set; } = new List<Recete>();
    }
}
