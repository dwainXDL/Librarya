using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Librarya.Classes;
using System.Data.SqlClient;

namespace Librarya
{
    public partial class returnTable : Form
    {
        SqlConnection connection = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=C:\USERS\PERSO\ONEDRIVE\DOCUMENTS\LIBRARYADB.MDF;Integrated Security=True;TrustServerCertificate=True");

        public returnTable()
        {
            InitializeComponent();

            // On run
            displayReturnsData();
        }

        // Global var
        private int returnID = 0;

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs x)
        {
            if (x.RowIndex > -1)
            {
                DataGridViewRow row = dataGridView1.Rows[x.RowIndex];
                returnID = (int)row.Cells[0].Value;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (returnID == 0)
            {
                MessageBox.Show("Select an entry to delete :)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connection.State != ConnectionState.Open)
                {
                    DialogResult dltCheck = MessageBox.Show("Are you sure you want to delete return " + returnID + "?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dltCheck == DialogResult.Yes)
                    {
                        connection.Open();
                        string deleteData = "DELETE FROM returns WHERE returnID = @returnID";

                        using (SqlCommand cmd = new SqlCommand(deleteData, connection))
                        {
                            cmd.Parameters.AddWithValue("@returnID", returnID);

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Deleted Book Return Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            displayReturnsData();
                        }
                    }
                }
            }
        }

        // Display date in grid
        public void displayReturnsData()
        {
            returnsData db = new returnsData();
            List<returnsData> listData = db.dataReturns();

            dataGridView1.DataSource = listData;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
            new homeForm().Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new returnForm().Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
