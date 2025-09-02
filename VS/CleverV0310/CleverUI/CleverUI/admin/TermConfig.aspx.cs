using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using CleverUI.BusinessLogic.TCLCleverBL;
using System.Data.SqlClient;

namespace CleverUI.Admin
{
    public partial class TermConfig : System.Web.UI.Page
    {
        public int? SearchTermId
        {
            get 
            {
                if (ViewState["SearchTermId"] != null)
                {
                    return Convert.ToInt32(ViewState["SearchTermId"]);
                }
                return null;
            }
            set { ViewState["SearchTermId"] = value; }
        }

        public int? SearchGroupPK
        {
            get
            {
                if (ViewState["SearchGroupPK"] != null)
                {
                    return Convert.ToInt32(ViewState["SearchGroupPK"]);
                }
                return null;
            }
            set { ViewState["SearchGroupPK"] = value; }
        }

        public string SearchTermCode
        {
            get
            {
                if (ViewState["SearchTermCode"] != null)
                {
                    return ViewState["SearchTermCode"].ToString();
                }
                return null;
            }
            set { ViewState["SearchTermCode"] = value; }
        }

        public string SearchTermDesc
        {
            get
            {
                if (ViewState["SearchTermDesc"] != null)
                {
                    return ViewState["SearchTermDesc"].ToString();
                }
                return null;
            }
            set { ViewState["SearchTermDesc"] = value; }
        }

        public bool IsEditMode
        {
            get
            {
                if (ViewState["IsEditMode"] != null)
                {
                    return Convert.ToBoolean(ViewState["IsEditMode"]);
                }
                return false;
            }
            set { ViewState["IsEditMode"] = value; }
        }

        public short? SelectedTermId
        {
            get
            {
                if (ViewState["SelectedTermId"] != null)
                {
                    return Convert.ToInt16(ViewState["SelectedTermId"]);
                }
                return null;
            }
            set { ViewState["SelectedTermId"] = value; }
        }

        public int? SelectedTermConfigId
        {
            get
            {
                if (ViewState["SelectedTermConfigId"] != null)
                {
                    return Convert.ToInt32(ViewState["SelectedTermConfigId"]);
                }
                return null;
            }
            set { ViewState["SelectedTermConfigId"] = value; }
        }

        public BusinessLogic.TCLCleverBL.TermConfig SelectedTermConfig
        {
            get
            {
                if (ViewState["SelectedTermConfig"] != null)
                {
                    return (BusinessLogic.TCLCleverBL.TermConfig)ViewState["SelectedTermConfig"];
                }
                return null;
            }
            set { ViewState["SelectedTermConfig"] = value; }
        }

        public DataTable SelectedTables
        {
            get
            {
                if (ViewState["SelectedTables"] != null)
                {
                    return (DataTable)ViewState["SelectedTables"];
                }
                return new DataTable();
            }
            set { ViewState["SelectedTables"] = value; }
        }

        //Supplier selected for edit
        private short? TermPK
        {
            get
            {
                short? val = null;
                if (ViewState["TermPK"] != null)
                {
                    val = Convert.ToInt16(ViewState["TermPK"]);
                }
                return val;
            }
            set
            {
                ViewState["TermPK"] = value;
            }
        }

        private byte? TermUIEnable
        {
            get
            {
                byte? val = null;
                if (ViewState["TermUIEnable"] != null)
                {
                    val = Convert.ToByte(ViewState["TermUIEnable"]);
                }
                return val;
            }
            set
            {
                ViewState["TermUIEnable"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            lblFormError.Visible = true;
            lblEditTermError.Visible = true;

            if (!IsPostBack)
            {
                ListGroups();

                ListSearchTerms();

                ListTerms();
            }
        }

        /// <summary>
        /// List all terms.
        /// </summary>
        /// <author>Slavena Asenova</author>
        /// <date>04/15/2013</date>
        private void ListSearchTerms()
        {
            ddlTerms.Items.Clear();
            int? groupSelected = null;
            if (ddlGroups.SelectedValue != String.Empty)
            {
                groupSelected = Convert.ToInt32(ddlGroups.SelectedValue);
            }
            
            DataTable dtTerms = new DataTable();
            try
            {
                dtTerms = Term.ListByGroupId(groupSelected);
            }
            catch (Exception ex)
            {
                lblError.Text = "Error occured while trying to retrieve the terms. Error details: " + ex.Message;
                lblError.Visible = true;
                return;
            }
            ddlTerms.DataSource = dtTerms;
            ddlTerms.DataBind();
            RadComboBoxItem itemDefault = new RadComboBoxItem("---Select Term Code--", String.Empty);
            ddlTerms.Items.Insert(0, itemDefault);            
        }

        /// <summary>
        /// List all terms.
        /// </summary>
        /// <author>Slavena Asenova</author>
        /// <date>04/15/2013</date>
        private void ListTerms()
        {
            rblSelectTerms.Items.Clear();

            DataTable dtTerms = new DataTable();
            try
            {
                dtTerms = Term.ListAll();                
            }
            catch (Exception ex)
            {
                lblError.Text = "Error occured while trying to retrieve the terms. Error details: " + ex.Message;
                lblError.Visible = true;
                return;
            }           

            rblSelectTerms.DataSource = dtTerms;
            rblSelectTerms.DataBind();
        }

        /// <summary>
        /// List all groups.
        /// </summary>
        /// <author>Slavena Asenova</author>
        /// <date>04/19/2013</date>
        private void ListGroups()
        {
            ddlGroups.Items.Clear();
           
            DataTable dtGroups = new DataTable();
            try
            {
                dtGroups = TermGroup.ListAll();
            }
            catch (Exception ex)
            {
                lblError.Text = "Error occured while trying to retrieve the term groups. Error details: " + ex.Message;
                lblError.Visible = true;
                return;
            }
            ddlGroups.DataSource = dtGroups;
            ddlGroups.DataBind();
            RadComboBoxItem itemDefault = new RadComboBoxItem("---Select Term Group--", String.Empty);
            ddlGroups.Items.Insert(0, itemDefault);
        }

        /// <summary>
        /// Change the term drop down.
        /// </summary>
        /// <author>Slavena Asenova</author>
        /// <date>04/19/2013</date>
        protected void ddlGroups_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {            
            ListSearchTerms();
        }

        /// <summary>
        /// Put selected term as filter and rebind the grid with filters.
        /// </summary>
        /// <author>Slavena Asenova</author>
        /// <date>04/12/2013</date>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //if (    (ddlTerms.SelectedValue != String.Empty) 
            //    ||  (ddlGroups.SelectedValue != String.Empty)
            //    ||  (rtbTermCode.Text.Trim() != String.Empty)
            //    ||  (rtbTermDesc.Text.Trim() != String.Empty))
            //{
                if (ddlTerms.SelectedValue != String.Empty)
                {
                    this.SearchTermId = Convert.ToInt32(ddlTerms.SelectedValue);
                }
                else
                {
                    this.SearchTermId = null;
                }
                if (ddlGroups.SelectedValue != String.Empty)
                {
                    this.SearchGroupPK = Convert.ToInt32(ddlGroups.SelectedValue);
                }
                else
                {
                    this.SearchGroupPK = null;
                }
                if (rtbTermCode.Text.Trim() != String.Empty)
                {
                    this.SearchTermCode = rtbTermCode.Text.Trim();
                }
                else
                {
                    this.SearchTermCode = null;
                }
                if (rtbTermDesc.Text.Trim() != String.Empty)
                {
                    this.SearchTermDesc = rtbTermDesc.Text.Trim();
                }
                else
                {
                    this.SearchTermDesc = null;
                }
                rgTerms.Rebind();
            //}
        }

        /// <summary>
        /// Display terms by the search.
        /// </summary>
        /// <author>Slavena Asenova</author>
        /// <date>04/19/2013</date>
        protected void rgTerms_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {           
            try
            {
                rgTerms.DataSource = Term.ListFiltered(this.SearchTermId, this.SearchGroupPK
                                                , this.SearchTermCode, this.SearchTermDesc);                
            }
            catch
            {
                lblError.Text = "Error retrieving terms.";
                lblError.Visible = true;                
            }
        }

        /// <summary>
        /// Populates configured terms when term is selected.
        /// </summary>      
        /// <author>Slavena Asenova</author>
        /// <date>04/19/2013</date>
        protected void rgTerms_SelectedIndexChanged1(object sender, EventArgs e)
        {
            this.SelectedTermId = Convert.ToInt16(rgTerms.SelectedValue);
            rgTermConfigs.Rebind();
            rdTermConfig.Collapsed = false;
            rdTermConfig.Visible = true;
            rdTerms.Collapsed = true;
        }

        /// <summary>
        /// Initializes the edit form of the RadGrid when edit term. 
        /// </summary>
        protected void rgTerms_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.EditFormItem
                   && e.Item.IsInEditMode)
            {
                //item is about to be edited
                e.Item.Edit = false;
            }
        }

        /// <summary>
        /// Edit for terms.
        /// </summary>
        protected void rgTerms_ItemCommand(object sender, GridCommandEventArgs e)
        {           
            if (e.CommandName == RadGrid.EditCommandName)
            {
                this.TermPK = Convert.ToInt16(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TermPK"]);
                GridDataItem item = (GridDataItem)e.Item;
                rwEditTerm.Title = "Update term";

                lblTermCode.Text = item["TermCode"].Text;
                lblTermDesc.Text = item["TermDesc"].Text;
                lblTermColumn.Text = item["TermColumnName"].Text;
                lblTermGroupCode.Text = item["TermGroupCode"].Text;

                RBLSelectItem(rblTermUIEnable, item["TermUIEnable"].Text);
                RBLSelectItem(rblTermGroupEnable, item["TermGroupEnable"].Text);
                lblTermIsComputed.Text = item["TermIsComputed"].Text;

                //Show popup for edit
                rwEditTerm.Visible = true;
                rwEditTerm.VisibleOnPageLoad = true;
            }
            else if (e.CommandName == "TermConfig")
            {
                this.SelectedTermId = Convert.ToInt16(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TermPK"]);
                rgTermConfigs.Rebind();
                rdTermConfig.Collapsed = false;
                rdTermConfig.Visible = true;
                rdTerms.Collapsed = true;

                byte termUIEnable;
                GridDataItem item = (GridDataItem)e.Item;
                byte.TryParse(item["TermUIEnable"].Text, out termUIEnable);
                this.TermUIEnable = termUIEnable;
                e.Item.Edit = false;
            }
        }

        private void RBLSelectItem(RadioButtonList rbl, string text)
        {
            rbl.SelectedIndex = -1;
            var item = rbl.Items.FindByValue(text);
            if (item != null)
                item.Selected = true;
        }
        
        /// <summary>
        /// List Term Configs
        /// </summary>
        /// <author>Slavena Asenova</author>
        /// <date>04/12/2013</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgTermConfigs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = BusinessLogic.TCLCleverBL.TermConfig.List(null,this.SelectedTermId);
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error listing term configs. Error message: " + ex.Message;
                return;
            }          

            rgTermConfigs.DataSource = dt;            
        }

        /// <summary>
        /// Open popup window to add/edit user details
        /// </summary>
        /// <author>Slavena Asenova</author>
        /// <date>04/12/2013</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgTermConfigs_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if ((e.CommandName == RadGrid.EditCommandName) || (e.CommandName == RadGrid.InitInsertCommandName))
            {
                rwAddEditTermConfig.Visible = true;
                rwAddEditTermConfig.VisibleOnPageLoad = true;                
            }
            else if (e.CommandName == RadGrid.ExpandCollapseCommandName)
            {
                rdTerms.Collapsed = true;
            }
        }

        /// <summary>
        /// Init the user add/edit form
        /// </summary>
        /// <author>Slavena Asenova</author>
        /// <date>04/12/2013</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgTermConfigs_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.EditFormItem && e.Item.IsInEditMode)
            {
                if (!e.Item.OwnerTableView.IsItemInserted)
                {
                    //item is about to be edited
                    e.Item.Edit = false;
                    if (rwAddEditTermConfig.Visible)
                    {
                        this.SelectedTermId = Convert.ToInt16(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TermFK"]);
                        rblSelectTerms.SelectedValue = this.SelectedTermId.ToString();
                        this.SelectedTermConfigId =  Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TermConfigPK"]);
                        rblSelectTerms.Enabled = false;
                        this.IsEditMode = true;

                        TermDetailsGet();
                        
                        TermCaclBind();

                        rgTableJoinPaths.Rebind();

                        txtCaption.Text = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TermCaption"].ToString();
                        rblEnableValue.SelectedValue = (Convert.ToBoolean(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsEnabled"])).ToString();
                        txtTermUIEnable.Value = Convert.ToInt16(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TermUIEnable"]);

                        short? parentTermUiEnabled = this.TermUIEnable;
                        if (parentTermUiEnabled.HasValue && parentTermUiEnabled.Value > 0)
                        {
                            txtTermUIEnable.Visible = true;
                            lblTermUIEnable.Visible = true;
                        }
                        else
                        {
                            txtTermUIEnable.Visible = false;
                            lblTermUIEnable.Visible = false;
                        }
                    }
                }
                else
                {
                    //new target will be created
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.Visible = false;

                    if (rwAddEditTermConfig.Visible)
                    {
                        this.SelectedTermId = null;
                        this.SelectedTermConfigId = null;
                        rblSelectTerms.SelectedIndex = 0;                        
                        this.SelectedTermId = Convert.ToInt16(rblSelectTerms.SelectedValue);
                        IsEditMode = false;

                        TermDetailsGet();

                        TermCaclBind();

                        rgTableJoinPaths.Rebind();

                        txtCaption.Text = String.Empty;
                        rblEnableValue.SelectedValue = "True";
                    }
                }                
            }
            else if (e.Item.ItemType == GridItemType.NestedView)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                Literal litTables = (Literal)e.Item.FindControl("litTables");

                if (drv["Table2Desc"].ToString() != String.Empty)
                { 
                    litTables.Text = "<tr><td>2</td><td>" + drv["Table2Desc"].ToString() 
                                    + "</td><td>" + drv["Table2JoinPathCode"].ToString() 
                                    + "</td><td>" + drv["Is2Default"].ToString() 
                                    + "</td><td>" + drv["JoinStatement2"].ToString() + "</td></tr>";    
                } 
                if (drv["Table3Desc"].ToString() != String.Empty)
                { 
                    litTables.Text = "<tr><td>3</td><td>" + drv["Table3Desc"].ToString() 
                                    + "</td><td>" + drv["Table3JoinPathCode"].ToString() 
                                    + "</td><td>" + drv["Is3Default"].ToString() 
                                    + "</td><td>" + drv["JoinStatement3"].ToString() + "</td></tr>";    
                } 
                if (drv["Table4Desc"].ToString() != String.Empty)
                { 
                    litTables.Text = "<tr><td>4</td><td>" + drv["Table4Desc"].ToString() 
                                    + "</td><td>" + drv["Table4JoinPathCode"].ToString() 
                                    + "</td><td>" + drv["Is4Default"].ToString() 
                                    + "</td><td>" + drv["JoinStatement4"].ToString() + "</td></tr>";    
                } 
                if (drv["Table5Desc"].ToString() != String.Empty)
                { 
                    litTables.Text = "<tr><td>5</td><td>" + drv["Table5Desc"].ToString() 
                                    + "</td><td>" + drv["Table5JoinPathCode"].ToString() 
                                    + "</td><td>" + drv["Is5Default"].ToString() 
                                    + "</td><td>" + drv["JoinStatement5"].ToString() + "</td></tr>";    
                }                               
                
            }
            
        }

        /// <summary>
        /// Delete selected term config.
        /// </summary>
        /// <author>Slavena Asenova</author>
        /// <date>04/12/2013</date>
        protected void rgTermConfigs_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            int termConfigPK = Convert.ToInt16(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TermConfigPK"]);
            try
            {
                CleverUI.BusinessLogic.TCLCleverBL.TermConfig.Delete(termConfigPK);
            }
            catch (Exception ex)
            {
                if (ex is SqlException && (ex as SqlException).Number == 547) // 547 is the number of the sql exception for reference dependences.
                {
                    lblError.Text = "The term config is already used and cannot be deleted. Error details: " + ex.Message;
                }
                else
                {
                    lblError.Text = "Error occured while trying to delete the term config. Error details: " + ex.Message;
                }

                lblError.Visible = true;
                e.Canceled = true;
                return;
            }
        }

        /// <summary>
        /// Get the Details label
        /// </summary>
        /// <author>Slavena Asenova</author>
        /// <date>04/12/2013</date>
        private void TermDetailsGet()
        {
            if (this.SelectedTermConfigId != null)
            {
                this.SelectedTermConfig = BusinessLogic.TCLCleverBL.TermConfig.Load(this.SelectedTermConfigId);
                lblTermConfigDetails.Text = this.SelectedTermConfig.TermConfigCode;
            }
            else
            {
                lblTermConfigDetails.Text = Term.FindByCode(rblSelectTerms.SelectedItem.Text).TermDesc;
            }
        }

        /// <summary>
        /// Change the selected term and join paths.
        /// </summary>
        /// <author>Slavena Asenova</author>
        /// <date>04/12/2013</date>
        private void TermCaclBind()
        {
            DataTable dtTermCalcsTables = Term.GetTermCalcsAndTables((short)this.SelectedTermId);

            rptTermCalc.DataSource = dtTermCalcsTables;
            rptTermCalc.DataBind();

            List<short> tablesFK =  new List<short>();
            this.SelectedTables =  new DataTable();
            this.SelectedTables.Columns.Add("TablePK", typeof(short));
            this.SelectedTables.Columns.Add("TableDesc", typeof(string));
            foreach(DataRow dr in dtTermCalcsTables.Rows)
            {
                if (!tablesFK.Contains((short)dr["TableFK"]))
                {
                    tablesFK.Add((short)dr["TableFK"]);
                    DataRow drTables = this.SelectedTables.NewRow();
                    drTables["TablePK"] = (short)dr["TableFK"];
                    drTables["TableDesc"] = dr["TableDesc"].ToString();
                    this.SelectedTables.Rows.Add(drTables);
                }
            }
        }

        /// <summary>
        /// Change the selected term and join paths.
        /// </summary>
        /// <author>Slavena Asenova</author>
        /// <date>04/12/2013</date>
        protected void rblSelectTerms_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SelectedTermId = Convert.ToInt16(rblSelectTerms.SelectedValue);

            TermDetailsGet();

            TermCaclBind();

            rgTableJoinPaths.Rebind();
        }

        /// <summary>
        /// Bind the term calcs.
        /// </summary>
        /// <author>Slavena Asenova</author>
        /// <date>04/15/2013</date>
        protected void rgTableJoinPaths_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!String.IsNullOrEmpty(rblSelectTerms.SelectedValue))
            {
                rgTableJoinPaths.DataSource = this.SelectedTables;
            }
            else
            {
                rgTableJoinPaths.DataSource = new DataTable();
            }
        }

        /// <summary>
        /// Bind the term calcs items.
        /// </summary>
        /// <author>Slavena Asenova</author>
        /// <date>04/15/2013</date>
        protected void rgTableJoinPaths_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
               || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                DataRowView dataItem = (DataRowView)e.Item.DataItem;
                short tablePK = Convert.ToInt16(dataItem["TablePK"]);
                string tableDesc = dataItem["TableDesc"].ToString();

                //Populate available join paths to the table.
                //TO DO: Return only available paths that no term config are connected to them
                RadComboBox rcbTableJoinPath = ((RadComboBox)e.Item.FindControl("rcbTableJoinPath"));
                rcbTableJoinPath.DataTextField = "TablesJoinPathCode";
                rcbTableJoinPath.DataValueField = "TablesJoinPathsPK";

                DataTable dtJoinPaths = new DataTable();
                try
                {
                    dtJoinPaths = TablesJoinPaths.List(null,tablePK);                    
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error listing available join paths for table: " + tableDesc + ". Error details: " + ex.Message;
                    lblError.Visible = true;
                    return;
                }

                //set default value
                foreach (DataRow dr in dtJoinPaths.Rows)
                { 
                    if(Convert.ToBoolean(dr["IsDefault"]))
                    {
                        dr["TablesJoinPathCode"] = dr["TablesJoinPathCode"] + " Default";
                        break;
                    }
                }
                rcbTableJoinPath.DataSource = dtJoinPaths;
                rcbTableJoinPath.DataBind();

                if (IsEditMode && this.SelectedTermConfigId != null)
                {
                    //select join path
                    foreach (RadComboBoxItem dr in rcbTableJoinPath.Items)
                    {
                        if(dr.Value == this.SelectedTermConfig.TablesJoinPathsFK.ToString())
                        {
                            dr.Selected = true;
                            break;
                        }
                        if (dr.Value == this.SelectedTermConfig.Table2JoinPathsFK.ToString())
                        {
                            dr.Selected = true;
                            break;
                        }
                        if (dr.Value == this.SelectedTermConfig.Table3JoinPathsFK.ToString())
                        {
                            dr.Selected = true;
                            break;
                        }
                        if (dr.Value == this.SelectedTermConfig.Table4JoinPathsFK.ToString())
                        {
                            dr.Selected = true;
                            break;
                        }
                        if (dr.Value == this.SelectedTermConfig.Table5JoinPathsFK.ToString())
                        {
                            dr.Selected = true;
                            break;
                        }

                    }
                    
                    rcbTableJoinPath.Enabled = false;
                }

                //Display join path joins from the selected join path.
                Repeater repJoinPathJoins = ((Repeater)e.Item.FindControl("repJoinPathJoins"));
                if (!String.IsNullOrEmpty(rcbTableJoinPath.SelectedValue))
                {
                    int tableJoinPathPK = Convert.ToInt32(rcbTableJoinPath.SelectedValue);
                    int joinPathFK = 0;
                    foreach (DataRow dr in dtJoinPaths.Rows)
                    {
                        if (Convert.ToInt32(dr["TablesJoinPathsPK"]) == tableJoinPathPK)
                        {
                            joinPathFK = Convert.ToInt32(dr["JoinPathFK"]);
                            break;
                        }
                    }
                    repJoinPathJoins.DataSource = JoinPath.GetJoinPathJoins(joinPathFK);
                    repJoinPathJoins.DataBind();
                }
                            
            }

        }

        /// <summary>
        /// Bind the term calcs join path join.
        /// </summary>
        /// <author>Slavena Asenova</author>
        /// <date>04/15/2013</date>
        protected void rcbTableJoinPath_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox rcbTableJoinPath = (RadComboBox)sender;
            int tableJoinPathPK = Convert.ToInt32(e.Value);
            
            GridDataItem item = (GridDataItem)rcbTableJoinPath.NamingContainer;
            Repeater repJoinPathJoins = ((Repeater)item.FindControl("repJoinPathJoins"));
            int joinPathFK = 0;
            DataTable dtTJ = TablesJoinPaths.List(tableJoinPathPK, null);
            if(dtTJ.Rows.Count > 0)
            {
                joinPathFK = Convert.ToInt32(dtTJ.Rows[0]["JoinPathFK"]);
            }
            repJoinPathJoins.DataSource = JoinPath.GetJoinPathJoins(joinPathFK);
            repJoinPathJoins.DataBind();
        }

        /// <summary>
        /// This method save the changed term config.
        /// </summary>
        /// <author>Slavena Asenova</author>
        /// <date>04/12/2013</date>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool isEnabled = Convert.ToBoolean(rblEnableValue.SelectedValue);
            byte? parentTermUiEnabled = this.TermUIEnable;
            byte termUIEnabled = parentTermUiEnabled.HasValue && parentTermUiEnabled.Value > 0 ? (byte)txtTermUIEnable.Value : (byte)0;

            if (this.SelectedTermConfigId != null)
            {
                //edit                
                string termCaption = txtCaption.Text.Trim();
                
                try
                {
                    BusinessLogic.TCLCleverBL.TermConfig.Save(this.SelectedTermConfigId, null, termCaption
                                                            , isEnabled, termUIEnabled, null, null
                                                            , null, null, null);
                }
                catch (Exception ex)
                {
                    lblFormError.Visible = true;
                    lblFormError.Text = "Error updating term config. Erorr message: " + ex.Message;

                    rwAddEditTermConfig.Visible = true;
                    rwAddEditTermConfig.VisibleOnPageLoad = true;
                    return;
                }                
            }
            else
            { 
                //add
                string termCaption = txtCaption.Text.Trim();               

                List<int> tableJoinPathsPKs = new List<int>();
                foreach (GridDataItem gdi in rgTableJoinPaths.Items)
                {
                    RadComboBox rcbTableJoinPath = ((RadComboBox)gdi.FindControl("rcbTableJoinPath"));
                    tableJoinPathsPKs.Add(Convert.ToInt32(rcbTableJoinPath.SelectedValue));
                }
                int? tableJoinPaths = null;
                int? table2JoinPaths = null;
                int? table3JoinPaths = null;
                int? table4JoinPaths = null;
                int? table5JoinPaths = null;
                foreach (int pk in tableJoinPathsPKs)
                {
                    if (tableJoinPaths == null)
                    {
                        tableJoinPaths = pk;
                    }
                    else if (table2JoinPaths == null)
                    {
                        table2JoinPaths = pk;
                    }
                    else if (table3JoinPaths == null)
                    {
                        table3JoinPaths = pk;
                    }
                    else if (table4JoinPaths == null)
                    {
                        table4JoinPaths = pk;
                    }
                    else if (table5JoinPaths == null)
                    {
                        table5JoinPaths = pk;
                    }
                }

                try
                {
                    BusinessLogic.TCLCleverBL.TermConfig.Save(null, this.SelectedTermId, termCaption
                                                            , isEnabled, termUIEnabled, tableJoinPaths, table2JoinPaths
                                                            , table3JoinPaths, table4JoinPaths, table5JoinPaths);
                }
                catch (Exception ex)
                {
                    lblFormError.Visible = true;
                    lblFormError.Text = "Error creating term config. Erorr message: " + ex.Message;

                    rwAddEditTermConfig.Visible = true;
                    rwAddEditTermConfig.VisibleOnPageLoad = true;
                    return;
                }                
            }
           

            rwAddEditTermConfig.Visible = false;
            rwAddEditTermConfig.VisibleOnPageLoad = false;

            rgTermConfigs.Rebind();
           
        }

        /// <summary>
        /// Save term changes.
        /// </summary>
        protected void btnSaveTerm_Click(object sender, EventArgs e)
        {
            var itemTermUIEnable = rblTermUIEnable.SelectedItem;
            var itemTermGroupEnable = rblTermGroupEnable.SelectedItem;

            if (this.TermPK.HasValue && itemTermUIEnable != null && itemTermGroupEnable != null)
            {
                try
                {
                    Term.UpdateTerm(this.TermPK.Value, Int16.Parse(itemTermUIEnable.Value), Int16.Parse(itemTermGroupEnable.Value));
                }
                catch(Exception ex)
                {
                    lblEditTermError.Text = "Error updating term. Error details: " + ex.Message;
                    lblEditTermError.Visible = true;
                    return;
                }
                rgTerms.Rebind();
            }

            rwEditTerm.Visible = false;
            rwEditTerm.VisibleOnPageLoad = false;
        }

        /// <summary>
        /// Closes the popup for term edit.
        /// </summary>
        protected void btnCancelTerm_Click(object sender, EventArgs e)
        {
            rwEditTerm.Visible = false;
            rwEditTerm.VisibleOnPageLoad = false;
        }

        /// <summary>
        /// This method cancel the changes in term config.
        /// </summary>
        /// <author>Slavena Asenova</author>
        /// <date>04/12/2013</date>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            rwAddEditTermConfig.Visible = false;
            rwAddEditTermConfig.VisibleOnPageLoad = false;
        }  
    }    
}