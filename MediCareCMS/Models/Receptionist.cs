using System.ComponentModel.DataAnnotations;

namespace MediCareCMS.Models
{
    public class Receptionist
    {
        public int ReceptionistId { get; set; }
        public string RecepEmpId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Contact { get; set; }

        public bool IsActive { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
    }
}
