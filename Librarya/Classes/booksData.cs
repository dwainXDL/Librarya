using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.ComponentModel;

namespace Librarya
{
    internal class booksData
    {
        SqlConnection connection = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=C:\USERS\PERSO\ONEDRIVE\DOCUMENTS\LIBRARYADB.MDF;Integrated Security=True;TrustServerCertificate=True");

        [DisplayName("Book ID")]
        public int bookID { set; get; }

        [DisplayName("Cover")]
        public string cover { set; get; }

        [DisplayName("Title")]
        public string title { set; get; }

        [DisplayName("Author")]
        public string author { set; get; }

        [DisplayName("Category")]
        public string category { set; get; }

        [DisplayName("Language")]
        public string language { set; get; }

        [DisplayName("Availability")]
        public string availability { set; get; }

        public List<booksData> addBooksData()
        {
            List<booksData> dataList = new List<booksData>();

            if(connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();

                    string selectData = "SELECT * FROM books";

                    using (SqlCommand select = new SqlCommand(selectData, connection))
                    {
                        SqlDataReader reader = select.ExecuteReader();

                        while (reader.Read())
                        {
                            booksData db = new booksData();
                            db.bookID = (int)reader["bookID"];
                            db.cover = reader["cover"].ToString();
                            db.title = reader["title"].ToString();
                            db.author = reader["author"].ToString();
                            db.category = reader["category"].ToString();
                            db.language = reader["language"].ToString();
                            db.availability = reader["availability"].ToString();

                            dataList.Add(db);
                        }

                        reader.Close();
                    }
                }
                catch (Exception x)
                {
                    MessageBox.Show("Database error: booksData.cs\n\n" + "Message:\n" + x, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
            return dataList;
        }

    }
}
