using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; 

namespace Librarya.Classes
{
    internal class membersData
    {
        SqlConnection connection = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=C:\USERS\PERSO\ONEDRIVE\DOCUMENTS\LIBRARYADB.MDF;Integrated Security=True;TrustServerCertificate=True");

        [DisplayName("Member ID")]
        public int memberID { set; get; }

        [DisplayName("Name")]
        public string name { set; get; }

        [DisplayName("NIC")]
        public string nic { set; get; }

        [DisplayName("Email")]
        public string email { set; get; }

        [DisplayName("Phone No")]
        public string phoneNo { set; get; }

        [DisplayName("Date Joined")]
        public string dateRegistered { set; get; }

        public List<membersData> dataMembers()
        {
            List<membersData> listData = new List<membersData>();

            if (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();

                    string selectData = "SELECT * FROM members";

                    using (SqlCommand cmd = new SqlCommand(selectData, connection))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            membersData db = new membersData();
                            db.memberID = (int)reader["memberID"];
                            db.name = reader["name"].ToString();
                            db.nic = reader["nic"].ToString();
                            db.email = reader["email"].ToString();
                            db.phoneNo = reader["phoneNo"].ToString();
                            db.dateRegistered = reader["dateRegistered"].ToString();

                            listData.Add(db);
                        }

                        reader.Close();
                    }

                }
                catch (Exception x)
                {
                    MessageBox.Show("Database error: membersData.cs\n\n" + "Message:\n" + x, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
