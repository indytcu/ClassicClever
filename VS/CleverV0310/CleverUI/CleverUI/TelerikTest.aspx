<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TelerikTest.aspx.cs" Inherits="TelerikTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Clever - Process Running Info</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
		<telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
	
		<telerik:RadWindow ID="rwTest" runat="server" VisibleOnPageLoad="true" Width="600px" Height="600px">
			<ContentTemplate>
				<telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server">
                
					<telerik:RadSplitter ID="RadSplitter1" runat="server">
						<telerik:RadPane ID="RadPane1" runat="server"></telerik:RadPane>
						<telerik:RadSplitBar ID="RadSplitBar1" runat="server"></telerik:RadSplitBar>
						<telerik:RadPane ID="RadPane2" runat="server"></telerik:RadPane>
					</telerik:RadSplitter>
					<asp:DropDownList ID="DropdownList1" runat="server" AutoPostBack="true">
						<asp:ListItem Text="Item1" Value="1" />
						<asp:ListItem Text="Item2" Value="2" />
						<asp:ListItem Text="Item3" Value="3" />
						<asp:ListItem Text="Item4" Value="4" />
					</asp:DropDownList>
				</telerik:RadAjaxPanel>
                <asp:Button ID="Button1" Text="ClickMe" runat="server" OnClick="Button1_Click" />
			</ContentTemplate>
		</telerik:RadWindow>
			
	
    </div>
    </form>
</body>
</html>
