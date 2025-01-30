using System.ComponentModel.DataAnnotations;

namespace HospitalWeb.Models.Domain
{
    public class Randevu : BaseEntity
    {
        public int? HastaId { get; set; }
        public Hasta? Hasta { get; set; }
        public int? DoktorId { get; set; }
        public Doktor? Doktor { get; set; }
        public int? PoliklinikId { get; set; }
        public Poliklinik? Poliklinik { get; set; }
        public DateTime Tarih { get; set; } // Randevu Tarihi
    }
}