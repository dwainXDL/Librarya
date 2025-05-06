using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
// SQL requisites
using System.Data.SqlClient;

namespace Librarya
{
    public partial class memberRegister : Form
    {

        // SQL connection 
        SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\perso\OneDrive\Documents\libraryaDB.mdf;Integrated Security=True;Connect Timeout=30");

        public memberRegister()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "" || textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Fill all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
            else
            {
                if(connection.State != ConnectionState.Open)
                {
                    try
                    {
                        connection.Open();

                        String checkName = "SELECT COUNT(*) FROM members WHERE name = @name";

                        using (SqlCommand checkFn = new SqlCommand(checkName, connection))
                        {
                            checkFn.Parameters.AddWithValue("@name", textBox1.Text.Trim());
                            int count = (int)checkFn.ExecuteScalar();

                            if (count >= 1)
                            {
                                MessageBox.Show(textBox1.Text.Trim() + " already registered","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                DateTime currentDate = DateTime.Today;

                                String enterData = "INSERT INTO members (name, nic, email, phoneNo, dateRegistered) " + "VALUES(@name, @nic, @email, @phoneNo, @dateRegistered)";

                                using (SqlCommand enterFn = new SqlCommand(enterData, connection))
                                {
                                    enterFn.Parameters.AddWithValue("@name", textBox1.Text.Trim());
                                    enterFn.Parameters.AddWithValue("@nic", textBox2.Text.Trim());
                                    enterFn.Parameters.AddWithValue("@email", textBox3.Text.Trim());
                                    enterFn.Parameters.AddWithValue("@phoneNo", textBox4.Text.Trim());
                                    enterFn.Parameters.AddWithValue("@dateRegistered", currentDate.ToString());

                                    enterFn.ExecuteNonQuery();

                                    MessageBox.Show("Added Member Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }


                    }
                    catch(Exception x)
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

        // Back arrow - to login form
        private void backArrow_Click(object sender, EventArgs e)
        {
            loginForm logForm = new loginForm();
            logForm.Show();
            this.Hide();
        }

        // X button
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Minimize button
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
