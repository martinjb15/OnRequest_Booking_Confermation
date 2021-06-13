<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="OnRequest_Booking_Confermation.WebForm2" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="css/bootstrap.css" rel="stylesheet" />
    <link href="css/app.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css?family=Lato" rel="stylesheet" />

    <style type="text/css">
        .auto-style1 {
            font-size: medium;
        }
        .auto-style2 {
            text-decoration: underline;
        }
        .auto-style3 {
            color: inherit;
            height: 376px;
            margin-bottom: 30px;
            padding-top: 24px;
            padding-bottom: 24px;
            background-color: #eee;
        }
    </style>

</head>
<body>
        <nav class="navbar navbar-inverse">
        <div class="container">
            <!-- Brand and toggle get grouped for better mobile display -->
            <div class="navbar-header">
                  <a class="navbar-brand" href="#"><img alt="Brand" src="pic/ %20logonav.jpg"></a>
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
        <div>
            <div style="text-align: center; vertical-align: middle; font-family: Verdana; color: Blue; position: absolute; top: 50%; left: 50%; margin-left: -88px; font-size: small;" id="dvProgress" runat="server">
                Please Wait ...<img src="pic/load.gif" style="vertical-align: middle" alt="Processing" />
            </div>
            <div class="container">
                <div id="content">
                    <div class="auto-style3" id="header_main" runat="server">
                        <h2 class="auto-style1"><strong>Welcome to the   Group online booking form</strong></h2>
                        <p class="auto-style1">
                           Please find attached our booking request(s) sent by  <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label> <br />
                        Please fill out the form below adding in additional comments for the booking or reason for not confirming your booking<br />
                        We look forward to your confirmation of these bookings at your earliest convenience within the next working week.<br />Kind regards,</p>
                        
                        <p><strong>  Group -   Ireland & UK</strong><br />Head Office: City Gate, 22 Bridge Street Lower<br />Dublin, D08, DW30, Ireland<br />Tel.: 0035316486100</p>
                        <p><span class="auto-style2">Please note:</span><br />The pax number is exclusive of drivers and guides. If a group travels with their own tour leader, (s)he will be counted separately (e.g. “20+1”).<br />If confirming for an alternative time, please ensure to select “suggest alternative” in the confirmation drop-down field.</p>
                    </div>
                    <div class="table-condensed" dir="ltr">
                        <asp:GridView ID="Gridview1" CssClass="table" Width="100%" AlternatingRowStyle-CssClass="word-break:break-all;" Font-Size="10px" AutoGenerateColumns="False" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal" OnRowDataBound="Gridview1_RowDataBound">
                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                            <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                            <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                            <SortedDescendingHeaderStyle BackColor="#242121" />
                            <AlternatingRowStyle CssClass="word-break:break-all;"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField HeaderText="ID" DataField="BSL_ID" />
                                <asp:BoundField HeaderText="Supplier" DataField="Supplier" />
                                <asp:BoundField HeaderText="Full Reference" DataField="Full_Reference" />
                                <asp:BoundField HeaderText="Agent" DataField="Agent" />
                                <asp:BoundField HeaderText="Name" DataField="Booking_Name" />
                                <asp:BoundField HeaderText="Description" DataField="Description" />
                                 <asp:BoundField HeaderText="Language" DataField="Language" />
                                <asp:BoundField HeaderText="Nationality" DataField="Nationality" />
                                <asp:BoundField HeaderText="BookingStatus" DataField="STATUS" />
                                <asp:BoundField HeaderText="Date/Time" DataField="Pickup_Date" />
                                <asp:BoundField HeaderText="End Date" DataField="DropOff_Date" />
                                <asp:BoundField HeaderText="Pax Excluding Guide" DataField="Pax" />
                                <asp:BoundField HeaderText="Contact Email" DataField="contact" />
                               
                                <asp:BoundField />
                               
                                <asp:TemplateField HeaderText="Confirmation">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="DropDownList1" runat="server">
                                            <asp:ListItem>Please Select</asp:ListItem>
                                            <asp:ListItem>Confirm</asp:ListItem>
                                            <asp:ListItem>Wait list</asp:ListItem>
                                            <asp:ListItem>Reject Request</asp:ListItem>
                                            <asp:ListItem>Propose Alternative</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required Field" ControlToValidate="DropDownList1" InitialValue="Please Select"></asp:RequiredFieldValidator>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Supplier Reference">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server" Text="" Height="30px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Comments/ Alternative Time">
                                    <ItemTemplate>
                                        <asp:TextBox TextMode="MultiLine" ID="TextBox1" runat="server" Text="" Height="30px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Supplier Message" DataField="Message_Text" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    <asp:Label ID="Label2" runat="server" Text="Error" Visible="False"></asp:Label>
                    <h4 id="header_form" runat="server">Please enter your email address: 
                           <asp:TextBox ID="textbox3" runat="server" CssClass="form-control" TextMode="Email" AutoCompleteType="Email"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="textbox3" ErrorMessage="Invalid Email Format"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required Field" ControlToValidate="textbox3"></asp:RequiredFieldValidator>
                    </h4>
               
                <asp:LinkButton ID="Button1" CssClass="btn btn-default btn-lg" runat="server" OnClick="Button1_Click">Submit</asp:LinkButton>
 </div>
            </div>
        </div>
    </form>
    <script
        src="https://code.jquery.com/jquery-3.2.1.js"
        integrity="sha256-DZAnKJ/6XZ9si04Hgrsxu/8s717jcIzLy3oi35EouyE="
        crossorigin="anonymous"></script>
    <script src="js/jquery.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>

</body>
</html>
