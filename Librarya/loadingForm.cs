using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Librarya.Classes;

namespace Librarya
{
    public partial class loadingForm : Form
    {
        public loadingForm()
        {
            // Mail service secrets load
            Dictionary<string, string> secrets = session.loadSecrets(@"D:\Work\Uni\NSBM\C# Assignment\Librarya\Librarya\Librarya\Secrets\mail.secret");
            session.emailSender = secrets["EMAIL_SENDER"];
            session.apiKey = secrets["MJ_APIKEY_PUBLIC"];
            session.apiSecret = secrets["MJ_APIKEY_PRIVATE"];

            overdueNotice overdueNote = new overdueNotice();
            overdueNote.overdueMails(12, 0);

            // Mail test
            //overdueNote.sendOverdueNotice();

            InitializeComponent();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            loadingBar.Width += 5;

            if (loadingBar.Width > 600)
            {
                timer.Stop();

                //loginForm login = new loginForm();
                //login.Show();
                //this.Hide();

                new loginForm().Show();
                this.Hide();
            }

            
        }
        private void Loading_Load(object sender, EventArgs e)
        {

            // Rounded corner for loading bar
            int radius = 15;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90); // Top-left
            path.AddArc(panel1.Width - radius, 0, radius, radius, 270, 90); // Top-right
            path.AddArc(panel1.Width - radius, panel1.Height - radius, radius, radius, 0, 90); // Bottom-right
            path.AddArc(0, panel1.Height - radius, radius, radius, 90, 90); // Bottom-left
            path.CloseAllFigures();
            panel1.Region = new Region(path);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
