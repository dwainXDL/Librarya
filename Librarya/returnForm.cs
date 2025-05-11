using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Librarya
{
    public partial class returnForm : Form
    {
        public returnForm()
        {
            InitializeComponent();
        }

        // Clear field class
        public void clearField()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox5.Text = "";
            dateTimePicker2.Checked = false;
            textBox8.Text = "";
            textBox3.Text = "";
            pictureBox5.Image = null;
            label8.Enabled = false;
            label9.Enabled = false;
        }

        // Clear button
        private void button3_Click(object sender, EventArgs e)
        {
            clearField();
        }

        private void backArrow_Click(object sender, EventArgs e)
        {
            new returnTable().Show();
            this.Hide();
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
