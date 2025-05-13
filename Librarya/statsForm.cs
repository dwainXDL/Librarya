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
using System.Runtime.InteropServices.ComTypes;

namespace Librarya
{
    public partial class statsForm : Form
    {
        private statsData stats = new statsData();

        public statsForm()
        {
            InitializeComponent();

            // User
            label12.Text = session.user;
        }

        private void showWeeklyChart()
        {
            DataTable dt = stats.getWeeklyCounts();
            stats.renderWeeklyChart(dt, plotView1);
        }

        private void showCategoryChart()
        {
            DataTable dt = stats.getCategoryCounts();
            stats.renderCategoryChart(dt, plotView1);
        }

        private void showEmptyChart()
        {
            var dt = stats.getWeeklyCounts();
            stats.renderEmptyChart(plotView1);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            showWeeklyChart();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            showCategoryChart();
        }

        private void statsForm_Load(object sender, EventArgs e)
        {
            showCategoryChart();
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
            new issueTable().Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
