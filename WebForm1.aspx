<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="OnRequest_Booking_Confermation.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/bootstrap.css" rel="stylesheet" />
    <link href="css/app.css" rel="stylesheet" />

    <link href="https://fonts.googleapis.com/css?family=Lato" rel="stylesheet" />
    <script type="text/javascript">
var TotalChkBx;
var Counter;

window.onload = function()
{
   //Get total no. of CheckBoxes in side the GridView.
   TotalChkBx = parseInt('<%= this.Gridview1.Rows.Count %>');

   //Get total no. of checked CheckBoxes in side the GridView.
   Counter = 0;
}

function HeaderClick(CheckBox)
{
   //Get target base & child control.
   var TargetBaseControl = 
       document.getElementById('<%= this.Gridview1.ClientID %>');
   var TargetChildControl = "CheckBox1";

   //Get all the control of the type INPUT in the base control.
   var Inputs = TargetBaseControl.getElementsByTagName("input");

   //Checked/Unchecked all the checkBoxes in side the GridView.
   for(var n = 0; n < Inputs.length; ++n)
      if(Inputs[n].type == 'checkbox' && 
                Inputs[n].id.indexOf(TargetChildControl,0) >= 0)
         Inputs[n].checked = CheckBox.checked;

   //Reset Counter
   Counter = CheckBox.checked ? TotalChkBx : 0;
}

function ChildClick(CheckBox, HCheckBox)
{
   //get target control.
   var HeaderCheckBox = document.getElementById(HCheckBox);

   //Modifiy Counter; 
   if(CheckBox.checked && Counter < TotalChkBx)
      Counter++;
   else if(Counter > 0) 
      Counter--;

   //Change state of the header CheckBox.
   if(Counter < TotalChkBx)
      HeaderCheckBox.checked = false;
   else if(Counter == TotalChkBx)
      HeaderCheckBox.checked = true;
}
</script>
  
</head>
<body>
    <nav class="navbar navbar-inverse">
        <div class="container">
            <!-- Brand and toggle get grouped for better mobile display -->
            <div class="navbar-header">
                <a class="navbar-brand" href="#">
                    <img alt="Brand" src="pic/ %20logonav.jpg"/></a>
                <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1" aria-expanded="false">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>

            </div>

            <!-- Collect the nav links, forms, and other content for toggling -->
            <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
            </div>
            <!-- /.navbar-collapse -->
        </div>
        <!-- /.container-fluid -->
    </nav>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                <div class="row">
                    <div class="col-lg-12">
                        <div id="content">
                            <h1>On Request booking</h1>
                            <h3>Please Choose your database</h3>
                            <div class="radio-inline">
                                <asp:RadioButtonList ID="RadioButtonList1" runat="server">
                                    <asp:ListItem Text="UK" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Ireland" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Moloney and Kelly" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <h3>Please enter your supplier code below</h3>
                            <div class="form-group">
                                <asp:TextBox ID="textbox1" runat="server" style="text-align: center" CssClass="form-control"></asp:TextBox>
                            </div>
                                   <h3>Please enter your Agent code below</h3>
                            <div class="form-group">
                                <asp:TextBox ID="textbox3" runat="server" style="text-align: center" CssClass="form-control"></asp:TextBox>
                            </div>
                            <h3>Please enter your   email address</h3>
                            <div class="form-group">
                                <asp:TextBox ID="textbox2" runat="server" style="text-align: center" CssClass="form-control" TextMode="Email"></asp:TextBox>

                            </div>
                            <asp:LinkButton ID="Button1" CssClass="btn btn-default btn-lg" runat="server" OnClick="Button1_Click1">Populate</asp:LinkButton>
                            <asp:LinkButton ID="LinkButton1" CssClass="btn btn-default btn-lg" runat="server" OnClick="LinkButton1_Click">Clear</asp:LinkButton>
                            <br />
                            <br />
                            <h4 id="text" runat="server" visible="false">Please tick the check box, to select the lines you wish to send to the supplier, then click generate</h4>
                            <div>
                                 <div>
      &nbsp;</div>
                                <div>
                                    <asp:GridView ID="Gridview1" CssClass="table" OnRowCreated="Gridview1_RowCreated" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal">
                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                        <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                                        <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                        <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                        <SortedDescendingHeaderStyle BackColor="#242121" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CheckBox1" runat="server" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkBxHeader"
                                                        onclick="javascript:HeaderClick(this);" runat="server" />
                                                </HeaderTemplate>
                                            </asp:TemplateField>
                                            
                                        </Columns>
                                    </asp:GridView>
                                </div>

                                <div>
                                    <asp:LinkButton ID="LinkButton2" CssClass="btn btn-default btn-lg" runat="server" OnClick="LinkButton2_Click" Visible="False">Generate Link</asp:LinkButton>
                                </div>
                                <br />
                                <div class="form-group">
                                    <label id="Label2" class="col-sm-2" runat="server" visible="false">Full Link</label>
                                    <asp:Label ID="Label1" runat="server" Text="Label" Visible="false"></asp:Label>
                                </div>
                                <div class="form-group">
                                    <label id="Label3" class="col-lg-2" runat="server" visible="false">HyperLink</label>
                                    <asp:Label ID="Label4" runat="server" Text="Label" Visible="false"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <br />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
