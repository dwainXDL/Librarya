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
        SqlConnection connection = new SqlConnection(session.connectionString);

        [DisplayName("Return ID")]
        public int returnID { set; get; }

        [DisplayName("Member ID")]
        public int memberID { set; get; }

        [DisplayName("Name")]
        public string memberName { set; get; }

        [DisplayName("ISBN")]
        public string isbn { set; get; }

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
                            db.memberName = reader["memberName"].ToString();
                            db.isbn = reader["isbn"].ToString();
                            db.returnDate = reader.GetDateTime(reader.GetOrdinal("returnDate")).ToString("yyyy-MM-dd");
                            db.overdueBy = (int)reader["overdueBy"];
                            db.remarks = reader["remarks"].ToString();

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
