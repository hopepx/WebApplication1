using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using System.Configuration;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.DAL
{
    public class StaffDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public StaffDAL()
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

        public Staff? Login(string loginId, string password)
        {
            Staff? result = null;
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement 
            cmd.CommandText = @"SELECT * FROM Staff";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end
            while (reader.Read())
            {
                // Convert email address to lowercase for comparison
                // Password comparison is case-sensitive
                if ((reader.GetString(2).ToLower() == loginId.ToLower()) &&
                (reader.GetString(3) == password))
                {
                    result = new Staff
                    {
                        StaffID = reader.GetInt32(0),
                        StaffName = reader.GetString(1),
                        LoginID = reader.GetString(2),
                        Password = reader.GetString(3),
                        Appointment = reader.GetString(4),
                        OfficeTelNo = reader.GetString(5),
                        Location = reader.GetString(6)
                    };
                    break; // Exit the while loop
                }
            }
            return result;
        }
        public List<Staff> GetDeliveryManList()
        {
            List<Staff> result = new List<Staff>();
            Staff staffName = null;
            // Create a SqlCommand object from the connection object
            SqlCommand cmd = conn.CreateCommand();
            // Specify the SELECT SQL statement with a WHERE clause to filter by appointment
            cmd.CommandText = "SELECT * FROM Staff WHERE Appointment = 'Delivery Man'";
            // Open a database connection
            conn.Open();
            // Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            // Read the first record (if exists)
            while (reader.Read())
            {

                staffName = new Staff
                {
                    StaffID = reader.GetInt32(0),
                    StaffName = reader.GetString(1),
                    LoginID = reader.GetString(2),
                    Password = reader.GetString(3),
                    Appointment = reader.GetString(4),
                    OfficeTelNo = reader.GetString(5),
                    Location = reader.GetString(6)
                };
                result.Add(staffName);

            }
            // Close the DataReader and database connection
            reader.Close();
            conn.Close();
            return result;

        }
        public List<Staff> GetDeliveryMen()
        {
            List<Staff> deliveryMenList = new List<Staff>();

            // Create a SqlCommand object from the connection object
            SqlCommand cmd = conn.CreateCommand();
            // Specify the SELECT SQL statement with a WHERE clause to filter by appointment
            cmd.CommandText = "SELECT * FROM Staff WHERE Appointment = 'Delivery Man'"; // Note the single quotes around 'Delivery Man'
                                                                                        // Open a database connection
            conn.Open();
            // Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            // Read all the records (if exist)
            while (reader.Read())
            {
                Staff deliveryMan = new Staff
                {
                    StaffID = reader.GetInt32(0),
                    StaffName = reader.GetString(1),
                    LoginID = reader.GetString(2),
                    Password = reader.GetString(3),
                    Appointment = reader.GetString(4),
                    OfficeTelNo = reader.GetString(5),
                    Location = reader.GetString(6)
                };

                deliveryMenList.Add(deliveryMan);
            }

            // Close the DataReader and database connection
            reader.Close();
            conn.Close();

            return deliveryMenList;
        }

        
        
        
            public void CompleteDevStatus(DeliveryHistory deliveryHistory)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            // Specify the SELECT SQL statement with a WHERE clause to filter by appointment
            cmd.CommandText = "SELECT * FROM Parcel WHERE DeliveryManID = 'staffId'";

            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE Parcel SET DeliveryStatus=3";
            cmd.CommandText = @"UPDATE DeliveryHistory SET Description=CONCAT('Parcel delivered successfully by ', staffId, ' on ', NOW())";
            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = 0;
            count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
        }
    }
}
