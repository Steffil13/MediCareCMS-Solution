using MediCareCMS.Models;
using MediCareCMS.Repository;
using MediCareCMS.ViewModel;
using System.Collections.Generic;

namespace MediCareCMS.Service
{
    public class ReceptionistService : IReceptionistService
    {
        private readonly IReceptionistRepository _repo;

        public ReceptionistService(IReceptionistRepository repo)
        {
            _repo = repo;
        }

        // ========== PATIENT METHODS ==========
        public void AddPatient(Patient patient) => _repo.AddPatient(patient);

        public void UpdatePatient(Patient patient) => _repo.UpdatePatient(patient);

        public void DeletePatient(int patientId) => _repo.DeletePatient(patientId);

        public Patient GetPatientById(int patientId) => _repo.GetPatientById(patientId);

        public List<Patient> GetAllPatients() => _repo.GetAllPatients();

        public List<Patient> SearchPatients(string keyword) => _repo.SearchPatients(keyword);

        // ========== APPOINTMENT METHODS ==========
        public List<AppointmentViewModel> GetAllAppointments() => _repo.GetAllAppointments();

        public int AddAppointment(AppointmentViewModel model)
        {
            // AppointmentNumber and Token will be generated inside the stored procedure
            return _repo.AddAppointment(model);
        }

        public void UpdateAppointment(AppointmentViewModel model)
        {
            // Token will be recalculated inside the stored procedure if needed
            _repo.UpdateAppointment(model);
        }

        public void DeleteAppointment(int appointmentId) =>
            _repo.DeleteAppointment(appointmentId);

        public BillViewModel GetBillByAppointmentId(int id) =>
            _repo.GetBillByAppointmentId(id);

        public BillViewModel GenerateBillForAppointment(int appointmentId)
        {
            return _repo.GetBillDetailsByAppointmentId(appointmentId);
        }


    }
}
