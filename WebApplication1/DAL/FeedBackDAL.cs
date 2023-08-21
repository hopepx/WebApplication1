using System.Data.SqlClient;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.DAL
{
    public class FeedBackDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //private MemberDAL MemberContext = new MemberDAL();
        private StaffDAL StaffContext = new StaffDAL();
        private Staff staff = new Staff();

        //Constructor
        public FeedBackDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "NPCSConnectionString");
            //Instantiate a SqlConnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);
        }

        public void Add(FeedbackEnquiry feedback, int memberid)
        {
            SqlCommand cmd = conn.CreateCommand();
            // Specify an INSERT SQL statement which will
            // return the auto-generated ParcelID after insertion
            cmd.CommandText = @"INSERT INTO FeedbackEnquiry (MemberID, Content, StaffID, Response, Status)
                        VALUES (@MemberID, @Content, @StaffID, @Response, @Status);
                        SELECT SCOPE_IDENTITY();";


            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@MemberID", memberid);
            cmd.Parameters.AddWithValue("@Content", feedback.Content);
            cmd.Parameters.AddWithValue("@StaffID", (object)feedback.StaffID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Response", (object)feedback.Response ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", feedback.Status);

            //A connection to database must be opened before any operations made.   
            conn.Open();

            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public List<FeedbackEnquiry> GetAllFeedBack()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement 
            cmd.CommandText = @"SELECT * FROM FeedBackEnquiry ORDER BY FeedBackEnquiryID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a staff list
            List<FeedbackEnquiry> feedbackList = new List<FeedbackEnquiry>();
            while (reader.Read())
            {
                feedbackList.Add(new FeedbackEnquiry
                {
                    FeedbackEnquiryID = reader.GetInt32(0),
                    MemberID = reader.GetInt32(1),
                    Content = reader.GetString(2),
                    DateTimePosted = reader.GetDateTime(3),
                    StaffID = !reader.IsDBNull(4) ? reader.GetInt32(4) : 0,
                    Response = !reader.IsDBNull(5) ? reader["Response"].ToString() : null,
                    Status = reader.GetString(6)
                });
                // feedbackList.Status = reader.GetChar(6);
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return feedbackList;
        }

        public FeedbackEnquiry GetFeedbackById(int feedbackId)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT * FROM FeedBackEnquiry WHERE FeedBackEnquiryID = @FeedbackId";
            cmd.Parameters.AddWithValue("@FeedbackId", feedbackId);
            // Open the connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            FeedbackEnquiry feedback = new FeedbackEnquiry();
            while (reader.Read())
            {
                feedback.FeedbackEnquiryID = reader.GetInt32(0);
                feedback.MemberID = reader.GetInt32(1);
                feedback.Content = reader.GetString(2);
                feedback.DateTimePosted = reader.GetDateTime(3);
                feedback.StaffID = !reader.IsDBNull(4) ? reader.GetInt32(4) : 0;
                feedback.Response = !reader.IsDBNull(5) ? reader["Response"].ToString() : null;
                // feedback.Status = reader.GetChar(6);
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return feedback;
        }

        public void Update(FeedbackEnquiry feedback, int staffid)
        {

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE FeedbackEnquiry SET Response = @response, StaffId = @staffid, Status = @status WHERE FeedBackEnquiryId = @feedbackenquiryid";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@staffid", staffid);
            cmd.Parameters.AddWithValue("@response", feedback.Response);
            cmd.Parameters.AddWithValue("@feedbackenquiryid", feedback.FeedbackEnquiryID);
            cmd.Parameters.AddWithValue("status", '1');
            //A connection to the database must be opened before any operations are made.   
            conn.Open();

            cmd.ExecuteNonQuery();

            //A connection qshould be closed after operations.
            conn.Close();
        }
    }
}

//    private IConfiguration Configuration { get; }
//    private SqlConnection conn;
//    //private MemberDAL MemberContext = new MemberDAL();
//    private StaffDAL StaffContext = new StaffDAL();
//    private Staff staff = new Staff();

//    //Constructor
//    public FeedBackDAL()
//    {
//        //Read ConnectionString from appsettings.json file
//        var builder = new ConfigurationBuilder()
//        .SetBasePath(Directory.GetCurrentDirectory())
//        .AddJsonFile("appsettings.json");
//        Configuration = builder.Build();
//        string strConn = Configuration.GetConnectionString(
//        "NPCSConnectionString");
//        //Instantiate a SqlConnection object with the
//        //Connection String read.
//        conn = new SqlConnection(strConn);
//    }

//    //public int GetMemberID(string email)
//    //{
//    //    SqlCommand cmd = conn.CreateCommand();
//    //    cmd.CommandText = "SELECT MemberID FROM Member WHERE EmailAddr = @Email";
//    //    cmd.Parameters.AddWithValue("@Email", email);

//    //    conn.Open();
//    //    int memberId = (int)cmd.ExecuteScalar();
//    //    conn.Close();

//    //    return memberId;
//    //}
//    //Create a SqlCommand object from connection object
//    //SqlCommand cmd = conn.CreateCommand();
//    //Specify an INSERT SQL statement which will
//    //return the auto-generated FeedbackEnquiryID after insertion
//    //INSERT INTO Feedback (MemberID, Comment)
//    //            SELECT m.MemberID, 'This is a feedback comment'
//    //FROM Member m
//    //INNER JOIN SomeOtherTable o ON m.SomeColumn = o.SomeColumn
//    //WHERE m.Email = 'example@example.com'
//    //SELECT m.MemberID, @Content, @DateTimePosted FROM Member m INNER JOIN FeedBackEnquiry f ON m.MemberID = f.MemberID WHERE m.MemberID = @MemberID
//    //OUTPUT INSERTED.FeedbackEnquiryID VALUES(@MemberID, @Content, @DateTimePosted)
//    public int Add(FeedBack feedback, int memberid)
//    {
//        SqlCommand cmd = new SqlCommand("INSERT INTO FeedbackEnquiry (MemberID, Content, DateTimePosted) OUTPUT INSERTED.FeedbackEnquiryID VALUES(@MemberID, @Content, @DateTimePosted)", conn);

//        //Define the parameters used in SQL statement, value for each parameter
//        //is retrieved from respective class's property.
//        cmd.Parameters.AddWithValue("@MemberID", memberid);
//        cmd.Parameters.AddWithValue("@Content", feedback.Content);
//        //cmd.Parameters.AddWithValue("@DateTimePosted", feedback.DateTimePosted);
//        feedback.DateTimePosted = DateTime.Now;
//        cmd.Parameters.AddWithValue("@DateTimePosted", feedback.DateTimePosted);

//        ////A connection to database must be opened before any operations made.   
//        conn.Open();

//        //ExecuteScalar is used to retrieve the auto-generated
//        //StaffID after executing the INSERT SQL statement
//        feedback.FeedBackEnquiryID = (int)cmd.ExecuteScalar();
//        //A connection should be closed after operations.
//        conn.Close();
//        //Return id when no error occurs.
//        return feedback.FeedBackEnquiryID;
//    }

//    public void Update(FeedBack feedback, int staffid)
//    {

//        SqlCommand cmd = conn.CreateCommand();
//        cmd.CommandText = @"UPDATE FeedbackEnquiry SET Response = @response, StaffId = @staffid, Status = @status WHERE FeedBackEnquiryId = @feedbackenquiryid";
//        //Define the parameters used in SQL statement, value for each parameter
//        //is retrieved from respective class's property.
//        cmd.Parameters.AddWithValue("@staffid", staffid);
//        cmd.Parameters.AddWithValue("@response", feedback.Response);
//        cmd.Parameters.AddWithValue("@feedbackenquiryid", feedback.FeedBackEnquiryID);
//        cmd.Parameters.AddWithValue("status", '1');
//        //A connection to the database must be opened before any operations are made.   
//        conn.Open();

//        cmd.ExecuteNonQuery();

//        //A connection qshould be closed after operations.
//        conn.Close();
//    }


//    public FeedBack GetFeedbackById(int feedbackId)
//    {
//        SqlCommand cmd = conn.CreateCommand();

//        cmd.CommandText = "SELECT * FROM FeedBackEnquiry WHERE FeedBackEnquiryID = @FeedbackId";
//        cmd.Parameters.AddWithValue("@FeedbackId", feedbackId);
//        // Open the connection
//        conn.Open();
//        //Execute the SELECT SQL through a DataReader
//        SqlDataReader reader = cmd.ExecuteReader();
//        //Read all records until the end, save data into a staff list
//        FeedBack feedback = new FeedBack();
//        while (reader.Read())
//        {
//            feedback.FeedBackEnquiryID = reader.GetInt32(0);
//            feedback.MemberID = reader.GetInt32(1);
//            feedback.Content = reader.GetString(2);
//            feedback.DateTimePosted = reader.GetDateTime(3);
//            feedback.StaffID = !reader.IsDBNull(4) ? reader.GetInt32(4) : 0;
//            feedback.Response = !reader.IsDBNull(5) ? reader["Response"].ToString() : null;
//            // feedback.Status = reader.GetChar(6);
//        }
//        //Close DataReader
//        reader.Close();
//        //Close the database connection
//        conn.Close();
//        return feedback;
//    }



//    public List<FeedBack> GetAllFeedBack()
//    {
//        //Create a SqlCommand object from connection object
//        SqlCommand cmd = conn.CreateCommand();
//        //Specify the SELECT SQL statement 
//        cmd.CommandText = @"SELECT * FROM FeedBackEnquiry ORDER BY FeedBackEnquiryID";
//        //Open a database connection
//        conn.Open();
//        //Execute the SELECT SQL through a DataReader
//        SqlDataReader reader = cmd.ExecuteReader();

//        //Read all records until the end, save data into a staff list
//        List<FeedBack> feedbackList = new List<FeedBack>();
//        while (reader.Read())
//        {
//            feedbackList.Add(new FeedBack
//            {
//                FeedBackEnquiryID = reader.GetInt32(0),
//                MemberID = reader.GetInt32(1),
//                Content = reader.GetString(2),
//                DateTimePosted = reader.GetDateTime(3),
//                StaffID = !reader.IsDBNull(4) ? reader.GetInt32(4) : 0,
//                Response = !reader.IsDBNull(5) ? reader["Response"].ToString() : null,
//            });
//            // feedbackList.Status = reader.GetChar(6);
//        }

//        //Close DataReader
//        reader.Close();
//        //Close the database connection
//        conn.Close();

//        return feedbackList;
//    }
//}