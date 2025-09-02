<%@ Page Title="Clever - Admin - Mange Help Documents" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="HelpDocs.aspx.cs" Inherits="CleverUI.Admin.HelpDocs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphAdminPage" runat="server">
    <telerik:RadDockLayout ID="RadDockLayout1" runat="server">
        <telerik:RadDockZone ID="rdzHelpTypes" runat="server" Orientation="Vertical" BorderStyle="None">
                <telerik:RadDock ID="rdHelpTypes" runat="server" Title=" - Help Categories" DefaultCommands="ExpandCollapse"
                Skin="Sunset" Width="100%">
                <ContentTemplate>
                    <asp:Label ID="lblErrorHelpTypes" runat="server" Visible="false" CssClass="error" />
                    <telerik:RadGrid ID="rgHelpTypes" runat="server" GridLines="None"
                        AllowAutomaticInserts="false" AllowAutomaticUpdates="false" 
                        OnNeedDataSource="rgHelpTypes_NeedDataSource" 
                        OnSelectedIndexChanged="rgHelpTypes_SelectedIndexChanged" Skin="Default" >
                        <MasterTableView DataKeyNames="HelpTypePK,HelpTypeDesc"  AutoGenerateColumns="False">
                            <CommandItemSettings ShowAddNewRecordButton="false" />
                            <RowIndicatorColumn>
                                <HeaderStyle Width="20px" />
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn>
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridBoundColumn DataField="HelpTypeCode" HeaderText="Code" UniqueName="HelpTypeCode" HeaderStyle-Width="200" ItemStyle-Width="200">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="HelpTypeDesc" HeaderText="Description" UniqueName="HelpTypeDesc" >
                                </telerik:GridBoundColumn>

                            </Columns>
                        </MasterTableView>
                        <ClientSettings EnablePostBackOnRowClick="True">
                            <Selecting AllowRowSelect="True" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </ContentTemplate>
            </telerik:RadDock>
                      <telerik:RadDock ID="rdHelpDocs" runat="server" Title=" - Help Documents" DefaultCommands="ExpandCollapse"
                Skin="Web20" Width="100%">
                <ContentTemplate>
                    <asp:Label ID="lblErrorHelpDocs" runat="server" Visible="false" CssClass="error" />
                    <telerik:RadGrid ID="rgHelpDocs" runat="server" AutoGenerateColumns="False" GridLines="None"
                        AllowPaging="false" AllowSorting="false" AllowAutomaticInserts="false"
                        AllowAutomaticUpdates="false" OnNeedDataSource="rgHelpDocs_NeedDataSource" OnItemDataBound="rgHelpDocs_ItemDataBound"
                        OnItemCommand="rgHelpDocs_ItemCommand" Skin="Default" OnDeleteCommand="rgHelpDocs_DeleteCommand">
                        <MasterTableView DataKeyNames="HelpDocPK,HelpDocUIOrder,HelpDocDesc,HelpTypeFK,IsVisible" CommandItemDisplay="Top" NoMasterRecordsText="No help documents found.">
                            <CommandItemSettings AddNewRecordText="Add new help document" />
                            <RowIndicatorColumn>
                                <HeaderStyle Width="20px" />
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn>
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridEditCommandColumn ButtonType="ImageButton" HeaderStyle-Width="15" />
                                <telerik:GridButtonColumn CommandName="Delete" ButtonType="ImageButton" ConfirmDialogType="RadWindow"
                                    ConfirmText="Are you sure you want to delete this help document?" HeaderStyle-Width="15" />
                                <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/App_Themes/Default/images/up.png"
                                    UniqueName="MoveUp" CommandName="MoveUp" Text="Move Up" HeaderStyle-Width="15" />
                                <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/App_Themes/Default/images/down.png"
                                    UniqueName="MoveDown" CommandName="MoveDown" Text="Move Down" HeaderStyle-Width="15" />
                                <telerik:GridBoundColumn DataField="HelpDocUIOrder" HeaderText="Order" 
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                    UniqueName="HelpDocUIOrder" HeaderStyle-Width="50">
                                </telerik:GridBoundColumn>
                                <telerik:GridButtonColumn CommandName="View" ButtonType="PushButton" Text="View" HeaderStyle-Width="15" />
                                <telerik:GridCheckBoxColumn DataField="IsVisible" HeaderText="Visible" UniqueName="IsVisible"  HeaderStyle-Width="50">
                                </telerik:GridCheckBoxColumn>
                                <telerik:GridBoundColumn DataField="HelpDocDesc" HeaderText="Description" UniqueName="HelpDocDesc" ItemStyle-Wrap="false" HeaderStyle-Width="250">
                                </telerik:GridBoundColumn>

                                 <telerik:GridBoundColumn DataField="DateUpdated" HeaderText="Last Updated" UniqueName="DateUpdated"  HeaderStyle-Width="150">
                                </telerik:GridBoundColumn>
                                 <telerik:GridBoundColumn DataField="UpdatedUserName" HeaderText="Last Updated By" UniqueName="UpdatedUserName">
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
            <telerik:RadWindow ID="rwAddEditDoc" runat="server" ShowContentDuringLoad="False"
        OnClientClose="ClickDocCancel" DestroyOnClose="false" Width="400px"
        Height="250px" Behaviors="Close,Move" VisibleOnPageLoad="false" Title=" - Update Help Document">
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnCancelDoc" />
                    <asp:PostBackTrigger ControlID="btnSaveDoc" />
                </Triggers>
                <ContentTemplate>
                    <asp:Label ID="lblDocFormError" runat="server" ForeColor="Red" />
                    <table class="term_form">
                        <tr>
                            <td>
                                Description <span class="validator">*</span>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtDocDesc" runat="server" MaxLength="255" Width="200" />
                                                                <div>
                                    <asp:RequiredFieldValidator ID="rfvDocDesc" runat="server" ControlToValidate="txtDocDesc"
                                        Display="Dynamic" ErrorMessage="Enter Description" ValidationGroup="DocData" />
                                </div>
                            </td>
                        </tr>
                       <tr>
                        <td>
                            Category <span class="required">*</span>
                        </td>
                        <td align="left">
                            <telerik:RadComboBox ID="rcbType" runat="server" Width="250" Skin="Default" DataTextField="HelpTypeDesc"
                                DataValueField="HelpTypePK"  />
                                <div>
                            <asp:RequiredFieldValidator ID="rfvType" runat="server" ControlToValidate="rcbType"
                                Display="Dynamic" ErrorMessage="Select Category" ValidationGroup="DocData" />
                                </div>
                        </td>
                    </tr>
                    <tr><td><asp:Label ID="lblDoc" runat="server"></asp:Label>
                     </td>
                        <td align="left">
                                        <telerik:RadUpload ID="RadUpload1" runat="server" OverwriteExistingFiles="true" ControlObjectsVisibility="None" AllowedFileExtensions=".pdf" Width="250"></telerik:RadUpload>
                                        <div>
                                        <asp:Label ID="lblUploadErr" runat="server"  ForeColor="Red"></asp:Label>
                                        </div>
                  
                    </td></tr>
                     <tr>
                        <td>
                            Is Visible
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="chkDocIsVisible" runat="server" />
                        </td>
                    </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnCancelDoc" runat="server" Text="Cancel" OnClick="btnCancelDoc_Click"
                                    CausesValidation="False" Width="60" />
                                &nbsp;
                                <asp:Button ID="btnSaveDoc" Text="Save" runat="server" CausesValidation="true" OnClick="btnSaveDoc_Click"
                                    ValidationGroup="DocData" Width="60" />
                            </td>
                        </tr>
                        
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:RadWindow>

        <script type="text/javascript" language="javascript">
            function ClickDocCancel(oWnd, args) {
                var rwAddEditDoc = $find("<%=rwAddEditDoc.ClientID%>");
                rwAddEditDoc.close();

                var btnCancel = document.getElementById("<%=btnCancelDoc.ClientID%>");
                btnCancel.click();
            }             
    </script>
</asp:Content>
