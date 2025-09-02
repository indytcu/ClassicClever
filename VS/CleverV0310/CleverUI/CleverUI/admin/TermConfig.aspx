<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true"
    CodeBehind="TermConfig.aspx.cs" Inherits="CleverUI.Admin.TermConfig" %>

<asp:Content ID="cAdminTermConfig" ContentPlaceHolderID="cphAdminPage" runat="server">
    <telerik:RadDockLayout ID="rdlTermConfig" runat="server">
        <telerik:RadDockZone ID="rdzTermConfigs" runat="server" Orientation="Vertical" CssClass="zone"
            BorderStyle="None" FitDocks="true">
            <telerik:RadDock ID="rdTerms" runat="server" Title="Terms" DefaultCommands="ExpandCollapse"
                Skin="Web20" Width="100%">
                <ContentTemplate>
                    <div style="padding-bottom: 4px;">
                        &nbsp; Groups: &nbsp;
                        <telerik:RadComboBox ID="ddlGroups" runat="server" Width="240" DataTextField="TermGroupCode"
                            DataValueField="TermGroupPK" Filter="StartsWith" AutoPostBack="true" OnSelectedIndexChanged="ddlGroups_SelectedIndexChanged" />
                        &nbsp; Terms: &nbsp;
                        <telerik:RadComboBox ID="ddlTerms" runat="server" Width="240" DataTextField="TermCode"
                            DataValueField="TermPK" Filter="StartsWith" />
                        &nbsp; Term Code: &nbsp;
                        <telerik:RadTextBox Width="200px" ID="rtbTermCode" runat="server" MaxLength="255">
                        </telerik:RadTextBox>
                        &nbsp; Term Desc: &nbsp;
                        <telerik:RadTextBox Width="200px" ID="rtbTermDesc" runat="server" MaxLength="255">
                        </telerik:RadTextBox>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" Style="vertical-align: middle;"
                            OnClick="btnSearch_Click" CausesValidation="false" />
                    </div>
                    <asp:Label ID="lblError" runat="server" CssClass="error" />
                    <telerik:RadGrid ID="rgTerms" runat="server" AutoGenerateColumns="False" GridLines="None"
                        OnNeedDataSource="rgTerms_NeedDataSource" AllowPaging="true" AllowSorting="true"
                        EnableHeaderContextMenu="true" 
                        OnItemDataBound="rgTerms_ItemDataBound" OnItemCommand="rgTerms_ItemCommand" Skin="Default"
                        PageSize="20">
                        <MasterTableView DataKeyNames="TermPK" NoMasterRecordsText="No terms found." CommandItemDisplay="Top">
                            <CommandItemSettings ShowAddNewRecordButton="false" />
                            <RowIndicatorColumn>
                                <HeaderStyle Width="20px" />
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn>
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridEditCommandColumn ButtonType="ImageButton" HeaderStyle-Width="30" />
                                <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/App_Themes/Default/Images/search.png" CommandName="TermConfig" />
                                <telerik:GridBoundColumn DataField="TermCode" HeaderText="Term Code" SortExpression="TermCode"
                                    UniqueName="TermCode">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TermDesc" HeaderText="Term Desc" SortExpression="TermDesc"
                                    UniqueName="TermDesc">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TermColumnName" HeaderText="Term Column Name"
                                    SortExpression="TermColumnName" UniqueName="TermColumnName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TermGroupCode" HeaderText="Term Group Code" SortExpression="TermGroupCode"
                                    UniqueName="TermGroupCode">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TermUIEnable" HeaderText="Term UI Enable" SortExpression="TermUIEnable"
                                    UniqueName="TermUIEnable">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TermGroupEnable" HeaderText="Term Group Enable"
                                    SortExpression="TermGroupEnable" UniqueName="TermGroupEnable">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TermIsComputed" HeaderText="Term Is Computed"
                                    SortExpression="TermIsComputed" UniqueName="TermIsComputed">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </ContentTemplate>
            </telerik:RadDock>
            <telerik:RadDock ID="rdTermConfig" runat="server" Title="Term Configs" DockMode="Docked"
                EnableDrag="false" DefaultCommands="None" Visible="false">
                <ContentTemplate>
                    <telerik:RadGrid ID="rgTermConfigs" runat="server" AllowPaging="True" PageSize="20"
                        EnableHeaderContextMenu="true" GridLines="None" AllowSorting="True" OnNeedDataSource="rgTermConfigs_NeedDataSource"
                        OnItemCommand="rgTermConfigs_ItemCommand" Skin="Default" OnItemDataBound="rgTermConfigs_ItemDataBound"
                        CellSpacing="0" OnDeleteCommand="rgTermConfigs_DeleteCommand">
                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="TermConfigPK,TermFK,TermCaption,IsEnabled,TermUIEnable"
                            NoMasterRecordsText="No configuration terms found." CommandItemDisplay="Top">
                            <CommandItemSettings ShowAddNewRecordButton="false" />
                            <RowIndicatorColumn>
                                <HeaderStyle Width="20px"></HeaderStyle>
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn>
                                <HeaderStyle Width="20px"></HeaderStyle>
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridEditCommandColumn ButtonType="ImageButton" HeaderStyle-Width="20" />                                
                                <telerik:GridBoundColumn DataField="TermConfigPK" HeaderText="TermConfigPK" SortExpression="TermConfigPK"
                                    Display="false" UniqueName="TermConfigPK" />
                                <telerik:GridBoundColumn DataField="TermFK" HeaderText="TermFK" SortExpression="TermFK"
                                    Display="false" UniqueName="TermFK" />
                                <telerik:GridCheckBoxColumn DataField="IsEnabled" HeaderText="Enabled" SortExpression="IsEnabled"
                                    UniqueName="IsEnabled" HeaderStyle-Width="20px">
                                </telerik:GridCheckBoxColumn>
                                <telerik:GridCheckBoxColumn DataField="IsTermDefault" HeaderText="Default" SortExpression="IsTermDefault"
                                    UniqueName="IsTermDefault" HeaderStyle-Width="20px">
                                </telerik:GridCheckBoxColumn>
                                <telerik:GridCheckBoxColumn DataField="TermIsNumeric" HeaderText="Numeric" SortExpression="TermIsNumeric"
                                    UniqueName="TermIsNumeric" HeaderStyle-Width="20px">
                                </telerik:GridCheckBoxColumn>
                                <telerik:GridBoundColumn DataField="TermConfigCode" HeaderText="Term Config Code"
                                    SortExpression="TermConfigCode" UniqueName="TermConfigCode">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TermColumnName" HeaderText="Term Column Name"
                                    SortExpression="TermColumnName" UniqueName="TermColumnName">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TermCode" HeaderText="Term Code" SortExpression="TermCode"
                                    UniqueName="TermCode" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TermHeading" HeaderText="Term Heading" SortExpression="TermHeading"
                                    UniqueName="TermHeading">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TermCaption" HeaderText="Term Caption" SortExpression="TermCaption"
                                    UniqueName="TermCaption">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="NbrTables" HeaderText="# of Tables" SortExpression="NbrTables"
                                    UniqueName="NbrTables">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TableDesc" HeaderText="Table Desc" SortExpression="TableDesc"
                                    UniqueName="TableDesc">
                                    </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TermUIEnable" HeaderText="Term UI Enable" SortExpression="TermUIEnable"
                                    UniqueName="TermUIEnable">
                                </telerik:GridBoundColumn>
                            </Columns>
                            <NestedViewSettings>
                                <ParentTableRelation>
                                    <telerik:GridRelationFields DetailKeyField="TermConfigPK" MasterKeyField="TermConfigPK" />
                                </ParentTableRelation>
                            </NestedViewSettings>
                            <NestedViewTemplate>
                                <table cellpadding="5" cellspacing="0">
                                    <tr>
                                        <td>
                                            <strong>Term Summary Column:</strong>
                                        </td>
                                        <td>
                                            <%#Eval("TermSummaryColumnName")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Select String:</strong>
                                        </td>
                                        <td>
                                            <%#Eval("SelectString") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Select Summary String:</strong>
                                        </td>
                                        <td>
                                            <%#Eval("SelectSummaryString") %>
                                        </td>
                                    </tr>
                                </table>
                                <table cellpadding="0" cellspacing="0" class="tableBlackWhite">
                                    <thead>
                                        <tr>
                                            <td>
                                                #
                                            </td>
                                            <td>
                                                Table
                                            </td>
                                            <td>
                                                Join Path Code
                                            </td>
                                            <td>
                                                Default
                                            </td>
                                            <td>
                                                Join statement
                                            </td>
                                        </tr>
                                    </thead>
                                    <tr>
                                        <td>
                                            1
                                        </td>
                                        <td>
                                            <%#Eval("TableDesc") %>
                                        </td>
                                        <td>
                                            <%#Eval("TablesJoinPathCode")%>
                                        </td>
                                        <td>
                                            <%#Eval("IsDefault")%>
                                        </td>
                                        <td>
                                            <%#Eval("JoinStatement")%>
                                        </td>
                                    </tr>
                                    <asp:Literal runat="server" ID="litTables" />
                                </table>
                            </NestedViewTemplate>
                        </MasterTableView>
                    </telerik:RadGrid>
                </ContentTemplate>
            </telerik:RadDock>
        </telerik:RadDockZone>
    </telerik:RadDockLayout>
    <telerik:RadWindow ID="rwEditTerm" runat="server" ShowContentDuringLoad="False" Title="Supplier"
        OnClientClose="ClickTermCancel" DestroyOnClose="false" Width="500px" Height="350px"
        VisibleOnPageLoad="false" Behaviors="Close, Move, Resize">
        <ContentTemplate>
            <asp:Label ID="lblEditTermError" runat="server" ForeColor="Red" />
            <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server">
                <table cellpadding="2" style="margin: 5px;">
                    <tr>
                        <td>
                            Term Code:
                        </td>
                        <td>
                            <asp:Label ID="lblTermCode" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Term Desciption:
                        </td>
                        <td>
                            <asp:Label ID="lblTermDesc" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Term Column Name:
                        </td>
                        <td>
                            <asp:Label ID="lblTermColumn" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Term Group Code:
                        </td>
                        <td>
                            <asp:Label ID="lblTermGroupCode" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Term UI Enable:
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblTermUIEnable" runat="server">
                                <asp:ListItem Text="True" Value="1" />
                                <asp:ListItem Text="False" Value="0" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Term Group Enable:
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblTermGroupEnable" runat="server">
                                <asp:ListItem Text="True" Value="1" />
                                <asp:ListItem Text="False" Value="0" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Term Is Computed:
                        </td>
                        <td>
                            <asp:Label ID="lblTermIsComputed" runat="server" />
                        </td>
                    </tr>
                </table>
                <br />
            </telerik:RadAjaxPanel>
            <div>
                <asp:Button ID="btnCancelTerm" runat="server" Text="Cancel" OnClick="btnCancelTerm_Click"
                    CausesValidation="False" Width="60" />
                &nbsp;
                <asp:Button ID="btnSaveTerm" Text="Save" runat="server" OnClick="btnSaveTerm_Click"
                    CausesValidation="true" ValidationGroup="TermData" Width="60" />
            </div>
        </ContentTemplate>
    </telerik:RadWindow>
    <script type="text/javascript" language="javascript">
        function ClickTermCancel(oWnd, args) {
            var btn = document.getElementById("<%=btnCancelTerm.ClientID%>");
            btn.click();

            var rwEditTerm = $find("<%=rwEditTerm.ClientID%>");
            rwEditTerm.close();
        }
    </script>
    <telerik:RadWindow ID="rwAddEditTermConfig" runat="server" ShowContentDuringLoad="False"
        Title="Add Edit Term Config" OnClientClose="ClickCancel" DestroyOnClose="false"
        Width="750px" Height="500px" VisibleOnPageLoad="false" Behaviors="Close, Move, Resize">
        <ContentTemplate>
            <asp:Label ID="lblFormError" runat="server" ForeColor="Red" />
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
                <table cellpadding="2" style="margin: 5px;">
                    <tr>
                        <td>
                            Terms:
                        </td>
                        <td>
                            <telerik:RadComboBox ID="rblSelectTerms" runat="server" Width="240" AutoPostBack="true"
                                OnSelectedIndexChanged="rblSelectTerms_SelectedIndexChanged" Filter="StartsWith"
                                DataTextField="TermCode" DataValueField="TermPK" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Details:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblTermConfigDetails" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Repeater runat="server" ID="rptTermCalc">
                                <HeaderTemplate>
                                    <table cellpadding="0" cellspacing="0" width="400" class="tableBlackWhite">
                                        <thead>
                                            <tr>
                                                <td>
                                                    Term Calc
                                                </td>
                                                <td>
                                                    Table
                                                </td>
                                            </tr>
                                        </thead>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <%# Eval("Replace") %>
                                        </td>
                                        <td>
                                            <%# Eval("TableDesc") %>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table></FooterTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <telerik:RadGrid ID="rgTableJoinPaths" runat="server" AutoGenerateColumns="false"
                                Width="650" OnNeedDataSource="rgTableJoinPaths_NeedDataSource" OnItemDataBound="rgTableJoinPaths_ItemDataBound"
                                DataKeyNames="TablePK">
                                <MasterTableView>
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="TableDesc" HeaderText="Table" SortExpression="TableDesc"
                                            UniqueName="TableDesc">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn HeaderText="Join Path" SortExpression="JoinPath" UniqueName="JoinPath">
                                            <ItemTemplate>
                                                <telerik:RadComboBox ID="rcbTableJoinPath" runat="server" Width="250" Skin="Default"
                                                    AutoPostBack="true" OnSelectedIndexChanged="rcbTableJoinPath_SelectedIndexChanged" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Join Statements" SortExpression="JoinStatements"
                                            UniqueName="JoinStatements">
                                            <ItemTemplate>
                                                <asp:Repeater ID="repJoinPathJoins" runat="server">
                                                    <ItemTemplate>
                                                        <div>
                                                            <%# Eval("JoinStatement") %></div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Caption
                        </td>
                        <td>
                            <asp:TextBox ID="txtCaption" runat="server" MaxLength="256" Width="250px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Enabled
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblEnableValue" runat="server">
                                <asp:ListItem Text="True" Value="True" />
                                <asp:ListItem Text="False" Value="False" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                          <asp:Label ID="lblTermUIEnable" runat="server" Text="Term UI Enable" ></asp:Label>
                        </td>
                        <td>
                            <telerik:RadNumericTextBox ID="txtTermUIEnable" runat="server" MaxLength="5" MinValue="0" NumberFormat-DecimalDigits="0" />
                        </td>
                    </tr>
                </table>
            </telerik:RadAjaxPanel>
            <div>
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                    CausesValidation="false" />
                &nbsp;
                <asp:Button ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click" CausesValidation="false" />
            </div>
        </ContentTemplate>
    </telerik:RadWindow>
    <script type="text/javascript" language="javascript">
        function ClickCancel(oWnd, args) {
            var btnCancel = document.getElementById("<%=btnCancel.ClientID%>");
            btnCancel.click();

            var rwAddEditTermConfig = $find("<%=rwAddEditTermConfig.ClientID%>");
            rwAddEditTermConfig.close();
        }  
    </script>
</asp:Content>
