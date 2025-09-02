<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Footer.ascx.cs" Inherits="CleverUI.Footer" %>

  <div class="footer">
        <asp:Image runat="server" ImageUrl="~/images/tclogic-logo.png" ImageAlign="AbsMiddle" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblActiveDatabase" runat="server" />
        Tel: 1-317-464-5152 | E-mail: <a href="mailto:support@tclogic.com">support@tclogic.com</a> &copy; 2009-<%= DateTime.Now.Year %> TCLogic, LLC
 </div>