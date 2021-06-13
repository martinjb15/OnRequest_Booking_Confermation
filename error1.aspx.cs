using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnRequest_Booking_Confermation
{
    public partial class error1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "  Tours Error";

            Label1.Text = "Sorry, there has been an error. <br /><br />Please start again or email admin@ .ie if the error continues.<br /><br />Kind regards.<br /><br />  Tours<br />";

            if ((Exception)Application["TheException"] != null)
            {
                Exception caughtException = (Exception)Application["TheException"];
                SmtpClient sc = new SmtpClient("mailhost");
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
                msg.To.Add("tom@ .ie");
                msg.From = new MailAddress("informationsystems@ .ie");
                msg.IsBodyHtml = true;
                msg.Subject = "Error: On request form";
                string erro = "";

                erro += AppDomain.CurrentDomain.BaseDirectory.ToString();

                if (caughtException.Message != null)
                {
                    erro += " <br><br>Message: <br><br>" + caughtException.Message.ToString();
                }
                /*if (caughtException.InnerException.InnerException != null)
                {
                    erro += " <br><br>InnerException: <br><br>" + caughtException.InnerException.InnerException.ToString();
                }*/

                if (caughtException.InnerException != null)
                {
                    erro += " <br><br>InnerException: <br><br>" + caughtException.InnerException.ToString();
                }
                if (caughtException.StackTrace != null)
                {
                    erro += " <br><br>StackTrace: <br><br>" + caughtException.StackTrace.ToString();
                }
                if (caughtException.Source != null)
                {
                    erro += " <br><br>Source: <br><br>" + caughtException.Source.ToString();
                }

                erro += "<br><br>" + Server.MachineName.ToString();
                msg.Body = erro;
                sc.Send(msg);
            }
            else
            {
                SmtpClient sc = new SmtpClient("mailhost");
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
                msg.To.Add("martin@ .ie");
                msg.From = new MailAddress("informationsystems@ .ie");
                msg.IsBodyHtml = true;
                msg.Subject = "Error: Unknown";
                string erro = "";
                msg.Body = "Some error happened on " + Server.MachineName.ToString();
                sc.Send(msg);
            }
        }
    }
    
}