namespace HospitalWeb.Models.DTO
{
    public class RegisterDTO
    {
        public string TC { get; set; }
        public string Sifre { get; set; }
        public string Isim { get; set; }
        public string SertifikaNo { get; set; }
        public int? PoliklinikId { get; set; }
    }
}