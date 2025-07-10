using MediCareCMS.Models;
using MediCareCMS.Repository;
using System.Collections.Generic;

namespace MediCareCMS.Service
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            this.doctorRepository = doctorRepository;
        }

        public List<Appointment> GetAppointmentsByDate(int doctorId, DateTime date)
        {
            return doctorRepository.GetAppointmentsByDoctorAndDate(doctorId, date);
        }

        public Appointment GetAppointmentById(int appointmentId)
        {
            return doctorRepository.GetAppointmentById(appointmentId);
        }

        public PatientSummary GetPatientSummary(int patientId)
        {
            return doctorRepository.GetPatientSummary(patientId);
        }

        public List<VisitedPatient> GetPatientHistory(int doctorId, string searchTerm)
        {
            return doctorRepository.GetPatientHistory(doctorId, searchTerm);
        }

        public List<MedicineInventory> GetMedicineInventory()
        {
            return doctorRepository.GetMedicineInventory();
        }

        public int SavePrescription(Prescription prescription)
        {
            return doctorRepository.SavePrescription(prescription);
        }

        public void UpdateDoctorSchedule(int doctorId, DateTime date, bool isAvailable)
        {
            doctorRepository.UpdateDoctorSchedule(doctorId, date, isAvailable);
        }

        public void Add(DoctorSchedule schedule)
        {
            doctorRepository.AddSchedule(schedule);
        }

        public void Update(DoctorSchedule schedule)
        {
            doctorRepository.UpdateSchedule(schedule);
        }

        public void Delete(int id)
        {
            doctorRepository.DeleteSchedule(id);
        }

        public DoctorSchedule GetScheduleById(int id)
        {
            return doctorRepository.GetScheduleById(id);
        }

        public List<DoctorSchedule> GetDoctorSchedule(int doctorId)
        {
            return doctorRepository.GetDoctorSchedule(doctorId);
        }

        public void MarkAppointmentAsConsulted(int appointmentId)
        {
            doctorRepository.MarkAppointmentAsConsulted(appointmentId);
        }

        public void SavePrescriptionLabTests(int prescriptionId, List<int> labTestId)
        {
            doctorRepository.SavePrescriptionLabTests(prescriptionId, labTestId);
        }

        public List<LabTest> GetAllLabTests()
        {
            return doctorRepository.GetAllLabTests();
        }

        public List<Doctor> GetAllDoctors()
        {
            return doctorRepository.GetAllDoctors();
        }

        // ✅ NEW METHOD IMPLEMENTED
        public List<Department> GetAllDepartments()
        {
            return doctorRepository.GetAllDepartments();
        }

        public void AddDepartment(Department department)
        {
            doctorRepository.AddDepartment(department);
        }

    }
}
