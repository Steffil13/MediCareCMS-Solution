﻿using MediCareCMS.Models;
using MediCareCMS.ViewModel;

namespace MediCareCMS.Repository
{
    public interface IAdminRepository
    {
        List<Receptionist> GetReceptionists();
        void AddReceptionist(StaffCreateViewModel receptionist);
        Receptionist GetReceptionistById(int id);
        void UpdateReceptionist(StaffCreateViewModel receptionist);
        void DeactivateReceptionist(int id);

        List<Doctor> GetDoctors();
        void AddDoctor(StaffCreateViewModel doctor);
        Doctor GetDoctorById(int id);
        void UpdateDoctor(StaffCreateViewModel doctor);
        void DeactivateDoctor(int id);

        List<Pharmacist> GetPharmacists();
        void AddPharmacist(StaffCreateViewModel pharmacist);
        Pharmacist GetPharmacistById(int id);
        void UpdatePharmacist(StaffCreateViewModel pharmacist);
        void DeactivatePharmacist(int id);

        List<Lab> GetLabTechnicians();
        void AddLabTechnician(StaffCreateViewModel lab);
        Lab GetLabTechnicianById(int id);
        void UpdateLabTechnician(StaffCreateViewModel lab);
        void DeactivateLabTechnician(int id);

        void AddMedicine(MedicineViewModel m);

        List<LabTest> GetLabTests();
        void AddLabTest(LabTestViewModel test);
        void SoftDeleteLabTest(int testId);

    }
}
