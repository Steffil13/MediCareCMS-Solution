using MediCareCMS.Models;
using MediCareCMS.ViewModels.Pharmacist;
using System.Data;
using Microsoft.Data.SqlClient;

namespace MediCareCMS.Repositories
{
    public class PharmacistRepository : IPharmacistRepository
    {
        private readonly string _connectionString;

        public PharmacistRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public Pharmacist Authenticate(string username, string password)
        {
            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("AuthenticatePharmacist", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", password);

            con.Open();
            using SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                return new Pharmacist
                {
                    PharmacistId = Convert.ToInt32(rdr["PharmacistId"]),
                    PhaEmpId = rdr["PhaEmpId"].ToString(),
                    Name = rdr["Name"].ToString(),
                    Contact = rdr["Contact"].ToString(),
                    Username = rdr["Username"].ToString(),
                    Password = rdr["Password"].ToString(),
                    IsActive = Convert.ToBoolean(rdr["IsActive"])
                };
            }
            return null;
        }

        public List<PrescriptionVM> GetAllPrescriptions()
        {
            List<PrescriptionVM> list = new();
            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("GetAllPrescriptions", con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            using SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new PrescriptionVM
                {
                    PrescriptionId = Convert.ToInt32(rdr["PrescriptionId"]),
                    PatientName = rdr["PatientName"].ToString(),
                    Symptoms = rdr["Symptoms"].ToString(),
                    Diagnosis = rdr["Diagnosis"].ToString(),
                    CreatedAt = Convert.ToDateTime(rdr["CreatedAt"])
                });
            }
            return list;
        }

        public List<PrescribedMedicineVM> GetMedicinesByPrescriptionId(int prescriptionId)
        {
            List<PrescribedMedicineVM> meds = new();
            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("GetMedicinesByPrescriptionId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PrescriptionId", prescriptionId);

            con.Open();
            using SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                meds.Add(new PrescribedMedicineVM
                {
                    MedicineName = rdr["MedicineName"].ToString(),
                    Dosage = rdr["Dosage"].ToString(),
                    Duration = rdr["Duration"].ToString()
                });
            }
            return meds;
        }

        public void IssueMedicines(int prescriptionId, int pharmacistId)
        {
            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("IssueMedicines", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PrescriptionId", prescriptionId);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void GeneratePharmacyBill(int prescriptionId, int pharmacistId, decimal totalAmount)
        {
            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("GeneratePharmacyBill", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PrescriptionId", prescriptionId);
            cmd.Parameters.AddWithValue("@PharmacistId", pharmacistId);
            cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public PatientHistoryVM GetPatientHistory(string patientId)
        {
            PatientHistoryVM history = new()
            {
                PatientName = "",
                Prescriptions = new List<PrescriptionVM>()
            };

            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("GetPatientHistory", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PatientId", patientId);

            con.Open();
            using SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (string.IsNullOrEmpty(history.PatientName))
                    history.PatientName = rdr["PatientName"].ToString();

                history.Prescriptions.Add(new PrescriptionVM
                {
                    PrescriptionId = Convert.ToInt32(rdr["PrescriptionId"]),
                    Symptoms = rdr["Symptoms"].ToString(),
                    Diagnosis = rdr["Diagnosis"].ToString(),
                    CreatedAt = Convert.ToDateTime(rdr["CreatedAt"])
                });
            }
            return history;
        }

        public List<MedicineStockVM> GetAllMedicines()
        {
            List<MedicineStockVM> meds = new();
            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("GetAllMedicines", con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            using SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                meds.Add(new MedicineStockVM
                {
                    MedicineId = Convert.ToInt32(rdr["MedicineId"]),
                    MedicineName = rdr["MedicineName"].ToString(),
                    Quantity = Convert.ToInt32(rdr["Quantity"])
                });
            }
            return meds;
        }

        public void UpdateMedicineStock(int medicineId, int quantity)
        {
            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("UpdateMedicineStock", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MedicineId", medicineId);
            cmd.Parameters.AddWithValue("@Quantity", quantity);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void AddNewMedicine(MedicineInventory medicine)
        {
            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("AddNewMedicine", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MedicineName", medicine.MedicineName);
            cmd.Parameters.AddWithValue("@Quantity", medicine.Quantity);
            con.Open();
            cmd.ExecuteNonQuery();
        }
        public List<PharmacyBillVM> GetBills(int pharmacistId)
        {
            List<PharmacyBillVM> list = new();
            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("GetPharmacyBills", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PharmacistId", pharmacistId);

            con.Open();
            using SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new PharmacyBillVM
                {
                    PrescriptionId = Convert.ToInt32(rdr["PrescriptionId"]),
                    PatientName = rdr["PatientName"].ToString(),
                    TotalAmount = Convert.ToDecimal(rdr["TotalAmount"]),
                    IssuedDate = Convert.ToDateTime(rdr["IssuedDate"])
                });
            }
            return list;
        }


        public PharmacyBillVM GetBillByPrescriptionId(int prescriptionId)
        {
            using SqlConnection conn = new(_connectionString);
            conn.Open();

            using SqlCommand cmd = new("GetPharmacyBillByPrescriptionId", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@PrescriptionId", prescriptionId);

            using SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                return new PharmacyBillVM
                {
                    PrescriptionId = rdr.GetInt32(0),
                    PatientName = rdr.GetString(1),
                    TotalAmount = rdr.GetDecimal(2),
                    IssuedDate = rdr.GetDateTime(3)
                };
            }
            return null;
        }

    }
}
