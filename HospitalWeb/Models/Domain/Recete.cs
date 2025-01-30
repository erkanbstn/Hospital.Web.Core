using System.ComponentModel.DataAnnotations;

namespace HospitalWeb.Models.Domain
{
    public class Recete : BaseEntity
    {
        public int? HastaId { get; set; }
        public Hasta? Hasta { get; set; }
        public int? DoktorId { get; set; }
        public Doktor? Doktor { get; set; }
        public string? Teshis { get; set; } // Teşhis
        public string? IlacAdi { get; set; } // İlaç Adı
    }
}