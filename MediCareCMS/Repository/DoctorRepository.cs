using MediCareCMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MediCareCMS.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly string _connectionString;

        public DoctorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Appointment> GetAppointmentsByDoctorAndDate(int doctorId, DateTime date)
        {
            var list = new List<Appointment>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetAppointmentsByDoctorAndDate", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                cmd.Parameters.AddWithValue("@Date", date.Date);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Appointment
                        {
                            AppointmentId = reader["AppointmentId"].ToString(),
                            DoctorId = (int)reader["DoctorId"],
                            PatientId = reader["PatientId"].ToString(),
                            Name = reader["PatientName"].ToString(),
                            Date = Convert.ToDateTime(reader["Date"]),
                            Time = reader["Time"].ToString(),
                            Token = Convert.ToInt32(reader["Token"]),
                            IsConsulted = Convert.ToBoolean(reader["IsConsulted"])
                        });
                    }
                }
            }
            return list;
        }

        public Appointment GetAppointmentById(string appointmentId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetAppointmentById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Appointment
                        {
                            AppointmentId = reader["AppointmentId"].ToString(),
                            DoctorId = Convert.ToInt32(reader["DoctorId"]),
                            PatientId = reader["PatientId"].ToString(),
                            Name = reader["PatientName"].ToString(),
                            Date = Convert.ToDateTime(reader["Date"]),
                            Time = reader["Time"].ToString(),
                            Token = Convert.ToInt32(reader["Token"]),
                            IsConsulted = Convert.ToBoolean(reader["IsConsulted"])
                        };
                    }
                }
            }
            return null;
        }
        public List<LabTest> GetAllLabTests()
        {
            var labTests = new List<LabTest>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetAllLabTests", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        labTests.Add(new LabTest
                        {
                            LabTestId = Convert.ToInt32(reader["LabTestId"]),

                            TestName = reader["TestName"].ToString()
                        });
                    }
                }
            }

            return labTests;
        }

        public PatientSummary GetPatientSummary(string patientId)
        {
            var summary = new PatientSummary { Medicines = new List<string>() };

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_GetPatientSummary", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PatientId", patientId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        summary.Disease = reader["Diagnosis"].ToString();
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            summary.Medicines.Add(reader["MedicineName"].ToString());
                        }
                    }
                }
            }

            return string.IsNullOrEmpty(summary.Disease) ? null : summary;
        }

        public List<MedicineInventory> GetMedicineInventory()
        {
            var list = new List<MedicineInventory>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_GetMedicineInventory", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new MedicineInventory
                        {
                            MedicineId = Convert.ToInt32(reader["MedicineId"]),
                            MedicineName = reader["MedicineName"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"])
                        });
                    }
                }
            }

            return list;
        }

        public int SavePrescription(Prescription prescription)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_SavePrescription", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AppointmentId", prescription.AppointmentId);
                cmd.Parameters.AddWithValue("@Symptoms", prescription.Symptoms);
                cmd.Parameters.AddWithValue("@Diagnosis", prescription.Diagnosis);

                conn.Open();
                int prescriptionId = Convert.ToInt32(cmd.ExecuteScalar());

                foreach (var med in prescription.Medicines)
                {
                    using (var medCmd = new SqlCommand("sp_AddPrescriptionMedicine", conn))
                    {
                        medCmd.CommandType = CommandType.StoredProcedure;
                        medCmd.Parameters.AddWithValue("@PrescriptionId", prescriptionId);
                        medCmd.Parameters.AddWithValue("@MedicineId", med.MedicineId);
                        medCmd.Parameters.AddWithValue("@Dosage", med.Dosage);
                        medCmd.Parameters.AddWithValue("@Duration", med.Duration);
                        medCmd.ExecuteNonQuery();
                    }
                }
                return prescriptionId;
            }
        }
        public void AddSchedule(DoctorSchedule schedule)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_AddSchedule", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DoctorId", schedule.DoctorId);
            cmd.Parameters.AddWithValue("@Date", schedule.Date);
            cmd.Parameters.AddWithValue("@IsAvailable", schedule.IsAvailable);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void UpdateDoctorSchedule(int doctorId, DateTime date, bool isAvailable)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_UpdateDoctorSchedule", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                cmd.Parameters.AddWithValue("@Date", date);
                cmd.Parameters.AddWithValue("@IsAvailable", isAvailable);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public DoctorSchedule GetScheduleById(int scheduleId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_GetScheduleById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ScheduleId", scheduleId);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new DoctorSchedule
                {
                    ScheduleId = (int)reader["ScheduleId"],
                    DoctorId = (int)reader["DoctorId"],
                    Date = (DateTime)reader["Date"],
                    IsAvailable = (bool)reader["IsAvailable"]
                };
            }
            return null;
        }

        public void UpdateSchedule(DoctorSchedule schedule)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_UpdateDoctorSchedule", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", schedule.ScheduleId);
            cmd.Parameters.AddWithValue("@DoctorId", schedule.DoctorId);
            cmd.Parameters.AddWithValue("@Date", schedule.Date);
            cmd.Parameters.AddWithValue("@IsAvailable", schedule.IsAvailable);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteSchedule(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_DeleteSchedule", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
        public void MarkAppointmentAsConsulted(string appointmentId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_MarkAppointmentAsConsulted", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void SavePrescriptionLabTest(int prescriptionId, int labTestId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_AddPrescriptionLabTest", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PrescriptionId", prescriptionId);
                cmd.Parameters.AddWithValue("@LabTestId", labTestId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public List<DoctorSchedule> GetDoctorSchedule(int doctorId)
        {
            var list = new List<DoctorSchedule>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_GetDoctorSchedule", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new DoctorSchedule
                        {
                            DoctorId = Convert.ToInt32(reader["DoctorId"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            IsAvailable = Convert.ToBoolean(reader["IsAvailable"])
                        });
                    }
                }
            }

            return list;
        }
    }
}