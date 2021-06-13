using System;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnRequest_Booking_Confermation
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string type = Request.QueryString["type"];
                string guid = Request.QueryString["guid"];
                Label1.Text = type;

                if (guid == null || guid == "")
                {
                    Label2.Visible = true;
                    Label2.Text = "Your request has not been found. If this is an error please contact help@ .ie";
                    Gridview1.Visible = false;
                    textbox3.Visible = false;
                    Button1.Visible = false;
                    header_form.Visible = false;
                    header_main.Visible = false;
                }
                else
                {
                    string dbname = " _tours_ireland";
                    string dbinstance = "";
                    dbinstance = " -tourplan";
                    string connetionString = "Server=" + dbinstance + ";Database=" + dbname + ";User ID=tpwebuser;Password= ";

                    System.Data.SqlClient.SqlConnection cnn = new SqlConnection(connetionString);
                    string query = "";
                    if (guid == "")
                    {
                        query = "select * from onrequest where Supplier= '"
                            + type + "' and guid is null and Status = 'RQ' ";

                    }
                    else
                    {
                        query = "select * from onrequest where Supplier= '"
                          + type +
                          "' and guid ='"
                          + guid + "' and Status = 'RQ' order by Pickup_Date";
                    }

                    cnn.Open();

                    SqlCommand queryCommand = new SqlCommand(query, cnn);
                    SqlDataReader queryCommandReader = queryCommand.ExecuteReader();
                    DataTable dataTable = new DataTable("AgentDetails");
                    dataTable.Load(queryCommandReader);

                    cnn.Close();
                    //foreach (GridViewRow row in Gridview1.Rows)
                    //{
                    //        HttpUtility.HtmlEncode(row.Cells[4].Text.ToString());  
                    //}

                    if (dataTable.Rows.Count > 0)
                    {
                        Gridview1.DataSource = dataTable;
                        Gridview1.DataBind();
                        Label1.Text = dataTable.Rows[0]["contact"].ToString();
                        Gridview1.Columns[0].Visible = false;
                        Gridview1.Columns[1].Visible = false;
                        Gridview1.Columns[3].Visible = false;
                    }
                    else
                    {
                        SmtpClient sc = new SmtpClient("mailhost");
                        System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
                        msg.To.Add("tom@ .ie");
                        msg.From = new MailAddress("informationsystems@ .ie");
                        msg.IsBodyHtml = true;
                        msg.Subject = "Error: On request form - NO ROWS";
                        string erro = "";

                        erro += AppDomain.CurrentDomain.BaseDirectory.ToString();

                        erro += "<br><br>GUID: " + guid;
                        erro += "<br><br>SUPPLIER: " + type;
                        erro += "<br><br>" + Server.MachineName.ToString();
                        msg.Body = erro;
                        sc.Send(msg);
                    }
                }
            }
           
        }


       

        public string rtfreturn(string rtftext)
        {
            string normaltext = "";
            try
            {
                Process rtfapp = new Process();
                ProcessStartInfo pstart = new ProcessStartInfo();
                pstart.FileName = "RTFtoTextApp.exe";
                pstart.RedirectStandardOutput = true;
                pstart.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\";
                pstart.UseShellExecute = false;
                pstart.Arguments = @"""" + rtftext + @"""";
                pstart.FileName = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\RTFtoTextApp.exe";
                rtfapp.StartInfo = pstart;
                rtfapp.Start();

                using (StreamReader output = rtfapp.StandardOutput)
                {
                    normaltext = output.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                normaltext = ex.InnerException.Message.ToString();
            }

            return normaltext;
        }



        protected void Gridview1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[5].Text = HttpUtility.HtmlDecode(e.Row.Cells[5].Text);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            string dbname = "";
            dbname = " _tours_ireland";
            string dbinstance = "";
            dbinstance = " -tourplan";
            string connetionString = "Server=" + dbinstance + ";Database=" + dbname + ";User ID=tpwebuser;Password= ";
            SqlConnection cnn = new SqlConnection(connetionString);
            cnn.Open();
            foreach (GridViewRow row in Gridview1.Rows)
            {
                string query1 = "select * from ONRequest where BSL_ID = '" + row.Cells[0].Text.ToString() + "'";

                SqlCommand queryCommand = new SqlCommand(query1, cnn);
                SqlDataReader queryCommandReader = queryCommand.ExecuteReader();
                DataTable dataTable = new DataTable("AgentDetails");
                dataTable.Load(queryCommandReader);
                string query = "update ONRequest set comments = @comments, Confirmed = @Confirmed, Agent_Reference = @Agent_Reference where bsl_id = @bsl_id";
                queryCommand.CommandText = query;

                if (row.RowType != DataControlRowType.Header)
                {
                    string test1 = (((DropDownList)row.Cells[12].FindControl("DropDownList1")).SelectedItem.Text);
                    string test2 = row.Cells[0].Text.ToString();
                    queryCommand.Parameters.AddWithValue("@BSL_ID", row.Cells[0].Text.ToString());
                    queryCommand.Parameters.AddWithValue("@comments", (((TextBox)row.Cells[11].FindControl("TextBox1")).Text));
                    queryCommand.Parameters.AddWithValue("@Agent_Reference", (((TextBox)row.Cells[13].FindControl("TextBox2")).Text));
                    queryCommand.Parameters.AddWithValue("@Confirmed", (((DropDownList)row.Cells[12].FindControl("DropDownList1")).SelectedItem.Text));


                    queryCommand.ExecuteNonQuery();
                }

            }
            cnn.Close();

            dbname = Request.QueryString["db"];
            if (dbname == "irl")
            {
                dbname = " _tours_ireland";
            }
            else if (dbname == "sco")
            {
                dbname = " _tours_scotland";
            }
            else if (dbname == "mk")
            {
                dbname = " _ireland";
            }
            dbinstance = "";
            dbinstance = " -tourplan";
            string connetionString1 = "Server=" + dbinstance + ";Database=" + dbname + ";User ID=tpwebuser;Password= ";
            SqlConnection cnn1 = new SqlConnection(connetionString1);
            cnn1.Open();
            foreach (GridViewRow row in Gridview1.Rows)
            {
                string confirmed = "";
                string waitlist = "";
                if (((DropDownList)row.Cells[12].FindControl("DropDownList1")).SelectedItem.Text == "Confirm")
                {
                    confirmed = "OK";

                    string query1 = "select * from ONRequest where BSL_ID = '" + row.Cells[0].Text.ToString() + "'";

                    SqlCommand queryCommand = new SqlCommand(query1, cnn1);

                    string query = "update BSL set SL_Status = @SL_Status, SUPPLIER_CONFIRMATION = @SUPPLIER_CONFIRMATION where bsl_id = @bsl_id";
                    queryCommand.CommandText = query;

                    if (row.RowType != DataControlRowType.Header)
                    {
                        string test1 = (((DropDownList)row.Cells[12].FindControl("DropDownList1")).SelectedItem.Text);
                        string test2 = row.Cells[0].Text.ToString();
                        queryCommand.Parameters.AddWithValue("@BSL_ID", row.Cells[0].Text.ToString());
                        queryCommand.Parameters.AddWithValue("@SUPPLIER_CONFIRMATION", (((TextBox)row.Cells[13].FindControl("TextBox2")).Text));
                        queryCommand.Parameters.AddWithValue("@SL_Status", confirmed);


                        queryCommand.ExecuteNonQuery();
                    }

                    queryCommand.Parameters.Clear();
                    string query2 = "update onRequest set Status = @SL_Status, Agent_Reference = @SUPPLIER_CONFIRMATION where bsl_id = @bsl_id";
                   
                    queryCommand.CommandText = query2;

                    if (row.RowType != DataControlRowType.Header)
                    {
                        string test1 = (((DropDownList)row.Cells[12].FindControl("DropDownList1")).SelectedItem.Text);
                        string test2 = row.Cells[0].Text.ToString();
                        queryCommand.Parameters.AddWithValue("@BSL_ID", row.Cells[0].Text.ToString());
                        queryCommand.Parameters.AddWithValue("@SUPPLIER_CONFIRMATION", (((TextBox)row.Cells[13].FindControl("TextBox2")).Text));
                        queryCommand.Parameters.AddWithValue("@SL_Status", confirmed);


                        queryCommand.ExecuteNonQuery();
                    }
                    
                }

                else if (((DropDownList)row.Cells[12].FindControl("DropDownList1")).SelectedItem.Text == "Wait list")
                {
                    waitlist = "WL";

                    string query1 = "select * from ONRequest where BSL_ID = '" + row.Cells[0].Text.ToString() + "'";

                    SqlCommand queryCommand = new SqlCommand(query1, cnn1);

                    string query = "update BSL set SL_Status = @SL_Status, SUPPLIER_CONFIRMATION = @SUPPLIER_CONFIRMATION where bsl_id = @bsl_id";
                    queryCommand.CommandText = query;

                    if (row.RowType != DataControlRowType.Header)
                    {
                        string test1 = (((DropDownList)row.Cells[12].FindControl("DropDownList1")).SelectedItem.Text);
                        string test2 = row.Cells[0].Text.ToString();
                        queryCommand.Parameters.AddWithValue("@BSL_ID", row.Cells[0].Text.ToString());
                        queryCommand.Parameters.AddWithValue("@SUPPLIER_CONFIRMATION", (((TextBox)row.Cells[13].FindControl("TextBox2")).Text));
                        queryCommand.Parameters.AddWithValue("@SL_Status", waitlist);


                        queryCommand.ExecuteNonQuery();
                    }
                    queryCommand.Parameters.Clear();
                    string query2 = "update onRequest set Status = @SL_Status, Agent_Reference = @SUPPLIER_CONFIRMATION where bsl_id = @bsl_id";

                    queryCommand.CommandText = query2;

                    if (row.RowType != DataControlRowType.Header)
                    {
                        string test1 = (((DropDownList)row.Cells[12].FindControl("DropDownList1")).SelectedItem.Text);
                        string test2 = row.Cells[0].Text.ToString();
                        queryCommand.Parameters.AddWithValue("@BSL_ID", row.Cells[0].Text.ToString());
                        queryCommand.Parameters.AddWithValue("@SUPPLIER_CONFIRMATION", (((TextBox)row.Cells[13].FindControl("TextBox2")).Text));
                        queryCommand.Parameters.AddWithValue("@SL_Status", waitlist);


                        queryCommand.ExecuteNonQuery();
                    }

                 
                }


                else if (((DropDownList)row.Cells[12].FindControl("DropDownList1")).SelectedItem.Text == "Reject Request")
                {
            
                }

            }
            cnn1.Close();

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    string dirpath = AppDomain.CurrentDomain.BaseDirectory;
                    dirpath += "pic\\emailTail.jpg";
                    Attachment inlineLogo = new Attachment(dirpath);
                    string contentID = "Image";
                    inlineLogo.ContentId = contentID;

                    //To make the image display as inline and not as attachment

                    inlineLogo.ContentDisposition.Inline = true;
                    inlineLogo.ContentDisposition.DispositionType = DispositionTypeNames.Inline;

                    StringReader sr = new StringReader(sw.ToString());

                    SmtpClient client = new SmtpClient("mailhost");
                    MailAddress from = new MailAddress("OnRequestBookings@ .ie");
                    MailAddress to = new MailAddress(Label1.Text.ToString());
                    MailMessage message = new MailMessage(from, to);
                    try
                    {
                        message.To.Add(textbox3.Text.ToString().Trim());

                    }
                    catch
                    {

                    }
                    message.Attachments.Add(inlineLogo);
                    message.Body =
                        "<h3>Thank you for submitting the On request form</h3>" +
                        "<p>Please find below your updated booking information.</p>" +
                        "</br>" +
                        GetGridviewData(Gridview1) +
                        "</br>" +
                        "<p>Submitted on: " + DateTime.Now + "</p>" +
                         "<p>Submitted by: " + textbox3.Text.ToString() +
                        "</br>" +
                        "" +
                        "Best Regards," + "</br>"+ "</br>"+
                        "  Group" + "</br>"+
                       "<img src =\"cid:" + contentID + "\">"+
                        "<br>";
                    



                    message.IsBodyHtml = true;
                    message.Subject = "On Request Bookings - " + Request.QueryString["type"];
                    client.Send(message);
                    inlineLogo.Dispose();
                }
            }

            Server.Transfer("HtmlPage1.html");
            Server.Transfer("HTMLPage1.html");


        }

        // This Method is used to render gridview control
        public string GetGridviewData(GridView gv)
        {
            StringBuilder strBuilder = new StringBuilder();
            StringWriter strWriter = new StringWriter(strBuilder);
            HtmlTextWriter htw = new HtmlTextWriter(strWriter);
            gv.RenderControl(htw);

            return strBuilder.ToString();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
    }
}