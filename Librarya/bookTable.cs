using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Librarya
{
    public partial class bookTable : Form
    {
        public bookTable()
        {
            InitializeComponent();

            booksTable();

            dataGridView1.CellPainting += dataGridView1_CellPainting;
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs x)
        {

        }
    }
}
