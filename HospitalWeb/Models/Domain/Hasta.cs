using System.ComponentModel.DataAnnotations;

namespace HospitalWeb.Models.Domain
{
    public class Hasta : BaseEntity
    {
        [Key]
        public string? TC { get; set; } // Hasta TC
        public string? Isim { get; set; } // Hasta Adı Soyadı
        public string? Sifre { get; set; } // Hasta Şifre
        public ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
        public ICollection<Recete> Receteler { get; set; } = new List<Recete>();
    }
}