using MediCareCMS.Models;
using MediCareCMS.ViewModels.Pharmacist;

namespace MediCareCMS.Repositories
{
    public interface IPharmacistRepository
    {
        Pharmacist Authenticate(string username, string password);

        List<PrescriptionVM> GetAllPrescriptions();
        List<PrescribedMedicineVM> GetMedicinesByPrescriptionId(int prescriptionId);

        void IssueMedicines(int prescriptionId, int pharmacistId);
        void GeneratePharmacyBill(int prescriptionId, int pharmacistId, decimal totalAmount);

        PatientHistoryVM GetPatientHistory(string patientId);
        PharmacyBillVM GetBillByPrescriptionId(int prescriptionId);


        List<MedicineStockVM> GetAllMedicines();
        void UpdateMedicineStock(int medicineId, int quantity);
        void AddNewMedicine(MedicineInventory medicine);
        List<PharmacyBillVM> GetBills(int pharmacistId);
    }
}
