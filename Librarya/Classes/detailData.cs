using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace Librarya.Classes
{
    internal class getDetailData
    {
        SqlConnection connection = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=C:\USERS\PERSO\ONEDRIVE\DOCUMENTS\LIBRARYADB.MDF;Integrated Security=True;TrustServerCertificate=True");

        public string coverURL { get; set; }

        public DataTable detailTable(int bookID)
        {
            DataTable db = new DataTable();
            db.Columns.Add("rowName", typeof(string));
            db.Columns.Add("value", typeof(string));

            if (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();

                    string getData = "SELECT bookID, isbn, title, author, category, language, availability, publishedYear, description, dateAdded, cover FROM books WHERE bookID = @bookID";

                    using (SqlCommand cmd = new SqlCommand(getData, connection))
                    {
                        cmd.Parameters.AddWithValue("@bookID", bookID);

                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                db.Rows.Add("Book ID", reader["bookID"].ToString());
                                db.Rows.Add("ISBN", reader["isbn"].ToString());
                                db.Rows.Add("Title", reader["title"].ToString());
                                db.Rows.Add("Author", reader["author"].ToString());
                                db.Rows.Add("Category", reader["category"].ToString());
                                db.Rows.Add("Language", reader["language"].ToString());
                                db.Rows.Add("Availability", reader["availability"].ToString());
                                db.Rows.Add("Published Year", reader["publishedYear"].ToString());
                                db.Rows.Add("Description", reader["description"].ToString());
                                db.Rows.Add("Date Added", reader["dateAdded"].ToString());
                                coverURL = reader["cover"].ToString();
                            }

                            reader.Close();
                        }
                    }
                }
                catch (Exception x)
                {
                    MessageBox.Show("Database error: detailData.cs\n\n" + "Message:\n" + x, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
            return db;
        }
    }
}
