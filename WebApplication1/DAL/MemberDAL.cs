using System.Configuration;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.DAL
{
    public class MemberDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public MemberDAL()
        {
            // Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("NPCSConnectionString");

            // Instantiate a SqlConnection object with the Connection String read
            conn = new SqlConnection(strConn);
        }

        public Member? Login(string loginId, string password)
        {
            Member? result = null;
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement 
            cmd.CommandText = @"SELECT * FROM Member";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end
            while (reader.Read())
            {
                // Convert email address to lowercase for comparison
                // Password comparison is case-sensitive
                if ((reader.GetString(4).ToLower() == loginId.ToLower()) &&
                (reader.GetString(5) == password))
                {
                    result = new Member
                    {
                        MemberID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Salutation = reader.GetString(2),
                        TelNo = reader.GetString(3),
                        EmailAddr = reader.GetString(4),
                        Password = reader.GetString(5),
                        BirthDate = reader.GetDateTime(6),
                        City = reader.GetString(7),
                        Country = reader.GetString(8)
                    };
                    break; // Exit the while loop
                }
            }
            return result;
        }

        public void Add(Member member)
        {
            // Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            // Specify an INSERT SQL statement which will
            // return the auto-generated MemberID after insertion
            cmd.CommandText = @"INSERT INTO Member (Name, Salutation, TelNo, EmailAddr, Password, BirthDate, City, Country) 
                        VALUES (@name, @salutation, @telno, @email, @password, @birthdate, @city, @country)";
            // Define the parameters used in SQL statement, value for each parameter
            // is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@name", member.Name);
            cmd.Parameters.AddWithValue("@salutation", member.Salutation);
            cmd.Parameters.AddWithValue("@telno", member.TelNo);
            cmd.Parameters.AddWithValue("@email", member.EmailAddr);
            cmd.Parameters.AddWithValue("@password", member.Password);
            cmd.Parameters.AddWithValue("@birthdate", member.BirthDate);
            cmd.Parameters.AddWithValue("@city", member.City);
            cmd.Parameters.AddWithValue("@country", member.Country);
            // A connection to the database must be opened before any operations made.
            conn.Open();

            cmd.ExecuteScalar();
            // A connection should be closed after operations.
            conn.Close();
        }

    }
}
