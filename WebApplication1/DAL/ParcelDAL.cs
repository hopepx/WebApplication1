using System.Configuration;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.DAL
{
    public class ParcelDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
      //private readonly IHttpContextAccessor httpContextAccessor;

        public ParcelDAL()
        {
            //IHttpContextAccessor httpContextAccessor
            //this.httpContextAccessor = httpContextAccessor;
            // Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("NPCSConnectionString");

            // Instantiate a SqlConnection object with the Connection String read
            conn = new SqlConnection(strConn);
        }

        public List<Parcel> GetAllParcels()
        {
            // Create a SqlCommand object from the connection object
            SqlCommand cmd = conn.CreateCommand();
            // Specify the SELECT SQL statement
            cmd.CommandText = "SELECT * FROM Parcel";
            // Open a database connection
            conn.Open();
            // Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            // Read all records until the end, save data into a parcel list
            List<Parcel> parcelList = new List<Parcel>();
            while (reader.Read())
            {
                Parcel parcel = new Parcel
                {
                    ParcelID = reader.GetInt32(0),
                    ItemDescription = reader.GetString(1),
                    SenderName = reader.GetString(2),
                    SenderTelNo = reader.GetString(3),
                    ReceiverName = reader.GetString(4),
                    ReceiverTelNo = reader.GetString(5),
                    DeliveryAddress = reader.GetString(6),
                    FromCity = reader.GetString(7),
                    FromCountry = reader.GetString(8),
                    ToCity = reader.GetString(9),
                    ToCountry = reader.GetString(10),
                    ParcelWeight = reader.GetDouble(11),
                    DeliveryCharge = reader.GetDecimal(12),
                    Currency = reader.GetString(13),
                    TargetDeliveryDate = reader.IsDBNull(14) ? null : (DateTime?)reader.GetDateTime(14),
                    DeliveryStatus = reader.GetString(15),
                    DeliveryManID = reader.IsDBNull(16) ? null : (int?)reader.GetInt32(16)
                };
                parcelList.Add(parcel);
            }
            // Close the DataReader and database connection
            reader.Close();
            conn.Close();
            return parcelList;
        }

        public Parcel GetParcelById(int parcelId)
        {
            Parcel parcel = null;
            // Create a SqlCommand object from the connection object
            SqlCommand cmd = conn.CreateCommand();
            // Specify the SELECT SQL statement with a WHERE clause to filter by ParcelID
            cmd.Parameters.AddWithValue("@ParcelID", parcelId);
            cmd.CommandText = "SELECT * FROM Parcel WHERE ParcelID = @ParcelID";
            // Open a database connection
            conn.Open();
            // Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            // Read the first record (if exists)
            if (reader.Read())
            {
                parcel = new Parcel
                {
                    ParcelID = reader.GetInt32(0),
                    ItemDescription = reader.GetString(1),
                    SenderName = reader.GetString(2),
                    SenderTelNo = reader.GetString(3),
                    ReceiverName = reader.GetString(4),
                    ReceiverTelNo = reader.GetString(5),
                    DeliveryAddress = reader.GetString(6),
                    FromCity = reader.GetString(7),
                    FromCountry = reader.GetString(8),
                    ToCity = reader.GetString(9),
                    ToCountry = reader.GetString(10),
                    ParcelWeight = reader.GetDouble(11),
                    DeliveryCharge = reader.GetDecimal(12),
                    Currency = reader.GetString(13),
                    TargetDeliveryDate = reader.IsDBNull(14) ? null : (DateTime?)reader.GetDateTime(14),
                    DeliveryStatus = reader.GetString(15),
                    DeliveryManID = reader.IsDBNull(16) ? null : (int?)reader.GetInt32(16)
                };
            }
            // Close the DataReader and database connection
            reader.Close();
            conn.Close();
            return parcel;
        }

        public List<Parcel> GetAllParcelsBySenderName(string senderName)
        {
            // Create a SqlCommand object from the connection object
            SqlCommand cmd = conn.CreateCommand();
            // Specify the SELECT SQL statement with a WHERE clause to filter by SenderName
            cmd.Parameters.AddWithValue("@SenderName", senderName);
            cmd.CommandText = "SELECT * FROM Parcel WHERE SenderName = @SenderName";
            // Open a database connection
            conn.Open();
            // Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            // Read all records until the end, save data into a parcel list
            List<Parcel> parcelList = new List<Parcel>();
            while (reader.Read())
            {
                Parcel parcel = new Parcel
                {
                    ParcelID = reader.GetInt32(0),
                    ItemDescription = reader.GetString(1),
                    SenderName = reader.GetString(2),
                    SenderTelNo = reader.GetString(3),
                    ReceiverName = reader.GetString(4),
                    ReceiverTelNo = reader.GetString(5),
                    DeliveryAddress = reader.GetString(6),
                    FromCity = reader.GetString(7),
                    FromCountry = reader.GetString(8),
                    ToCity = reader.GetString(9),
                    ToCountry = reader.GetString(10),
                    ParcelWeight = reader.GetDouble(11),
                    DeliveryCharge = reader.GetDecimal(12),
                    Currency = reader.GetString(13),
                    TargetDeliveryDate = reader.IsDBNull(14) ? null : (DateTime?)reader.GetDateTime(14),
                    DeliveryStatus = reader.GetString(15),
                    DeliveryManID = reader.IsDBNull(16) ? null : (int?)reader.GetInt32(16)
                };
                parcelList.Add(parcel);
            }
            // Close the DataReader and database connection
            reader.Close();
            conn.Close();
            return parcelList;
        }

        public int AddParcel(Parcel parcel)
        {
            int parcelId = 0;

            // Create a SqlCommand object from the connection object
            SqlCommand cmd = conn.CreateCommand();
            // Specify an INSERT SQL statement which will
            // return the auto-generated ParcelID after insertion
            cmd.CommandText = @"INSERT INTO Parcel (ItemDescription, SenderName, SenderTelNo, ReceiverName, ReceiverTelNo, DeliveryAddress, FromCity, FromCountry, ToCity, ToCountry, ParcelWeight, DeliveryCharge, Currency, TargetDeliveryDate, DeliveryStatus, DeliveryManID)
                        VALUES (@ItemDescription, @SenderName, @SenderTelNo, @ReceiverName, @ReceiverTelNo, @DeliveryAddress, @FromCity, @FromCountry, @ToCity, @ToCountry, @ParcelWeight, @DeliveryCharge, @Currency, @TargetDeliveryDate, @DeliveryStatus, @DeliveryManID);
                        SELECT SCOPE_IDENTITY();";

            // Set the parameter values
            cmd.Parameters.AddWithValue("@ItemDescription", parcel.ItemDescription);
            cmd.Parameters.AddWithValue("@SenderName", parcel.SenderName);
            cmd.Parameters.AddWithValue("@SenderTelNo", parcel.SenderTelNo);
            cmd.Parameters.AddWithValue("@ReceiverName", parcel.ReceiverName);
            cmd.Parameters.AddWithValue("@ReceiverTelNo", parcel.ReceiverTelNo);
            cmd.Parameters.AddWithValue("@DeliveryAddress", parcel.DeliveryAddress);
            cmd.Parameters.AddWithValue("@FromCity", parcel.FromCity);
            cmd.Parameters.AddWithValue("@FromCountry", parcel.FromCountry);
            cmd.Parameters.AddWithValue("@ToCity", parcel.ToCity);
            cmd.Parameters.AddWithValue("@ToCountry", parcel.ToCountry);
            cmd.Parameters.AddWithValue("@ParcelWeight", parcel.ParcelWeight);
            cmd.Parameters.AddWithValue("@DeliveryCharge", parcel.DeliveryCharge);
            cmd.Parameters.AddWithValue("@Currency", parcel.Currency);
            cmd.Parameters.AddWithValue("@TargetDeliveryDate", (object)parcel.TargetDeliveryDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DeliveryStatus", parcel.DeliveryStatus);
            cmd.Parameters.AddWithValue("@DeliveryManID", (object)parcel.DeliveryManID ?? DBNull.Value);

            // Open the connection
            conn.Open();
            // Execute the INSERT SQL statement and retrieve the auto-generated ParcelID
            parcelId = Convert.ToInt32(cmd.ExecuteScalar());
            // Close the connection
            conn.Close();

            return parcelId;
        }

        public List<Parcel> GetAllParcelsByDMID(int deliveryManID)
        {
            // Create a SqlCommand object from the connection object
            SqlCommand cmd = conn.CreateCommand();
            // Specify the SELECT SQL statement with a WHERE clause to filter by SenderName
            cmd.Parameters.AddWithValue("@DeliveryManID", deliveryManID);
            cmd.CommandText = "SELECT * FROM Parcel WHERE DeliveryManID = @DeliveryManID";
            // Open a database connection
            conn.Open();
            // Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            // Read all records until the end, save data into a parcel list
            List<Parcel> parcelList = new List<Parcel>();
            while (reader.Read())
            {
                Parcel parcel = new Parcel
                {
                    ParcelID = reader.GetInt32(0),
                    ItemDescription = reader.GetString(1),
                    SenderName = reader.GetString(2),
                    SenderTelNo = reader.GetString(3),
                    ReceiverName = reader.GetString(4),
                    ReceiverTelNo = reader.GetString(5),
                    DeliveryAddress = reader.GetString(6),
                    FromCity = reader.GetString(7),
                    FromCountry = reader.GetString(8),
                    ToCity = reader.GetString(9),
                    ToCountry = reader.GetString(10),
                    ParcelWeight = reader.GetDouble(11),
                    DeliveryCharge = reader.GetDecimal(12),
                    Currency = reader.GetString(13),
                    TargetDeliveryDate = reader.IsDBNull(14) ? null : (DateTime?)reader.GetDateTime(14),
                    DeliveryStatus = reader.GetString(15),
                    DeliveryManID = reader.IsDBNull(16) ? null : (int?)reader.GetInt32(16)
                };
                parcelList.Add(parcel);
            }
            // Close the DataReader and database connection
            reader.Close();
            conn.Close();
            return parcelList;
        }


    }
}
