<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="ConnectionLost.aspx.cs" Inherits="CleverUI.ConnStringExpired" %>
<%@ Register Src="~/Footer.ascx" TagPrefix="uc" TagName="Footer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" defaultfocus="">
    
       
     <div class="session_exp_text"> 
     <img src="images/clever-logo.gif" class="session_exp_logo" alt="tclogic-logo-small" /> </br>
        <asp:Label ID="lblExpired" runat="server" Text="No connection to the database" />
    </div>
    <uc:footer runat="server" />
    </form>
</body>
</html>

