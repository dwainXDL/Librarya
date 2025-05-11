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

namespace Librarya
{
    public partial class memberTable : Form
    {
        SqlConnection connection = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=C:\USERS\PERSO\ONEDRIVE\DOCUMENTS\LIBRARYADB.MDF;Integrated Security=True;TrustServerCertificate=True");

        public memberTable()
        {
            InitializeComponent();

            displayMembersData();
        }

        // Global var
        private int memberID = 0;

        // Cell click Event
        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs x)
        {
            if (x.RowIndex > -1)
            {
                DataGridViewRow row = dataGridView1.Rows[x.RowIndex];
                memberID = (int)row.Cells[0].Value;
            }
        }

        // Remove button 
        private void button3_Click(object sender, EventArgs e)
        {
            if (memberID == 0)
            {
                MessageBox.Show("Select an entry to delete :)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connection.State != ConnectionState.Open)
                {
                    DialogResult dltCheck = MessageBox.Show("Are you sure you want to delete member " + memberID + "?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dltCheck == DialogResult.Yes)
                    {
                        connection.Open();
                        string deleteData = "DELETE FROM members WHERE memberID = @memberID";

                        using (SqlCommand cmd = new SqlCommand(deleteData, connection))
                        {
                            cmd.Parameters.AddWithValue("@memberID", memberID);

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Deleted Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            displayMembersData();
                        }
                    }
                }
            }
        }

        // Display date in grid
        public void displayMembersData()
        {
            membersData db = new membersData();
            List<membersData> listData = db.dataMembers();

            dataGridView1.DataSource = listData;
        }

        private void backArrow_Click(object sender, EventArgs e)
        {
            new homeForm().Show();
            this.Hide();
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

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new memberRegister().Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
