using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using WebApplication1.Models;

namespace WebApplication1.DAL
{
    public class ShippingRateDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public ShippingRateDAL()
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

        public List<string> GetToCityAndCountryList()
        {
            List<string> cityAndCountryList = new List<string>();
            // Create a SqlCommand object from the connection object
            SqlCommand cmd = conn.CreateCommand();
            // Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM ShippingRate";
            // Open a database connection
            conn.Open();
            // Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            // Read all records until the end
            while (reader.Read())
            {
                string toCity = reader.GetString(3);
                string toCountry = reader.GetString(4);

                string cityAndCountry = $"{toCity}, {toCountry}";
                cityAndCountryList.Add(cityAndCountry);
            }
            conn.Close();
            return cityAndCountryList;
        }

        public ShippingRateModel GetShippingRate(string toCity, string toCountry)
        {
            // Create a SqlCommand object from the connection object
            SqlCommand cmd = conn.CreateCommand();
            // Specify the SELECT SQL statement with parameters
            cmd.CommandText = @"SELECT * FROM ShippingRate WHERE ToCity = @ToCity AND ToCountry = @ToCountry";
            // Set the parameter values
            cmd.Parameters.AddWithValue("@ToCity", toCity);
            cmd.Parameters.AddWithValue("@ToCountry", toCountry);
            // Open a database connection
            conn.Open();
            // Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            // Read the first record if available
            ShippingRateModel shippingRate = null;
            if (reader.Read())
            {
                shippingRate = new ShippingRateModel
                {
                    ShippingRateID = reader.GetInt32(0),
                    FromCity = reader.GetString(1),
                    FromCountry = reader.GetString(2),
                    ToCity = reader.GetString(3),
                    ToCountry = reader.GetString(4),
                    ShippingRate = reader.GetDecimal(5),
                    Currency = reader.GetString(6),
                    TransitTime = reader.GetInt32(7),
                    LastUpdatedBy = reader.GetInt32(8)
                };
            }
            conn.Close();
            return shippingRate;
        }
    }
}
