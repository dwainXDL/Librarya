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
    public partial class detailForm : Form
    {
        public detailForm(int bookID)
        {
            InitializeComponent();

            loadBookDetails(bookID);

            // Passed variables
            this.bookID = bookID;
        }

        // Global var
        private readonly int bookID;

        // Load book details
        private void loadBookDetails(int bookID)
        {
            Librarya.Classes.getDetailData detailLoader = new Librarya.Classes.getDetailData();
            DataTable dataDetail = detailLoader.detailTable(bookID);
            string coverURL = detailLoader.coverURL;

            dataGridView1.DataSource = dataDetail;
            pictureBox5.LoadAsync(coverURL);

            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.Columns[0].Width = 120;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        // Remove button
        private void button3_Click(object sender, EventArgs e)
        {
            
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
                new loginForm().Show();
                this.Hide();
            }
        }

        private void backArrow_Click(object sender, EventArgs e)
        {
            new bookTable().Show();
            this.Hide();
        }
    }
}
