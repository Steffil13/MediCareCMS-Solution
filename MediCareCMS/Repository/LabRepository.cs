using MediCareCMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace MediCareCMS.Repositories
{
    public class LabRepository : ILabRepository
    {
        private readonly string _cs;
        public LabRepository(IConfiguration cfg) => _cs = cfg.GetConnectionString("DefaultConnection");

        /*──────────────────── 1. Get assigned tests ───────────────────*/
        public List<LabTestRequest> GetAssignedTests(string empId, string? doctorFilter)
        {
            var list = new List<LabTestRequest>();

            using var con = new SqlConnection(_cs);
            using var cmd = new SqlCommand("sp_Lab_GetAssignedTests", con)
            { CommandType = System.Data.CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@LabEmpId", empId);
            cmd.Parameters.AddWithValue("@DocEmpId",
                string.IsNullOrWhiteSpace(doctorFilter) ? (object)DBNull.Value : doctorFilter);

            con.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new LabTestRequest
                {
                    RequestId = rd.GetInt32(rd.GetOrdinal("RequestId")),
                    PatientId = Convert.ToInt32(rd["PatientId"]),
                    DoctorId = rd["DocEmpId"].ToString()!,
                    TestId = rd.GetInt32(rd.GetOrdinal("TestId")),
                    RequestedDate = rd.GetDateTime(rd.GetOrdinal("RequestedDate")),
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
        public void SaveTestResult(TestResults r)
        {
            using var con = new SqlConnection(_cs);
            using var cmd = new SqlCommand("sp_Lab_SaveResult", con)
            { CommandType = System.Data.CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@RequestId", r.RequestId);
            cmd.Parameters.AddWithValue("@ResultValue", r.ResultValue ?? string.Empty);
            cmd.Parameters.AddWithValue("@Remarks", r.Remarks ?? string.Empty);
            cmd.Parameters.AddWithValue("@RecordedDt", r.RecordedDate);

            con.Open();
            cmd.ExecuteNonQuery();
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
