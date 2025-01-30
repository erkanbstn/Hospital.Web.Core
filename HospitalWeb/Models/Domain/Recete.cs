using System.ComponentModel.DataAnnotations;

namespace HospitalWeb.Models.Domain
{
    public class Recete : BaseEntity
    {
        [Key]
        public int? Id { get; set; } // Reçete ID
        public string? HastaTC { get; set; }
        public Hasta? Hasta { get; set; }
        public string? DoktorTC { get; set; }
        public Doktor? Doktor { get; set; }
        public string? Teshis { get; set; } // Teşhis
        public string? IlacAdi { get; set; } // İlaç Adı
    }
}