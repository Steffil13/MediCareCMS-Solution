﻿using MediCareCMS.Models;
using MediCareCMS.ViewModel;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace MediCareCMS.Repository
{
    public class ReceptionistRepository : IReceptionistRepository
    {
        private readonly string _connectionString;

        public ReceptionistRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ========== PATIENT ==========
        public void AddPatient(Patient patient)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_AddPatient", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", patient.Name);
                    cmd.Parameters.AddWithValue("@Gender", patient.Gender);
                    cmd.Parameters.AddWithValue("@Age", patient.Age);
                    cmd.Parameters.AddWithValue("@Phone", patient.Phone);
                    cmd.Parameters.AddWithValue("@Address", patient.Address);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Log error if needed
                throw new Exception("Failed to add patient: " + ex.Message);
            }
        }


        public void UpdatePatient(Patient patient)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdatePatient", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PatientId", patient.PatientId);
                cmd.Parameters.AddWithValue("@Name", patient.Name);
                cmd.Parameters.AddWithValue("@Gender", patient.Gender);
                cmd.Parameters.AddWithValue("@Age", patient.Age);
                cmd.Parameters.AddWithValue("@Phone", patient.Phone);
                cmd.Parameters.AddWithValue("@Address", patient.Address);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeletePatient(int patientId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_DeletePatient", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PatientId", patientId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<Patient> GetAllPatients()
        {
            var patients = new List<Patient>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllPatients", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    patients.Add(new Patient
                    {
                        PatientId = Convert.ToInt32(reader["PatientId"]),
                        PatientRegNum = reader["PatientRegNum"].ToString(),
                        Name = reader["Name"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        Age = Convert.ToInt32(reader["Age"]),
                        Phone = reader["Contact"].ToString(),
                        Address = reader["Address"].ToString()
                    });
                }
            }

            return patients;
        }

        public Patient GetPatientById(int patientId)
        {
            Patient patient = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetPatientById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PatientId", patientId);

                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    patient = new Patient
                    {
                        PatientId = Convert.ToInt32(reader["PatientId"]),
                        PatientRegNum = reader["PatientRegNum"].ToString(),
                        Name = reader["Name"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        Age = Convert.ToInt32(reader["Age"]),
                        Phone = reader["Contact"].ToString(),
                        Address = reader["Address"].ToString()
                    };
                }
            }

            return patient;
        }

        public List<Patient> SearchPatients(string searchTerm)
        {
            var patients = new List<Patient>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_SearchPatients", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);

                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    patients.Add(new Patient
                    {
                        PatientId = Convert.ToInt32(reader["PatientId"]),
                        PatientRegNum = reader["PatientRegNum"].ToString(),
                        Name = reader["Name"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        Age = Convert.ToInt32(reader["Age"]),
                        Phone = reader["Contact"].ToString(),
                        Address = reader["Address"].ToString()
                    });
                }
            }

            return patients;
        }

        // ========== APPOINTMENT ==========
        public List<AppointmentViewModel> GetAllAppointments()
        {
            var list = new List<AppointmentViewModel>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllAppointments", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new AppointmentViewModel
                    {
                        AppointmentId = Convert.ToInt32(dr["AppointmentId"]),
                        AppointmentNumber = dr["AppointmentNumber"].ToString(),
                        PatientId = Convert.ToInt32(dr["PatientId"]),
                        PatientName = dr["PatientName"].ToString(),
                        DoctorId = Convert.ToInt32(dr["DoctorId"]),
                        DoctorName = dr["DoctorName"].ToString(),
                        Date = Convert.ToDateTime(dr["Date"]),
                        Time = dr["Time"].ToString(),
                        Token = Convert.ToInt32(dr["Token"])
                    });
                }
            }
            return list;
        }

        public int AddAppointment(AppointmentViewModel model)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                int appointmentId = 0;

                SqlCommand cmd = new SqlCommand("sp_AddAppointment", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Input parameters
                cmd.Parameters.AddWithValue("@PatientId", model.PatientId);
                cmd.Parameters.AddWithValue("@DoctorId", model.DoctorId);
                cmd.Parameters.AddWithValue("@Date", model.Date);
                cmd.Parameters.AddWithValue("@Time", model.Time);

                // Output parameter
                SqlParameter outputParam = new SqlParameter("@AppointmentId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                // Open and execute
                con.Open();
                cmd.ExecuteNonQuery();

                // Check if value is not DBNull before converting
                if (outputParam.Value != DBNull.Value)
                {
                    appointmentId = Convert.ToInt32(outputParam.Value);
                }

                return appointmentId;
            }
        }




        public void UpdateAppointment(AppointmentViewModel model)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateAppointment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AppointmentId", model.AppointmentId);
                cmd.Parameters.AddWithValue("@Date", model.Date);
                cmd.Parameters.AddWithValue("@Time", model.Time);
                cmd.Parameters.AddWithValue("@Token", model.Token);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteAppointment(int appointmentId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteAppointment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public int AddAppointmentReturnId(AppointmentViewModel model)
        {
            int appointmentId = 0;
            using SqlConnection con = new(_connectionString);
            SqlCommand cmd = new("sp_AddAppointment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PatientId", model.PatientId);
            cmd.Parameters.AddWithValue("@DoctorId", model.DoctorId);
            cmd.Parameters.AddWithValue("@Date", model.Date);
            cmd.Parameters.AddWithValue("@Time", model.Time);

            // Output parameters (assumes token and apt number generated in SP)
            cmd.Parameters.Add("@AppointmentId", SqlDbType.Int).Direction = ParameterDirection.Output;

            con.Open();
            cmd.ExecuteNonQuery();
            appointmentId = Convert.ToInt32(cmd.Parameters["@AppointmentId"].Value);

            return appointmentId;
        }

        public BillViewModel GetBillByAppointmentId(int id)
        {
            BillViewModel bill = null;
            using SqlConnection con = new(_connectionString);
            SqlCommand cmd = new("sp_GetBillByAppointmentId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AppointmentId", id);
            con.Open();
            var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                bill = new BillViewModel
                {
                    AppointmentNumber = dr["AppointmentNumber"].ToString(),
                    PatientName = dr["PatientName"].ToString(),
                    DoctorName = dr["DoctorName"].ToString(),
                    Department = dr["Department"].ToString(), // ✅ Now this column will exist
                    Date = Convert.ToDateTime(dr["Date"]),
                    Time = dr["Time"].ToString(),
                    Token = Convert.ToInt32(dr["Token"]),
                    Fee = Convert.ToDecimal(dr["Fee"]) // now also fetched from DB
                };
            }

            return bill;
        }

        public BillViewModel GetBillDetailsByAppointmentId(int id)
        {
            BillViewModel bill = null;

            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("sp_GenerateBillForAppointment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AppointmentId", id);

            con.Open();
            using SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                bill = new BillViewModel
                {
                    AppointmentNumber = dr["AppointmentNumber"]?.ToString(),
                    PatientName = dr["PatientName"]?.ToString(),
                    DoctorName = dr["DoctorName"]?.ToString(),
                    Department = dr["Department"]?.ToString(), // Now properly fetched via join with Departments table
                    Date = dr["Date"] != DBNull.Value ? Convert.ToDateTime(dr["Date"]) : DateTime.MinValue,
                    Time = dr["Time"]?.ToString(),
                    Token = dr["Token"] != DBNull.Value ? Convert.ToInt32(dr["Token"]) : 0,
                    Fee = dr["Fee"] != DBNull.Value ? Convert.ToDecimal(dr["Fee"]) : 300 // Use fee from SP, fallback if needed
                };
            }


            return bill;
        }

        public List<BillViewModel> SearchBills(string appointmentNumber, string patientRegNum)
        {
            List<BillViewModel> bills = new();
            using SqlConnection con = new(_connectionString);
            using SqlCommand cmd = new("sp_SearchBills", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@AppointmentNumber", string.IsNullOrEmpty(appointmentNumber) ? (object)DBNull.Value : appointmentNumber);
            cmd.Parameters.AddWithValue("@PatientRegNum", string.IsNullOrEmpty(patientRegNum) ? (object)DBNull.Value : patientRegNum);

            con.Open();
            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                bills.Add(new BillViewModel
                {
                    AppointmentNumber = dr["AppointmentNumber"]?.ToString(),
                    PatientName = dr["PatientName"]?.ToString(),
                    DoctorName = dr["DoctorName"]?.ToString(),
                    Department = dr["Department"]?.ToString(),
                    Date = dr["Date"] != DBNull.Value ? Convert.ToDateTime(dr["Date"]) : DateTime.MinValue,
                    Time = dr["Time"]?.ToString(),
                    Token = dr["Token"] != DBNull.Value ? Convert.ToInt32(dr["Token"]) : 0,
                    Fee = dr["Fee"] != DBNull.Value ? Convert.ToDecimal(dr["Fee"]) : 0,
                    GeneratedDate = dr["GeneratedDate"] != DBNull.Value ? Convert.ToDateTime(dr["GeneratedDate"]) : DateTime.MinValue
                });
            }
            return bills;
        }




    }
}
