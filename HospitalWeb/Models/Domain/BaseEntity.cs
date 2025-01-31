using System.ComponentModel.DataAnnotations;

namespace HospitalWeb.Models.Domain
{
    public class BaseEntity
    {
        [Key]
        public int? Id { get; set; } 
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? DeletedAt { get; set; }
        public bool? Status { get; set; } = true;
    }
}