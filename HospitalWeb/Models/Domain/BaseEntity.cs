namespace HospitalWeb.Models.Domain
{
    public class BaseEntity
    {
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? DeletedAt { get; set; } = DateTime.Now;
        public bool? Status { get; set; } = true;
    }
}