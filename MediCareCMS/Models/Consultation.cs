namespace MediCareCMS.Models
{
    public class Consultation
    {
            public int consultationId { get; set; }
            public int AppointmentId { get; set; }
            public string Symptoms { get; set; }
            public string Diagnosis { get; set; }
        }
    }

