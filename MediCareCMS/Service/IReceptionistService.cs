using MediCareCMS.Models;
using MediCareCMS.ViewModel;

namespace MediCareCMS.Service
{
    public interface IReceptionistService
    {
        void AddPatient(Patient patient);
        void UpdatePatient(Patient patient);
        void DeletePatient(int patientId);
        Patient GetPatientById(int patientId);
        List<Patient> GetAllPatients();
        List<Patient> SearchPatients(string keyword);

        // ======== APPOINTMENT METHODS ========
        public int AddAppointment(AppointmentViewModel appointment);
        void UpdateAppointment(AppointmentViewModel appointment);
        void DeleteAppointment(int appointmentId);
        List<AppointmentViewModel> GetAllAppointments();



        // BILLING
        BillViewModel GetBillByAppointmentId(int appointmentId);
        //List<BillViewModel> GetBillableAppointments(); // Optional for billing list view
        BillViewModel GenerateBillForAppointment(int appointmentId);

    }

}
