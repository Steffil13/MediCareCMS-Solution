﻿using MediCareCMS.Models;

namespace MediCareCMS.Service
{
    public interface IDoctorService
    {
        List<Appointment> GetAppointmentsByDate(int doctorId, DateTime date);
        Appointment GetAppointmentById(int appointmentId);
        PatientSummary GetPatientSummary(int patientId);
        //List<VisitedPatient> GetPatientHistory(int doctorId, string searchTerm);
        List<PatientHistory> GetHistoryByDoctorId(int doctorId);
        List<VisitedPatient> GetPatientHistoryByDoctorId(int doctorId, string searchTerm);

        void SavePatientHistory(PatientHistory history);

        //List<PatientHistory> GetHistory(int patientId);
        List<MedicineInventory> GetMedicineInventory();
        int SavePrescription(Prescription prescription);
        void UpdateDoctorSchedule(int doctorId, DateTime date, bool isAvailable);
        void Add(DoctorSchedule schedule);
        void Update(DoctorSchedule schedule);
        DoctorSchedule GetScheduleById(int id);
        List<DoctorSchedule> GetDoctorSchedule(int doctorId);
        void Delete(int id);
        void MarkAppointmentAsConsulted(int appointmentId);

        void SavePrescriptionLabTests(int prescriptionId, List<int> labTestId);
        List<LabTest> GetAllLabTests();
        List<Doctor> GetAllDoctors();

        // ✅ New method to get departments for dropdown
        List<Department> GetAllDepartments();

        void AddDepartment(Department department);

    }
}
