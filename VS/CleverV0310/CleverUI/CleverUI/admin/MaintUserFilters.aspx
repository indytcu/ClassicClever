<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="MaintUserFilters.aspx.cs" Inherits="CleverUI.Admin.MaintUserFilters" %>

<%@ Register TagPrefix="uc" TagName="FilterManagerTreeList" Src="~/User/FilterManagerTreeList.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdminPage" runat="server">
 <telerik:RadDockLayout ID="rdlUsers" runat="server">
        <telerik:RadDockZone ID="rdzUsers" runat="server" Orientation="Vertical" CssClass="zone"
            BorderStyle="None" FitDocks="true">
            <telerik:RadDock ID="rdUsers" runat="server" Title=" - Manage User Filters" DockMode="Docked"
                EnableDrag="false" DefaultCommands="None">
                <ContentTemplate>
                    <asp:Label ID="lblError" runat="server" CssClass="error" />
                    <telerik:RadGrid ID="rgUsers" runat="server" AllowPaging="True" PageSize="20" EnableHeaderContextMenu="true"
                        GridLines="None" AllowSorting="True" OnNeedDataSource="rgUsers_NeedDataSource"
                        OnItemCommand="rgUsers_ItemCommand" Skin="Default"
                        OnItemDataBound="rgUsers_ItemDataBound" CellSpacing="0">
                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="UserName,FilterPK"
                            CommandItemDisplay="Top">
                            <RowIndicatorColumn>
                                <HeaderStyle Width="20px"></HeaderStyle>
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn>
                                <HeaderStyle Width="20px"></HeaderStyle>
                            </ExpandCollapseColumn>
                            <Columns>
                               
                                <telerik:GridEditCommandColumn ButtonType="ImageButton" HeaderStyle-Width="30" />                        
                                
                                <telerik:GridBoundColumn DataField="UserName" HeaderText="User name" SortExpression="UserName"
                                    UniqueName="UserName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Email" HeaderText="E-mail" SortExpression="Email"
                                    UniqueName="Email">
                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="HasFilter" HeaderText="Has Filter" SortExpression="HasFilter"
                                    UniqueName="HasFilter">
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
                             <CommandItemSettings ShowAddNewRecordButton="false" />
                           </MasterTableView>
                    </telerik:RadGrid>
                     <telerik:RadWindow ID="rwAddEditUser" runat="server" ShowContentDuringLoad="False"
                        Title=" - Add User Filter" OnClientClose="ClickCancel" DestroyOnClose="false" Width="550px"
                        Height="400px" VisibleOnPageLoad="false" Behaviors="Close, Move, Resize">
                        <ContentTemplate>
                                              <asp:Label ID="lblFormError" runat="server" ForeColor="Red" />
                            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
                                <table cellpadding="2" style="margin: 5px;">
                                    <tr>
                                        <td>
                                            UserName 
                                        </td>
                                        <td colspan="2">
                                             <asp:TextBox ID="txtUserName" runat="server" MaxLength="256" Width="200px" Enabled="false" />
                                        </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Create system user filter <span class="validator">*</span>
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="rcbTableName" Width="180" runat="server" DataTextField="TableName"
                                                    DataValueField="TablePK" EmptyMessage="Filter Table" ShowMoreResultsBox="true"
                                                    EnableItemCaching="True" EnableLoadOnDemand="True" EnableVirtualScrolling="true"
                                                    Filter="StartsWith" MarkFirstMatch="True" Skin="Web20" />
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvTableName" runat="server" ControlToValidate="rcbTableName"
                                                    Display="Dynamic" ErrorMessage="Select table for the filter." ValidationGroup="AddEditUser" />
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
                    </script>
                </ContentTemplate>
            </telerik:RadDock>
        </telerik:RadDockZone>
    </telerik:RadDockLayout>
    <uc:FilterManagerTreeList ID="ucUserFilterManagerTreeList" runat="server"  Visible="false" />
</asp:Content>
