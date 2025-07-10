using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCareCMS.ViewModel
{
    public class StaffCreateViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Contact { get; set; }
        public int ReceptionistId { get; set; }
        public int DoctorId { get; set; }
        public int PharmacistId { get; set; }
        public int LabId { get; set; }

        public int DepartmentId { get; set; }

        [NotMapped]
        [BindNever]
        [ValidateNever]
        public List<SelectListItem> DepartmentList { get; set; }


    }
}
