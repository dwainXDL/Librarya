using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Librarya.Classes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Librarya
{
    public partial class returnForm : Form
    {
        SqlConnection connection = new SqlConnection(session.connectionString);

        public returnForm()
        {
            InitializeComponent();

            timer1.Start();

            // Set variables
            label5.Text = session.user;
        }

        // Global variables
        DateTime issueReturnDate;
        DateTime today = DateTime.Today;
        private int daysDifference = 0;

        private void textBox2_Leave(object sender, EventArgs e)
        {
            string isbn = textBox2.Text.Trim();
            if (string.IsNullOrEmpty(isbn))
            {
                return;
            }

            try
            {
                // Selecting data from books
                string bookData = "SELECT title, cover FROM books WHERE isbn = @isbn";
                using (SqlCommand cmd = new SqlCommand(bookData, connection))
                {
                    connection.Open();

                    cmd.Parameters.AddWithValue("@isbn", isbn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Fill your fields
                            textBox3.Text = reader.GetString(reader.GetOrdinal("title"));
                            pictureBox6.LoadAsync(reader.GetString(reader.GetOrdinal("cover")));
                        }
                        else
                        {
                            // ISBN not found
                            textBox2.Clear();
                            MessageBox.Show($"Book was not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception x)
            {
                MessageBox.Show("Lookup error: returnForm.cs/books\n" + x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }

            try
            {
                // Selecing data from issues table
                string issueData = "SELECT returnDate FROM issues WHERE isbn = @isbn";
                using (SqlCommand cmd = new SqlCommand(issueData, connection))
                {
                    connection.Open();

                    cmd.Parameters.AddWithValue("@isbn", isbn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Fill your fields
                            issueReturnDate = reader.GetDateTime(reader.GetOrdinal("returnDate"));

                        }
                        else
                        {
                            // ISBN not found
                            textBox2.Clear();
                            MessageBox.Show($"Book was not issued", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception x)
            {
                MessageBox.Show("Lookup error: returnForm.cs/issues\n" + x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }

            // Check overdue
            TimeSpan difference = today - issueReturnDate;
            int daysDifference = difference.Days;

            if (issueReturnDate < today)
            {
                textBox5.Text = "Overdue: " + daysDifference + " days";
            }
            else
            {
                textBox5.Text = "Not Overdue: " + Math.Abs(daysDifference) + " days left";
            }
        }

        // Check every second
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Button disable if not filled
            if (string.IsNullOrEmpty(textBox3.Text) == true || string.IsNullOrEmpty(textBox6.Text) == true)
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
        }

        // Member auto fill
        private void textBox1_Leave(object sender, EventArgs e)
        {
            string memberID = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(memberID))
            {
                return;
            }

            try
            {
                string selectData = "SELECT name FROM members WHERE memberID = @memberID";

                using (SqlCommand cmd = new SqlCommand(selectData, connection))
                {
                    connection.Open();

                    cmd.Parameters.AddWithValue("@memberID", memberID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Fill fields
                            textBox6.Text = reader.GetString(reader.GetOrdinal("name"));
                        }
                        else
                        {
                            // Member not found
                            textBox1.Clear();
                            MessageBox.Show($"Member '{memberID}' not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception x)
            {
                MessageBox.Show("Lookup error: returnForm.cs\n" + x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        // Return button
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Please fill required * fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connection.State != ConnectionState.Open)
                {
                    try
                    {
                        connection.Open();

                        string insertData = "INSERT INTO returns (memberID, isbn, returnDate, remarks, overdueBy, returnBy, memberName) VALUES(@memberID, @isbn, @returnDate, @remarks, @overdueBy, @returnBy, @memberName)";

                        using (SqlCommand cmd = new SqlCommand(insertData, connection))
                        {
                            cmd.Parameters.AddWithValue("@memberID", textBox1.Text.Trim());
                            cmd.Parameters.AddWithValue("@isbn", textBox2.Text.Trim());
                            cmd.Parameters.AddWithValue("@returnDate", today.ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@remarks", textBox8.Text.Trim());
                            cmd.Parameters.AddWithValue("@overdueBy", daysDifference);
                            cmd.Parameters.AddWithValue("@returnBy", session.user);
                            cmd.Parameters.AddWithValue("@memberName", textBox6.Text.Trim());

                            cmd.ExecuteNonQuery();

                            string updateData = "UPDATE books SET availability = @availability WHERE isbn = @isbn";
                            using (SqlCommand updateCmd = new SqlCommand(updateData, connection))
                            {
                                updateCmd.Parameters.AddWithValue("@availability", "Available");
                                updateCmd.Parameters.AddWithValue("@isbn", textBox2.Text.Trim());

                                updateCmd.ExecuteNonQuery();
                            }

                            MessageBox.Show("Book Returned Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            clearField();
                        }
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show("Database error: returnForm.cs\n\n" + "Message:\n" + x, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        // Clear field class
        public void clearField()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox6.Text = "";
            textBox8.Text = "";
            textBox3.Text = "";
            textBox5.Text = "";
            pictureBox6.Image = null;
        }

        // Clear button
        private void button3_Click(object sender, EventArgs e)
        {
            clearField();
        }

        private void backArrow_Click(object sender, EventArgs e)
        {
            new returnTable().Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DialogResult logoutCheck = MessageBox.Show("Do you want to logout?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (logoutCheck == DialogResult.Yes)
            {
                new loginForm().Show();
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

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click_1(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void returnForm_Load(object sender, EventArgs e)
        {

        }
    }
}
