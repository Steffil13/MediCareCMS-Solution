using MediCareCMS.Models;

namespace MediCareCMS.Repository
{
    public interface IDoctorRepository
    {
            List<Appointment> GetAppointmentsByDoctorAndDate(int doctorId, DateTime date);
            Appointment GetAppointmentById(string appointmentId);
            PatientSummary GetPatientSummary(string patientId);
            List<MedicineInventory> GetMedicineInventory();
            void SavePrescription(Prescription prescription);
            void UpdateDoctorSchedule(int doctorId, DateTime date, bool isAvailable);
            List<DoctorSchedule> GetDoctorSchedule(int doctorId);
        }
    }

