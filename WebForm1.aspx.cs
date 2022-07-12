using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnRequest_Booking_Confermation
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            string dbname = "";
            if (RadioButtonList1.Text == "0")
            {
                dbname = " _tours_Scotland";
            }
            if (RadioButtonList1.Text == "1")
            {
                dbname = " _tours_ireland";
            }
            if (RadioButtonList1.Text == "2")
            {
                dbname = " _ireland";
            }
            string dbinstance = "";
            string query = "";
            dbinstance = " -tourplan";
            string connetionString = "Server=" + dbinstance + ";Database=" + dbname + ";User ID=;Password=";
            SqlConnection cnn = new SqlConnection(connetionString);

            //and bhd.agent = '"+textbox3.Text.ToString().Trim() +"'
            string query7 = "";
            if (textbox3.Text.ToString().Trim() != "")
            {
                query7 += "and bhd.agent = '" + textbox3.Text.ToString().Trim() + "'";
            }


            //query = "select distinct bsl.BSL_ID, opt.Supplier, bhd.FULL_REFERENCE, bhd.AGENT, bhd.Name,opt.Description, SL_STATUS, bsl.PICKUP_DATE, bsl.DROPOFF_DATE, bsd.pax, cast(case when CATEGORY='SSN' then nts.MESSAGE_TEXT else '' end as varchar(max)) as 'message_text',opt.service, sa4.DESCRIPTION as 'Language' from BHD left join BSL on bhd.BHD_ID = BSL.BHD_ID left join OPT on opt.OPT_ID = BSL.OPT_ID left join BSD on bsd.BHD_ID = BHD.BHD_ID and BSD.BSL_ID = bsl.BSL_ID left join nts on bsl.BSL_ID = nts.BSL_ID left join DRM on bhd.AGENT = drm.CODE left join sa4 on SALE4 = sa4.CODE left join  _Tours_Ireland.dbo.ONRequest on  _Tours_Ireland.dbo.ONRequest.BSL_ID= bsl.BSL_ID where opt.Supplier= '"
            //    + textbox1.Text.ToString().Trim() +
            //    "' and SL_Status = 'RQ' and ONRequest.BSL_ID is null";

            query = "select distinct bsl.BSL_ID, opt.Supplier, bhd.FULL_REFERENCE, bhd.AGENT, bhd.Name,opt.Description, SL_STATUS, bsl.PICKUP_DATE, bsl.DROPOFF_DATE, case when bsd.ESC > 0 then CAST(bsd.pax as varchar) + '+' + CAST(bsd.esc as varchar) else CAST(bsd.pax as varchar) end as 'pax', " +
                    "cast(case when CATEGORY = 'SSN' then nts.MESSAGE_TEXT else '' end as varchar(max)) as 'message_text',opt.service, sa4.DESCRIPTION as 'Language', sa6.DESCRIPTION as 'Nationality' from BHD " +
                    "left join BSL on bhd.BHD_ID = BSL.BHD_ID " +
                    "left join OPT on opt.OPT_ID = BSL.OPT_ID " +
                    "left join BSD on bsd.BHD_ID = BHD.BHD_ID and BSD.BSL_ID = bsl.BSL_ID " +
                    "left join nts on bsl.BSL_ID = nts.BSL_ID " +
                    "left join DRM on bhd.AGENT = drm.CODE " +
                    "left join sa4 on SALE4 = sa4.CODE " +
                    "left join sa6 on SALE6 = sa6.CODE " +
                    "left join  _Tours_Ireland.dbo.ONRequest on  _Tours_Ireland.dbo.ONRequest.BSL_ID = bsl.BSL_ID where opt.Supplier = '" + textbox1.Text.ToString().Trim() + "' " + query7 + " and SL_Status = 'RQ' and ONRequest.BSL_ID is null ";

            cnn.Open();

            SqlCommand queryCommand = new SqlCommand(query, cnn);
            SqlDataReader queryCommandReader = queryCommand.ExecuteReader();
            DataTable dataTable = new DataTable("AgentDetails");
            dataTable.Load(queryCommandReader);
            dataTable.Columns["MESSAGE_TEXT"].ReadOnly = false;
            cnn.Close();
            if (Gridview1.Rows.Count >= 0)
            {
                foreach (DataRow dr in dataTable.Rows)
                    if (dr["MESSAGE_TEXT"].ToString().Trim() != "")
                    {
                        {
                            string any = dr["MESSAGE_TEXT"].ToString().Trim();
                            dr["MESSAGE_TEXT"] = rtfreturn(dr["MESSAGE_TEXT"].ToString().Trim());

                            if (dr["MESSAGE_TEXT"].ToString().Contains("Error"))
                            {
                                dr["MESSAGE_TEXT"] = "ERROR  " + any;
                            }
                        }
                    }
            }
            Gridview1.DataSource = dataTable;
            Gridview1.DataBind();
           

            LinkButton2.Visible = true;
            text.Visible = true;
            }
        

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            textbox1.Text = string.Empty;
            Gridview1.DataSource = null;
            Gridview1.DataBind();
            Label1.Text = "";
            Label4.Text = "";
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Guid newGuid = Guid.NewGuid();
            string dbname = "";
            dbname = " _tours_ireland";
            string dbinstance = "";
            dbinstance = " -tourplan";
            string connetionString = "Server=" + dbinstance + ";Database=" + dbname + ";User ID=;Password=";
            SqlConnection cnn = new SqlConnection(connetionString);
            cnn.Open();
            foreach (GridViewRow row in Gridview1.Rows)
            {
                if (((CheckBox)row.Cells[0].FindControl("CheckBox1")).Checked)
                {
                    string query1 = "select * from ONRequest where BSL_ID = '" + row.Cells[1].Text.ToString() + "'";

                    SqlCommand queryCommand = new SqlCommand(query1, cnn);
                    SqlDataReader queryCommandReader = queryCommand.ExecuteReader();
                    DataTable dataTable = new DataTable("AgentDetails");
                    dataTable.Load(queryCommandReader);


                    if (dataTable.Rows.Count == 0)
                    {

                        string query = "insert into ONRequest values (@Supplier, @Full_Reference, @Agent, @Booking_Name, @Description,  @Status, @Pickup_Date, @DropOff_Date, '', '', '', @BSL_ID, @contact, @Pax, @Message_text,  @GUID, @Language, @Nationality_val, @Date)";
                        queryCommand.CommandText = query;

                        if (row.RowType != DataControlRowType.Header)
                        {
                            try
                            {
                                queryCommand.Parameters.AddWithValue("@BSL_ID", row.Cells[1].Text.ToString().Trim());
                                queryCommand.Parameters.AddWithValue("@Supplier", row.Cells[2].Text.ToString().Trim());
                                queryCommand.Parameters.AddWithValue("@Full_Reference", row.Cells[3].Text.ToString().Trim());
                                queryCommand.Parameters.AddWithValue("@Agent", HttpUtility.HtmlDecode(row.Cells[4].Text.ToString().Trim()));
                                queryCommand.Parameters.AddWithValue("@Booking_Name", HttpUtility.HtmlDecode(row.Cells[5].Text.ToString().Trim()));
                                queryCommand.Parameters.AddWithValue("@Description", HttpUtility.HtmlDecode(row.Cells[6].Text.ToString().Trim()));
                                queryCommand.Parameters.AddWithValue("@Status", row.Cells[7].Text.ToString().Trim());
                                queryCommand.Parameters.AddWithValue("@Pickup_Date", Convert.ToDateTime(row.Cells[8].Text.ToString().Trim()));
                                queryCommand.Parameters.AddWithValue("@DropOff_Date", Convert.ToDateTime(row.Cells[9].Text.ToString().Trim()));
                                queryCommand.Parameters.AddWithValue("@contact", textbox2.Text.ToString().Trim());
                                queryCommand.Parameters.AddWithValue("@Pax", row.Cells[10].Text.ToString().Trim());
                                string tem = row.Cells[11].Text.ToString().Trim();
                                if (tem == "&nbsp;")
                                {
                                    tem = "";
                                }
                                queryCommand.Parameters.AddWithValue("@Message_text", tem);
                                queryCommand.Parameters.AddWithValue("@Language", row.Cells[13].Text.ToString().Trim());
                                queryCommand.Parameters.AddWithValue("@Nationality_val", row.Cells[14].Text.ToString().Trim());
                                queryCommand.Parameters.AddWithValue("@GUID", newGuid);
                                queryCommand.Parameters.AddWithValue("@Date", DateTime.Now.ToShortDateString().Trim());
                                int row1 = queryCommand.ExecuteNonQuery();
                            }
                            catch(Exception xe)
                            {
                                string ex = xe.Message;
                            }
                        }

                    }
                    else
                    {
                        string query2 = "update ONRequest set BSL_ID = @BSL_ID, Supplier = @Supplier, Full_Reference = @Full_Reference, Agent = @Agent, Description = @Description, Booking_Name = @Booking_Name, Status = @Status, Pickup_Date = @Pickup_Date, DropOff_Date = @DropOff_Date, contact = @contact, Pax = @Pax, GUID = @GUID, Language = @Language, Nationality = @Nationality, Date = @Date where bsl_id = @bsl_id";
                        queryCommand.CommandText = query2;

                         if (row.RowType != DataControlRowType.Header)
                        {
                            queryCommand.Parameters.AddWithValue("@BSL_ID", row.Cells[1].Text.ToString());
                            queryCommand.Parameters.AddWithValue("@Supplier", row.Cells[2].Text.ToString());
                            queryCommand.Parameters.AddWithValue("@Full_Reference", row.Cells[3].Text.ToString());
                            queryCommand.Parameters.AddWithValue("@Agent", HttpUtility.HtmlEncode(row.Cells[4].Text.ToString()));
                            queryCommand.Parameters.AddWithValue("@Booking_Name", HttpUtility.HtmlDecode(row.Cells[5].Text.ToString()));
                            queryCommand.Parameters.AddWithValue("@Description", HttpUtility.HtmlDecode(row.Cells[6].Text.ToString()));
                            queryCommand.Parameters.AddWithValue("@Status", row.Cells[7].Text.ToString());
                            queryCommand.Parameters.AddWithValue("@Pickup_Date", Convert.ToDateTime(row.Cells[8].Text.ToString()));
                            queryCommand.Parameters.AddWithValue("@DropOff_Date", Convert.ToDateTime(row.Cells[9].Text.ToString()));
                            queryCommand.Parameters.AddWithValue("@contact", textbox2.Text.ToString());
                            queryCommand.Parameters.AddWithValue("@Pax", row.Cells[10].Text.ToString());
                            queryCommand.Parameters.AddWithValue("@Message_text", row.Cells[11].Text.ToString());
                            queryCommand.Parameters.AddWithValue("@Language", row.Cells[13].Text.ToString());
                            queryCommand.Parameters.AddWithValue("@Nationality", row.Cells[14].Text.ToString());
                            queryCommand.Parameters.AddWithValue("@GUID", newGuid);
                            queryCommand.Parameters.AddWithValue("@Date", DateTime.Now.ToShortDateString());

                            int row1 = queryCommand.ExecuteNonQuery();
                        }
                    }
                }

            }
            cnn.Close();


            dbname = "";
            if (RadioButtonList1.Text == "0")
            {
                dbname = "sco";
            }
            if (RadioButtonList1.Text == "1")
            {
                dbname = "irl";
            }
            if (RadioButtonList1.Text == "2")
            {
                dbname = "mk";
            }

            string url = "https://web. .ie/onRequest/webform2.aspx";
            string type = "?type=" + textbox1.Text.ToString().Trim();
            string type2 = "&db=" + dbname;
            Label1.Text = url + type + type2 + "&guid=" + newGuid;

            Label4.Text = "<a href=" + url + type + type2 + "&guid="+ newGuid + "> Here<a/>";

            SmtpClient client = new SmtpClient("mailhost");
            MailAddress from = new MailAddress("OnRequestBookings@ .ie");
            MailAddress to = new MailAddress(textbox2.Text.ToString().Trim());
            MailMessage message = new MailMessage(from, to);
            message.Body = "<br /> <h3>You have successfully submitted your request to " + textbox1.Text.ToString();
            message.IsBodyHtml = true;
            message.Subject = "OnRequestBookings";
            client.Send(message);

            Label1.Visible = true;
            Label2.Visible = true;
            Label3.Visible = true;
            Label4.Visible = true;

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

        protected void Gridview1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow &&
   (e.Row.RowState == DataControlRowState.Normal ||
    e.Row.RowState == DataControlRowState.Alternate))
            {
                CheckBox CheckBox1 = (CheckBox)e.Row.Cells[1].FindControl("CheckBox1");
                CheckBox chkBxHeader = (CheckBox)this.Gridview1.HeaderRow.FindControl("chkBxHeader");
                CheckBox1.Attributes["onclick"] = string.Format
                                                       (
                                                          "javascript:ChildClick(this,'{0}');",
                                                          chkBxHeader.ClientID
                                                       );
            }
        }
    }

}
