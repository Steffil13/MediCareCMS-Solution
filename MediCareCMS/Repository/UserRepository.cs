using MediCareCMS.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MediCareCMS.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public User ValidateUser(string username, string password)
        {
            User user = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_ValidateUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    user = new User
                    {
                        UserId = Convert.ToInt32(reader["UserId"]),
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        Role = reader["Role"].ToString()
                    };
                }
            }

            return user;
        }

        public Doctor GetDoctorByUsername(string username)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Doctors WHERE Username = @Username";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Doctor
                        {
                            DoctorId = Convert.ToInt32(reader["DoctorId"]),
                            Username = reader["Username"].ToString(),
                            // Add other fields if needed
                        };
                    }
                }
            }
            return null;
        }

    }
}
