using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace Librarya.Classes
{
    internal class issuesData
    {
        SqlConnection connection = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=C:\USERS\PERSO\ONEDRIVE\DOCUMENTS\LIBRARYADB.MDF;Integrated Security=True;TrustServerCertificate=True");

        [DisplayName("Issue ID")]
        public int issueID { set; get; }

        [DisplayName("Book ID")]
        public int bookID { set; get; }

        [DisplayName("ISBN")]
        public string isbn { set; get; }

        [DisplayName("Issued By")]
        public string issuedBy { set; get; }

        [DisplayName("Member ID")]
        public int memberID { set; get; }

        [DisplayName("Member Name")]
        public string memberName { set; get; }

        [DisplayName("Issued Date")]
        public string issueDate { set; get; }

        [DisplayName("Return Date")]
        public string returnDate { set; get; }

        [DisplayName("Remarks")]
        public string remarks { set; get; }

        public List<issuesData> dataIssues()
        {
            List<issuesData> listData = new List<issuesData>();

            if(connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();

                    string selectData = "SELECT * FROM issues";

                    using (SqlCommand cmd = new SqlCommand(selectData, connection))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            issuesData db = new issuesData();
                            db.issueID = (int)reader["issueID"];
                            db.bookID = (int)reader["bookID"];
                            db.isbn = reader["isbn"].ToString();
                            db.issuedBy = reader["issuedBy"].ToString();
                            db.memberID = (int)reader["memberID"];
                            db.memberName = reader["memberName"].ToString();
                            db.issueDate = reader.GetDateTime(reader.GetOrdinal("issueDate")).ToString("yyyy-MM-dd");
                            db.returnDate = reader.GetDateTime(reader.GetOrdinal("returnDate")).ToString("yyyy-MM-dd");
                            db.remarks = reader["remarks"].ToString();

                            listData.Add(db);
                        }

                        reader.Close();
                    }

                }
                catch (Exception x)
                {
                    MessageBox.Show("Database error: issueData.cs\n\n" + "Message:\n" + x, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
