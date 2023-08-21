using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;
using WebApplication1.Models;

namespace WebApplication1.DAL
{
    public class DeliveryHistoryDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public DeliveryHistoryDAL()
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

        public void AddDeliveryHistory(DeliveryHistory deliveryHistory)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO DeliveryHistory (ParcelID, Description)
                           
                            VALUES(@parcelID, @description)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@parcelID", deliveryHistory.ParcelID);
            cmd.Parameters.AddWithValue("@description", deliveryHistory.Description);

            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
        }

        public void UpdateDevStatus(DeliveryHistory deliveryHistory, string loginID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            
            cmd.CommandText = @"UPDATE Parcel SET DeliveryStatus = @deliveryStatus, DeliveryManID = @loginID WHERE ParcelID = @parcelId";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@deliveryStatus", 2);
            cmd.Parameters.AddWithValue("@parcelID", deliveryHistory.ParcelID);
            cmd.Parameters.AddWithValue("@loginID", loginID);
            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = 0;
            count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
        }

        public void AddDeliveryHistorySM(DeliveryHistory deliveryHistory)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            
            cmd.CommandText = @"INSERT INTO DeliveryHistory (ParcelID, Description)
                            VALUES(@parcelID, @description)";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.]
            cmd.Parameters.AddWithValue("@parcelID", deliveryHistory.ParcelID);
            cmd.Parameters.AddWithValue("@description", deliveryHistory.Description);

            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
        }

        public void AddDeliveryHistoryDM(DeliveryHistory deliveryHistory)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            // Specify the SELECT SQL statement with a WHERE clause to filter by appointment
            cmd.CommandText = "SELECT * FROM Parcel WHERE DeliveryManID = 'loginID'";
            //Specify an UPDATE SQL statement

            cmd.CommandText = @"INSERT INTO DeliveryHistory (ParcelID, Description)
                            VALUES(@parcelID, @description)";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@deliveryStatus", 3);
            cmd.Parameters.AddWithValue("@parcelID", deliveryHistory.ParcelID);
            cmd.Parameters.AddWithValue("@description", deliveryHistory.Description);

            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
        }

        public List<DeliveryHistory> GetDeliveryHistoryList()
        {
            List<DeliveryHistory> deliveryHistoryList = new List<DeliveryHistory>();

            // Create a SqlCommand object from the connection object
            SqlCommand cmd = conn.CreateCommand();
            // Specify the SELECT SQL statement to get the latest delivery history for each parcel
            cmd.CommandText = @"
        SELECT dh.RecordID, dh.ParcelID, dh.Description
        FROM DeliveryHistory dh
        INNER JOIN (
            SELECT ParcelID, MAX(RecordID) AS MaxRecordID
            FROM DeliveryHistory
            GROUP BY ParcelID
        ) maxRecords ON dh.ParcelID = maxRecords.ParcelID AND dh.RecordID = maxRecords.MaxRecordID";

            conn.Open();
            // Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            // Read all the records (if exist)
            while (reader.Read())
            {
                DeliveryHistory deliveryHistory = new DeliveryHistory
                {
                    RecordID = reader.GetInt32(0),
                    ParcelID = reader.GetInt32(1),
                    Description = reader.GetString(2)
                };

                deliveryHistoryList.Add(deliveryHistory);
            }

            // Close the DataReader and database connection
            reader.Close();
            conn.Close();

            return deliveryHistoryList;
        }
    }
}
