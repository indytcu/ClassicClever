<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProcessRunningInfo.aspx.cs"
    Inherits="CleverUI.ProcessRunning" %>
<%@ Register Src="~/Footer.ascx" TagPrefix="uc" TagName="Footer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Clever - Process Running Info</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="body">
        <div class="header">
            <div class="left">
                <asp:Hyperlink  runat="server" NavigateUrl="~/User/Default.aspx" ImageUrl="~/images/clever-logo.gif" alt="tclogic-logo-small" />
            </div>
            <div class="clear">
            </div>
             <div style="padding:20px 0;" align="center">
                    <h1 class="title">Welcome to Clever</h1>
                  <span style="color:Red;font-size:16px;font-weight:bold;">Process is running. Please wait until it finish.</span>
                 <a href="User/Default.aspx">Retry</a>
                 
            </div>
           
           <uc:Footer runat="server" />
        </div>
    </div>
    </form>
</body>
</html>
