using MySqlConnector;
using Persistence;
using DAL;

namespace DAL
{
    public class StaffDAL
    {
        private string query = "";
        private MySqlConnection connection = DbConfig.GetConnection();

        internal Staff GetStaff(MySqlDataReader reader)
        {
            Staff staff = new Staff();
            staff.StaffID = reader.GetInt32("staff_id");
            staff.StaffName = reader.GetString("staff_name");
            staff.UserName = reader.GetString("user_name");
            staff.Password = reader.GetString("password");
            staff.Role_ID = reader.GetInt32("role_id");
            staff.Status = reader.GetInt32("status");
            return staff;
        }

        public Staff GetStaffByID(int staffID)
        {
            Staff staff = new Staff();
            try
            {
                MySqlCommand command = new MySqlCommand("", connection);
                query = "SELECT * FROM staffs WHERE staff_id = @staffid;";
                command.CommandText = query;
                command.Parameters.AddWithValue("@staffid", staffID);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    staff = GetStaff(reader);
                }
                reader.Close();
            }
            catch { }
            return staff;
        }

        public Staff GetAccount(string userName)
        {
            Staff staff = new Staff();
            try
            {
                MySqlCommand command = new MySqlCommand("", connection);
                query = "SELECT * FROM staffs WHERE user_name = @username;";
                command.CommandText = query;
                command.Parameters.AddWithValue("@username", userName);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    staff = GetStaff(reader);
                    
                }
                reader.Close();
            }
            catch { }
            return staff;
        }
        public string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes); // .NET 5 +

                // Convert the byte array to hexadecimal string prior to .NET 5
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

    }
}
