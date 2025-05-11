using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Librarya
{
    public partial class issueForm : Form
    {
        SqlConnection connection = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=C:\USERS\PERSO\ONEDRIVE\DOCUMENTS\LIBRARYADB.MDF;Integrated Security=True;TrustServerCertificate=True");

        public issueForm()
        {
            InitializeComponent();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            string isbn = textBox1.Text.Trim();
            if(string.IsNullOrEmpty(isbn))
            {
                return;
            }

            try
            {
                using (var cmd = new SqlCommand(
                    "SELECT title, availability" +
                    "  FROM books " +
                    " WHERE isbn = @isbn",
                    connection
                ))
                {
                    connection.Open();

                    cmd.Parameters.AddWithValue("@isbn", isbn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Fill your other fields:
                            textBox3.Text = reader.GetString(reader.GetOrdinal("title"));
                            textBox4.Text = reader.GetString(reader.GetOrdinal("availability"));
                        }
                        else
                        {
                            // ISBN not found—clear or notify
                            textBox3.Clear();
                            // … clear other fields if you like …
                            MessageBox.Show($"ISBN '{isbn}' not found.", "Lookup",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }

                        
                }
                            }
            catch (Exception ex)
            {
                MessageBox.Show("Error looking up book:\n" + ex.Message,
                                "Database Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backArrow_Click(object sender, EventArgs e)
        {
            issueTable issue = new issueTable();
            issue.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DialogResult logoutCheck = MessageBox.Show("Do you want to logout?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (logoutCheck == DialogResult.Yes)
            {
                loginForm login = new loginForm();
                login.Show();
                this.Hide();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}
