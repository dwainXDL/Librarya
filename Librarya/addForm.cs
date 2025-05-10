using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using Newtonsoft.Json.Linq;

namespace Librarya
{
    public partial class addForm : Form
    {

        SqlConnection connection = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=C:\USERS\PERSO\ONEDRIVE\DOCUMENTS\LIBRARYADB.MDF;Integrated Security=True;TrustServerCertificate=True");

        // Global image path
        private string imgPath = "";

        public addForm()
        {
            InitializeComponent();
        }

        //// Parsing text fields
        //public string titleText
        //{
        //    get => textBox1.Text;
        //    set => textBox1.Text = value;
        //}

        // Clear fields class
        public void clearField()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox3.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            textBox5.Text = "";
            textBox6.Text = "";
            textBox8.Text = "";
            pictureBox5.Image = null;
        }

        // Clear button
        private void button3_Click(object sender, EventArgs e)
        {
            clearField();
        }

        // Insert image button
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Image files (*.jpg;*.png)|*.jpg;*.png";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    imgPath = dialog.FileName;
                    pictureBox5.ImageLocation = imgPath;
                }
            }
            catch (Exception x)
            {
                MessageBox.Show("Error: " + x, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Add button
        private void button1_Click(object sender, EventArgs e)
        {
            if(pictureBox5.Image == null || textBox1.Text == "" || textBox2.Text == "" || comboBox2.Text == "" || textBox6.Text == "")
            {
                MessageBox.Show("Please fill required * fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if(connection.State == ConnectionState.Closed)
                {
                    try
                    {
                        DateTime today = DateTime.Today;
                        connection.Open();
                        string insertData = "INSERT INTO books " + "(cover, title, author, category, language, publishedYear, isbn, description, dateAdded) " + "VALUES(@cover, @title, @author, @category, @language, @publishedYear, @isbn, @description, @dateAdded)";

                        using (SqlCommand cmd = new SqlCommand(insertData, connection))
                        {
                            cmd.Parameters.AddWithValue("@cover", pictureBox5.ImageLocation);
                            cmd.Parameters.AddWithValue("@author", textBox2.Text.Trim());
                            cmd.Parameters.AddWithValue("@title", textBox1.Text.Trim());
                            cmd.Parameters.AddWithValue("@category", comboBox3.Text.Trim());
                            cmd.Parameters.AddWithValue("@language", comboBox2.Text.Trim());
                            cmd.Parameters.AddWithValue("@publishedYear", textBox5.Text.Trim());
                            cmd.Parameters.AddWithValue("@isbn", textBox6.Text.Trim());
                            cmd.Parameters.AddWithValue("@description", textBox8.Text.Trim());
                            cmd.Parameters.AddWithValue("@dateAdded", today.ToString());

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Book Added Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            clearField();
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
        }

        // Blocks everything except numbers
        private void textBox5_KeyPress(object sender, KeyPressEventArgs key)
        {
            if (char.IsControl(key.KeyChar))
            {
                return;
            }
            
            if (!char.IsDigit(key.KeyChar))
                key.Handled = true;
        }

        private void backArrow_Click(object sender, EventArgs e)
        {
            bookTable book = new bookTable();
            book.Show();
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

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}
