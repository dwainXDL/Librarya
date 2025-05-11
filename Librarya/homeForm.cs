using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Librarya
{
    public partial class homeForm : Form
    {
        SqlConnection connection = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=C:\USERS\PERSO\ONEDRIVE\DOCUMENTS\LIBRARYADB.MDF;Integrated Security=True;TrustServerCertificate=True");

        public homeForm()
        {
            InitializeComponent();

            // On run
            bookCountLabel();
            issueCountLabel();
            memberCountLabel();
        }

        // Database record count updates
        private void bookCountLabel()
        {
            string countData = "SELECT COUNT(*) FROM books";

            if(connection.State == ConnectionState.Closed)
            {
                try
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(countData, connection))
                    {
                        int count = (int)cmd.ExecuteScalar();
                        label2.Text = count.ToString();
                    }
                }
                catch (Exception x)
                {
                    MessageBox.Show("Database error: \n\n" + "Message:\n" + x, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void issueCountLabel()
        {
            string countData = "SELECT COUNT(*) FROM issues";

            if (connection.State == ConnectionState.Closed)
            {
                try
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(countData, connection))
                    {
                        int count = (int)cmd.ExecuteScalar();
                        label5.Text = count.ToString();
                    }
                }
                catch (Exception x)
                {
                    MessageBox.Show("Database error: \n\n" + "Message:\n" + x, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void memberCountLabel()
        {
            string countData = "SELECT COUNT(*) FROM members";

            if (connection.State == ConnectionState.Closed)
            {
                try
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(countData, connection))
                    {
                        int count = (int)cmd.ExecuteScalar();
                        label6.Text = count.ToString();
                    }
                }
                catch (Exception x)
                {
                    MessageBox.Show("Database error: \n\n" + "Message:\n" + x, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        // Home button
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            homeForm home = new homeForm();
            home.Show();
            this.Hide();
        }

        // Members button
        private void label6_Click(object sender, EventArgs e)
        {
            memberTable member = new memberTable();
            member.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            memberTable member = new memberTable();
            member.Show();
            this.Hide();
        }

        // Issue button

        private void label5_Click(object sender, EventArgs e)
        {
            issueTable issue = new issueTable();
            issue.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            issueTable issue = new issueTable();
            issue.Show();
            this.Hide();
        }

        // Return button
        private void label3_Click(object sender, EventArgs e)
        {
            returnTable returnT = new returnTable();
            returnT.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            returnTable returnT = new returnTable();
            returnT.Show();
            this.Hide();
        }

        // Book button
        private void label2_Click(object sender, EventArgs e)
        {
            bookTable book = new bookTable();
            book.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bookTable book = new bookTable();
            book.Show();
            this.Hide();
        }

        // Logout button
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

        // Minimize button
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // Exit button
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void homeForm_Load(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
