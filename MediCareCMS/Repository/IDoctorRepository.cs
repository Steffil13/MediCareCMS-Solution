using MediCareCMS.Models;

namespace MediCareCMS.Repository
{
    public interface IDoctorRepository
    {
        List<Appointment> GetAppointmentsByDoctorAndDate(int doctorId, DateTime date);
        Appointment GetAppointmentById(int appointmentId);
        PatientSummary GetPatientSummary(string patientId);
        List<MedicineInventory> GetMedicineInventory();
        int SavePrescription(Prescription prescription);
        void AddSchedule(DoctorSchedule schedule);
        void DeleteSchedule(int id);
        void UpdateSchedule(DoctorSchedule schedule);
        DoctorSchedule GetScheduleById(int id);

        void UpdateDoctorSchedule(int doctorId, DateTime date, bool isAvailable);
        List<DoctorSchedule> GetDoctorSchedule(int doctorId);
        void MarkAppointmentAsConsulted(int appointmentId);
        void SavePrescriptionLabTest(int prescriptionId, int labTestId); // NEW
        List<LabTest> GetAllLabTests();
        



    }
}

