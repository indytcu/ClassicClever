<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="etlTransformationSpec.aspx.cs"
    MasterPageFile="~/Admin/Admin.master" Inherits="CleverUI.Admin.etlTransformationSpec" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphAdminPage">
    <telerik:RadDockLayout ID="RadDockLayout1" runat="server">
        <telerik:RadDockZone ID="rdzTables" runat="server" Orientation="Vertical" BorderStyle="None">
            <telerik:RadDock ID="rdTables" runat="server" Title="ERP Transformation Details"
                DefaultCommands="ExpandCollapse" Skin="Web20" Width="100%">
                <ContentTemplate>
                      <telerik:RadComboBox ID="rcbBusUnit" Width="180" runat="server" EmptyMessage="Select BusUnit"
                            DataTextField="BusUnitCode" DataValueField="BusUnitCode" />
                    <div style="padding-bottom: 4px;">
                        <telerik:RadComboBox ID="rcbFilterCleverTable" Width="180" runat="server" EmptyMessage="Select Clever Table"
                            EnableLoadOnDemand="True" DataTextField="TableName" DataValueField="TablePK"
                            ItemRequestTimeout="500" ItemsPerRequest="10" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                            OnItemsRequested="rcbFilterCleverTable_ItemsRequested" EnableItemCaching="true" />
                        &nbsp;
                        <telerik:RadComboBox ID="rcbFilterCustTable" Width="180" runat="server" EmptyMessage="Select Cust Table"
                            EnableLoadOnDemand="True" DataTextField="TableName" DataValueField="TablePK"
                            ItemRequestTimeout="500" ItemsPerRequest="10" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                            OnItemsRequested="rcbFilterCustTable_ItemsRequested" EnableItemCaching="true" />
                        &nbsp;
                        <asp:Button ID="btnSearch" runat="server" Text="Search" Style="vertical-align: middle;"
                            OnClick="btnSearch_Click" CausesValidation="true" />
                    </div>
                    <asp:Label ID="lblError" runat="server" Visible="false" CssClass="error" />
                    <telerik:RadGrid ID="rgTransformations" runat="server" AutoGenerateColumns="False"
                        GridLines="None" AllowPaging="false" AllowSorting="true" EnableHeaderContextMenu="true" OnNeedDataSource="rgTransformations_NeedDataSource"
                        OnItemDataBound="rgTransformations_ItemDataBound" Skin="Default" 
                        OnItemCommand="rgTransformations_ItemCommand"
                        OnDeleteCommand="rgTransformations_DeleteCommand"
                        >
                        <MasterTableView DataKeyNames="transformdataPK,TransTypeCode" CommandItemDisplay="Top"
                            NoMasterRecordsText="">
                            <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add new transformation" />
                            <RowIndicatorColumn>
                                <HeaderStyle Width="20px" />
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn>
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridEditCommandColumn ButtonType="ImageButton" HeaderStyle-Width="30" />
                                <telerik:GridButtonColumn CommandName="Delete" ButtonType="ImageButton" ConfirmDialogType="RadWindow"
                                    ConfirmText="Are you sure you want to delete this transformation?" HeaderStyle-Width="30" />
                                <telerik:GridBoundColumn DataField="clvrTableName" HeaderText="Clever Table" SortExpression="clvrTableName"
                                    UniqueName="clvrTableName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="clvrColumnName" HeaderText="Clever Column" SortExpression="clvrColumnName"
                                    UniqueName="clvrColumnName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TransTypeCode" HeaderText="Transformation Type"
                                    SortExpression="TransTypeCode" UniqueName="TransTypeCode">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Trans Details">
                                    <ItemTemplate>
                                        <div id="divOneToOne" runat="server">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="padding-right: 10px;">
                                                        <b>Cust Table:</b>
                                                        <asp:Label ID="lblCustTable" runat="server" />
                                                    </td>
                                                    <td>
                                                        <b>Cust Column:</b>
                                                        <asp:Label ID="lblCustColumn" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="divOneToMany" runat="server">
                                            <asp:Repeater runat="server" ID="repRelations">
                                                <HeaderTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <b>Cust Table:</b>
                                                            <asp:Label ID="lblCustTable" runat="server" Text='<%# Eval("custTableName") %>' />
                                                          
                                                        </td>
                                                        <td>
                                                            <b>Cust Column:</b>
                                                            <asp:Label ID="lblCustColumn" runat="server" Text='<%# Eval("custColumnName") %>'/>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </div>
                                        <div id="divRefValue" runat="server">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="padding-right: 10px;">
                                                        <b>Ref Table:</b>
                                                        <asp:Label ID="refTableName" runat="server" />
                                                    </td>
                                                    <td>
                                                        <b>Ref Value:</b>
                                                       <asp:Label ID="refValue" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                           
                                            
                                        </div>
                                        <div id="divOther" runat="server" style="padding-left: 7px;">
                                            -
                                        </div>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Trans Desc">
                                    <ItemTemplate>
                                        <%# Eval("transformdataDesc") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Trans Memo">
                                    <ItemTemplate>
                                        <%# Eval("transformMemo")%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                        <%--   <ClientSettings>
                            <Selecting AllowRowSelect="True" />
                        </ClientSettings>--%>
                    </telerik:RadGrid>
                    <!--Add/Edit Transformation -->
                    <telerik:RadWindow ID="rwTransDetails" runat="server" ShowContentDuringLoad="False"
                        Title="Transformation Details" OnClientClose="ClickCancel" DestroyOnClose="false"
                        Width="880px" Height="500px" VisibleOnPageLoad="false" Behaviors="Close, Move, Resize">
                        <ContentTemplate>
                            
                            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
                                Width="100%" CssClass="ajax_panel">
                                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" IsSticky="true" runat="server"
                                   >
                                </telerik:RadAjaxLoadingPanel>
                                <asp:Label ID="lblFormError" runat="server" CssClass="error" />
                                <table>
                                    <tr>
                                        <td style="width: 100px;">
                                            Trans Type
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="rcbTransType" runat="server" DataTextField="TransTypeCode"
                                                DataValueField="TransTypePK" Width="180" CssClass="rcb_web20" AutoPostBack="true"
                                                OnSelectedIndexChanged="rcbTransType_SelectedIndexChanged">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Clever Table <span class="validator">*</span>
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="rcbCleverTable" runat="server" Width="180" DropDownWidth="258"
                                                DataTextField="TableName" DataValueField="TablePK" EnableLoadOnDemand="True"
                                                ItemRequestTimeout="500" ItemsPerRequest="10" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                                                OnItemsRequested="rcbCleverTable_ItemsRequested" EnableItemCaching="true" 
                                               AutoPostBack="true">
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="rfvCleverTable" runat="server" ControlToValidate="rcbCleverTable"
                                                Display="Dynamic" ErrorMessage="Select table" ValidationGroup="TransData" />
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trCleverColumn">
                                        <td>
                                            Clever Column <span class="validator">*</span>
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="rcbCleverColumn" runat="server" Width="180" DropDownWidth="258"
                                                DataTextField="ColumnName" DataValueField="ColumnPK" EnableLoadOnDemand="True"
                                                ItemRequestTimeout="500" ItemsPerRequest="10" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                                                OnItemsRequested="rcbCleverColumn_ItemsRequested" EnableItemCaching="true">
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="rfvCleverColumn" runat="server" ControlToValidate="rcbCleverColumn"
                                                Display="Dynamic" ErrorMessage="Select column" ValidationGroup="TransData" />
                                        </td>
                                    </tr>
                                    <asp:PlaceHolder ID="phRefValue" runat="server">
                                        <tr>
                                            <td>Ref Table<span class="validator">*</span></td>
                                            <td>
                                             <telerik:RadComboBox ID="rcbRefTable" runat="server" Width="180" DropDownWidth="258"
                                                DataTextField="TableName" DataValueField="TablePK" EnableLoadOnDemand="True"
                                                ItemRequestTimeout="500" ItemsPerRequest="10" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                                                OnItemsRequested="rcbRefTable_ItemsRequested" EnableItemCaching="true" AutoPostBack="true"
                                                OnSelectedIndexChanged="rcbRefTable_SelectedIndexChanged">
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="rfvRefTable" runat="server" ControlToValidate="rcbCleverTable"
                                                Display="Dynamic" ErrorMessage="Select table" ValidationGroup="TransData" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Ref Value<span class="validator">*</span>
                                            </td>
                                            <td>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <telerik:RadComboBox ID="rcbRefValue" runat="server" CssClass="rcb_web20" Width="180">
                                                            </telerik:RadComboBox>
                                                            <asp:RequiredFieldValidator ID="rfvRefValue" runat="server" ControlToValidate="rcbRefValue"
                                                                Display="Dynamic" ErrorMessage="Select value" ValidationGroup="TransData" />
                                                        </td>
                                                        <td style="padding-left:5px;">
                                                            <asp:TextBox ID="txtRefValue" runat="server" MaxLength="50" />
                                                            <asp:RequiredFieldValidator ID="rfvRevValueEntered" runat="server" ControlToValidate="txtRefValue"
                                                                Display="Static" ErrorMessage="*" ValidationGroup="RefTableValue" />
                                                        </td>
                                                        <td>
                                                            <asp:Button runat="server" Text="Add" ID="btnAdd" OnClick="btnAdd_Click" CausesValidation="true"
                                                                ValidationGroup="RefTableValue" Width="60" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="phOneToOne" runat="server">
                                        <tr>
                                            <td>
                                                Cust Table <span class="validator">*</span>
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="rcbCustTable" runat="server" DataTextField="TableName" DataValueField="TablePK"
                                                    Width="180" CssClass="rcb_web20" AutoPostBack="true" OnSelectedIndexChanged="rcbCustTable_SelectedIndexChanged">
                                                </telerik:RadComboBox>
                                                <asp:RequiredFieldValidator ID="rfvCustTable" runat="server" ControlToValidate="rcbCustTable"
                                                    Display="Dynamic" ErrorMessage="Select column" ValidationGroup="TransData" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Cust Column <span class="validator">*</span>
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="rcbCustColumn" runat="server" DataTextField="ColumnName"
                                                    DataValueField="ColumnPK" Width="180" CssClass="rcb_web20">
                                                </telerik:RadComboBox>
                                                <asp:RequiredFieldValidator ID="rfvCustColumn" runat="server" ControlToValidate="rcbCustColumn"
                                                    Display="Dynamic" ErrorMessage="Select column" ValidationGroup="TransData" />
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="phOneToMany" runat="server">
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <telerik:RadGrid ID="rgCustColumns" runat="server" AutoGenerateColumns="False" AllowMultiRowEdit="True"
                                                    OnPreRender="rgCustColumns_PreRender" OnItemDataBound="rgCustColumns_ItemDataBound"
                                                    OnItemCreated="rgCustColumns_ItemCreated" Width="90%" RegisterWithScriptManager="false">
                                                    <MasterTableView EditMode="InPlace" Width="90%">
                                                        <Columns>
                                                            <telerik:GridDropDownColumn UniqueName="CustTableName" DataField="CustTableName" 
                                                                HeaderText="Cust Table" />
                                                            <telerik:GridDropDownColumn UniqueName="CustColumnName" DataField="CustColumnName"
                                                                HeaderText="Cust Column" />
                                                            <telerik:GridTemplateColumn UniqueName="Desc" HeaderText="Desc" HeaderStyle-Width="100">
                                                                <ItemTemplate>
                                                                    <asp:TextBox runat="server" ID="txtDesc"></asp:TextBox>
                                                                      <asp:HiddenField ID="hfPK" runat="server" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="Memo" HeaderText="Memo" HeaderStyle-Width="100">
                                                                <ItemTemplate>
                                                                    <asp:TextBox runat="server" ID="txtMemo"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                    <tr>
                                        <td>
                                            Trans Desc
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTransDesc" runat="server" Width="300" MaxLength="50" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Trans Memo
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTransMemo" runat="server" Width="300" Rows="3" TextMode="MultiLine" />
                                        </td>
                                    </tr>
                                </table>
                            </telerik:RadAjaxPanel>
                            <div>
                                <br />
                                <br />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                    CausesValidation="False" Width="60" />
                                &nbsp;
                                <asp:Button ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click" CausesValidation="true"
                                    ValidationGroup="TransData" Width="60" />
                            </div>
                        </ContentTemplate>
                    </telerik:RadWindow>

                    <script type="text/javascript" language="javascript">
                        function ClickCancel(oWnd, args) {
                            var btnCancel = document.getElementById("<%=btnCancel.ClientID%>");
                            btnCancel.click();

                            var rwTransDetails = $find("<%=rwTransDetails.ClientID%>");
                            rwTransDetails.close();
                        }
                    </script>

                </ContentTemplate>
            </telerik:RadDock>
        </telerik:RadDockZone>
    </telerik:RadDockLayout>
</asp:Content>
