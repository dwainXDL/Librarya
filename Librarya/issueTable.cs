using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Librarya.Classes;

namespace Librarya
{
    public partial class issueTable : Form
    {
        SqlConnection connection = new SqlConnection(session.connectionString);

        public issueTable()
        {
            InitializeComponent();

            displayIssuesData();

            label5.Text = session.user;
        }

        // Global var
        private int issueID = 0;

        // Cell click Event
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs x)
        {
            if (x.RowIndex > -1)
            {
                DataGridViewRow row = dataGridView1.Rows[x.RowIndex];
                issueID = (int)row.Cells[0].Value;
            }
        }

        // Remove button 
        private void button1_Click(object sender, EventArgs e)
        {
            if (issueID == 0)
            {
                MessageBox.Show("Select an entry to delete :)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connection.State != ConnectionState.Open)
                {
                    DialogResult dltCheck = MessageBox.Show("Are you sure you want to delete issue " + issueID + "?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dltCheck == DialogResult.Yes)
                    {
                        connection.Open();
                        string deleteData = "DELETE FROM issues WHERE issueID = @issueID";

                        using (SqlCommand cmd = new SqlCommand(deleteData, connection))
                        {
                            cmd.Parameters.AddWithValue("@issueID", issueID);

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Deleted Book Issue Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            displayIssuesData();
                        }
                    }
                }
            }
        }

        // Display date in grid
        public void displayIssuesData()
        {
            issuesData db = new issuesData();
            List<issuesData> listData = db.dataIssues();

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
            dataGridView1.Columns["issueID"].Width = 95;
            dataGridView1.Columns["bookID"].Width = 100;
            dataGridView1.Columns["isbn"].Width = 200;
            dataGridView1.Columns["issuedBy"].Width = 100;
            dataGridView1.Columns["memberID"].Width = 125;
            dataGridView1.Columns["memberName"].Width = 150;
            dataGridView1.Columns["issueDate"].Width = 130;
            dataGridView1.Columns["returnDate"].Width = 130;
            dataGridView1.Columns["remarks"].Width = 235;
        }

        private void backArrow_Click(object sender, EventArgs e)
        {
            homeForm home = new homeForm();
            home.Show();
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

        private void button2_Click(object sender, EventArgs e)
        {
            issueForm issue = new issueForm();
            issue.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
