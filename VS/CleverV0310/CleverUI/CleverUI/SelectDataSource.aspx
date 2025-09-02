<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectDataSource.aspx.cs"
    Inherits="CleverUI.SelectDataSource"  MasterPageFile="~/GlobalAdmin.master" Title=""%>

<asp:Content ID="cAdminPage" ContentPlaceHolderID="cphAdminPage" runat="server">
    <div class="source_box" style="text-align: left">
        <div>
            <h2 class="title" style="padding: 0px; margin: 20px 0px 10px 0px;">
                Select a database:</h2>
            <div style="text-align: left; padding-bottom: 20px;">
                <asp:Label ID="lblErrorMessage" runat="server" CssClass="error" />
                <telerik:RadGrid ID="rgDataSources" runat="server" AutoGenerateColumns="false" Skin="Default"
                    OnItemCreated="rgDataSources_ItemCreated" OnItemDataBound="rgDataSources_ItemDataBound"
                    OnNeedDataSource="rgDataSources_NeedDataSource">
                    <MasterTableView NoMasterRecordsText="No data sources assigned to currently logged user"
                        DataKeyNames="CleverDataSourcePK,Title" CommandItemDisplay="Top">
                        <Columns>
                            <telerik:GridTemplateColumn UniqueName="Selected" HeaderText="Select">
                                <ItemTemplate>
                                    <asp:RadioButton ID="rbSelected" runat="server" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="Title" HeaderText="Title" />
                            <telerik:GridBoundColumn DataField="CleverDataSourceTypeDesc" HeaderText="Type" />
                            <telerik:GridBoundColumn DataField="Description" HeaderText="Description" />
                        </Columns>
                        <CommandItemSettings ShowAddNewRecordButton="false" />
                    </MasterTableView>
                </telerik:RadGrid>
                <script type="text/javascript">
                    function SelectRadioButtonInGrid(gridName, radio, index) {
                        var grid = document.getElementById(gridName);
                        checked = radio.checked;

                        var inputs = grid.getElementsByTagName("input");
                        for (i = 0; i < inputs.length; i++) {

                            if (inputs[i].type == "radio") {
                                inputs[i].checked = false;
                            }
                        }

                        radio.checked = checked;
                    } 
                </script>
            </div>
            <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" CausesValidation="true"
                ValidationGroup="DBConnection" Width="60" />
        </div>
    </div>
</asp:Content>
