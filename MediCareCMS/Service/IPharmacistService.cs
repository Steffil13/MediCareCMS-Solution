using MediCareCMS.Models;
using MediCareCMS.ViewModels.Pharmacist;


namespace MediCareCMS.Services
{
    public interface IPharmacistService
    {
        Pharmacist Authenticate(string username, string password);

        List<PrescriptionVM> GetAllPrescriptions();
        List<PrescribedMedicineVM> GetMedicinesByPrescriptionId(int prescriptionId);

        void IssueMedicines(int prescriptionId, int pharmacistId);
        void GeneratePharmacyBill(int prescriptionId, int pharmacistId, decimal totalAmount);

        PatientHistoryVM GetPatientHistory(string patientId);

        List<MedicineStockVM> GetAllMedicines();
        void UpdateMedicineStock(int medicineId, int quantity);
        void AddNewMedicine(MedicineInventory medicine);
        PharmacyBillVM GetBillByPrescriptionId(int prescriptionId);


        List<PharmacyBillVM> GetBills(int pharmacistId);
        

    }
}
