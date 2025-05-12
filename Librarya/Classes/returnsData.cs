using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.ComponentModel;

namespace Librarya.Classes
{
    internal class returnsData
    {
        SqlConnection connection = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=C:\USERS\PERSO\ONEDRIVE\DOCUMENTS\LIBRARYADB.MDF;Integrated Security=True;TrustServerCertificate=True");

        [DisplayName("Return ID")]
        public int returnID { set; get; }

        [DisplayName("Member ID")]
        public int memberID { set; get; }

        [DisplayName("ISBN")]
        public string isbn { set; get; }

        [DisplayName("Returned By")]
        public string returnBy { set; get; }

        [DisplayName("Returned Date")]
        public string returnDate { set; get; }

        [DisplayName("Remarks")]
        public string remarks { set; get; }

        [DisplayName("Overdue By")]
        public int overdueBy { set; get; }

        public List<returnsData> dataReturns()
        {
            List<returnsData> listData = new List<returnsData>();

            if (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();

                    string selectData = "SELECT * FROM returns";

                    using (SqlCommand cmd = new SqlCommand(selectData, connection))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            returnsData db = new returnsData();
                            db.returnID = (int)reader["returnID"];
                            db.memberID = (int)reader["memberID"];
                            db.isbn = reader["isbn"].ToString();
                            db.returnBy = reader["returnBy"].ToString();
                            db.returnDate = reader["returnDate"].ToString();
                            db.remarks = reader["remarks"].ToString();
                            db.overdueBy = (int)reader["overdueBy"];

                            listData.Add(db);
                        }

                        reader.Close();
                    }

                }
                catch (Exception x)
                {
                    MessageBox.Show("Database error: returnsData.cs\n\n" + "Message:\n" + x, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }

            return listData;
        }

    }
}
