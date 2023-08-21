using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;
using WebApplication1.Models;

namespace WebApplication1.DAL
{
    public class PaymentTransactionDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public PaymentTransactionDAL()
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

        public List<PaymentTransaction> GetAllTransactions()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM PaymentTransaction ORDER BY TransactionID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            List<PaymentTransaction> transactionList = new List<PaymentTransaction>();
            while (reader.Read())
            {
                transactionList.Add(
                  new PaymentTransaction
                  {
                      TransactionID = reader.GetInt32(0),
                      ParcelID = reader.GetInt32(1),
                      AmtTran = reader.GetDecimal(2),
                      Currency = reader.GetString(3),
                      TranType = reader.GetString(4),
                      TranDate = reader.GetDateTime(5)
                  }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection conn.Close();
            return transactionList;

        }

        public void AddPaymentTransaction(PaymentTransaction paymentTransaction)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO PaymentTransaction (ParcelID, AmtTran, Currency, TranType)
                           
                            VALUES(@parcelID, @amtTran, @currency, @tranType)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@parcelID", paymentTransaction.ParcelID);
            cmd.Parameters.AddWithValue("@amtTran", paymentTransaction.AmtTran);
            cmd.Parameters.AddWithValue("@currency", paymentTransaction.Currency);
            cmd.Parameters.AddWithValue("@tranType", paymentTransaction.TranType);


            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
        }
    }
}
