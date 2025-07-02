namespace MediCareCMS.Models
{
    public class Patient
    {
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public int Age { get; set; }
        public bool IsActive { get; set; }
    }
}
