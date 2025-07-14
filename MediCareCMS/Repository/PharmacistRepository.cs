using MediCareCMS.Models;
using MediCareCMS.ViewModels.Pharmacist;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;

namespace MediCareCMS.Repositories
{
    public class PharmacistRepository : IPharmacistRepository
    {
        private readonly string _cs;
        public PharmacistRepository(IConfiguration cfg) =>
            _cs = cfg.GetConnectionString("DefaultConnection");

        /*──────────────────── 1. Authenticate ────────────────────────*/
        public Pharmacist? Authenticate(string username, string password)
        {
            using var con = new SqlConnection(_cs);
            using var cmd = new SqlCommand("AuthenticatePharmacist", con)
            { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", password);

            con.Open();
            using var rd = cmd.ExecuteReader();
            return rd.Read()
                ? new Pharmacist
                {
                    PharmacistId = rd.GetInt32(rd.GetOrdinal("PharmacistId")),
                    PhaEmpId = rd["PhaEmpId"].ToString(),
                    Name = rd["Name"].ToString(),
                    Contact = rd["Contact"].ToString(),
                    Username = rd["Username"].ToString(),
                    Password = rd["Password"].ToString(),
                    IsActive = rd.GetBoolean(rd.GetOrdinal("IsActive"))
                }
                : null;
        }

        /*──────────────────── 2. All prescriptions ───────────────────*/
        public List<PrescriptionVM> GetAllPrescriptions()
        {
            var list = new List<PrescriptionVM>();
            using var con = new SqlConnection(_cs);
            using var cmd = new SqlCommand("GetAllPrescriptions", con)
            { CommandType = CommandType.StoredProcedure };

            con.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new PrescriptionVM
                {
                    PrescriptionId = rd.GetInt32(rd.GetOrdinal("PrescriptionId")),
                    PatientName = rd["PatientName"].ToString(),
                    Symptoms = rd["Symptoms"].ToString(),
                    Diagnosis = rd["Diagnosis"].ToString(),
                    CreatedAt = rd.GetDateTime(rd.GetOrdinal("CreatedAt"))
                });
            }
            return list;
        }

        /*──────────────────── 3. Medicines by prescription ───────────*/
        public List<PrescribedMedicineVM> GetMedicinesByPrescriptionId(int prescriptionId)
        {
            var meds = new List<PrescribedMedicineVM>();
            using var con = new SqlConnection(_cs);
            using var cmd = new SqlCommand("GetMedicinesByPrescriptionId", con)
            { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@PrescriptionId", prescriptionId);

            con.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                meds.Add(new PrescribedMedicineVM
                {
                    MedicineName = rd["MedicineName"].ToString(),
                    Dosage = rd["Dosage"].ToString(),
                    Duration = rd["Duration"].ToString()
                });
            }
            return meds;
        }

        /*──────────────────── 4. Issue medicines ─────────────────────*/
        public void IssueMedicines(int prescriptionId, int pharmacistId)
        {
            using var con = new SqlConnection(_cs);
            using var cmd = new SqlCommand("sp_Pha_IssueMedicines", con)
            { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@PrescriptionId", prescriptionId);
            cmd.Parameters.AddWithValue("@PharmacistId", pharmacistId);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        /*──────────────────── 5. Generate bill ───────────────────────*/
        public void GeneratePharmacyBill(int prescriptionId, int pharmacistId, decimal totalAmount)
        {
            using (SqlConnection con = new SqlConnection(_cs))
            {
                string query = @"INSERT INTO PharmacyBills (PrescriptionId, PharmacistId, TotalAmount, IssuedDate)
                         VALUES (@PrescriptionId, @PharmacistId, @TotalAmount, GETDATE())";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@PrescriptionId", prescriptionId);
                cmd.Parameters.AddWithValue("@PharmacistId", pharmacistId);
                cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }


        /*──────────────────── 6. Patient history ─────────────────────*/
        public PatientHistoryVM GetPatientHistory(string patientId)
        {
            var history = new PatientHistoryVM
            {
                PatientName = string.Empty,
                Prescriptions = new List<PrescriptionVM>()
            };

            using var con = new SqlConnection(_cs);
            using var cmd = new SqlCommand("sp_Pha_GetPatientHistory", con)
            { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@PatientId", patientId);

            con.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                if (string.IsNullOrEmpty(history.PatientName))
                    history.PatientName = rd["PatientName"].ToString();

                history.Prescriptions.Add(new PrescriptionVM
                {
                    PrescriptionId = rd.GetInt32(rd.GetOrdinal("PrescriptionId")),
                    Symptoms = rd["Symptoms"].ToString(),
                    Diagnosis = rd["Diagnosis"].ToString(),
                    CreatedAt = rd.GetDateTime(rd.GetOrdinal("CreatedAt"))
                });
            }
            return history;
        }

        /*──────────────────── 7. Inventory look‑ups ──────────────────*/
        public List<MedicineStockVM> GetAllMedicines()
        {
            var meds = new List<MedicineStockVM>();
            using var con = new SqlConnection(_cs);
            using var cmd = new SqlCommand("GetAllMedicines", con)
            { CommandType = CommandType.StoredProcedure };

            con.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                meds.Add(new MedicineStockVM
                {
                    MedicineId = rd.GetInt32(rd.GetOrdinal("MedicineId")),
                    MedicineName = rd["MedicineName"].ToString(),
                    Quantity = rd.GetInt32(rd.GetOrdinal("Quantity"))
                });
            }
            return meds;
        }

        public void UpdateMedicineStock(int medicineId, int quantity)
        {
            using var con = new SqlConnection(_cs);
            using var cmd = new SqlCommand("UpdateMedicineStock", con)
            { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@MedicineId", medicineId);
            cmd.Parameters.AddWithValue("@Quantity", quantity);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void AddNewMedicine(MedicineInventory medicine)
        {
            using var con = new SqlConnection(_cs);
            using var cmd = new SqlCommand("AddNewMedicine", con)
            { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@MedicineName", medicine.MedicineName);
            cmd.Parameters.AddWithValue("@Quantity", medicine.Quantity);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        /*──────────────────── 8. Billing look‑ups ────────────────────*/
        public List<PharmacyBillVM> GetBills(int pharmacistId)
        {
            var list = new List<PharmacyBillVM>();
            using var con = new SqlConnection(_cs);
            using var cmd = new SqlCommand("GetPharmacyBills", con)
            { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@PharmacistId", pharmacistId);

            con.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new PharmacyBillVM
                {
                    PrescriptionId = rd.GetInt32(rd.GetOrdinal("PrescriptionId")),
                    PatientName = rd["PatientName"].ToString(),
                    TotalAmount = rd.GetDecimal(rd.GetOrdinal("TotalAmount")),
                    IssuedDate = rd.GetDateTime(rd.GetOrdinal("IssuedDate"))
                });
            }
            return list;
        }

        public PharmacyBillVM? GetBillByPrescriptionId(int prescriptionId)
        {
            using var con = new SqlConnection(_cs);
            using var cmd = new SqlCommand("GetPharmacyBillByPrescriptionId", con)
            { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@PrescriptionId", prescriptionId);

            con.Open();
            using var rd = cmd.ExecuteReader();
            return rd.Read()
                ? new PharmacyBillVM
                {
                    PrescriptionId = rd.GetInt32(0),
                    PatientName = rd.GetString(1),
                    TotalAmount = rd.GetDecimal(2),
                    IssuedDate = rd.GetDateTime(3)
                }
                : null;
        }
    }
}
