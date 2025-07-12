using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace MediCareCMS.Models
{
    public class Patient
    {
        public int PatientId { get; set; }             

        [BindNever]
        [ValidateNever]
        public string PatientRegNum { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression("^[A-Za-z ]+$", ErrorMessage = "Name must contain only alphabets and spaces.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Gender must be Male, Female, or Other.")]
        public string Gender { get; set; }
        
        [Required(ErrorMessage = "Age is required.")]
        [Range(0, 120, ErrorMessage = "Age must be between 0 and 120.")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression("^[6-9][0-9]{9}$", ErrorMessage = "Phone number must start with 6, 7, 8, or 9 and be 10 digits.")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Address must be at least 5 characters and no more than 200 characters.")]
        public string Address { get; set; }
    }

}
