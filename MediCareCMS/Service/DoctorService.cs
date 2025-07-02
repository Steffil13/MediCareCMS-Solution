using MediCareCMS.Models;
using MediCareCMS.Repository;

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

        public Appointment GetAppointmentById(string appointmentId)
        {
            return doctorRepository.GetAppointmentById(appointmentId);
        }

        public PatientSummary GetPatientSummary(string patientId)
        {
            return doctorRepository.GetPatientSummary(patientId);
        }

        public List<MedicineInventory> GetMedicineInventory()
        {
            return doctorRepository.GetMedicineInventory();
        }

        public void SavePrescription(Prescription prescription)
        {
            doctorRepository.SavePrescription(prescription);
        }

        public void UpdateDoctorSchedule(int doctorId, DateTime date, bool isAvailable)
        {
            doctorRepository.UpdateDoctorSchedule(doctorId, date, isAvailable);
        }

        public List<DoctorSchedule> GetDoctorSchedule(int doctorId)
        {
            return doctorRepository.GetDoctorSchedule(doctorId);
        }
    }
}
