using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Mail dependencies
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Windows.Forms;
using System.Data;
using System.Drawing.Text;

namespace Librarya.Classes
{
    internal class overdueNotice
    {
        SqlConnection connection = new SqlConnection(session.connectionString);

        private System.Threading.Timer dailyTimer;

        public void overdueMails(int hour, int minute)
        {
            DateTime now = DateTime.Now;
            DateTime nextRunTime = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);

            if (nextRunTime <= now)
            {
                nextRunTime = nextRunTime.AddDays(1);
            }

            TimeSpan initialDelay = nextRunTime - now;
            if (initialDelay.TotalMilliseconds < 0)
            {
                initialDelay = TimeSpan.Zero;
            }

            TimeSpan period = TimeSpan.FromDays(1);

            dailyTimer = new System.Threading.Timer(timerCallBack, null, initialDelay, period);
        }

        private void timerCallBack(object state)
        {
            try
            {
                sendOverdueNotice();
            }
            catch (Exception x)
            {
                MessageBox.Show("Mail error: overdueNotice.cs\n\n" + "Message:\n" + x, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void sendOverdueNotice()
        {
            if (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();

                    string selectCmd = @"SELECT m.email, m.name, b.title, i.returnDate FROM dbo.issues i JOIN dbo.members   m ON i.memberID = m.memberID JOIN dbo.books   b ON i.bookID    = b.bookID WHERE CAST(i.returnDate AS DATE) = CAST(DATEADD(DAY, 1, GETDATE()) AS DATE) AND b.availability = N'Not Available';";
                    using (SqlCommand cmd = new SqlCommand(selectCmd, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string email = reader["email"].ToString();
                                string name = reader["name"].ToString();
                                string title = reader["title"].ToString();

                                string subjectCrt = "Librarya - Book Overdue Notice";
                                string bodyCrt =
                                $"Dear {name},<br><br>" +
                                $"The book “<b>{title}</b>” is due <b>Tomorrow</b>.<br><br>" +
                                $"Please do return it at your earliest convenience. " +
                                $"Failing to do so may result in <b>fine</b>.<br><br>" +
                                "Thank you,<br>" +
                                "Librarya";

                                sendMail(email, subjectCrt, bodyCrt);
                            }
                            reader.Close();
                        }
                    }
                }
                catch (Exception x)
                {
                    MessageBox.Show("Database error: overdueNotice.cs\n\n" + "Message:\n" + x, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void sendMail(string memberMail, string subject, string body)
        {
            try
            {
                MailMessage msg = new MailMessage(session.emailSender, memberMail, subject, body);

                using (SmtpClient smtp = new SmtpClient(session.smtpHost, session.smtpPort))
                {
                    smtp.Credentials = new NetworkCredential(session.apiKey, session.apiSecret);
                    smtp.EnableSsl = true;
                    msg.IsBodyHtml = true;
                    smtp.Send(msg);
                }
            }
            catch (Exception x)
            {
                MessageBox.Show("Mail error: overdueNotice.cs\n\n" + "Message:\n" + x, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
