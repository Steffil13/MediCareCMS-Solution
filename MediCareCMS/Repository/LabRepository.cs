using MediCareCMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;

namespace MediCareCMS.Repositories
{
    public class LabRepository : ILabRepository
    {
        private readonly string _cs;
        public LabRepository(IConfiguration cfg) => _cs = cfg.GetConnectionString("DefaultConnection");

        /*──────────────────── 1. Get assigned tests ───────────────────*/
        public List<LabTestRequest> GetAssignedTests()
        {
            var list = new List<LabTestRequest>();

            using var con = new SqlConnection(_cs);
            using var cmd = new SqlCommand("sp_Lab_GetAssignedTests", con)
            { CommandType = System.Data.CommandType.StoredProcedure };

            //cmd.Parameters.AddWithValue("@TestId", empId);

            // ✅ Conditionally add @DocEmpId only if doctorFilter is not null or empty
            //if (doctorId.HasValue)
            //    cmd.Parameters.AddWithValue("@DoctorId", doctorId.Value);
            //else
            //    cmd.Parameters.AddWithValue("@DoctorId", DBNull.Value);
            // Match optional param logic in SQL

            con.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new LabTestRequest
                {
                    RequestId = rd.GetInt32(rd.GetOrdinal("TestRequestId")),
                    PatientId = Convert.ToInt32(rd["PatientId"]),
                    DoctorId = rd["DocEmpId"].ToString()!,
                    TestId = Convert.ToInt32(rd["TestId"]), // ✅ Read actual TestId from result
                    RequestedDate = DateTime.Now, // You can update this if SP returns RequestedDate
                    Status = rd["Status"].ToString()!,
                    PatientName = rd["PatientName"].ToString()!,
                    DoctorName = rd["DoctorName"].ToString()!,
                    TestName = rd["TestName"].ToString()!
                });
            }
            return list;
        }





        /*──────────────────── 2. Mark completed ───────────────────────*/
        public void MarkTestCompleted(int id)
        {
            using var con = new SqlConnection(_cs);
            using var cmd = new SqlCommand("sp_Lab_MarkCompleted", con)
            { CommandType = System.Data.CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@RequestId", id);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        /*──────────────────── 3. Save result ──────────────────────────*/
        public void SaveTestResult(TestResults result)
        {
            using var con = new SqlConnection(_cs);
            con.Open();

            // 1. Save test result
            using (var cmd = new SqlCommand("sp_SaveTestResult", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestId", result.RequestId);
                cmd.Parameters.AddWithValue("@ResultValue", result.ResultValue);
                cmd.Parameters.AddWithValue("@Remarks", result.Remarks ?? "");
                cmd.Parameters.AddWithValue("@RecordedDate", result.RecordedDate);
                cmd.ExecuteNonQuery();
            }

            // 2. Generate Bill (After test is done)
            using (var cmd2 = new SqlCommand("sp_Lab_GenerateBill", con))
            {
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@RequestId", result.RequestId);
                cmd2.ExecuteNonQuery();
            }
        }

        public void GenerateLabBill(int requestId)
        {
            using var con = new SqlConnection(_cs);
            using var cmd = new SqlCommand("sp_Lab_GenerateBill", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@RequestId", requestId);

            con.Open();
            cmd.ExecuteNonQuery();
        }


        public LabBill GetBillByRequestId(int requestId)
        {
            LabBill bill = null;

            using var con = new SqlConnection(_cs);
            using var cmd = new SqlCommand("sp_Lab_GetBillByRequestId", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@RequestId", requestId);

            con.Open();
            using var rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                bill = new LabBill
                {
                    BillId = rd.GetInt32(rd.GetOrdinal("BillId")),
                    PatientId = Convert.ToInt32(rd["PatientId"]),
                    Amount = rd.GetDecimal(rd.GetOrdinal("Amount")),
                    BillDate = rd.GetDateTime(rd.GetOrdinal("BillDate")),
                    Status = rd["Status"].ToString()
                };
            }

            return bill!;
        }



        /*──────────────────── 4. Get all results ──────────────────────*/
        public List<TestResults> GetAllResults(string empId)
        {
            var list = new List<TestResults>();

            using var con = new SqlConnection(_cs);
            using var cmd = new SqlCommand("sp_Lab_GetAllResults", con)
            { CommandType = System.Data.CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@LabEmpId", empId);

            con.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new TestResults
                {
                    ResultId = rd.GetInt32(rd.GetOrdinal("ResultId")),
                    RequestId = rd.GetInt32(rd.GetOrdinal("RequestId")),
                    ResultValue = rd["ResultValue"].ToString()!,
                    Remarks = rd["Remarks"].ToString()!,
                    RecordedDate = rd.GetDateTime(rd.GetOrdinal("RecordedDate")),
                    TestName = rd["TestName"].ToString()!
                });
            }
            return list;
        }

        /*──────────────────── 5. Patient history ──────────────────────*/
        public List<TestResults> GetPatientHistory(string pid)
        {
            var list = new List<TestResults>();

            using var con = new SqlConnection(_cs);
            using var cmd = new SqlCommand("sp_Lab_GetPatientHistory", con)
            { CommandType = System.Data.CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@PatientId", pid);

            con.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new TestResults
                {
                    ResultId = rd.GetInt32(rd.GetOrdinal("ResultId")),
                    RequestId = rd.GetInt32(rd.GetOrdinal("RequestId")),
                    ResultValue = rd["ResultValue"].ToString()!,
                    Remarks = rd["Remarks"].ToString()!,
                    RecordedDate = rd.GetDateTime(rd.GetOrdinal("RecordedDate")),
                    TestName = rd["TestName"].ToString()!
                });
            }
            return list;
        }

        /*──────────────────── 6. Bills ────────────────────────────────*/
        public List<LabBill> GetBills(string empId)
        {
            var list = new List<LabBill>();

            using var con = new SqlConnection(_cs);
            using var cmd = new SqlCommand("sp_Lab_GetBills", con)
            { CommandType = System.Data.CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@LabEmpId", empId);

            con.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new LabBill
                {
                    BillId = rd.GetInt32(rd.GetOrdinal("BillId")),
                    RequestId = rd.GetInt32(rd.GetOrdinal("RequestId")),
                    PatientId = Convert.ToInt32(rd["PatientId"]),
                    TestId = rd.GetInt32(rd.GetOrdinal("TestId")),
                    Amount = rd.GetDecimal(rd.GetOrdinal("Amount")),
                    BillDate = rd.GetDateTime(rd.GetOrdinal("BillDate")),
                    TestName = rd["TestName"].ToString()!
                });
            }
            return list;
        }
    }
}
