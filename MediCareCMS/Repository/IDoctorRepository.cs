using MediCareCMS.Models;

namespace MediCareCMS.Repository
{
    public interface IDoctorRepository
    {
        List<Appointment> GetAppointmentsByDoctorAndDate(int doctorId, DateTime date);
        Appointment GetAppointmentById(int appointmentId);
        PatientSummary GetPatientSummary(int patientId);
        //List<VisitedPatient> GetPatientHistory(int doctorId, string searchTerm);
        List<PatientHistory> GetHistoryByDoctorId(int doctorId);
        void SavePatientHistory(PatientHistory history);

        //List<PatientHistory> GetHistoryByPatientId(int patientId);

        List<MedicineInventory> GetMedicineInventory();
        int SavePrescription(Prescription prescription);
        void AddSchedule(DoctorSchedule schedule);
        void DeleteSchedule(int id);
        void UpdateSchedule(DoctorSchedule schedule);
        DoctorSchedule GetScheduleById(int id);

        void UpdateDoctorSchedule(int doctorId, DateTime date, bool isAvailable);
        List<DoctorSchedule> GetDoctorSchedule(int doctorId);
        void MarkAppointmentAsConsulted(int appointmentId);
        List<LabTest> GetAllLabTests();
        void SavePrescriptionLabTests(int prescriptionId, List<int> labTestIds);
        List<Doctor> GetAllDoctors();

        // ✅ NEW: Fetch list of departments
        List<Department> GetAllDepartments();

        void AddDepartment(Department department);

    }
}
