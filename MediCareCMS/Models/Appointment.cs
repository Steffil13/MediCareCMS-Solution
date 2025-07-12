using System.ComponentModel.DataAnnotations;

namespace MediCareCMS.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression("^[A-Za-z ]+$", ErrorMessage = "Name must contain only alphabets and spaces.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }
      
        public int Token { get; set; }
        public bool IsConsulted { get; set; }
    }
}
