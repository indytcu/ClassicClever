using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CleverDAO;
using CleverDAO.DockSetTableAdapters;
using CleverUI.BusinessLogic.TCLCleverBL;
using Telerik.Web.UI;
using CleverUI.InterfaceLogic;
using CleverUI.AppCode.BusinessLogic.TCLCleverBL;
using System.Web.Security;
using System.Text;

namespace CleverUI.Admin
{
    public partial class MaintDocks : System.Web.UI.Page
    {
        private const string DynamicPortalURL = "~/User/DynamicPortal.aspx";

        #region *** Control Properties *********

        public byte? SelectedPortalID
        {
            get
            {
                byte? val = null;
                if (ViewState["SelectedPortalID"] != null)
                {
                    val = (byte)ViewState["SelectedPortalID"];
                }
                return val;
            }
            set
            {
                ViewState["SelectedPortalID"] = value;
            }
        }

        public short? SelectedDockID
        {
            get
            {
                short? val = null;
                if (ViewState["SelectedDockID"] != null)
                {
                    val = Convert.ToInt16(ViewState["SelectedDockID"]);
                }
                return val;
            }
            set
            {
                ViewState["SelectedDockID"] = value;
            }
        }

        #endregion *** Control Properties ******

        /// <summary>
        /// Init the page on first load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Nevena Uzunova</author>
        /// <date>11/09/2010</date>
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible        = false;
            lblErrorTermsSection.Visible = false;
            lblDockFormError.Visible= false;
            lblFormError.Visible    = false;
            lblErrorPortal.Visible = false;

            if (!IsPostBack)
            {
                //Collapse dock terms (dock is not selected and that's why terms are not displayed) 
                rdDockTerms.Visible = false;
                rdDock.Visible = false;
            }

          
        }

        protected void rgPortals_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SelectedPortalID = Convert.ToByte(rgPortals.SelectedValue);

            rgDock.Rebind();
            this.SelectedDockID = null;
            
            string portCode = rgPortals.MasterTableView.DataKeyValues[rgPortals.SelectedItems[0].ItemIndex]["PortalCode"].ToString();
            string portDesc = rgPortals.MasterTableView.DataKeyValues[rgPortals.SelectedItems[0].ItemIndex]["PortalDesc"].ToString();

            rdDock.Title = "Docks (" + portCode + " - " + portDesc + ")";
            rdDockTerms.Visible = false;
            rdDock.Visible = true;
            rdDock.Collapsed = false;
            rdPortals.Collapsed = true;
        }

        /// <summary>
        /// Display docks of the currently selected portal.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        /// <author>Nevena Uzunova</author>
        /// <date>11/09/2010</date>
        protected void rgDock_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            rgDock.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.Top;
            rgDock.MasterTableView.NoMasterRecordsText = "No docks to display.";
            try
            {
                rgDock.DataSource = Dock.List(this.SelectedPortalID.Value);
                rgDock.Visible = true;
            }
            catch
            {
                lblError.Text = "Error retrieving docks.";
                lblError.Visible = true;
                rgDock.Visible = false;
            }
        }

        protected void rgPortals_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            rgPortals.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.Top;
            rgPortals.MasterTableView.NoMasterRecordsText = "No portals to display.";
            try
            {
                DataTable portals = Portal.ListAllForAdmin();
                portals.DefaultView.Sort = "PortalUIOrder";
                rgPortals.DataSource = portals.DefaultView;
                rgPortals.Visible = true;
            }
            catch
            {
                lblErrorPortal.Text = "Error retrieving portals.";
                lblErrorPortal.Visible = true;
                rgPortals.Visible = false;
            }
        }

        /// <summary>
        /// Populates dock terms when dock is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Nevena Uzunova</author>
        /// <date>11/09/2010</date>
        protected void rgDock_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SelectedDockID = Convert.ToInt16(rgDock.SelectedValue);
            rgDockTerms.Rebind();
            rdDockTerms.Collapsed = false;
            rdDockTerms.Visible = true;
            string dockCode = rgDock.MasterTableView.DataKeyValues[rgDock.SelectedItems[0].ItemIndex]["DockCode"].ToString();
            string dockDesc = rgDock.MasterTableView.DataKeyValues[rgDock.SelectedItems[0].ItemIndex]["DockDesc"].ToString();

            rdDockTerms.Title = "Dock Terms (" + dockCode + " - " + dockDesc + ")";
            rdDock.Collapsed = true;
        }

        protected void rgPortals_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.EditCommandName)
            {
                rwAddEditPortal.Visible = true;
                rwAddEditPortal.VisibleOnPageLoad = true;
            }
            else 
            {
                byte otherPortalPK;
                if (e.CommandName == "MoveUp")
                {
                    otherPortalPK = (byte)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex - 1]["PortalPK"];
                }
                else if (e.CommandName == "MoveDown")
                {
                    otherPortalPK = (byte)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex + 1]["PortalPK"];
                }
                else
                {
                    return;
                }

                byte portalPK = (byte)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PortalPK"];

                try
                {
                    Portal.SwapPortalsUIOrder(otherPortalPK, portalPK);
                }
                catch (Exception ex)
                {
                    lblErrorPortal.Text = "Error changing UI Order. Error details: " + ex.Message;
                    lblErrorPortal.Visible = true;
                    return;
                }

                CleverUI.StateManagers.SessionManager.ClearPortals();

                Response.Redirect("~/Admin/MaintDocks.aspx");
            }

        }

        /// <summary>
        /// Handles Add/Edit on new Dock
        /// </summary>
        /// <author>Mario Berberyan</author>
        /// <date>01/17/2011</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgDock_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.EditCommandName)
            {
                rwAddEditDock.Title = "Update Dock";
                rwAddEditDock.Visible = true;
                rwAddEditDock.VisibleOnPageLoad = true;
            }
            else if (e.CommandName == "DockFilter")
            {
                int filterID = Convert.ToInt32(rgDock.MasterTableView.DataKeyValues[e.Item.ItemIndex]["appFilterFK"]);
                //this.ucDockFilterManager.SelectedFilterID = filterID;
                //this.ucDockFilterManager.Visible = true;
                //this.ucDockFilterManager.ShowPopup();

                this.ucFilterManagerTreeList.SelectedFilterID = filterID;
                this.ucFilterManagerTreeList.Visible = true;
                this.ucFilterManagerTreeList.ShowPopup();
            }
        }

        /// <summary>
        /// Init the edit form of the dock grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Mario Berberyan</author>
        /// <date>01/18/2011</date>
        protected void rgDock_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.EditFormItem && e.Item.IsInEditMode)
            {
                //**** Update ******************
                DataRowView item = (DataRowView)e.Item.DataItem;
                hfPortalDockPK.Value = item["PortalDockPK"].ToString();
                chkDockIsExpanded.Checked = (bool)item["IsExpanded"];
                chkDockIsVisible.Checked = (bool)item["IsVisible"];
                lblDockCode.Text = item["DockCode"].ToString();
                txtDockDesc.Text = item["DockDesc"].ToString();

                trDockTitle.Visible = false;
                trDockMode.Visible = false;
                trDockOrder.Visible = false;
                trDockZone.Visible = false;
                trDockControlURL.Visible = false;
                pnlAnalysisChart.Visible = false;
                pnlSelOver.Visible = false;
                trDockType.Visible = false;

                DataTable tbl = Portal.GetPortalByPK(Convert.ToInt16(this.SelectedPortalID));
                if (tbl != null && (string)tbl.Rows[0]["NavigateURL"] == DynamicPortalURL)
                {
                    trDockMode.Visible = true;
                    trDockOrder.Visible = true;
                    trDockZone.Visible = true;
                    trDockType.Visible = true;
                    trDockTitle.Visible = true;

                    if (item["DockTitle"] != DBNull.Value)
                        txtDockTitle.Text = item["DockTitle"].ToString();
                    else
                        txtDockTitle.Text = string.Empty;

                    if (item["DockMode"] != DBNull.Value)
                        rcbDockMode.SelectedValue = item["DockMode"].ToString();
                    else
                        rcbDockMode.SelectedIndex = -1;

                    if (item["DockZone"] != DBNull.Value)
                        rcbDockZone.SelectedValue = item["DockZone"].ToString();
                    else
                        rcbDockZone.SelectedIndex = -1;

                    if (item["DockOrder"] != DBNull.Value)
                        txtDockOrder.Text = item["DockOrder"].ToString();
                    else
                        txtDockOrder.Text = string.Empty;

                    if (item["ControlURL"] != DBNull.Value)
                        txtDockControlURL.Text = (string)item["ControlURL"];
                    else
                        txtDockControlURL.Text = string.Empty;

                    if (item["ModeManagerVisible"] != DBNull.Value)
                        chkModeManVis.Checked = (bool)item["ModeManagerVisible"];
                    else
                        chkModeManVis.Checked = false;

                    if (item["AllowGroupByChange"] != DBNull.Value)
                        chkAllowGrpBy.Checked = (bool)item["AllowGroupByChange"];
                    else
                        chkAllowGrpBy.Checked = false;

                    if (item["ManagePortalDockFilter"] != DBNull.Value)
                        chkMngFilter.Checked = (bool)item["ManagePortalDockFilter"];
                    else
                        chkMngFilter.Checked = false;

                    if (item["SummaryLinkVisible"] != DBNull.Value)
                        chkSummLinkVis.Checked = (bool)item["SummaryLinkVisible"];
                    else
                        chkSummLinkVis.Checked = false;

                    if (item["ShowExcelExport"] != DBNull.Value)
                        chkShowExcel.Checked = (bool)item["ShowExcelExport"];
                    else
                        chkShowExcel.Checked = false;

                    if (item["MoreOptionsVisible"] != DBNull.Value)
                        chkMoreOptions.Checked = (bool)item["MoreOptionsVisible"];
                    else
                        chkMoreOptions.Checked = false;

                    if (item["ShowSelectLists"] != DBNull.Value)
                        chkShowSelLists.Checked = (bool)item["ShowSelectLists"];
                    else
                        chkShowSelLists.Checked = false;

                    if (item["SummaryLinkText"] != DBNull.Value)
                        txtSummaryLinkText.Text = (string)item["SummaryLinkText"];
                    else
                        txtSummaryLinkText.Text = string.Empty;

                    if (item["SummaryLinkURL"] != DBNull.Value)
                        txtSummaryLinkURL.Text = (string)item["SummaryLinkURL"];
                    else
                        txtSummaryLinkURL.Text = string.Empty;

                    if (item["YAxisMaxValue"] != DBNull.Value)
                        txtYAxisMax.Text = item["YAxisMaxValue"].ToString();
                    else
                        txtYAxisMax.Text = string.Empty;

                    if (item["SecondaryYAxisMaxValue"] != DBNull.Value)
                        txtSecYAxisMax.Text = item["SecondaryYAxisMaxValue"].ToString();
                    else
                        txtSecYAxisMax.Text = string.Empty;

                    if (item["YAxisMinValue"] != DBNull.Value)
                        txtYAxisMin.Text = item["YAxisMinValue"].ToString();
                    else
                        txtYAxisMin.Text = string.Empty;

                    if (item["SecondaryYAxisMinValue"] != DBNull.Value)
                        txtSecYAxisMin.Text = item["SecondaryYAxisMinValue"].ToString();
                    else
                        txtSecYAxisMin.Text = string.Empty;

                    if (item["SecondarySeriesColor"] != DBNull.Value)
                        txtSecSeriesColor.Text = (string)item["SecondarySeriesColor"];
                    else
                        txtSecSeriesColor.Text = string.Empty;

                    if (item["SeriesColor"] != DBNull.Value)
                        txtSeriesColor.Text = (string)item["SeriesColor"];
                    else
                        txtSeriesColor.Text = string.Empty;

                    if (item["YAxisFormat"] != DBNull.Value)
                        rcbYAxisFormat.SelectedValue = item["YAxisFormat"].ToString();
                    else
                        rcbYAxisFormat.SelectedIndex = -1;

                    if (item["SecondaryYAxisFormat"] != DBNull.Value)
                        rcbSecYAxisFormat.SelectedValue = item["SecondaryYAxisFormat"].ToString();
                    else
                        rcbSecYAxisFormat.SelectedIndex = -1;

                    if (item["SeriesName"] != DBNull.Value)
                        txtSeriesName.Text = (string)item["SeriesName"];
                    else
                        txtSeriesName.Text = string.Empty;

                    if (item["SecondarySeriesName"] != DBNull.Value)
                        txtSecSeriesName.Text = (string)item["SecondarySeriesName"];
                    else
                        txtSecSeriesName.Text = string.Empty;

                    if (item["GroupByColumnName"] != DBNull.Value)
                        txtGropuByCol.Text = (string)item["GroupByColumnName"];
                    else
                        txtGropuByCol.Text = string.Empty;

                    if (item["SecondaryGroupByColumnName"] != DBNull.Value)
                        txtSecGropuByCol.Text = (string)item["SecondaryGroupByColumnName"];
                    else
                        txtSecGropuByCol.Text = string.Empty;

                    if (item["XCoordColumnName"] != DBNull.Value)
                        txtXColName.Text = (string)item["XCoordColumnName"];
                    else
                        txtXColName.Text = string.Empty;

                    if (item["YCoordColumnName"] != DBNull.Value)
                        txtYColName.Text = (string)item["YCoordColumnName"];
                    else
                        txtYColName.Text = string.Empty;

                    if (item["SecondaryYCoordColumnName"] != DBNull.Value)
                        txtSecYColName.Text = (string)item["SecondaryYCoordColumnName"];
                    else
                        txtSecYColName.Text = string.Empty;

                    if (item["SecondaryChartType"] != DBNull.Value)
                        rcbSecChartType.SelectedValue = item["SecondaryChartType"].ToString();
                    else
                        rcbSecChartType.SelectedIndex = -1;

                    if (item["ChartType"] != DBNull.Value)
                        rcbChartType.SelectedValue = item["ChartType"].ToString();
                    else
                        rcbChartType.SelectedIndex = -1;

                    if (item["SeriesDockCode"] != DBNull.Value)
                        txtSeriesDockCode.Text = (string)item["SeriesDockCode"];
                    else
                        txtSeriesDockCode.Text = string.Empty;

                    if (item["SecondarySeriesDockCode"] != DBNull.Value)
                        txtSecSeriesDockCode.Text = (string)item["SecondarySeriesDockCode"];
                    else
                        txtSecSeriesDockCode.Text = string.Empty;

                    if (item["DockType"] != DBNull.Value)
                    {
                        rcbDockType.SelectedValue = item["DockType"].ToString();

                        if ((DockTypes)item["DockType"] == DockTypes.UserControl || (DockTypes)item["DockType"] == DockTypes.Chart)
                            trDockControlURL.Visible = true;
                        else if ((DockTypes)item["DockType"] == DockTypes.SelectiveDockOverviewGrid)
                            pnlSelOver.Visible = true;
                        else if ((DockTypes)item["DockType"] == DockTypes.AnalysisChart)
                            pnlAnalysisChart.Visible = true;

                    }
                    else
                    {
                        rcbDockType.SelectedIndex = -1;
                    }
                }


                //hide edit radgrid form
                e.Item.Edit = false;
            }
            else if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                DataRowView item = (DataRowView)e.Item.DataItem;
                
                string strMode = "-";
                if (item["DockMode"] != DBNull.Value)
                    strMode = ((DockControlMode)Convert.ToInt16(item["DockMode"])).ToString();
                
                ((Literal)e.Item.FindControl("lblDockMode")).Text = strMode;

                string strZone = "-";
                if (item["DockZone"] != DBNull.Value)
                    strZone = ((DockZones)Convert.ToInt16(item["DockZone"])).ToString();

                ((Literal)e.Item.FindControl("lblDockZone")).Text = strZone;
            }
        }

        protected void rgPortals_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.EditFormItem  && e.Item.IsInEditMode)
            {
                //**** Update ******************
                DataRowView item = (DataRowView)e.Item.DataItem;
                txtPortalCode.Text = (string)item["PortalCode"];
                txtPortalDesc.Text = (string)item["PortalDesc"];
                txtNavigateURL.Text = item["NavigateURL"].ToString();
                chkPortalIsVisible.Checked = (bool)item["IsVisible"];
                if (txtNavigateURL.Text == DynamicPortalURL)
                {
                    txtNavigateURL.Enabled = false;
                    chkPortalIsDynamic.Checked = true;
                }
                else
                {
                    txtNavigateURL.Enabled = true;
                    chkPortalIsDynamic.Checked = false;
                }
                hfPortalPK.Value = item["PortalPK"].ToString();

				// set portal roles
				cxblRoles.DataSource = Roles.GetAllRoles();
				cxblRoles.DataBind();
				string roles = item["Roles"].ToString();
				if (!string.IsNullOrEmpty(roles))
				{
					string[] portalRoles = (roles).Split(',');
					foreach (string role in Roles.GetAllRoles())
					{
						if (portalRoles.Contains(role))
						{
							cxblRoles.Items.FindByText(role).Selected = true;
						}
					}
				}

                //hide edit radgrid form
                e.Item.Edit = false;
            }
            else if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                if (e.Item.ItemIndex == 0)
                {
                    ((GridDataItem)e.Item)["MoveUp"].Controls[0].Visible = false;
                }
                else if (e.Item.ItemIndex == ((DataView)e.Item.OwnerTableView.DataSource).Count - 1)
                {
                    ((GridDataItem)e.Item)["MoveDown"].Controls[0].Visible = false;
                }
            }
        }


        /// <summary>
        /// Handles the Insert command and create a new dock.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Mario Berberyan</author>
        /// <date>01/18/2011</date>
        protected void btnSaveDock_Click(object sender, EventArgs e)
        {
                #region Update Portal Dock

                //save comment
                try
                {
                    byte? dockMode = null;
                    byte? dockZone = null;
                    short? dispOrder = null;
                    byte? DockType = null;

                    string ControlURL = null;
                    bool? ModeManagerVisible = null;
                    bool? AllowGroupByChange = null;
                    bool? ManagePortalDockFilter = null;
                    bool? SummaryLinkVisible = null;
                    string SummaryLinkText = null;
                    string SummaryLinkURL = null;
                    bool? ShowExcelExport = null;
                    bool? MoreOptionsVisible = null;
                    bool? ShowSelectLists = null;
                    double? YAxisMaxValue = null;
                    double? SecondaryYAxisMaxValue = null;
                    double? YAxisMinValue = null;
                    double? SecondaryYAxisMinValue = null;
                    string SecondarySeriesColor = null;
                    string SeriesColor = null;
                    short? YAxisFormat = null;
                    short? SecondaryYAxisFormat = null;
                    string SeriesName = null;
                    string SecondarySeriesName = null;
                    string GroupByColumnName = null;
                    string SecondaryGroupByColumnName = null;
                    string XCoordColumnName = null;
                    string YCoordColumnName = null;
                    string SecondaryYCoordColumnName = null;
                    short? SecondaryChartType = null;
                    short? ChartType = null;
                    string SeriesDockCode = null;
                    string SecondarySeriesDockCode = null;

                    if (trDockMode.Visible)
                        dockMode = Convert.ToByte(rcbDockMode.SelectedValue);
                    if (trDockZone.Visible)
                        dockZone = Convert.ToByte(rcbDockZone.SelectedValue);
                    if (trDockOrder.Visible)
                        dispOrder = Convert.ToInt16(txtDockOrder.Text);

                    if (trDockType.Visible)
                    {
                        DockType = Convert.ToByte(rcbDockType.SelectedValue);

                        if (trDockControlURL.Visible)
                        {
                            ControlURL = txtDockControlURL.Text.Trim();
                        }
                        else if (pnlAnalysisChart.Visible)
                        {
                            if (!string.IsNullOrEmpty(txtYAxisMax.Text))
                                YAxisMaxValue = double.Parse(txtYAxisMax.Text);
                            if (!string.IsNullOrEmpty(txtSecYAxisMax.Text))
                                SecondaryYAxisMaxValue = double.Parse(txtSecYAxisMax.Text);
                            if (!string.IsNullOrEmpty(txtYAxisMin.Text))
                                YAxisMinValue = double.Parse(txtYAxisMin.Text);
                            if (!string.IsNullOrEmpty(txtSecYAxisMin.Text))
                                SecondaryYAxisMinValue = double.Parse(txtSecYAxisMin.Text);
                            SecondarySeriesColor = txtSecSeriesColor.Text.Trim();
                            SeriesColor = txtSeriesColor.Text.Trim();
                            if (rcbYAxisFormat.SelectedIndex > 0)
                                YAxisFormat = short.Parse(rcbYAxisFormat.SelectedValue);
                            if (rcbSecYAxisFormat.SelectedIndex > 0)
                                SecondaryYAxisFormat = short.Parse(rcbSecYAxisFormat.SelectedValue);
                            SeriesName = txtSeriesName.Text.Trim();
                            SecondarySeriesName = txtSecSeriesName.Text.Trim();
                            GroupByColumnName = txtGropuByCol.Text.Trim();
                            SecondaryGroupByColumnName = txtSecGropuByCol.Text.Trim();
                            XCoordColumnName = txtXColName.Text.Trim();
                            YCoordColumnName = txtYColName.Text.Trim();
                            SecondaryYCoordColumnName = txtSecYColName.Text.Trim();
                            if (rcbSecChartType.SelectedIndex > 0)
                                SecondaryChartType = short.Parse(rcbSecChartType.SelectedValue);
                            if (rcbChartType.SelectedIndex > 0)
                                ChartType = short.Parse(rcbChartType.SelectedValue);
                            SeriesDockCode = txtSeriesDockCode.Text.Trim();
                            SecondarySeriesDockCode = txtSecSeriesDockCode.Text.Trim();
                        }
                        else if (pnlSelOver.Visible)
                        {
                            ModeManagerVisible = chkModeManVis.Checked;
                            AllowGroupByChange = chkAllowGrpBy.Checked;
                            ManagePortalDockFilter = chkMngFilter.Checked;
                            SummaryLinkVisible = chkSummLinkVis.Checked;
                            SummaryLinkText = txtSummaryLinkText.Text.Trim();
                            SummaryLinkURL = txtSummaryLinkURL.Text.Trim();
                            ShowExcelExport = chkShowExcel.Checked;
                            MoreOptionsVisible = chkMoreOptions.Checked;
                            ShowSelectLists = chkShowSelLists.Checked;
                        }
                    }
                    
                    PortalDock.UpdatePortalDock(short.Parse(hfPortalDockPK.Value), chkDockIsVisible.Checked, chkDockIsExpanded.Checked, txtDockDesc.Text.Trim(), dockMode, 
                        dockZone, dispOrder, txtDockTitle.Text.Trim(), DockType, ControlURL,  ModeManagerVisible,  AllowGroupByChange,  ManagePortalDockFilter,  SummaryLinkVisible,
                        SummaryLinkText, SummaryLinkURL,  ShowExcelExport, MoreOptionsVisible,  ShowSelectLists,  YAxisMaxValue,  SecondaryYAxisMaxValue,
                         YAxisMinValue,  SecondaryYAxisMinValue,  SecondarySeriesColor,  SeriesColor,  YAxisFormat,  SecondaryYAxisFormat,
                         SeriesName,  SecondarySeriesName,  GroupByColumnName,  SecondaryGroupByColumnName,  XCoordColumnName,  YCoordColumnName,
                         SecondaryYCoordColumnName,  SecondaryChartType,  ChartType,  SeriesDockCode,  SecondarySeriesDockCode);

                    rgDock.Rebind();

                    rwAddEditDock.Visible = false;
                    rwAddEditDock.VisibleOnPageLoad = false;
                }
                catch (Exception ex)
                {
                    Label lblDockFormError = (Label)rwAddEditDock.ContentContainer.FindControl("lblDockFormError");
                    lblDockFormError.Text = "Error occured while trying to update the Dock. Error message: " + ex.Message;
                    lblDockFormError.Visible = true;

                    rwAddEditDock.Visible = true;
                    rwAddEditDock.VisibleOnPageLoad = true;

                    return;
                }

                rgDock.MasterTableView.ClearEditItems();
                rgDock.Rebind();

                rwAddEditDock.Visible = false;
                rwAddEditDock.VisibleOnPageLoad = false;

                #endregion Update Dock
        }

        protected void btnSavePortal_Click(object sender, EventArgs e)
        {
            try
            {
				StringBuilder RolesXml = new StringBuilder("<Roles>");
				foreach (System.Web.UI.WebControls.ListItem Item in cxblRoles.Items)
				{
					if (Item.Selected)
					{
						RolesXml.Append("<Role>" + Item.Text + "</Role>");
					}
				}
				RolesXml.Append("</Roles>");
				Portal.UpdatePortal(Convert.ToByte(hfPortalPK.Value), txtPortalCode.Text.Trim(), txtPortalDesc.Text.Trim(), chkPortalIsVisible.Checked, txtNavigateURL.Text.Trim(), RolesXml);
                rgPortals.Rebind();

                CleverUI.StateManagers.SessionManager.ClearPortals();

                Response.Redirect("~/Admin/MaintDocks.aspx");
            }
            catch (Exception ex)
            {
                lblPortalFormError.Text = "Error occured while trying to update the Portal. Error message: " + ex.Message;
                lblPortalFormError.Visible = true;

                return;
            }
        }

        /// <summary>
        /// Closes insert popup screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Mario Berberyan</author>
        /// <date>01/18/2011</date>
        protected void btnCancelDock_Click(object sender, EventArgs e)
        {
            rwAddEditDock.Visible = false;
            rwAddEditDock.VisibleOnPageLoad = false;
            //rgDock.Rebind();
        }

        protected void btnCancelPortal_Click(object sender, EventArgs e)
        {
            rwAddEditPortal.Visible = false;
            rwAddEditPortal.VisibleOnPageLoad = false;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblDockMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            rgDockTerms.Rebind();
        }

        /// <summary>
        /// Display dock terms for the currently selected dock.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        /// <author>Nevena Uzunova</author>
        /// <date>11/09/2010</date>
        protected void rgDockTerms_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            bool isOverviewMode = Convert.ToBoolean(Convert.ToByte(rblDockMode.SelectedValue));
            try
            {
                rgDockTerms.DataSource = DockTerm.ListByDockMode(this.SelectedDockID.Value, isOverviewMode);
                rgDockTerms.Visible = true;
            }
            catch
            {
                lblErrorTermsSection.Text = "Error retrieving dock terms.";
                lblErrorTermsSection.Visible = true;
                rgDockTerms.Visible = false;
            }
        }

        /// <summary>
        /// Handles add/edit on new Dock Term
        /// </summary>
        /// <author>Mario Berberyan</author>
        /// <date>11/11/2010</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgDockTerms_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.EditCommandName)
            {
                hfIsInsert.Value = "False";
                lblFormError.Visible = false;
                rwAddEditDockTerm.Visible = true;
                rwAddEditDockTerm.VisibleOnPageLoad = true;
            }
            else if (e.CommandName == RadGrid.InitInsertCommandName)
            {
                hfIsInsert.Value = "True";
                lblFormError.Visible = false;
                rwAddEditDockTerm.Visible = true;
                rwAddEditDockTerm.VisibleOnPageLoad = true;
            }
            else if (e.CommandName == "MoveUp")
            {
                short dockTermPK = Convert.ToInt16(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["DockTermsPK"]);
                short dockTermGroup = Convert.ToInt16(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TermGroup"]);
                short dockTermOrder = Convert.ToInt16(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TermOrder"]);

                short dockTermPrevPK = Convert.ToInt16(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex-1]["DockTermsPK"]);
                short dockTermPrevGroup = Convert.ToInt16(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex-1]["TermGroup"]);
                short dockTermPrevOrder = Convert.ToInt16(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex-1]["TermOrder"]);

                if (dockTermGroup != dockTermPrevGroup)
                {
                    //Move the selected term in previous group at last place 
                    short newTermOrder = (short)(dockTermPrevOrder + 1);
                    try
                    {
                        CleverUI.BusinessLogic.TCLCleverBL.DockTerm.ChangeOrder(dockTermPK, dockTermPrevGroup, newTermOrder);
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "Error moving up. Error details: " + ex.Message;
                        lblError.Visible = true;
                        return;
                    }
                }
                else
                {
                   //Change places of the selected term with the previous one.
                    try
                    {
                        CleverUI.BusinessLogic.TCLCleverBL.DockTerm.ChangeOrder(dockTermPK, dockTermGroup, dockTermPrevOrder);
                        CleverUI.BusinessLogic.TCLCleverBL.DockTerm.ChangeOrder(dockTermPrevPK, dockTermGroup, dockTermOrder);
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "Error moving up. Error details: " + ex.Message;
                        lblError.Visible = true;
                        return;
                    }
                }
               
                e.Item.OwnerTableView.Rebind();
            }
            else if (e.CommandName == "MoveDown")
            {
                short dockTermPK = Convert.ToInt16(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["DockTermsPK"]);
                short dockTermGroup = Convert.ToInt16(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TermGroup"]);
                short dockTermOrder = Convert.ToInt16(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TermOrder"]);

                short dockTermNextPK = Convert.ToInt16(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex + 1]["DockTermsPK"]);
                short dockTermNextGroup = Convert.ToInt16(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex + 1]["TermGroup"]);
                short dockTermNextOrder = Convert.ToInt16(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex + 1]["TermOrder"]);

                if (dockTermGroup != dockTermNextGroup)
                {
                    //Move the selected term in next group at last place
                    try
                    {
                        CleverUI.BusinessLogic.TCLCleverBL.DockTerm.ChangeOrder(dockTermPK, dockTermNextGroup, dockTermNextOrder);
                        CleverUI.BusinessLogic.TCLCleverBL.DockTerm.IncrementNextTermsOrder(dockTermPK);
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "Error moving down. Error details: " + ex.Message;
                        lblError.Visible = true;
                        return;
                    }
                }
                else
                {
                    //Change places of the selected term with the next one.
                    try
                    {
                        CleverUI.BusinessLogic.TCLCleverBL.DockTerm.ChangeOrder(dockTermPK, dockTermGroup, dockTermNextOrder);
                        CleverUI.BusinessLogic.TCLCleverBL.DockTerm.ChangeOrder(dockTermNextPK, dockTermGroup, dockTermOrder);
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "Error moving down. Error details: " + ex.Message;
                        lblError.Visible = true;
                        return;
                    }
                }

                e.Item.OwnerTableView.Rebind();
            }
        }

        /// <summary>
        /// Init the edit form of the dock terms grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Nevena Uzunova</author>
        /// <date>11/09/2010</date>
        protected void rgDockTerms_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                DataRowView dateItem = (DataRowView)e.Item.DataItem;

                //Manage visibility of MoveUp and MoveDown buttons:
                // - MoveUp on the first row is not visible
                // - MoveUp not visible on the first term in group because moving to another group is not allowed
                // - MoveDown on the last row is not visible
                // - MoveDown not visible on the last term in group 

                short currentTermGroup = Convert.ToInt16(dateItem["TermGroup"]);
                if (e.Item.ItemIndex == 0)
                {
                    ((GridDataItem)e.Item)["MoveUp"].Controls[0].Visible = false; 
                }
                else if (e.Item.ItemIndex == ((DataTable)e.Item.OwnerTableView.DataSource).Rows.Count - 1)
                {
                    ((GridDataItem)e.Item)["MoveDown"].Controls[0].Visible = false;
                }


                if (e.Item.ItemIndex != 0)
                {
                    short prevTermGroup = Convert.ToInt16(((DataRowView)(e.Item.OwnerTableView.Items[e.Item.ItemIndex - 1].DataItem))["TermGroup"]);
                    if (prevTermGroup != currentTermGroup)
                    {
                        ((GridDataItem)e.Item)["MoveUp"].Controls[0].Visible = false; 
                    }

                    if (e.Item.ItemIndex != ((DataTable)e.Item.OwnerTableView.DataSource).Rows.Count - 1)
                    {
                        if (prevTermGroup != currentTermGroup)
                        {
                            e.Item.OwnerTableView.Items[e.Item.ItemIndex - 1]["MoveDown"].Controls[0].Visible = false;
                        }
                    }
                }  
                

                string strMode = "-";
                if (dateItem["ModeEnabled"] != DBNull.Value)
                {
                    strMode = ((DockControlMode)Convert.ToInt16(dateItem["ModeEnabled"])).ToString();
                }
                
                ((Literal)e.Item.FindControl("lblModeEnabled")).Text = strMode;

                string strOrderBy = "-";
                if (dateItem["OrderBy"] != DBNull.Value)
                {
                    byte orderByIndex = Convert.ToByte(dateItem["OrderBy"]);
                    if (orderByIndex == 1)
                    {
                        strOrderBy = "ASC";
                    }
                    else if (orderByIndex == 2)
                    {
                        strOrderBy = "DESC";
                    }
                }
                ((Literal)e.Item.FindControl("lblOrderBy")).Text = strOrderBy;
            }
            else if (e.Item.ItemType == GridItemType.EditFormItem
             && e.Item.IsInEditMode)
            {
                //Init Insert/Edit Form

                //bind terms
                int filterID = Dock.GetDefaultFilter(this.SelectedDockID.Value);
                rcbTermConfig.DataSource = BusinessLogic.TCLCleverBL.TermConfig.ListByFilter(filterID, null); //Term.List(this.SelectedDockID.Value);
                rcbTermConfig.DataBind();
                rcbTermConfig.Items.Insert(0, new RadComboBoxItem());


                //bind term ops
                DataTable dtTermOps = Term.ListTermOps();
                DataView dwTermOps  = new DataView(dtTermOps);

                if (rblDockMode.SelectedValue == "1") //Overview dock mode
                {
                    dwTermOps.RowFilter = "TermOpsCode <> '0' and TermOpsCode <> 'Detail'"; //Exlude "Unassigned"
                }
                else //Detail dock mode
                {
                    dwTermOps.RowFilter = "TermOpsCode = 'Detail' or TermOpsCode = 'GroupBy1'"; //Exlude "Unassigned"
                }

               
                rcbTermOps.DataSource = dwTermOps;
                rcbTermOps.DataBind();
                rcbTermOps.Items.Insert(0, new RadComboBoxItem());
                
                if (e.Item.OwnerTableView.IsItemInserted)
                {
                    //**** Insert ******************
                    rcbTermConfig.Enabled = true;
                    rfvTermConfig.Enabled = true;

                    //Init form
                    hfDockTermsPK.Value = "";
                    rcbTermConfig.SelectedValue = "";
                    rcbTermOps.SelectedValue = "";
                    litTermDetails.Text = "";
                    //rcbJoinPath.SelectedValue = "";
                    rcbOrderBy.SelectedValue = "";
                    rcbModeEnabled.SelectedValue = "";
                    rcbFormatString.SelectedValue = "";
                    rtbCaption.Text = "";
                    chkIsEnabled.Checked = false;
                    chkIsRequired.Checked = false;
                    chkIsVisible.Checked = false;

                    //Hide insert radgrid form
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.Visible = false;
                    
                }
                else
                {
                    //**** Update ******************
                    rcbTermConfig.Enabled = false;
                    rfvTermConfig.Enabled = false;

                    //Display current settings in the form
                    DataRowView item = (DataRowView)e.Item.DataItem;
                    hfDockTermsPK.Value = item["DockTermsPK"].ToString();
                    rcbTermConfig.SelectedValue = item["TermConfigFK"].ToString();
                    //get join statements
                    int selectedTermConfig = Convert.ToInt32(rcbTermConfig.SelectedValue);
                    DataTable dtTermConfigs = BusinessLogic.TCLCleverBL.TermConfig.List(selectedTermConfig, null);
                    if (dtTermConfigs.Rows.Count > 0)
                    {
                        litTermDetails.Text = "<table cellspacing='5' cellpadding='0'>" +
                                                    "<tr>" +
                                                        "<td><strong>Term Code: </strong></td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["TermCode"].ToString() + "</td>" +
                                                    "</tr><tr>" +
                                                        "<td><strong>Term Column Name: </strong></td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["TermColumnName"].ToString() + "</td>" +
                                                    "</tr><tr>" +
                                                        "<td><strong>Term Summary Column Name: </strong></td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["TermSummaryColumnName"].ToString() + "</td>" +
                                                    "</tr><tr>" +
                                                        "<td><strong>Term Heading: </strong></td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["TermHeading"].ToString() + "</td>" +
                                                    "</tr><tr>" +
                                                        "<td><strong>Term Caption: </strong></td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["TermCaption"].ToString() + "</td>" +
                                                    "</tr><tr>" +
                                                        "<td><strong>Term Numeric: </strong></td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["TermIsNumeric"].ToString() + "</td>" +
                                                    "</tr><tr>" +
                                                        "<td><strong>Term Default: </strong></td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["IsTermDefault"].ToString() + "</td>" +
                                                    "</tr><tr>" +
                                                        "<td><strong>Select String: </strong></td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["SelectString"].ToString() + "</td>" +
                                                    "</tr><tr>" +
                                                        "<td><strong>Select Summary String: </strong></td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["SelectSummaryString"].ToString() + "</td>" +
                                                    "</tr></table>" +

                                                "<table cellpadding=\"0\" cellspacing=\"0\" class=\"tableBlackWhite\">" +
                                                    "<thead>" +
                                                    "<tr>" +
                                                        "<td>#</td>" +
                                                        "<td>Table</td>" +
                                                        "<td>Join Path Code</td>" +
                                                        "<td>Default</td>" +
                                                        "<td>Join statement</td>" +
                                                    "</tr>" +
                                                    "</thead>" +
                                                    "<tr>" +
                                                        "<td>1</td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["TableDesc"].ToString() + "</td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["TablesJoinPathCode"].ToString() + "</td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["IsDefault"].ToString() + "</td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["JoinStatement"].ToString() + "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td>2</td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["Table2Desc"].ToString() + "</td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["Table2JoinPathCode"].ToString() + "</td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["Is2Default"].ToString() + "</td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["JoinStatement2"].ToString() + "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td>3</td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["Table3Desc"].ToString() + "</td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["Table3JoinPathCode"].ToString() + "</td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["Is3Default"].ToString() + "</td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["JoinStatement3"].ToString() + "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td>4</td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["Table4Desc"].ToString() + "</td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["Table4JoinPathCode"].ToString() + "</td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["Is4Default"].ToString() + "</td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["JoinStatement4"].ToString() + "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td>5</td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["Table5Desc"].ToString() + "</td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["Table5JoinPathCode"].ToString() + "</td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["Is5Default"].ToString() + "</td>" +
                                                        "<td>" + dtTermConfigs.Rows[0]["JoinStatement5"].ToString() + "</td>" +
                                                    "</tr>" +
                                                "</table>";
                    }
                    rcbTermOps.SelectedValue = item["TermOpsFK"].ToString();
                    rtbCaption.Text = item["TermCaption"].ToString();
                    if (item["ModeEnabled"] == DBNull.Value)
                    {
                        rcbModeEnabled.SelectedValue = "";
                    }
                    else
                    {
                        rcbModeEnabled.SelectedValue = item["ModeEnabled"].ToString();
                    }
                    rcbFormatString.SelectedValue = item["TermFormatString"].ToString();
                    rcbOrderBy.SelectedValue = item["OrderBy"]!=null ? item["OrderBy"].ToString() : "0";
                    chkIsEnabled.Checked  = Convert.ToBoolean(item["TermIsEnabled"] != System.DBNull.Value ? item["TermIsEnabled"] : null);
                    chkIsRequired.Checked = Convert.ToBoolean(item["IsRequired"]    != System.DBNull.Value ? item["IsRequired"]    : null);
                    chkIsVisible.Checked  = Convert.ToBoolean(item["TermIsVisible"] != System.DBNull.Value ? item["TermIsVisible"] : null);
                    txtGroup.Value = Convert.ToInt16(item["TermGroup"]);

                    //hide edit radgrid form
                    e.Item.Edit = false;
                }               
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgDockTerms_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            short dockTermsPK = Convert.ToInt16(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["DockTermsPK"]);
            try
            {
                CleverUI.BusinessLogic.TCLCleverBL.DockTerm.Delete(dockTermsPK);
            }
            catch (Exception ex)
            {
                lblErrorTermsSection.Text = "Error occured while trying to delete the term from the dock. Error details: " + ex.Message;
                lblErrorTermsSection.Visible = true;
                e.Canceled = true;
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rcbTermConfig_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //get join statements
            int selectedTermConfig = Convert.ToInt32(rcbTermConfig.SelectedValue);
            DataTable dtTermConfigs = BusinessLogic.TCLCleverBL.TermConfig.List(selectedTermConfig, null);
            if (dtTermConfigs.Rows.Count > 0)
            {
                if (rtbCaption.Text.Trim() == String.Empty)
                {
                    rtbCaption.Text = dtTermConfigs.Rows[0]["TermCaption"].ToString();
                }
                litTermDetails.Text = "<table cellspacing='5' cellpadding='0'>" +
                                            "<tr>" +
                                                "<td><strong>Term Code: </strong></td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["TermCode"].ToString() + "</td>" +
                                            "</tr><tr>" +
                                                "<td><strong>Term Column Name: </strong></td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["TermColumnName"].ToString() + "</td>" +
                                            "</tr><tr>" +
                                                "<td><strong>Term Summary Column Name: </strong></td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["TermSummaryColumnName"].ToString() + "</td>" +
                                            "</tr><tr>" +
                                                "<td><strong>Term Heading: </strong></td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["TermHeading"].ToString() + "</td>" +
                                            "</tr><tr>" +
                                                "<td><strong>Term Caption: </strong></td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["TermCaption"].ToString() + "</td>" +
                                            "</tr><tr>" +
                                                "<td><strong>Term Numeric: </strong></td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["TermIsNumeric"].ToString() + "</td>" +
                                            "</tr><tr>" +
                                                "<td><strong>Term Default: </strong></td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["IsTermDefault"].ToString() + "</td>" +
                                            "</tr><tr>" +
                                                "<td><strong>Select String: </strong></td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["SelectString"].ToString() + "</td>" +
                                            "</tr><tr>" +
                                                "<td><strong>Select Summary String: </strong></td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["SelectSummaryString"].ToString() + "</td>" +
                                            "</tr></table>" +

                                        "<table cellpadding=\"0\" cellspacing=\"0\" class=\"tableBlackWhite\">" +
                                            "<thead>" +
                                            "<tr>" +
                                                "<td>#</td>" +
                                                "<td>Table</td>" +
                                                "<td>Join Path Code</td>" +
                                                "<td>Default</td>" +
                                                "<td>Join statement</td>" +
                                            "</tr>" +
                                            "</thead>" +
                                            "<tr>" +
                                                "<td>1</td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["TableDesc"].ToString() + "</td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["TablesJoinPathCode"].ToString() + "</td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["IsDefault"].ToString() + "</td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["JoinStatement"].ToString() + "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>2</td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["Table2Desc"].ToString() + "</td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["Table2JoinPathCode"].ToString() + "</td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["Is2Default"].ToString() + "</td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["JoinStatement2"].ToString() + "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>3</td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["Table3Desc"].ToString() + "</td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["Table3JoinPathCode"].ToString() + "</td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["Is3Default"].ToString() + "</td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["JoinStatement3"].ToString() + "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>4</td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["Table4Desc"].ToString() + "</td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["Table4JoinPathCode"].ToString() + "</td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["Is4Default"].ToString() + "</td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["JoinStatement4"].ToString() + "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>5</td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["Table5Desc"].ToString() + "</td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["Table5JoinPathCode"].ToString() + "</td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["Is5Default"].ToString() + "</td>" +
                                                "<td>" + dtTermConfigs.Rows[0]["JoinStatement5"].ToString() + "</td>" +
                                            "</tr>" +
                                        "</table>";
            }
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rcbTermOps_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            short termOpsPK = Convert.ToInt16(e.Value);

            DataTable dtTermOps = Term.ListTermOps();
            DataRow[] drs = dtTermOps.Select("TermOpsPK = " + termOpsPK);

            if (drs.Length > 0)
            {
                string termOpsCode = drs[0]["TermOpsCode"].ToString();
                if (termOpsCode == "GroupBy1")
                {
                    txtGroup.Value = 1;
                }
                else if (termOpsCode == "Calc")
                {
                    txtGroup.Value = 3;
                }
                else
                {
                    bool isSummaryTerm = false;
                    if (!drs[0].IsNull("IsSummary"))
                    {
                        isSummaryTerm = Convert.ToBoolean(drs[0]["IsSummary"]);
                    }

                    if (isSummaryTerm)
                    {
                        txtGroup.Value = 3;
                    }
                    else
                    {
                        txtGroup.Value = 2;
                    }
                }

            }
        }

        /// <summary>
        /// Handles the Insert command and create a new dock term.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Nevena Uzunova</author>
        /// <date>09/20/2010</date>
        protected void btnSaveDockTerm_Click(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(hfIsInsert.Value))
            {
                #region Create Dock Term

                DockTerm newDockTerm = new DockTerm();
                newDockTerm.DockFK = this.SelectedDockID.Value;
                newDockTerm.TermConfigFK = Convert.ToInt16(rcbTermConfig.SelectedValue);                
                newDockTerm.TermOpsFK = Convert.ToInt16(rcbTermOps.SelectedValue);
                newDockTerm.TermCaption = rtbCaption.Text;
                newDockTerm.TermGroup = Convert.ToInt16(txtGroup.Value);
                
                if (rcbModeEnabled.SelectedValue != String.Empty)
                {
                    newDockTerm.ModeEnabled = Convert.ToInt16(rcbModeEnabled.SelectedValue);
                }
                string formatString = rcbFormatString.SelectedValue;
                if (formatString != String.Empty)
                {
                    newDockTerm.TermFormatString = formatString;
                }
                if (rcbOrderBy.SelectedValue != String.Empty)
                {
                    newDockTerm.OrderBy = Convert.ToByte(rcbOrderBy.SelectedValue);
                }

                //Flags
                newDockTerm.IsRequired = chkIsRequired.Checked;
                newDockTerm.TermIsEnabled = chkIsEnabled.Checked;
                newDockTerm.TermIsVisible = chkIsVisible.Checked;

                //save comment
                try
                {
                    DockTerm.Save(newDockTerm);
                    rgDockTerms.Rebind();

                    rwAddEditDockTerm.Visible = false;
                    rwAddEditDockTerm.VisibleOnPageLoad = false;
                }
                catch (Exception ex)
                {                    
                    Label lblFormError = (Label)rwAddEditDockTerm.ContentContainer.FindControl("lblFormError");
                    lblFormError.Text = "Error adding term to dock. Error details: " + ex.Message;
                    lblFormError.Visible = true;

                    rwAddEditDockTerm.Visible = true;
                    rwAddEditDockTerm.VisibleOnPageLoad = true;

                    return;
                }

                #endregion
            }
            else
            {
                #region Update Dock Term
                
                // update DockTerm info
                DockTerm dockTerm = new DockTerm();
                dockTerm.DockFK = this.SelectedDockID.Value;
                dockTerm.DockTermsPK = Convert.ToInt16(hfDockTermsPK.Value);                                                                    
                dockTerm.TermConfigFK = Convert.ToInt16(rcbTermConfig.SelectedValue);
                dockTerm.TermOpsFK = Convert.ToInt16(rcbTermOps.SelectedValue);
                dockTerm.TermCaption = rtbCaption.Text.Trim();            

                if (rcbModeEnabled.SelectedValue != String.Empty)
                {
                    dockTerm.ModeEnabled = Convert.ToInt16(rcbModeEnabled.SelectedValue);
                }
                else
                {
                    dockTerm.ModeEnabled = null;

                }
                string formatString = rcbFormatString.SelectedValue;
                if (formatString != String.Empty)
                {
                    dockTerm.TermFormatString = formatString;
                }
                if (rcbOrderBy.SelectedValue != String.Empty)
                {
                    dockTerm.OrderBy = Convert.ToByte(rcbOrderBy.SelectedValue);
                }

                //Flags
                dockTerm.TermIsEnabled = chkIsEnabled.Checked;
                dockTerm.IsRequired = chkIsRequired.Checked;
                dockTerm.TermIsVisible = chkIsVisible.Checked;
                dockTerm.TermGroup = Convert.ToInt16(txtGroup.Value);
                // update dock term
                try
                {
                    DockTerm.Save(dockTerm);
                    rgDockTerms.MasterTableView.ClearEditItems();
                    rgDockTerms.Rebind();

                    rwAddEditDockTerm.Visible = false;
                    rwAddEditDockTerm.VisibleOnPageLoad = false;
                }
                catch
                {                        
                    lblFormError.Text = "Error occured while trying to update the Dock Terms.";
                    lblFormError.Visible = true;

                    rwAddEditDockTerm.Visible = true;
                    rwAddEditDockTerm.VisibleOnPageLoad = true;

                    return;
                }                

                rgDockTerms.MasterTableView.ClearEditItems();
                rgDockTerms.Rebind();

                rwAddEditDockTerm.Visible = false;
                rwAddEditDockTerm.VisibleOnPageLoad = false;
                                
                #endregion
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Nevena Uzunova</author>
        /// <date>09/20/2010</date>
        protected void btnCancelDockTerm_Click(object sender, EventArgs e)
        {                        
            rwAddEditDockTerm.Visible = false;
            rwAddEditDockTerm.VisibleOnPageLoad = false;           
        }

        protected void chkPortalIsDynamic_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPortalIsDynamic.Checked)
            {
                txtNavigateURL.Text = DynamicPortalURL;
                txtNavigateURL.Enabled = false;
            }
            else
            {
                txtNavigateURL.Text = string.Empty;
                txtNavigateURL.Enabled = true;
            }
        }

        protected void rcbDockType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            trDockControlURL.Visible = false;
            pnlAnalysisChart.Visible = false;
            pnlSelOver.Visible = false;

            if (rcbDockType.SelectedIndex > 0)
            {
                DockTypes typ = (DockTypes)Convert.ToByte(rcbDockType.SelectedValue);

                if (typ == DockTypes.UserControl || typ == DockTypes.Chart)
                {
                    trDockControlURL.Visible = true;
                }
                else if (typ == DockTypes.SelectiveDockOverviewGrid)
                {
                    pnlSelOver.Visible = true;
                }
                else if (typ == DockTypes.AnalysisChart)
                {
                    pnlAnalysisChart.Visible = true;
                }
            }
        }
    }
}
