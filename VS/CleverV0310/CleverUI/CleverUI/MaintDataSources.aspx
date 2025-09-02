<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="MaintDataSources.aspx.cs" Inherits="CleverUI.MaintDataSources" MasterPageFile="~/GlobalAdmin.master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="cAdminPage" ContentPlaceHolderID="cphAdminPage" runat="server">
    <telerik:RadDockLayout ID="rdlDataSources" runat="server">
        <telerik:RadDockZone ID="rdzDataSources" runat="server" Orientation="Vertical" CssClass="zone"
            BorderStyle="None" FitDocks="true">
            <telerik:RadDock ID="rdDataSources" runat="server" Title="Manage Data Sources" DockMode="Docked"
                EnableDrag="false" DefaultCommands="None">
                <ContentTemplate>
                    <asp:Label ID="lblError" runat="server" Visible="false" CssClass="error" />
                    <telerik:RadGrid ID="rgDataSources" runat="server" Skin="Default" AllowPaging="false"
                        GridLines="None" AllowSorting="true" EnableHeaderContextMenu="true" OnNeedDataSource="rgDataSources_NeedDataSource"
                        OnItemCommand="rgDataSources_ItemCommand" OnItemDataBound="rgDataSources_ItemDataBound" OnDeleteCommand="rgDataSources_DeleteCommand">
                        <MasterTableView DataKeyNames="CleverDataSourcePK,Title" CommandItemDisplay="Top" AutoGenerateColumns="false">
                            <CommandItemSettings AddNewRecordText="Add new data source" />
                            <RowIndicatorColumn>
                                <HeaderStyle Width="20px" />
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn>
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                            <Columns>
                          
                                                       <telerik:GridButtonColumn Text="Users" CommandName="AssignUsers" ButtonType="PushButton"
                                    HeaderStyle-Width="60">
                                </telerik:GridButtonColumn>
                                <telerik:GridEditCommandColumn ButtonType="ImageButton" HeaderStyle-Width="30" />
                                <telerik:GridButtonColumn CommandName="Delete" ButtonType="ImageButton" ConfirmDialogType="RadWindow"
                                    ConfirmText="Are you sure you want to delete this data source?" HeaderStyle-Width="30" />
                            <telerik:GridBoundColumn DataField="Title" HeaderText="Title" HeaderStyle-Width="300px" />
                            <telerik:GridBoundColumn DataField="CleverDataSourceTypeDesc" HeaderText="Type" HeaderStyle-Width="150px" />
                             <telerik:GridCheckBoxColumn DataField="IsDefault" HeaderText="Is Default" HeaderStyle-Width="75px" />
                            <telerik:GridBoundColumn DataField="Description" HeaderText="Description" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>

                     <telerik:RadWindow ID="rwAddEditDataSource" runat="server" ShowContentDuringLoad="False"
                        Title="Manager Data Source" OnClientClose="ClickCancel" DestroyOnClose="false" Width="550px"
                        Height="400px" VisibleOnPageLoad="false" Behaviors="Close, Move, Resize">
                        <ContentTemplate>
                            <asp:HiddenField ID="hfIsInsert" runat="server" />
                            <asp:HiddenField ID="hfID" runat="server" />
                            <asp:Label ID="lblFormError" runat="server" ForeColor="Red" />
                            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
                                <table cellpadding="2" style="margin: 5px;">
                                                                    <tr>
                                        <td>
                                            Title <span class="validator">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTitle" runat="server" MaxLength="256" Width="200px" />
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtTitle"
                                                Display="Dynamic" ErrorMessage="Enter server name" ValidationGroup="AddEditDataSource" />
                                        </td>
                                    </tr>
                                                                                                     <tr>
                                        <td>
                                            Description 
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDescription" runat="server" MaxLength="1024" Width="200px" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                                                        <tr>
                                        <td>
                                            Type <span class="validator">*</span>
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="rcbDataSourceType" runat="server" Skin="Default" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Server Name <span class="validator">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtServerName" runat="server" MaxLength="50" Width="200px" />
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvServerName" runat="server" ControlToValidate="txtServerName"
                                                Display="Dynamic" ErrorMessage="Enter server name" ValidationGroup="AddEditDataSource" />
                                        </td>
                                    </tr>
                                                                        <tr>
                                        <td>
                                            Database Name <span class="validator">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDatabaseName" runat="server" MaxLength="128" Width="200px" />
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvDatabaseName" runat="server" ControlToValidate="txtDatabaseName"
                                                Display="Dynamic" ErrorMessage="Enter database name" ValidationGroup="AddEditDataSource" />
                                        </td>
                                    </tr>
                                                                        <tr>
                                        <td>
                                            User ID <span class="validator">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDatabaseLogin" runat="server" MaxLength="50" Width="200px" />
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvDatabaseLogin" runat="server" ControlToValidate="txtDatabaseLogin"
                                                Display="Dynamic" ErrorMessage="Enter user id" ValidationGroup="AddEditDataSource" />
                                        </td>
                                    </tr>
                                                                        <tr>
                                        <td>
                                            Password <span class="validator">*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDatabasePassword" runat="server" MaxLength="50" Width="200px" TextMode="Password" />
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvDatabasePassword" runat="server" ControlToValidate="txtDatabasePassword"
                                                Display="Dynamic" ErrorMessage="Enter password" ValidationGroup="AddEditDataSource" />
                                        </td>
                                    </tr>
                                                                      <tr>
                                        <td>
                                            Is Default <span class="validator">*</span>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsDefault" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </telerik:RadAjaxPanel>
                            <div>
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                    CausesValidation="False" />
                                &nbsp;
                                <asp:Button ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click" CausesValidation="true"
                                    ValidationGroup="AddEditDataSource" />
                                        &nbsp;
                                <asp:Button ID="btnTest" Text="Test" runat="server" OnClick="btnTest_Click" CausesValidation="true"
                                    ValidationGroup="AddEditDataSource" />
                            </div>
                        </ContentTemplate>
                    </telerik:RadWindow>

                    <telerik:RadWindow ID="rwAssignUsers" runat="server" ShowContentDuringLoad="False"
                        Title="Grant User Access" OnClientClose="ClickCancelAssignUsers" DestroyOnClose="false" Width="300px"
                        Height="600px" VisibleOnPageLoad="false" Behaviors="Close, Move, Resize">
                        <ContentTemplate>
                            <asp:HiddenField ID="hfDataSourceID" runat="server" />
                            <asp:Label ID="lblUsersFormError" runat="server" ForeColor="Red" />
                            <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server">
                                <telerik:RadTreeView ID="rtvAssignUsers" runat="server" DataTextField="code" DataValueField="id" CheckBoxes="true" CheckChildNodes="true" TriStateCheckBoxes="true"
        SingleExpandPath="false" MultipleSelect="true" style="margin: 5px;"></telerik:RadTreeView>

                            </telerik:RadAjaxPanel>
                            <div>
                                <asp:Button ID="btnCancelAssignUsers" runat="server" Text="Cancel" OnClick="btnCancelAssignUsers_Click"
                                    CausesValidation="False" />
                                &nbsp;
                                <asp:Button ID="btnSaveAssignUsers" Text="Save" runat="server" OnClick="btnSaveAssignUsers_Click" />
                            </div>
                        </ContentTemplate>
                    </telerik:RadWindow>

                    <script type="text/javascript" language="javascript">
                        function ClickCancel(oWnd, args) {
                            var btnCancel = document.getElementById("<%=btnCancel.ClientID%>");
                            btnCancel.click();

                            var rwAddEditUser = $find("<%=rwAddEditDataSource.ClientID%>");
                            rwAddEditUser.close();
                        }
                        function ClickCancelAssignUsers(oWnd, args) {
                            var btnCancel = document.getElementById("<%=btnCancelAssignUsers.ClientID%>");
                            btnCancel.click();

                            var rwAddEditUser = $find("<%=rwAssignUsers.ClientID%>");
                            rwAddEditUser.close();
                        }
                    </script>
           </ContentTemplate>
            </telerik:RadDock>
        </telerik:RadDockZone>
    </telerik:RadDockLayout>
</asp:Content>
