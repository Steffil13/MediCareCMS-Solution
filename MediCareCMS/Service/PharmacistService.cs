using MediCareCMS.Models;
using MediCareCMS.Repositories;
using MediCareCMS.ViewModels.Pharmacist;

namespace MediCareCMS.Services
{
    public class PharmacistService : IPharmacistService
    {
        private readonly IPharmacistRepository _repository;

        public PharmacistService(IPharmacistRepository repository)
        {
            _repository = repository;
        }

        public PharmacyBillVM GetBillByPrescriptionId(int prescriptionId)
        {
            return _repository.GetBillByPrescriptionId(prescriptionId);
        }


        public List<PharmacyBillVM> GetBills(int pharmacistId)
        {
            return _repository.GetBills(pharmacistId);
        }


        public Pharmacist Authenticate(string username, string password)
        {
            return _repository.Authenticate(username, password);
        }

        public List<PrescriptionVM> GetAllPrescriptions()
        {
            return _repository.GetAllPrescriptions();
        }

        public List<PrescribedMedicineVM> GetMedicinesByPrescriptionId(int prescriptionId)
        {
            return _repository.GetMedicinesByPrescriptionId(prescriptionId);
        }

        public void IssueMedicines(int prescriptionId, int pharmacistId)
        {
            _repository.IssueMedicines(prescriptionId, pharmacistId);
        }

        public void GeneratePharmacyBill(int prescriptionId, int pharmacistId, decimal totalAmount)
        {
            _repository.GeneratePharmacyBill(prescriptionId, pharmacistId, totalAmount);
        }

        public PatientHistoryVM GetPatientHistory(string patientId)
        {
            return _repository.GetPatientHistory(patientId);
        }

        public List<MedicineStockVM> GetAllMedicines()
        {
            return _repository.GetAllMedicines();
        }

        public void UpdateMedicineStock(int medicineId, int quantity)
        {
            _repository.UpdateMedicineStock(medicineId, quantity);
        }

        public void AddNewMedicine(MedicineInventory medicine)
        {
            _repository.AddNewMedicine(medicine);
        }
    }
}
