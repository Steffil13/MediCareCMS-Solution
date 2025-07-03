using System.ComponentModel.DataAnnotations;

namespace MediCareCMS.Models
{
    public class Lab
    {
        [Key]
        public int LabId { get; set; }

        [Required(ErrorMessage = "Lab Employee ID is required.")]
        [StringLength(10, ErrorMessage = "Lab Employee ID cannot exceed 10 characters.")]
        [Display(Name = "Lab Employee ID")]
        public string LabEmpId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [StringLength(20, ErrorMessage = "Contact number cannot exceed 20 digits.")]
        [Display(Name = "Contact Number")]
        [DataType(DataType.PhoneNumber)]
        public string Contact { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(100, ErrorMessage = "Username cannot exceed 100 characters.")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "Password cannot exceed 100 characters.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
}
