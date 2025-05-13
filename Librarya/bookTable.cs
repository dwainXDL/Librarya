using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Librarya.Classes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Librarya
{
    public partial class bookTable : Form
    {
        SqlConnection connection = new SqlConnection(session.connectionString);

        public bookTable()
        {
            InitializeComponent();

            // On run load table
            booksTable();

            label5.Text = session.user;
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

        // Detail button
        private void button3_Click_1(object sender, EventArgs e)
        {
            if (bookID == 0)
            {
                MessageBox.Show("Select a book", "Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                new detailForm(bookID).Show();
                this.Hide();
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

                            MessageBox.Show("Deleted Book Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        }

        // Table changes
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.EnableHeadersVisualStyles = false;

            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToResizeRows = false;

            // header styling
            var headerStyle = dataGridView1.ColumnHeadersDefaultCellStyle;
            headerStyle.BackColor = Color.FromArgb(92, 78, 78);  
            headerStyle.ForeColor = Color.FromArgb(232,232,232);
            headerStyle.Font = new Font("Inknut Antiqua", 10F, FontStyle.Bold);
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridView1.ColumnHeadersHeight = 50;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;



            // column sizing
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridView1.Columns["bookID"].Width = 85;
            dataGridView1.Columns["cover"].Width = 175;
            dataGridView1.Columns["title"].Width = 350;
            dataGridView1.Columns["author"].Width = 180;
            dataGridView1.Columns["language"].Width = 125;
            dataGridView1.Columns["category"].Width = 220;
            dataGridView1.Columns["availability"].Width = 130;
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

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
