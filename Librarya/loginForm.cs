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
using Librarya.Classes;
using BCrypt.Net;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Drawing.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Librarya
{
    public partial class loginForm : Form
    {

        SqlConnection connection = new SqlConnection(session.connectionString);

        public loginForm()
        {
            InitializeComponent();
        }

        // Global var
        string storeHash;

        // Only letters and backspace allowed
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true; // Block the input
            }
        }

        // login button
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Fill all required * fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connection.State != ConnectionState.Open)
                {
                    try
                    {
                        connection.Open();

                        String userData = "SELECT password FROM users WHERE username = @username";

                        using(SqlCommand dataCMD = new SqlCommand(userData, connection))
                        {
                            dataCMD.Parameters.AddWithValue("@username", textBox1.Text.Trim());

                            // To check if object received is null
                            var dataPull = dataCMD.ExecuteScalar();

                            if (dataPull == null)
                            {
                                MessageBox.Show("Incorrect username or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            
                            storeHash = dataPull.ToString();
                            string entPass = textBox2.Text.Trim();

                            // Verifying hash
                            bool passMatch = BCrypt.Net.BCrypt.Verify(entPass, storeHash);

                            //SqlDataAdapter adapter = new SqlDataAdapter(dataCMD);
                            //DataTable tempTable = new DataTable();
                            //// adapter.Fill(tempTable);

                            if(passMatch)
                            {
                                MessageBox.Show("Login Successful!\n\n" + "Welcome!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                session.user = textBox1.Text.Trim();

                                homeForm home = new homeForm();
                                home.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Incorrect username or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show("Connecting to database failed\n\n" + "Message:\n" + x, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine(x);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        // Show password checkbox
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.PasswordChar = '\0';
            }
            else
            {
                textBox2.PasswordChar = '*';
            }
        }

        // X button
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Minimize button
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void loginForm_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        
    }
}
