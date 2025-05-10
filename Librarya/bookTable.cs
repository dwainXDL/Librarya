using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Librarya
{
    public partial class bookTable : Form
    {
        SqlConnection connection = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=C:\USERS\PERSO\ONEDRIVE\DOCUMENTS\LIBRARYADB.MDF;Integrated Security=True;TrustServerCertificate=True");

        public bookTable()
        {
            InitializeComponent();

            // On run load table
            booksTable();
        }

        // Global variables
        private int bookID = 0;

        // Cell click get info
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs x)
        {
            if(x.RowIndex != -1)
            {
                DataGridViewRow row = dataGridView1.Rows[x.RowIndex];
                bookID = (int)row.Cells[0].Value;

                // Update Function Temp
                //string title = row.Cells[3].Value.ToString();

                // var frm = new addForm();
                // frm.titleText = title;
            }
        }

        // Remove button
        private void button2_Click(object sender, EventArgs e)
        {
            if (bookID == 0)
            {
                MessageBox.Show("Select an entry to delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connection.State != ConnectionState.Open)
                {
                    DialogResult dltCheck = MessageBox.Show("Are you sure you want to delete book ID " + bookID + "?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dltCheck == DialogResult.Yes)
                    {
                        connection.Open();
                        string deleteData = "DELETE FROM books WHERE bookID = @bookID";

                        using (SqlCommand cmd = new SqlCommand(deleteData, connection))
                        {
                            cmd.Parameters.AddWithValue("@bookID", bookID);

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Deleted Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            booksTable();
                        }
                    }
                }
            }
        }
        public void booksTable()
        {
            booksData db = new booksData();
            List<booksData> dataList = db.addBooksData();

            dataGridView1.DataSource = dataList;

            // Column resizing
            dataGridView1.Columns["bookID"].Width = 100;
            dataGridView1.Columns["cover"].Width = 175;
            dataGridView1.Columns["title"].Width = 325;
            dataGridView1.Columns["author"].Width = 250;
            dataGridView1.Columns["language"].Width = 125;
            dataGridView1.Columns["category"].Width = 175;
            dataGridView1.Columns["availability"].Width = 125;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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

        private void backArrow_Click(object sender, EventArgs e)
        {
            homeForm home = new homeForm();
            home.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addForm add = new addForm();
            add.Show();
            this.Hide();
        }

        private void bookTable_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
