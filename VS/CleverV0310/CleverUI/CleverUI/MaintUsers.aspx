<%@ Page Title="" Language="C#" MasterPageFile="~/GlobalAdmin.master" AutoEventWireup="true"
    CodeBehind="MaintUsers.aspx.cs" Inherits="CleverUI.MaintUsers" EnableEventValidation="false" %>

<asp:Content ID="cAdminPage" ContentPlaceHolderID="cphAdminPage" runat="server">
    <telerik:RadDockLayout ID="rdlUsers" runat="server">
        <telerik:RadDockZone ID="rdzUsers" runat="server" Orientation="Vertical" CssClass="zone"
            BorderStyle="None" FitDocks="true">
            <telerik:RadDock ID="rdUsers" runat="server" Title="Manage Users" DockMode="Docked"
                EnableDrag="false" DefaultCommands="None">
                <ContentTemplate>
                    <asp:Label ID="lblError" runat="server" CssClass="error" />
                    <telerik:RadGrid ID="rgUsers" runat="server" AllowPaging="True" PageSize="20" EnableHeaderContextMenu="true"
                        GridLines="None" AllowSorting="True" OnNeedDataSource="rgUsers_NeedDataSource"
                        OnItemCommand="rgUsers_ItemCommand" 
                        OnItemDataBound="rgUsers_ItemDataBound" OnDeleteCommand="rgUsers_DeleteCommand" 
                        CellSpacing="0"  Skin="Default" >
                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="IsLockedOut,ProviderUserKey"
                            CommandItemDisplay="Top">
                            <RowIndicatorColumn>
                                <HeaderStyle Width="20px"></HeaderStyle>
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn>
                                <HeaderStyle Width="20px"></HeaderStyle>
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridEditCommandColumn ButtonType="ImageButton" HeaderStyle-Width="30" />
                                <telerik:GridButtonColumn CommandName="Delete" ButtonType="ImageButton" ConfirmDialogType="RadWindow"
                                    ConfirmText="Are you sure you want to delete this user?" HeaderStyle-Width="30" />
                                <telerik:GridTemplateColumn>
                                    <ItemTemplate>
                                        <asp:Button ID="btnUnlock" runat="server" Text="Unlock" CommandName="Unlock" CommandArgument='<%# Eval("UserName") %>'/>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                           
                                
                                <telerik:GridBoundColumn DataField="UserName" HeaderText="User name" SortExpression="UserName"
                                    UniqueName="UserName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Email" HeaderText="E-mail" SortExpression="Email"
                                    UniqueName="Email">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="IsApproved" HeaderText="Is Active" SortExpression="IsApproved"
                                    UniqueName="IsApproved">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="IsLockedOut" HeaderText="Is Locked" SortExpression="IsLockedOut"
                                    UniqueName="IsLockedOut">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CreationDate" HeaderText="Date created" SortExpression="CreationDate"
                                    UniqueName="CreationDate">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="LastLoginDate" HeaderText="Last login" SortExpression="LastLoginDate"
                                    UniqueName="LastLoginDate">
                                </telerik:GridBoundColumn>
                            </Columns>
                            <CommandItemSettings AddNewRecordText="Add new user" />
                        </MasterTableView>
                    </telerik:RadGrid>
                    <telerik:RadWindow ID="rwAddEditUser" runat="server" ShowContentDuringLoad="False"
                        Title="Manager User" OnClientClose="ClickCancel" DestroyOnClose="false" Width="550px"
                        Height="500px" VisibleOnPageLoad="false" Behaviors="Close, Move, Resize">
                        <ContentTemplate>
                            <asp:HiddenField ID="hfIsInsert" runat="server" />
                            <asp:HiddenField ID="hfGuid" runat="server" />
                            <asp:Label ID="lblFormError" runat="server" ForeColor="Red" />
                            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
                                <table cellpadding="2" style="margin: 5px;">
                                    <tr>
                                        <td>
                                            UserName <span class="validator">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUserName" runat="server" MaxLength="256" Width="200px" />
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="txtUserName"
                                                Display="Dynamic" ErrorMessage="Enter username" ValidationGroup="AddEditUser" />
                                        </td>
                                    </tr>
                                    <tr id="trPassword" runat="server">
                                        <td>
                                            Password <span class="validator">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="200px" />
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                                                Display="Dynamic" ErrorMessage="Enter password" ValidationGroup="AddEditUser" />
                                        </td>
                                    </tr>
                                    <tr id="trRetypePassword" runat="server">
                                        <td>
                                            Retype password <span class="validator">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRetypePassword" runat="server" TextMode="Password" Width="200px" />
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvRetypePassword" runat="server" ControlToValidate="txtRetypePassword"
                                                Display="Dynamic" ErrorMessage="Retype password" ValidationGroup="AddEditUser" />
                                            <asp:CompareValidator ID="cvPassword" runat="server" Display="Dynamic" ControlToValidate="txtRetypePassword"
                                                ControlToCompare="txtPassword" ErrorMessage="Passwords do not match" Operator="Equal" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            E-mail <span class="validator">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEmail" runat="server" MaxLength="256" Width="200px" />
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                                Display="Dynamic" ErrorMessage="Enter e-mail" ValidationGroup="AddEditUser" />
                                            <asp:RegularExpressionValidator ID="revEmail" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                ControlToValidate="txtEmail" ErrorMessage="Invalid e-mail format" runat="server"
                                                ValidationGroup="AddEditUser" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            User Role <span class="validator">*</span>
                                        </td>
                                        <td colspan="2">
                                            <asp:CheckBoxList ID="cxblUserRoles" runat="server" RepeatDirection="Horizontal"
                                                Style="margin-left: -3px ! important;" />
                                        </td>
                                        </tr><tr>
                                        <td></td>
                                        <td colspan="2">
                                            <asp:CustomValidator ID="cvUserRoles" runat="server" ClientValidationFunction="IsCheckBoxChecked"
                                                ErrorMessage="Select user role" Display="Dynamic" ValidationGroup="AddEditUser" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Is Active
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsActive" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                    <td valign="top">Data Sources</td>
                                    <td>
                                                        <telerik:RadGrid ID="rgDataSources" runat="server" Skin="Default" 
                        GridLines="None" AllowPaging="false" AllowSorting="false" OnNeedDataSource="rgDataSources_NeedDataSource" OnItemDataBound="rgDataSources_ItemDataBound">
                                                               <ClientSettings>
                                                               <Scrolling AllowScroll="true" ScrollHeight="200" />
                                                               </ClientSettings>
                                                <MasterTableView DataKeyNames="CleverDataSourcePK" ShowHeader="false" CommandItemDisplay="None" AutoGenerateColumns="false">
                            <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                            <RowIndicatorColumn>
                                <HeaderStyle Width="20px" />
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn>
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                            <Columns>
                                                            <telerik:GridTemplateColumn>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDataSource" runat="server"/>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="Title" HeaderText="Title" HeaderStyle-Width="300px" />
                                                        </Columns>
                        </MasterTableView>
                        </telerik:RadGrid>
                                    </td>
                                    </tr>
                                </table>
                            </telerik:RadAjaxPanel>
                            <div>
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                    CausesValidation="False" />
                                &nbsp;
                                <asp:Button ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click" CausesValidation="true"
                                    ValidationGroup="AddEditUser" />
                            </div>
                        </ContentTemplate>
                    </telerik:RadWindow>

                    <script type="text/javascript" language="javascript">
                        function ClickCancel(oWnd, args) {
                            var btnCancel = document.getElementById("<%=btnCancel.ClientID%>");
                            btnCancel.click();

                            var rwAddEditUser = $find("<%=rwAddEditUser.ClientID%>");
                            rwAddEditUser.close();
                        }

                        function IsCheckBoxChecked(source, arguments) {
                            arguments.IsValid = false;
                            var list = document.getElementById('<%= cxblUserRoles.ClientID %>');

                            if (list != null) {
                                for (var i = 0; i < list.rows.length; i++) {
                                    for (var j = 0; j < list.rows[i].cells.length; j++) {
                                        var listControl = list.rows[i].cells[j].childNodes[0];
                                        if (listControl.checked) {
                                            arguments.IsValid = true;
                                        }
                                    }
                                }
                            }
                        }    
                    </script>

                </ContentTemplate>
            </telerik:RadDock>
        </telerik:RadDockZone>
    </telerik:RadDockLayout>
</asp:Content>
