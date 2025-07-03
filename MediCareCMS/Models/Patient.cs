namespace MediCareCMS.Models
{
    public class Patient
    {
        public string PatientId { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        public bool IsActive { get; set; }
    }
}
