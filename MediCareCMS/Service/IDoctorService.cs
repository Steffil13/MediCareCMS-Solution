using MediCareCMS.Models;

namespace MediCareCMS.Service
{
    public interface IDoctorService
    {

        List<Appointment> GetAppointmentsByDate(int doctorId, DateTime date);
        Appointment GetAppointmentById(string appointmentId);
        PatientSummary GetPatientSummary(string patientId);
        List<MedicineInventory> GetMedicineInventory();
        void SavePrescription(Prescription prescription);
        void UpdateDoctorSchedule(int doctorId, DateTime date, bool isAvailable);
        List<DoctorSchedule> GetDoctorSchedule(int doctorId);
        void MarkAppointmentAsConsulted(string appointmentId);


    }
}
