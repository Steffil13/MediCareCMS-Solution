using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MediCareCMS.Models
{
    public class Patient
    {
        public int PatientId { get; set; }             

        [BindNever]
        [ValidateNever]
        public string PatientRegNum { get; set; }         
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }

}
