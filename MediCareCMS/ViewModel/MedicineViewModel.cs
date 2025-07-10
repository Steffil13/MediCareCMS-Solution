using System.ComponentModel.DataAnnotations;

namespace MediCareCMS.ViewModel
{
    public class MedicineViewModel
    {
        [Required]
        public string MedicineName { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
