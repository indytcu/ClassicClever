<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CleverUI.Login" %>
<%@ Register Src="~/Footer.ascx" TagPrefix="uc" TagName="Footer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Clever Login</title>
</head>
<body>
    <form id="form1" runat="server" defaultfocus="Login1_UserName">
    <div class="login_box">
        <asp:Login ID="cntrlLogin" runat="server" DisplayRememberMe="false" CssClass="login_cntrl"
            TitleText="Login to Clever" OnLoginError="cntrlLogin_LoginError" OnLoggingIn="cntrlLogin_LoggingIn"
            OnLoggedIn="cntrlLogin_LoggedIn" DestinationPageUrl="~/SelectDataSource.aspx">
            <LayoutTemplate>
                <img src="images/clever-logo.gif" class="login_logo" alt="tclogic-logo-small" />
                <table border="0" cellpadding="0" class="login_form">
                    <%-- <tr>
                        <th colspan="2">
                            Login to Clever
                        </th>
                    </tr>--%>
                    <tr>
                        <td>
                            User Name <span class="required">*</span>
                        </td>
                        <td>
                            <asp:TextBox ID="UserName" runat="server" Width="160" />
                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                Display="None" ToolTip="User Name is required." ValidationGroup="Login1" ErrorMessage="User Name is required." />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Password <span class="required">*</span>
                        </td>
                        <td>
                            <asp:TextBox ID="Password" runat="server" TextMode="Password" Width="160" />
                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                Display="None" ErrorMessage="Password is required." ToolTip="Password is required."
                                ValidationGroup="Login1" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2">
                            <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Login" ValidationGroup="Login1"
                                Skin="Web20" Width="60" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="use_prohibit">
                            Unauthorized Use Is Prohibited
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary runat="server" ValidationGroup="Login1" />
                <asp:Label ID="FailureText" runat="server" EnableViewState="False" CssClass="error"
                    Width="300" />
            </LayoutTemplate>
        </asp:Login>
    </div>
    <%--  <div style="width:100%;height:40px;background-image:url(/images/short_top2.gif);">
        <asp:Image runat="server" ImageUrl="~/images/clever-logo.gif" alt="tclogic-logo-small" style="padding-top:8px;padding-left:10px;" />
    </div>
    <div style="padding-top: 20px;" align="center">
        <table style="text-align: center;">
            <tr>
                <td>
                    <div style="margin-bottom: 15px;">
                        <h1>
                            Welcome to Clever</h1>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <div style="border: solid 1px gray;">
                     
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <h2 style="color: Red; margin-top: 20px; border: solid 1px gray; padding: 5px 5px;">
                        Unauthorized Use Is Prohibited</h2>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="padding-top: 15px;">
                       
                    </div>
                </td>
            </tr>
        </table>
    </div>--%>
    <uc:Footer runat="server" />
    </form>
</body>
</html>
