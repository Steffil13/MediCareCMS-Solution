namespace MediCareCMS.Models
{
    public class Doctor
    {
      
            public int DoctorId { get; set; }
            public string DoctorName { get; set; }
            public string DocEmpId { get; set; }
            public string Phone { get; set; }
            public int DeptId { get; set; }
            public int UserId { get; set; }
            public bool IsActive { get; set; }

            public string Username { get; set; }
            public string Password { get; set; }
            public string Name { get; set; }
            public string Contact { get; set; }
    }
    }

