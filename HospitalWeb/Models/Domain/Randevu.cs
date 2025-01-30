using System.ComponentModel.DataAnnotations;

namespace HospitalWeb.Models.Domain
{
    public class Randevu : BaseEntity
    {
        [Key]
        public int? Id { get; set; } // Randevu ID
        public string? HastaTC { get; set; }
        public Hasta? Hasta { get; set; }
        public string? DoktorTC { get; set; }
        public Doktor? Doktor { get; set; }
        public int? PoliklinikId { get; set; }
        public Poliklinik? Poliklinik { get; set; }
        public DateTime Tarih { get; set; } // Randevu Tarihi
    }
}