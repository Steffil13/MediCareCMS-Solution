using System.ComponentModel.DataAnnotations;

namespace MediCareCMS.ViewModel
{
    public class LabTestViewModel
    {
        [Required]
        public string TestName { get; set; }

        [Required]
        public decimal TestPrice { get; set; }
    }

}
