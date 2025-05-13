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
        SqlConnection connection = new SqlConnection(session.connectionString);

        public returnTable()
        {
            InitializeComponent();

            // On run
            displayReturnsData();

            label5.Text = session.user;
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
            headerStyle.ForeColor = Color.FromArgb(232, 232, 232);
            headerStyle.Font = new Font("Inknut Antiqua", 10F, FontStyle.Bold);
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridView1.ColumnHeadersHeight = 50;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // column sizing
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridView1.Columns["returnID"].Width = 110;
            dataGridView1.Columns["memberID"].Width = 125;
            dataGridView1.Columns["memberName"].Width = 200;
            dataGridView1.Columns["isbn"].Width = 200;
            dataGridView1.Columns["returnDate"].Width = 150;
            dataGridView1.Columns["remarks"].Width = 380;
            dataGridView1.Columns["overdueBy"].Width = 100;
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

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }
    }
}
