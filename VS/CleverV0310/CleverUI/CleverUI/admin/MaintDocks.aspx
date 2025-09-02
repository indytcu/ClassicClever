<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true"
    CodeBehind="MaintDocks.aspx.cs" Inherits="CleverUI.Admin.MaintDocks" Title="Clever - Admin - Manage Docks" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="FilterManager" Src="~/User/FilterManager.ascx" %>

<%@ Register TagPrefix="uc" TagName="FilterManagerTreeList" Src="~/User/FilterManagerTreeList.ascx" %>



<asp:Content ID="cAdminPage" ContentPlaceHolderID="cphAdminPage" runat="server">
    <telerik:RadDockLayout ID="RadDockLayout1" runat="server">
        <telerik:RadDockZone ID="rdzDocks" runat="server" Orientation="Vertical" BorderStyle="None">
                <telerik:RadDock ID="rdPortals" runat="server" Title=" - Portals" DefaultCommands="ExpandCollapse"
                Skin="Sunset" Width="100%">
                <ContentTemplate>
                    <asp:Label ID="lblErrorPortal" runat="server" Visible="false" CssClass="error" />
                    <telerik:RadGrid ID="rgPortals" runat="server" GridLines="None" OnItemDataBound="rgPortals_ItemDataBound"
                        AllowAutomaticInserts="false" AllowAutomaticUpdates="false" OnItemCommand="rgPortals_ItemCommand"
                        OnNeedDataSource="rgPortals_NeedDataSource" 
                        OnSelectedIndexChanged="rgPortals_SelectedIndexChanged" Skin="Default" >
                        <MasterTableView DataKeyNames="PortalPK,PortalCode,PortalDesc"  AutoGenerateColumns="False">
                            <CommandItemSettings ShowAddNewRecordButton="false" />
                            <RowIndicatorColumn>
                                <HeaderStyle Width="20px" />
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn>
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridEditCommandColumn ButtonType="ImageButton" HeaderStyle-Width="15" ItemStyle-Width="15" />
                                                                                                 <telerik:GridBoundColumn DataField="PortalUIOrder" UniqueName="PortalUIOrder" HeaderText="UI Order" HeaderStyle-Width="50" ItemStyle-Width="50"
                                            HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"  />
                                             <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/App_Themes/Default/images/up.png"
                                    UniqueName="MoveUp" CommandName="MoveUp" Text="Move Up" HeaderStyle-Width="15" />
                                <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/App_Themes/Default/images/down.png"
                                    UniqueName="MoveDown" CommandName="MoveDown" Text="Move Down" HeaderStyle-Width="15" />
                          <telerik:GridCheckBoxColumn DataField="IsVisible" HeaderText="Visible"  UniqueName="IsVisible" HeaderStyle-Width="30" ItemStyle-Width="30">
                                </telerik:GridCheckBoxColumn>
                                <telerik:GridBoundColumn DataField="PortalCode" HeaderText="Portal Code" UniqueName="PortalCode" HeaderStyle-Width="200" ItemStyle-Width="200">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PortalDesc" HeaderText="Desc" UniqueName="PortalDesc" >
                                </telerik:GridBoundColumn>
								<telerik:GridBoundColumn DataField="Roles" HeaderText="Roles" UniqueName="Roles" >
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings EnablePostBackOnRowClick="True">
                            <Selecting AllowRowSelect="True" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </ContentTemplate>
            </telerik:RadDock>
            <telerik:RadDock ID="rdDock" runat="server" Title=" - Docks" DefaultCommands="ExpandCollapse"
                Skin="Web20" Width="100%">
                <ContentTemplate>

                    <asp:Label ID="lblError" runat="server" Visible="false" CssClass="error" />
                    <telerik:RadGrid ID="rgDock" runat="server" AutoGenerateColumns="False" GridLines="None"
                        AllowAutomaticInserts="false" AllowAutomaticUpdates="false" OnItemCommand="rgDock_ItemCommand"
                        OnNeedDataSource="rgDock_NeedDataSource" OnItemDataBound="rgDock_ItemDataBound"
                        OnSelectedIndexChanged="rgDock_SelectedIndexChanged" Skin="Default">
                        <MasterTableView DataKeyNames="DockPK,DockCode,appFilterFK,DockDesc,PortalDockPK" NoMasterRecordsText="No docks found.">
                            <CommandItemSettings ShowAddNewRecordButton="false" />
                            <RowIndicatorColumn>
                                <HeaderStyle Width="20px" />
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn>
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridEditCommandColumn ButtonType="ImageButton" HeaderStyle-Width="30px" />
                                <telerik:GridButtonColumn Text="Filter" CommandName="DockFilter" ButtonType="PushButton"
                                    HeaderStyle-Width="60">
                                </telerik:GridButtonColumn>
                                <telerik:GridBoundColumn DataField="DockCode" HeaderText="Dock Code" SortExpression="DockCode"
                                    UniqueName="DockCode" HeaderStyle-Width="100px">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="PortalDockDesc" HeaderText="Portal Dock Desc" SortExpression="PortalDockDesc"
                                    UniqueName="PortalDockDesc" HeaderStyle-Width="100px">
                                </telerik:GridBoundColumn>
                                <telerik:GridCheckBoxColumn DataField="IsVisible" HeaderText="Visible" SortExpression="IsVisible"
                                    UniqueName="IsVisible" HeaderStyle-Width="50px">
                                </telerik:GridCheckBoxColumn>
                                <telerik:GridCheckBoxColumn DataField="IsExpanded" HeaderText="Expanded" SortExpression="IsExpanded"
                                    UniqueName="IsExpanded" HeaderStyle-Width="50px">
                                </telerik:GridCheckBoxColumn>
                               <telerik:GridBoundColumn DataField="DockDesc" HeaderText="Dock Desc" SortExpression="DockDesc"
                                    UniqueName="DockDesc" HeaderStyle-Width="150px">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Default Mode" SortExpression="DockMode"
                                    UniqueName="DockMode" HeaderStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:Literal ID="lblDockMode" runat="server" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Dock Zone" SortExpression="DockZone"
                                    UniqueName="DockZone" HeaderStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:Literal ID="lblDockZone" runat="server" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="DockOrder" HeaderText="Display Order" SortExpression="DockOrder"
                                    UniqueName="DockOrder" HeaderStyle-Width="100px">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings EnablePostBackOnRowClick="True">
                            <Selecting AllowRowSelect="True" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </ContentTemplate>
            </telerik:RadDock>
            <telerik:RadDock ID="rdDockTerms" runat="server" Title=" - Dock Terms" DefaultCommands="ExpandCollapse"
                Skin="Web20" Width="100%">
                <ContentTemplate>
                    <asp:Label ID="lblErrorTermsSection" runat="server" Visible="false" CssClass="error" />
                    <asp:RadioButtonList ID="rblDockMode" runat="server" RepeatDirection="Horizontal"
                        AutoPostBack="true" OnSelectedIndexChanged="rblDockMode_SelectedIndexChanged">
                        <asp:ListItem Text="Overview" Value="1" Selected="True" />
                        <asp:ListItem Text="Detail" Value="0" />
                    </asp:RadioButtonList>
                    <telerik:RadGrid ID="rgDockTerms" runat="server" AutoGenerateColumns="False" GridLines="None"
                        AllowPaging="false" AllowSorting="false" AllowAutomaticInserts="false"
                        AllowAutomaticUpdates="false" OnNeedDataSource="rgDockTerms_NeedDataSource" OnItemDataBound="rgDockTerms_ItemDataBound"
                        OnItemCommand="rgDockTerms_ItemCommand" Skin="Default" OnDeleteCommand="rgDockTerms_DeleteCommand">
                        <MasterTableView DataKeyNames="DockTermsPK,TermGroup,TermOrder" CommandItemDisplay="Top" NoMasterRecordsText="No dock terms found.">
                            <CommandItemSettings AddNewRecordText="Add new term" />
                            <RowIndicatorColumn>
                                <HeaderStyle Width="20px" />
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn>
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridEditCommandColumn ButtonType="ImageButton" HeaderStyle-Width="15" />
                                <telerik:GridButtonColumn CommandName="Delete" ButtonType="ImageButton" ConfirmDialogType="RadWindow"
                                    ConfirmText="Are you sure you want to delete this term from the dock?" HeaderStyle-Width="15" />
                                <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/App_Themes/Default/images/up.png"
                                    UniqueName="MoveUp" CommandName="MoveUp" Text="Move Up" HeaderStyle-Width="15" />
                                <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/App_Themes/Default/images/down.png"
                                    UniqueName="MoveDown" CommandName="MoveDown" Text="Move Down" HeaderStyle-Width="15" />
                                <telerik:GridBoundColumn DataField="TermOrder" HeaderText="Order" SortExpression="TermOrder"
                                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                    UniqueName="TermOrder" HeaderStyle-Width="50">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TermCaption" HeaderText="Caption" SortExpression="TermCaption"
                                    UniqueName="TermCaption" ItemStyle-Wrap="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TermConfigCode" HeaderText="Term Config Code" SortExpression="TermConfigCode"
                                    UniqueName="TermConfigCode">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TermAlias" HeaderText="Alias" SortExpression="TermAlias"
                                    UniqueName="TermAlias" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TermColumnName" HeaderText="Column Name" SortExpression="TermColumnName"
                                    UniqueName="TermColumnName" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TermSelectString" HeaderText="Select String"
                                    SortExpression="TermSelectString" UniqueName="TermSelectString" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TermOpsCode" HeaderText="Ops" SortExpression="TermOpsCode"
                                    UniqueName="TermOpsCode">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Visible Mode" SortExpression="ModeEnabled"
                                    UniqueName="ModeEnabled">
                                    <ItemTemplate>
                                        <asp:Literal ID="lblModeEnabled" runat="server" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="TermFormatString" HeaderText="Format" SortExpression="TermFormatString"
                                    UniqueName="TermFormatString">
                                </telerik:GridBoundColumn>
                                <telerik:GridCheckBoxColumn DataField="TermIsEnabled" HeaderText="Enabled" SortExpression="TermIsEnabled"
                                    UniqueName="TermIsEnabled">
                                </telerik:GridCheckBoxColumn>
                                <telerik:GridCheckBoxColumn DataField="TermIsVisible" HeaderText="Visible" SortExpression="TermIsVisible"
                                    UniqueName="TermIsVisible">
                                </telerik:GridCheckBoxColumn>
                                <telerik:GridTemplateColumn HeaderText="OrderBy" SortExpression="OrderBy" UniqueName="OrderBy"
                                    Display="false">
                                    <ItemTemplate>
                                        <asp:Literal ID="lblOrderBy" runat="server" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="TermGroup" HeaderText="Group" SortExpression="TermGroup" 
                                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                    UniqueName="TermGroup">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings>
                            <Selecting AllowRowSelect="True" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </ContentTemplate>
            </telerik:RadDock>
        </telerik:RadDockZone>
    </telerik:RadDockLayout>
	<telerik:RadWindow ID="rwAddEditPortal" runat="server" ShowContentDuringLoad="False"
		OnClientClose="ClickDockCancel" DestroyOnClose="false" Width="500px" Height="350px"
		Behaviors="Close,Move" VisibleOnPageLoad="false" Title=" - Update Portal">
		<ContentTemplate>
			<asp:UpdatePanel ID="UpdatePanel3" runat="server">
				<Triggers>
					<asp:PostBackTrigger ControlID="btnCancelPortal" />
					<asp:PostBackTrigger ControlID="btnSavePortal" />
				</Triggers>
				<ContentTemplate>
					<asp:HiddenField ID="hfPortalPK" runat="server" />
					<asp:Label ID="lblPortalFormError" runat="server" ForeColor="Red" />
					<table class="term_form">
						<tr>
							<td>
								Code <span class="validator">*</span>
							</td>
							<td>
								<telerik:RadTextBox ID="txtPortalCode" runat="server" MaxLength="50" Width="200" />
								<div>
									<asp:RequiredFieldValidator ID="rfvPortalCode" runat="server" ControlToValidate="txtPortalCode"
										Display="Dynamic" ErrorMessage="Enter code" ValidationGroup="PortalData" />
								</div>
							</td>
						</tr>
						<tr>
							<td>
								Description <span class="validator">*</span>
							</td>
							<td>
								<telerik:RadTextBox ID="txtPortalDesc" runat="server" MaxLength="255" Width="200" />
								<div>
									<asp:RequiredFieldValidator ID="rfvPortalDesc" runat="server" ControlToValidate="txtPortalDesc"
										Display="Dynamic" ErrorMessage="Enter Description" ValidationGroup="PortalData" />
								</div>
							</td>
						</tr>
						<tr>
							<td>
								Navigate URL
							</td>
							<td>
								<telerik:RadTextBox ID="txtNavigateURL" runat="server" MaxLength="255" Width="200" />
							</td>
						</tr>
                        <tr>
							<td>
								Is Dynamic
							</td>
							<td align="left">
								<asp:CheckBox ID="chkPortalIsDynamic" runat="server" AutoPostBack="true" OnCheckedChanged="chkPortalIsDynamic_CheckedChanged" />
							</td>
						</tr>
						<tr>
							<td>
								Is Visible
							</td>
							<td align="left">
								<asp:CheckBox ID="chkPortalIsVisible" runat="server" />
							</td>
						</tr>
						<tr>
							<td>
								Roles
							</td>
							<td>
								<%--<telerik:RadTreeView ID="rtvRows" runat="server" DataTextField="code" DataValueField="id" CheckBoxes="true" CheckChildNodes="true" TriStateCheckBoxes="true"
        SingleExpandPath="false" MultipleSelect="true" style="margin: 5px;"></telerik:RadTreeView>--%>
								<asp:CheckBoxList ID="cxblRoles" runat="server" RepeatDirection="Horizontal" Style="margin-left: -3px ! important;" />

							</td>
						</tr>
						<tr>
							<td colspan="2">
								<asp:Button ID="btnCancelPortal" runat="server" Text="Cancel" OnClick="btnCancelPortal_Click"
									CausesValidation="False" Width="60" />
								&nbsp;
								<asp:Button ID="btnSavePortal" Text="Save" runat="server" CausesValidation="true"
									OnClick="btnSavePortal_Click" ValidationGroup="PortalData" Width="60" />
							</td>
						</tr>
					</table>
				</ContentTemplate>
			</asp:UpdatePanel>
		</ContentTemplate>
	</telerik:RadWindow>
    <telerik:RadWindow ID="rwAddEditDock" runat="server" ShowContentDuringLoad="False"
        Title=" - Dock" OnClientClose="ClickDockCancel" DestroyOnClose="false" Width="600px"
         AutoSize="true" AutoSizeBehaviors="Height" Behaviors="Close,Move" VisibleOnPageLoad="false">
        <ContentTemplate>
            <asp:HiddenField ID="hfPortalDockPK" runat="server" />
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnCancelDock" />
                    <asp:PostBackTrigger ControlID="btnSaveDock" />
                </Triggers>
                <ContentTemplate>
                    <asp:Label ID="lblDockFormError" runat="server" ForeColor="Red" />
                    <table class="term_form">
                        <tr>
                            <td>
                                Code 
                            </td>
                            <td>
                                <asp:Label ID="lblDockCode" runat="server"></asp:Label>
                            </td>
                        </tr>
                                            <tr>
                        <td>
                            Description <span class="required">*</span>
                        </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtDockDesc" runat="server" Width="245" Skin="Default" MaxLength="255" />
                            <asp:RequiredFieldValidator ID="rfvDockDesc" runat="server" ControlToValidate="txtDockDesc"
                                Display="Dynamic" ErrorMessage="Enter description" ValidationGroup="DockData" />
                        </td>
                    </tr>
                                           <tr id="trDockTitle" runat="server">
                        <td>
                            Title <span class="required">*</span>
                        </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtDockTitle" runat="server" Width="245" Skin="Default" MaxLength="255" />
                            <asp:RequiredFieldValidator ID="rfvDockTitle" runat="server" ControlToValidate="txtDockTitle"
                                Display="Dynamic" ErrorMessage="Enter title" ValidationGroup="DockData" />
                        </td>
                    </tr>
                     <tr id="trDockMode" runat="server">
                        <td>
                            Default Mode <span class="required">*</span>
                        </td>
                        <td align="left">
                            <telerik:RadComboBox ID="rcbDockMode" runat="server" Width="250" Skin="Default">
                                <Items>
                                    <telerik:RadComboBoxItem Text="" Value="" />
                                    <telerik:RadComboBoxItem Text="Preview" Value="1" />
                                    <telerik:RadComboBoxItem Text="Summary" Value="2" />
                                    <telerik:RadComboBoxItem Text="Details" Value="3" />
                                </Items>
                            </telerik:RadComboBox>
                            <asp:RequiredFieldValidator ID="rfvDockMode" runat="server" ControlToValidate="rcbDockMode"
                                Display="Dynamic" ErrorMessage="Select mode" ValidationGroup="DockData" />
                        </td>
                    </tr>
                     <tr id="trDockZone" runat="server">
                        <td>
                            Dock Zone <span class="required">*</span>
                        </td>
                        <td align="left">
                            <telerik:RadComboBox ID="rcbDockZone" runat="server" Width="250" Skin="Default">
                                <Items>
                                    <telerik:RadComboBoxItem Text="" Value="" />
                                    <telerik:RadComboBoxItem Text="Left" Value="1" />
                                    <telerik:RadComboBoxItem Text="Right" Value="2" />
                                </Items>
                            </telerik:RadComboBox>
                            <asp:RequiredFieldValidator ID="rfvDockZone" runat="server" ControlToValidate="rcbDockZone"
                                Display="Dynamic" ErrorMessage="Select zone" ValidationGroup="DockData" />
                        </td>
                    </tr>
                      <tr id="trDockOrder" runat="server">
                        <td>
                            Display Order <span class="required">*</span>
                        </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtDockOrder" runat="server" Width="245" Skin="Default" />
                            <asp:RequiredFieldValidator ID="rfvDockOrder" runat="server" ControlToValidate="txtDockOrder"
                                Display="Dynamic" ErrorMessage="Enter order" ValidationGroup="DockData" />
                            <asp:CompareValidator ID="cvDockOrder" runat="server" ErrorMessage="Number required" ControlToValidate="txtDockOrder" ValidationGroup="DockData"
                             Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                        </td>
                    </tr>
                     <tr id="trDockType" runat="server">
                        <td>
                            Dock Type <span class="required">*</span>
                        </td>
                        <td align="left">
                            <telerik:RadComboBox ID="rcbDockType" runat="server" Width="250" Skin="Default" AutoPostBack="true" OnSelectedIndexChanged="rcbDockType_SelectedIndexChanged">
                                <Items>
                                    <telerik:RadComboBoxItem Text="" Value="" />
                                    <telerik:RadComboBoxItem Text="Overview Grid" Value="1" />
                                    <telerik:RadComboBoxItem Text="User Control" Value="2" />
                                    <telerik:RadComboBoxItem Text="Chart Control" Value="3" />
                                    <telerik:RadComboBoxItem Text="Analysis Chart" Value="4" />
                                </Items>
                            </telerik:RadComboBox>
                            <asp:RequiredFieldValidator ID="rfvDockType" runat="server" ControlToValidate="rcbDockType"
                                Display="Dynamic" ErrorMessage="Select type" ValidationGroup="DockData" />
                        </td>
                    </tr>
                       <tr id="trDockControlURL" runat="server">
                        <td>
                            Control URL <span class="required">*</span>
                        </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtDockControlURL" runat="server" Width="245" Skin="Default" MaxLength="250" />
                            <asp:RequiredFieldValidator ID="rfvDockControlURL" runat="server" ControlToValidate="txtDockControlURL"
                                Display="Dynamic" ErrorMessage="Enter URL" ValidationGroup="DockData" />
                        </td>
                    </tr>

                        <asp:Panel ID="pnlSelOver" runat="server">
                                               <tr>
                        <td>
                            Summary Link Text
                        </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtSummaryLinkText" runat="server" Width="245" Skin="Default" MaxLength="50" />
                        </td>
                    </tr>
                                               <tr>
                        <td>
                            Summary Link URL
                        </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtSummaryLinkURL" runat="server" Width="245" Skin="Default" MaxLength="250" />
                        </td>
                    </tr>
                                                                        <tr>
                            <td>
                                Is Mode Manager Visible
                            </td>
                            <td>
                                  <asp:CheckBox ID="chkModeManVis" runat="server"  />
                            </td>
                        </tr>
                                                                        <tr>
                            <td>
                                Allow Group By Change
                            </td>
                            <td>
                                  <asp:CheckBox ID="chkAllowGrpBy" runat="server"  />
                            </td>
                        </tr>
                                                                        <tr>
                            <td>
                                Manage Portal Dock Filter
                            </td>
                            <td>
                                  <asp:CheckBox ID="chkMngFilter" runat="server"  />
                            </td>
                        </tr>
                                                                        <tr>
                            <td>
                                Is Summary Link Visible
                            </td>
                            <td>
                                  <asp:CheckBox ID="chkSummLinkVis" runat="server"  />
                            </td>
                        </tr>
                                                                        <tr>
                            <td>
                                Show Excel Export
                            </td>
                            <td>
                                  <asp:CheckBox ID="chkShowExcel" runat="server"  />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Show More Options
                            </td>
                            <td>
                                  <asp:CheckBox ID="chkMoreOptions" runat="server"  />
                            </td>
                        </tr>
                                                                        <tr>
                            <td>
                                Show Select Lists
                            </td>
                            <td>
                                  <asp:CheckBox ID="chkShowSelLists" runat="server"  />
                            </td>
                        </tr>
                        </asp:Panel>
                        <asp:Panel ID="pnlAnalysisChart" runat="server">
                                                                                                         <tr>
                        <td>
                            X Coordinate Column Name
                                                    </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtXColName" runat="server" Width="245" Skin="Default" MaxLength="50" />
                                                        <asp:RequiredFieldValidator ID="rfvXColName" runat="server" ControlToValidate="txtXColName"
                                Display="Dynamic" ErrorMessage="Enter name" ValidationGroup="DockData" />
                        </td>
                    </tr>
                                                                                                     <tr>
                        <td>
                            Y Coordinate Column Name
                                                    </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtYColName" runat="server" Width="245" Skin="Default" MaxLength="50" />
                                                        <asp:RequiredFieldValidator ID="rfvYColName" runat="server" ControlToValidate="txtYColName"
                                Display="Dynamic" ErrorMessage="Enter name" ValidationGroup="DockData" />
                        </td>
                    </tr>
                                                                                                                         <tr>
                        <td>
                            Secondary Y Coordinate Column Name
                                                    </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtSecYColName" runat="server" Width="245" Skin="Default" MaxLength="50" />
                        </td>
                    </tr>
                        
                                                                                                                                             <tr>
                        <td>
                            Series Dock Code
                                                    </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtSeriesDockCode" runat="server" Width="245" Skin="Default" MaxLength="50" />
                                                        <asp:RequiredFieldValidator ID="rfvSeriesDockCode" runat="server" ControlToValidate="txtSeriesDockCode"
                                Display="Dynamic" ErrorMessage="Enter code" ValidationGroup="DockData" />
                        </td>
                    </tr>
                                                                                                                                                                 <tr>
                        <td>
                            Secondary Series Dock Code
                                                    </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtSecSeriesDockCode" runat="server" Width="245" Skin="Default" MaxLength="50" />
                        </td>
                    </tr>
                        
                                                                 <tr>
                        <td>
                            Y Axis Format
                        </td>
                        <td align="left">
                            <telerik:RadComboBox ID="rcbYAxisFormat" runat="server" Width="250" Skin="Default">
                                <Items>
                                    <telerik:RadComboBoxItem Text="" Value="" />
                                    <telerik:RadComboBoxItem Text="Numeric" Value="0" />
                                    <telerik:RadComboBoxItem Text="Percentage" Value="1" />
                                    </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                                                             <tr>
                        <td>
                            Secondary Y Axis Format
                        </td>
                        <td align="left">
                            <telerik:RadComboBox ID="rcbSecYAxisFormat" runat="server" Width="250" Skin="Default">
                                <Items>
                                    <telerik:RadComboBoxItem Text="" Value="" />
                                    <telerik:RadComboBoxItem Text="Numeric" Value="0" />
                                    <telerik:RadComboBoxItem Text="Percentage" Value="1" />
                                    </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>

                                                                                                     <tr>
                        <td>
                            Chart Type
                        </td>
                        <td align="left">
                            <telerik:RadComboBox ID="rcbChartType" runat="server" Width="250" Skin="Default">
                                <Items>
                                    <telerik:RadComboBoxItem Text="" Value="" />
                                    <telerik:RadComboBoxItem Text="Bar" Value="1" />
                                    <telerik:RadComboBoxItem Text="Line" Value="4" />
                                    </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>

                                                                                                     <tr>
                        <td>
                            Secondary Chart Type
                        </td>
                        <td align="left">
                            <telerik:RadComboBox ID="rcbSecChartType" runat="server" Width="250" Skin="Default">
                                <Items>
                                    <telerik:RadComboBoxItem Text="" Value="" />
                                    <telerik:RadComboBoxItem Text="Bar" Value="1" />
                                    <telerik:RadComboBoxItem Text="Line" Value="4" />
                                    </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                                                                                                          <tr>
                        <td>
                            Series Name
                                                    </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtSeriesName" runat="server" Width="245" Skin="Default" MaxLength="50" />

                        </td>
                    </tr>
                                                                                                                              <tr>
                        <td>
                            Secondary Series Name
                                                    </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtSecSeriesName" runat="server" Width="245" Skin="Default" MaxLength="50" />
                        </td>
                    </tr>
                                                                                                                                                  <tr>
                        <td>
                            Group By Column Name
                                                    </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtGropuByCol" runat="server" Width="245" Skin="Default" MaxLength="50" />
                        </td>
                    </tr>
                                                                                                                                                                      <tr>
                        <td>
                            Secondary Group By Column Name
                                                    </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtSecGropuByCol" runat="server" Width="245" Skin="Default" MaxLength="50" />
                        </td>
                    </tr>




                                              <tr>
                        <td>
                            Y Axis Max Value
                                                    </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtYAxisMax" runat="server" Width="245" Skin="Default" />
                            <asp:CompareValidator ID="cmpYAxisMax" runat="server" ErrorMessage="Number required" ControlToValidate="txtYAxisMax" ValidationGroup="DockData"
                             Display="Dynamic" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                        </td>
                    </tr>
                                                                  <tr>
                        <td>
                            Secondary Y Axis Max Value
                                                    </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtSecYAxisMax" runat="server" Width="245" Skin="Default" />
                            <asp:CompareValidator ID="cmpSecYAxisMax" runat="server" ErrorMessage="Number required" ControlToValidate="txtSecYAxisMax" ValidationGroup="DockData"
                             Display="Dynamic" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                        </td>
                    </tr>
                                                                  <tr>
                        <td>
                            Y Axis Min Value
                                                    </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtYAxisMin" runat="server" Width="245" Skin="Default" />
                            <asp:CompareValidator ID="cmpYAxisMin" runat="server" ErrorMessage="Number required" ControlToValidate="txtYAxisMin" ValidationGroup="DockData"
                             Display="Dynamic" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                        </td>
                    </tr>
                                                                  <tr>
                        <td>
                            Secondary Y Axis Min Value
                                                    </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtSecYAxisMin" runat="server" Width="245" Skin="Default" />
                            <asp:CompareValidator ID="cmpSecYAxisMin" runat="server" ErrorMessage="Number required" ControlToValidate="txtSecYAxisMin" ValidationGroup="DockData"
                             Display="Dynamic" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                        </td>
                    </tr>
                                                                                                          <tr>
                        <td>
                            Series Color
                                                    </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtSeriesColor" runat="server" Width="245" Skin="Default" MaxLength="50" />
                        </td>
                    </tr>
                                                                                      <tr>
                        <td>
                            Secondary Series Color
                                                    </td>
                        <td align="left">
                            <telerik:RadTextBox ID="txtSecSeriesColor" runat="server" Width="245" Skin="Default" MaxLength="50" />
                        </td>
                    </tr>
                        </asp:Panel>
                                                <tr>
                            <td>
                                Is Visible
                            </td>
                            <td>
                                  <asp:CheckBox ID="chkDockIsVisible" runat="server"  />
                            </td>
                        </tr>
                                                <tr>
                            <td>
                                Is Expanded
                            </td>
                            <td>
                                  <asp:CheckBox ID="chkDockIsExpanded" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnCancelDock" runat="server" Text="Cancel" OnClick="btnCancelDock_Click"
                                    CausesValidation="False" Width="60" />
                                &nbsp;
                                <asp:Button ID="btnSaveDock" Text="Save" runat="server" CausesValidation="true" OnClick="btnSaveDock_Click"
                                    ValidationGroup="DockData" Width="60" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:RadWindow>
   <%-- Changed the Filter Button to show ucFilterManagerTreeList--%>
    <uc:FilterManager ID="ucDockFilterManager" runat="server" FilterType="Dock" Visible="false" />
    
    <uc:FilterManagerTreeList ID="ucFilterManagerTreeList" runat="server" filtertype="Dock" Visible="false" />
    <telerik:RadWindow ID="rwAddEditDockTerm" runat="server" ShowContentDuringLoad="False"
        Title=" - Dock Term" OnClientClose="ClickDockTermCancel" DestroyOnClose="false"
        Width="750px" Height="550px" Behaviors="Close, Move" VisibleOnPageLoad="false">
        <ContentTemplate>
            <telerik:RadAjaxPanel runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" IsSticky="true" runat="server"
                    Transparency="20">
                </telerik:RadAjaxLoadingPanel>
                <asp:Label ID="lblFormError" runat="server" ForeColor="Red" />
                <table style="margin: 5px 0 5px 5px">
                    <tr>
                        <td valign="top" width="100">
                            Term Config<span class="required">*</span>
                        </td>
                        <td align="left">
                            <telerik:RadComboBox ID="rcbTermConfig" runat="server" Width="250" DataTextField="TermConfigDesc"
                                DataValueField="TermConfigPK" AutoPostBack="true" OnSelectedIndexChanged="rcbTermConfig_SelectedIndexChanged"
                                ShowMoreResultsBox="true" EnableVirtualScrolling="true" Filter="StartsWith" MarkFirstMatch="True"
                                EnableItemCaching="True" />
                            <asp:RequiredFieldValidator ID="rfvTermConfig" runat="server" ControlToValidate="rcbTermConfig"
                                Display="Dynamic" ErrorMessage="Select term config" ValidationGroup="DockTermData" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <telerik:RadDockLayout ID="rdlTermConfigDetails" runat="server">
                                <telerik:RadDockZone ID="rdzTermConfigDetails" runat="server" Orientation="Vertical" CssClass="zone"
                                    BorderStyle="None" FitDocks="true">
                                    <telerik:RadDock ID="rdTermDetails" runat="server" Title=" - Term Config Details" DefaultCommands="ExpandCollapse"
                                        Skin="Web20" Width="100%" Collapsed="true">
                                        <ContentTemplate>
                                            <asp:Literal runat="server" ID="litTermDetails" Mode="Transform" /> 
                                        </ContentTemplate>  
                                    </telerik:RadDock>
                                </telerik:RadDockZone>      
                            </telerik:RadDockLayout>   
                        </td>
                    </tr>            
                    <tr>
                        <td>
                            Term Ops <span class="required">*</span>
                        </td>
                        <td align="left">
                            <telerik:RadComboBox ID="rcbTermOps" runat="server" Width="250" Skin="Default" DataTextField="TermOpsDesc"
                                DataValueField="TermOpsPK" AutoPostBack="true" OnSelectedIndexChanged="rcbTermOps_SelectedIndexChanged" />
                            <asp:RequiredFieldValidator ID="rfvTermOps" runat="server" ControlToValidate="rcbTermOps"
                                Display="Dynamic" ErrorMessage="Select term ops" ValidationGroup="DockTermData" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Caption <span class="required">*</span>
                        </td>
                        <td align="left">
                            <telerik:RadTextBox ID="rtbCaption" runat="server" Width="245" Skin="Default" MaxLength="255" />
                            <asp:RequiredFieldValidator ID="rfvCaption" runat="server" ControlToValidate="rtbCaption"
                                Display="Dynamic" ErrorMessage="Enter caption" ValidationGroup="DockTermData" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Mode Enabled <span class="required">*</span>
                        </td>
                        <td align="left">
                            <telerik:RadComboBox ID="rcbModeEnabled" runat="server" Width="250" Skin="Default">
                                <Items>
                                    <telerik:RadComboBoxItem Text="" Value="" />
                                    <telerik:RadComboBoxItem Text="Preview" Value="1" />
                                    <telerik:RadComboBoxItem Text="Summary" Value="2" />
                                    <telerik:RadComboBoxItem Text="Details" Value="3" />
                                </Items>
                            </telerik:RadComboBox>
                            <asp:RequiredFieldValidator ID="rfvModeEnabled" runat="server" ControlToValidate="rcbModeEnabled"
                                Display="Dynamic" ErrorMessage="Select mode" ValidationGroup="DockTermData" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Format
                        </td>
                        <td align="left">
                            <telerik:RadComboBox ID="rcbFormatString" runat="server" Width="250" Skin="Default">
                                <Items>
                                    <telerik:RadComboBoxItem Text="" Value="" />
                                    <telerik:RadComboBoxItem Text="{0:MM/dd/yyyy}" Value="{0:MM/dd/yyyy}" />
                                    <telerik:RadComboBoxItem Text="{0:N0} - ex. 0" Value="{0:N0}" />
                                    <telerik:RadComboBoxItem Text="{0:N1} - ex. 0.0" Value="{0:N1}" />
                                    <telerik:RadComboBoxItem Text="{0:N2} - ex. 0.00" Value="{0:N2}" />
                                    <telerik:RadComboBoxItem Text="{0:P0} - ex. 0%" Value="{0:P0}" />
                                    <telerik:RadComboBoxItem Text="{0:P1} - ex. 0.0%" Value="{0:P1}" />
                                    <telerik:RadComboBoxItem Text="{0:P2} - ex. 0.00%" Value="{0:P2}" />
                                    <telerik:RadComboBoxItem Text="{0:C0} - ex. $0" Value="{0:C0}" />
                                    <telerik:RadComboBoxItem Text="{0:C1} - ex. $0.0" Value="{0:C1}" />
                                    <telerik:RadComboBoxItem Text="{0:C2} - ex. $0.00" Value="{0:C2}" />
                                    <telerik:RadComboBoxItem Text="{0:C3} - ex. $0.000" Value="{0:C3}" />
                                    <telerik:RadComboBoxItem Text="{0:C4} - ex. $0.0000" Value="{0:C4}" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Order By
                        </td>
                        <td align="left">
                            <telerik:RadComboBox ID="rcbOrderBy" runat="server" Width="250" Skin="Default">
                                <Items>
                                    <telerik:RadComboBoxItem Text="" Value="0" />
                                    <telerik:RadComboBoxItem Text="ACS" Value="1" />
                                    <telerik:RadComboBoxItem Text="DESC" Value="2" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Group <span class="required">*</span>
                        </td>
                        <td>
                            <telerik:RadNumericTextBox ID="txtGroup" runat="server" MinValue="1" MaxValue="20"
                                NumberFormat-DecimalDigits="0" Width="50" />
                            <asp:RequiredFieldValidator ID="rfvGroup" runat="server" ControlToValidate="txtGroup"
                                Display="Dynamic" ErrorMessage="Enter group number" ValidationGroup="DockTermData" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Is Enabled
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="chkIsEnabled" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Is Required
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="chkIsRequired" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Is Visible
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="chkIsVisible" runat="server" />
                        </td>
                    </tr>
                </table>
            </telerik:RadAjaxPanel>
            <asp:HiddenField ID="hfIsInsert" runat="server" />
            <asp:HiddenField ID="hfDockTermsPK" runat="server" />
            <asp:Button ID="btnCancelDockTerm" runat="server" Text="Cancel" OnClick="btnCancelDockTerm_Click"
                CausesValidation="False" Width="60" />
            &nbsp;
            <asp:Button ID="btnSaveDockTerm" Text="Save" runat="server" CausesValidation="true"
                OnClick="btnSaveDockTerm_Click" ValidationGroup="DockTermData" Width="60" />
        </ContentTemplate>
    </telerik:RadWindow>

    <script type="text/javascript" language="javascript">
        function ClickDockCancel(oWnd, args) {
            var rwAddEditDock = $find("<%=rwAddEditDock.ClientID%>");
            rwAddEditDock.close();

            var btnCancel = document.getElementById("<%=btnCancelDock.ClientID%>");
            btnCancel.click();
        }

        function ClickPortalCancel(oWnd, args) {
            var rwAddEditPortal = $find("<%=rwAddEditPortal.ClientID%>");
            rwAddEditPortal.close();

            var btnCancel = document.getElementById("<%=btnCancelPortal.ClientID%>");
            btnCancel.click();
        }

        function ClickDockTermCancel(oWnd, args) {
            var rwAddEditDockTerm = $find("<%=rwAddEditDockTerm.ClientID%>");
            rwAddEditDockTerm.close();

            var btnCancel = document.getElementById("<%=btnCancelDockTerm.ClientID%>");
            btnCancel.click();
        }               
    </script>

</asp:Content>
