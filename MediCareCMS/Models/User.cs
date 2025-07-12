using System.ComponentModel.DataAnnotations;

namespace MediCareCMS.Models
{
    public class User
    {
        public int UserId { get; set; }

        
        [Required(ErrorMessage = "Name is required.")]
        
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public int RoleId { get; set; }
        public string Role { get; set; }

        public bool IsActive { get; set; }
    }
}
