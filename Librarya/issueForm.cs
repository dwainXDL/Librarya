using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Librarya.Classes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Librarya
{
    public partial class issueForm : Form
    {
        SqlConnection connection = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=C:\USERS\PERSO\ONEDRIVE\DOCUMENTS\LIBRARYADB.MDF;Integrated Security=True;TrustServerCertificate=True");

        public issueForm()
        {
            InitializeComponent();

            timer1.Start();
        }

        // Global var
        public int bookID = 0;
        public string username = "";

        // For return date 2 weeks time
        private void issueForm_Load(object sender, EventArgs e)
        {
            dateTimePicker2.Value = DateTime.Today.AddDays(14);
            dateTimePicker2.MinDate = DateTime.Today;
        }

        // Check for button
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Button disable if not filled
            if (string.IsNullOrEmpty(textBox1.Text) == true || textBox4.Text == "Not Available" || string.IsNullOrEmpty(textBox2.Text) == true)
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
        }

        // Member auto fill
        private void textBox2_Leave(object sender, EventArgs e)
        {
            string memberID = textBox2.Text.Trim();
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
                            textBox5.Text = reader.GetString(reader.GetOrdinal("name"));
                        }
                        else
                        {
                            // Member not found
                            textBox2.Clear();
                            MessageBox.Show($"Member '{memberID}' not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception x)
            {
                MessageBox.Show("Lookup error:\n" + x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        // Clear field class
        public void clearField()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            dateTimePicker2.Checked = false;    
            textBox6.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            pictureBox5.Image = null;
            textBox5.Text = "";
        }

        // Clear button
        private void button3_Click(object sender, EventArgs e)
        {
            clearField();
        }

        // ISBN number auto fill
        private void textBox1_Leave(object sender, EventArgs e)
        {
            string isbn = textBox1.Text.Trim();
            if(string.IsNullOrEmpty(isbn))
            {
                return;
            }

            try
            {
                string selectData = "SELECT bookID, title, availability, cover FROM books WHERE isbn = @isbn";

                using (SqlCommand cmd = new SqlCommand(selectData, connection))
                {
                    connection.Open();

                    cmd.Parameters.AddWithValue("@isbn", isbn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Fill your fields
                            bookID = reader.GetInt32(reader.GetOrdinal("bookID"));
                            textBox3.Text = reader.GetString(reader.GetOrdinal("title"));
                            textBox4.Text = reader.GetString(reader.GetOrdinal("availability"));
                            pictureBox5.LoadAsync(reader.GetString(reader.GetOrdinal("cover")));
                        }
                        else
                        {
                            // ISBN not found
                            textBox1.Clear();
                            MessageBox.Show($"ISBN '{isbn}' not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }   
                }
                            }
            catch (Exception x)
            {
                MessageBox.Show("Lookup error:\n" + x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }

        }

        // Issue button
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

                        string insertData = "INSERT INTO issues (bookID, isbn, issuedBy, memberID, issueDate, returnDate, remarks, memberName) VALUES(@bookID, @isbn, @issuedBy, @memberID, @issueDate, @returnDate, @remarks, @memberName)";

                        using (SqlCommand cmd = new SqlCommand(insertData, connection))
                        {
                            cmd.Parameters.AddWithValue("@bookID", bookID);
                            cmd.Parameters.AddWithValue("@isbn", textBox1.Text.Trim());
                            cmd.Parameters.AddWithValue("@issuedBy", session.user);
                            cmd.Parameters.AddWithValue("@memberID", textBox2.Text.Trim());
                            cmd.Parameters.AddWithValue("@issueDate", dateTimePicker1.Value);
                            cmd.Parameters.AddWithValue("@returnDate", dateTimePicker2.Value);
                            cmd.Parameters.AddWithValue("@remarks", textBox6.Text.Trim());
                            cmd.Parameters.AddWithValue("@memberName", textBox5.Text.Trim());

                            cmd.ExecuteNonQuery();

                            string updateData = "UPDATE books SET availability = @availability WHERE bookID = @bookID";
                            using (SqlCommand updateCmd = new SqlCommand(updateData, connection))
                            {
                                updateCmd.Parameters.AddWithValue("@availability", "Not Available");
                                updateCmd.Parameters.AddWithValue("@bookID", bookID);

                                updateCmd.ExecuteNonQuery();
                            }

                            MessageBox.Show("Book Issued Added Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            clearField();
                        }
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show("Database error: issueForm.cs\n\n" + "Message:\n" + x, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void backArrow_Click(object sender, EventArgs e)
        {
            new issueTable().Show();
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

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}
