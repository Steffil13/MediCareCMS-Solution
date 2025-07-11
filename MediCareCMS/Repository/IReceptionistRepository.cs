using MediCareCMS.Models;
using MediCareCMS.ViewModel;

namespace MediCareCMS.Repository
{
    public interface IReceptionistRepository
    {
        // Patient
        void AddPatient(Patient patient);
        void UpdatePatient(Patient patient);
        void DeletePatient(int patientId);
        Patient GetPatientById(int patientId);
        List<Patient> GetAllPatients();
        List<Patient> SearchPatients(string keyword);

        //Appointment
        List<AppointmentViewModel> GetAllAppointments();
        public int AddAppointment(AppointmentViewModel model);
        void UpdateAppointment(AppointmentViewModel model);
        void DeleteAppointment(int appointmentId);

        // BILLING METHODS
        BillViewModel GetBillByAppointmentId(int appointmentId);
        // List<BillViewModel> GetBillableAppointments(); // Optional
        BillViewModel GetBillDetailsByAppointmentId(int appointmentId);
        List<BillViewModel> SearchBills(string appointmentNumber, string patientRegNum);


    }
}
