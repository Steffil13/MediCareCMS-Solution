using MediCareCMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MediCareCMS.Repositories
{
    public class LabRepository : ILabRepository
    {
        private readonly string _cs;
        public LabRepository(IConfiguration cfg) =>
            _cs = cfg.GetConnectionString("DefaultConnection");

        
        public List<LabTestRequest> GetAssignedTests(string empId)
        {
            var list = new List<LabTestRequest>();

            try
            {
                using var conn = new SqlConnection(_cs);
                var sql = @"
            SELECT r.RequestId, r.PatientId, r.TestId, r.RequestedDate, r.Status,
                   p.Name  AS PatientName,
                   d.Name  AS DoctorName,
                   t.TestName
            FROM   dbo.LabTestRequests r
            JOIN   dbo.Patient   p ON p.PatientId = r.PatientId
            JOIN   dbo.Doctors   d ON d.DoctorId  = r.DoctorId
            JOIN   dbo.Test      t ON t.TestId    = r.TestId
            WHERE  r.LabEmpId = @EmpId
              AND  r.Status = 'Pending'";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@EmpId", empId);
                conn.Open();

                using var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    list.Add(new LabTestRequest
                    {
                        RequestId = rd.GetInt32(0),
                        PatientId = rd["PatientId"].ToString()!,
                        DoctorName = rd["DoctorName"].ToString()!,
                        PatientName = rd["PatientName"].ToString()!,
                        TestName = rd["TestName"].ToString()!,
                        TestId = rd.GetInt32(rd.GetOrdinal("TestId")),
                        LabEmpId = empId,
                        RequestedDate = rd.GetDateTime(rd.GetOrdinal("RequestedDate")),
                        Status = rd["Status"].ToString()!
                    });
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                throw; // Rethrow or handle
            }

            return list;
        }


        // ──────────────────────────────────────
        public void MarkTestCompleted(int requestId)
        {
            using var conn = new SqlConnection(_cs);
            var cmd = new SqlCommand(
                "UPDATE dbo.LabTestRequests SET Status = 'Completed' WHERE RequestId = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", requestId);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        // ──────────────────────────────────────
        public void SaveTestResult(TestResults r)
        {
            using var conn = new SqlConnection(_cs);
            var sql = @"
                INSERT INTO dbo.TestResults (RequestId, ResultValue, Remarks, RecordedDate)
                VALUES (@ReqId, @Val, @Rem, @Date);";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ReqId", r.RequestId);
            cmd.Parameters.AddWithValue("@Val", r.ResultValue);
            cmd.Parameters.AddWithValue("@Rem", r.Remarks);
            cmd.Parameters.AddWithValue("@Date", r.RecordedDate);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        // ──────────────────────────────────────
        public List<TestResults> GetPatientHistory(string patientId)
        {
            var list = new List<TestResults>();

            using var conn = new SqlConnection(_cs);
            var sql = @"
                SELECT tr.*
                FROM   dbo.TestResults     tr
                JOIN   dbo.LabTestRequests lr ON lr.RequestId = tr.RequestId
                WHERE  lr.PatientId = @Pid;";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Pid", patientId);
            conn.Open();

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new TestResults
                {
                    ResultId = rd.GetInt32(rd.GetOrdinal("ResultId")),
                    RequestId = rd.GetInt32(rd.GetOrdinal("RequestId")),
                    ResultValue = rd["ResultValue"].ToString()!,
                    Remarks = rd["Remarks"].ToString()!,
                    RecordedDate = rd.GetDateTime(rd.GetOrdinal("RecordedDate"))
                });
            }
            return list;
        }

        // ──────────────────────────────────────
        public List<LabBill> GetBills(string empId)
        {
            var list = new List<LabBill>();

            using var conn = new SqlConnection(_cs);
            var sql = @"
                SELECT lb.*, t.TestName
                FROM   dbo.LabBills        lb
                JOIN   dbo.LabTestRequests lr ON lr.RequestId = lb.RequestId
                JOIN   dbo.Test            t  ON t.TestId     = lb.TestId
                WHERE  lr.LabEmpId = @EmpId;";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@EmpId", empId);
            conn.Open();

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new LabBill
                {
                    BillId = rd.GetInt32(rd.GetOrdinal("BillId")),
                    RequestId = rd.GetInt32(rd.GetOrdinal("RequestId")),
                    PatientId = rd["PatientId"].ToString()!,
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
