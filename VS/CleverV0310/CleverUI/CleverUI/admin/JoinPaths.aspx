<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true"
    CodeBehind="JoinPaths.aspx.cs" Inherits="CleverUI.Admin.JoinPaths" %>

<asp:Content runat="server" ContentPlaceHolderID="cphAdminPage">
    <telerik:RadDockLayout ID="RadDockLayout1" runat="server">
        <telerik:RadDockZone ID="rdzJoinPaths" runat="server" Orientation="Vertical" BorderStyle="None">
            <telerik:RadDock ID="rdJoinPath" runat="server" Title="Join Paths" DefaultCommands="ExpandCollapse"
                Skin="Web20" Width="100%">
                <ContentTemplate>
                    <div style="padding-bottom: 4px;">
                        <telerik:RadComboBox ID="rcbFilterFromTable" Width="180" runat="server" EmptyMessage="Select Start Table"
                            EnableLoadOnDemand="True" DataTextField="TableName" DataValueField="TablePK"
                            ItemRequestTimeout="500" ItemsPerRequest="10" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                            OnItemsRequested="rcbFilterFromTable_ItemsRequested" EnableItemCaching="true"
                            AutoPostBack="true" OnSelectedIndexChanged="rcbFilterFromTable_SelectedIndexChanged" />
                        &nbsp;<span class="required">*</span> &nbsp;
                          <telerik:RadComboBox ID="rcbFilterNodeTable" Width="180" runat="server" EmptyMessage="Select First Node Table"
                           DataTextField="TableName" DataValueField="TablePK"/>
                        &nbsp;   
                        <telerik:RadComboBox ID="rcbFilterToTable" Width="180" runat="server" EmptyMessage="Select End Table"
                            EnableLoadOnDemand="True" DataTextField="TableName" DataValueField="TablePK"
                            ItemRequestTimeout="500" ItemsPerRequest="10" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                            OnItemsRequested="rcbFilterToTable_ItemsRequested" EnableItemCaching="true" />
                        &nbsp;
                       
                        <asp:Button ID="btnSearch" runat="server" Text="Search" Style="vertical-align: middle;"
                            OnClick="btnSearch_Click" CausesValidation="true" ValidationGroup="SearchData" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="rcbFilterFromTable"
                            ErrorMessage="Select start table." Display="Dynamic" ValidationGroup="SearchData" />
                    </div>
                    <asp:Label ID="lblError" runat="server" Visible="false" CssClass="error" />
                    <telerik:RadGrid ID="rgJoinPaths" runat="server" AutoGenerateColumns="False" GridLines="None"
                        OnNeedDataSource="rgJoinPaths_NeedDataSource" OnDetailTableDataBind="rgJoinPaths_DetailTableDataBind"
                        OnItemCommand="rgJoinPaths_ItemCommand" OnItemDataBound="rgJoinPaths_ItemDataBound"
                        OnDeleteCommand="rgJoinPaths_DeleteCommand"
                        Skin="Default" AllowPaging="true" PageSize="20" AllowSorting="true" EnableHeaderContextMenu="true">
                        <MasterTableView Name="JoinPaths" DataKeyNames="JoinPathPK,StartTableName,EndTableName"
                            CommandItemDisplay="Top">
                            <CommandItemSettings AddNewRecordText="Add new join path" />
                            <RowIndicatorColumn>
                                <HeaderStyle Width="20px" />
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn>
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridEditCommandColumn ButtonType="ImageButton" HeaderStyle-Width="15" />
                                <telerik:GridButtonColumn CommandName="Delete" ButtonType="ImageButton" ConfirmDialogType="RadWindow"
                                    ConfirmText="Are you sure you want to delete this join path?" HeaderStyle-Width="15" />
                                <telerik:GridBoundColumn DataField="JoinPathPK" HeaderText="Key" SortExpression="JoinPathPK"
                                    UniqueName="JoinPathPK" HeaderStyle-Width="75">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="StartTableName" HeaderText="Start Table" SortExpression="StartTableName"
                                    UniqueName="StartTableName" HeaderStyle-Width="150">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="EndTableName" HeaderText="End Table" SortExpression="EndTableName"
                                    UniqueName="EndTableName" HeaderStyle-Width="150">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="JoinPathCode" HeaderText="JoinPathCode" SortExpression="JoinPathCode"
                                    UniqueName="JoinPathCode" >
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="NbrNodes" HeaderText="Nbr Nodes" SortExpression="NbrNodes"
                                    UniqueName="NbrNodes" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="70">
                                </telerik:GridBoundColumn >
                                <telerik:GridCheckBoxColumn DataField="IsDefault" HeaderText="Is Default" SortExpression="IsDefault"
                                    UniqueName="IsDefault" HeaderStyle-Width="70">
                                </telerik:GridCheckBoxColumn>
                            </Columns>
                            <DetailTables>
                                <telerik:GridTableView Name="JoinPathJoins" AutoGenerateColumns="false" DataKeyNames="JoinPathJoinPK"
                                    Width="100%">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="JoinPathJoinPK" HeaderText="Key" SortExpression="JoinPathJoinPK"
                                            UniqueName="JoinPathJoinPK">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="NodeSequence" HeaderText="Node Sequence" SortExpression="NodeSequence"
                                            UniqueName="NodeSequence" HeaderStyle-Width="110">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="FromTableName" HeaderText="From Table" SortExpression="FromTableName"
                                            UniqueName="FromTableName" HeaderStyle-Width="150">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="ToTableName" HeaderText="To Table" SortExpression="ToTableName"
                                            UniqueName="ToTableName" HeaderStyle-Width="150">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="JoinAlias" HeaderText="Join Alias" SortExpression="JoinAlias"
                                            UniqueName="JoinAlias" HeaderStyle-Width="80">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="JoinStatement" HeaderText="Join Statement" SortExpression="JoinStatement"
                                            UniqueName="JoinStatement">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </telerik:GridTableView>
                            </DetailTables>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <!--Add/Edit Form Join Path >> -->
                    <telerik:RadWindow ID="rwFormJoinPath" runat="server" ShowContentDuringLoad="False"
                        Title="Join Path" OnClientClose="ClickCancel" DestroyOnClose="false" Width="980px"
                        Height="420px" VisibleOnPageLoad="false" Behaviors="Close, Move, Resize">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        Start Table:
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;<asp:Label ID="lblStartTable" runat="server" />
                                    </td>
                                    <td style="padding-left:30px;">  <asp:CheckBox ID="chkIsDefault" runat="server" />Is Default</td>
                                   
                                </tr>
                                <tr>
                                    <td>
                                        End Table:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblEndTable" runat="server" /> <span class="required" runat="server" id="spanEndTableValidator">*</span>
                                        <telerik:RadComboBox ID="rcbEndTable" Width="180" runat="server" EmptyMessage="Select End Table"
                                            EnableLoadOnDemand="True" DataTextField="TableName" DataValueField="TablePK"
                                            ItemRequestTimeout="500" ItemsPerRequest="10" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                                            OnItemsRequested="rcbEndTable_ItemsRequested" EnableItemCaching="true" AutoPostBack="true" 
                                            OnSelectedIndexChanged="rcbEndTable_SelectedIndexChanged" />
                                        <asp:RequiredFieldValidator ID="rfvEndTable" runat="server" ControlToValidate="rcbEndTable" ValidationGroup="JoinPathData"
                                        ErrorMessage="Select end table" Display="Dynamic" />    
                                    </td>
                                    <td></td>
                                </tr>
                            </table>
                            <asp:Label ID="lblFormError" runat="server" CssClass="error" />
                            <table width="100%">
                                <tr>
                                    <td colspan="2">
                                        <telerik:RadAjaxPanel runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Width="100%">
                                            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" IsSticky="true" runat="server"
                                                Transparency="20">
                                            </telerik:RadAjaxLoadingPanel>
                                            <telerik:RadGrid ID="rgJoinPathJoins" runat="server" AutoGenerateColumns="False"
                                                AllowMultiRowEdit="True" OnItemDataBound="rgJoinPathJoins_ItemDataBound"
                                               Width="100%"  OnItemCreated="rgJoinPathJoins_ItemCreated" >
                                                <MasterTableView EditMode="InPlace" Width="100%">
                                                    <Columns>
                                                          <telerik:GridTemplateColumn UniqueName="FromTableName" HeaderText="From Table" HeaderStyle-Width="150">
                                                            <ItemTemplate>
                                                               <telerik:RadComboBox runat="server" ID="rcbFromTableName" Skin="Default"  />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                           <telerik:GridTemplateColumn UniqueName="FromTableKey" HeaderText="From Table Key" HeaderStyle-Width="150" >
                                                            <ItemTemplate>
                                                               <telerik:RadComboBox runat="server" ID="rcbFromTableKey" Skin="Default"  />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                             <telerik:GridTemplateColumn UniqueName="ToTableName" HeaderText="To Table" HeaderStyle-Width="150" >
                                                            <ItemTemplate>
                                                               <telerik:RadComboBox runat="server" ID="rcbToTableName" Skin="Default" />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                     <%--   <telerik:GridDropDownColumn UniqueName="FromTableName" DataField="FromTableName"
                                                            HeaderText="From Table" HeaderStyle-Width="150" />
                                                        <telerik:GridDropDownColumn UniqueName="FromTableKey" DataField="FromTableKey" HeaderText="From Table Key" HeaderStyle-Width="150" />
                                                        <telerik:GridDropDownColumn UniqueName="ToTableName" DataField="ToTableName" HeaderText="To Table" HeaderStyle-Width="150" />--%>
                                                        <telerik:GridTemplateColumn UniqueName="TableJoinStatement" HeaderText="Table Join Statement">
                                                            <ItemTemplate>
                                                                &nbsp;<asp:Label ID="lblTableJoinStatement" runat="server" />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn Display="false" UniqueName="TableJoinAlias" HeaderText="">
                                                            <ItemTemplate>
                                                                &nbsp;<asp:Label ID="lblTableJoinAlias" runat="server" />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="JoinPathStatement" HeaderText="Table Join Statement">
                                                            <ItemTemplate>
                                                               &nbsp; <asp:Label ID="lblJoinPathStatement" runat="server" />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </telerik:RadAjaxPanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                            CausesValidation="False" Width="60"  />&nbsp;
                                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="60" OnClick="btnSave_Click" CausesValidation="true"
                                        ValidationGroup="JoinPathData" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </telerik:RadWindow>
                    <!-- << Add/Edit Form Join Path -->

                    <script type="text/javascript" language="javascript">
                        function ClickCancel(oWnd, args) {
                            var btnCancel = document.getElementById("<%=btnCancel.ClientID%>");
                            btnCancel.click();

                            var rwFormJoinPath = $find("<%=rwFormJoinPath.ClientID%>");
                            rwFormJoinPath.close();
                        }
                    </script>

                </ContentTemplate>
            </telerik:RadDock>
        </telerik:RadDockZone>
    </telerik:RadDockLayout>
</asp:Content>
