<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.master"
    CodeBehind="OnlineUsers.aspx.cs" Inherits="CleverUI.Admin.OnlineUsers" %>

<asp:Content ID="cAdminPage" ContentPlaceHolderID="cphAdminPage" runat="server">
    <telerik:RadDockLayout ID="rdlUsers" runat="server">
        <telerik:RadDockZone ID="rdzUsers" runat="server" Orientation="Vertical" CssClass="zone"
            BorderStyle="None" FitDocks="true">
            <telerik:RadDock ID="rdUsers" runat="server" Title=" - Online Users" DockMode="Docked"
                EnableDrag="false" DefaultCommands="None">
                <ContentTemplate>
                    <asp:Label ID="lblError" runat="server" CssClass="error" />
                    <telerik:RadGrid ID="rgUsers" runat="server" Skin="Default" 
                        AllowPaging="true" PageSize="20" EnableHeaderContextMenu="true"
                        GridLines="None" AllowSorting="true" OnNeedDataSource="rgUsers_NeedDataSource">
                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="ProviderUserKey" CommandItemDisplay="Top">
                            <RowIndicatorColumn>
                                <HeaderStyle Width="20px"></HeaderStyle>
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn>
                                <HeaderStyle Width="20px"></HeaderStyle>
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridBoundColumn DataField="UserName" HeaderText="User name" SortExpression="UserName"
                                    UniqueName="UserName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Email" HeaderText="E-mail" SortExpression="Email"
                                    UniqueName="Email">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="LastLoginDate" HeaderText="Last Login" SortExpression="LastLoginDate"
                                    UniqueName="LastLoginDate">
                                </telerik:GridBoundColumn>
                               
                            </Columns>
                            <CommandItemSettings ShowAddNewRecordButton="false" />
                        </MasterTableView>
                       
                    </telerik:RadGrid>
                </ContentTemplate>
            </telerik:RadDock>
        </telerik:RadDockZone>
    </telerik:RadDockLayout>
</asp:Content>
