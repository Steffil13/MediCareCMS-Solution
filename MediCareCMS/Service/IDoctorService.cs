using MediCareCMS.Models;

namespace MediCareCMS.Service
{
    public interface IDoctorService
    {

        List<Appointment> GetAppointmentsByDate(int doctorId, DateTime date);
        Appointment GetAppointmentById(int appointmentId);
        PatientSummary GetPatientSummary(string patientId);
        List<VisitedPatient> GetPatientHistory(int doctorId, string searchTerm);

        List<MedicineInventory> GetMedicineInventory();
        int SavePrescription(Prescription prescription);
        void UpdateDoctorSchedule(int doctorId, DateTime date, bool isAvailable);
        void Add(DoctorSchedule schedule);
        void Update(DoctorSchedule schedule);
        DoctorSchedule GetScheduleById(int id);
        //void DeleteSchedule(int id);


        List<DoctorSchedule> GetDoctorSchedule(int doctorId);
        void Delete(int id);
        void MarkAppointmentAsConsulted(int appointmentId);

        void SavePrescriptionLabTests(int prescriptionId, List<int> labTestId);
        List<LabTest> GetAllLabTests();
        List<Doctor> GetAllDoctors();





    }
}
