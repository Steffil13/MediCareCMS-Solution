using System.ComponentModel.DataAnnotations;

namespace MediCareCMS.ViewModel
{
    public class DepartmentViewModel
    {
        [Required(ErrorMessage = "Department Name is required")]
        public string DepartmentName { get; set; }
    }

}
