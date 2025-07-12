using System.ComponentModel.DataAnnotations;

namespace MediCareCMS.Models
{
    public class Receptionist
    {
        public int ReceptionistId { get; set; }
        public string RecepEmpId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression("^[A-Za-z ]+$", ErrorMessage = "Name must contain only alphabets and spaces.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression("^[6-9][0-9]{9}$", ErrorMessage = "Phone number must start with 6, 7, 8, or 9 and be 10 digits.")]
        public string Contact { get; set; }

        public bool IsActive { get; set; }
       
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
