using System.ComponentModel.DataAnnotations;

namespace HospitalWeb.Models.Domain
{
    public class Poliklinik : BaseEntity
    {
        public string? Ad { get; set; } // Poliklinik Adı
        public ICollection<Doktor> Doktorlar { get; set; } = new List<Doktor>();
        public ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
    }
}