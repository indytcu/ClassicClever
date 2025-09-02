using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using System.Data.SqlClient;

namespace CleverUI.Admin
{
    public partial class JoinPaths : System.Web.UI.Page
    {
        #region Fields & Properties

        protected short? FilterStartTable
        {
            get
            {
                short? val = null;
                if (ViewState["FilterStartTable"] != null)
                {
                    val = Convert.ToInt16(ViewState["FilterStartTable"]);
                }
                return val;
            }
            set
            {
                ViewState["FilterStartTable"] = value;
            }
        }

        protected short? FilterFirstNodeTable
        {
            get
            {
                short? val = null;
                if (ViewState["FilterFirstNodeTable"] != null)
                {
                    val = Convert.ToInt16(ViewState["FilterFirstNodeTable"]);
                }
                return val;
            }
            set
            {
                ViewState["FilterFirstNodeTable"] = value;
            }
        }

        protected short? FilterEndTable
        {
            get
            {
                short? val = null;
                if (ViewState["FilterEndTable"] != null)
                {
                    val = Convert.ToInt16(ViewState["FilterEndTable"]);
                }
                return val;
            }
            set
            {
                ViewState["FilterEndTable"] = value;
            }
        }

        protected short? JoinPathPK
        {
            get
            {
                short? val = null;
                if (Session["JoinPathPK"] != null)
                {
                    val = Convert.ToInt16(Session["JoinPathPK"]);
                }
                return val;
            }
            set
            {
                Session["JoinPathPK"] = value;
            }
        }

        protected DataTable JoinPathJoins
        {
            get
            {
                DataTable val = null;
                if (Session["JoinPathJoins"] != null)
                {
                    val = (DataTable)Session["JoinPathJoins"];
                }
                return val;
            }
            set
            {
                Session["JoinPathJoins"] = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
          
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            lblFormError.Visible = false;
            if (!IsPostBack)
            {
                rwFormJoinPath.VisibleOnPageLoad = false;
                rwFormJoinPath.Visible = false;

                RadComboBoxItemsRequestedEventArgs args = new RadComboBoxItemsRequestedEventArgs();
                args.Text = "factInventory";
                rcbFilterFromTable_ItemsRequested(sender, args);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        protected void rcbFilterFromTable_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
          
            string code = e.Text.Trim();
            code = code != "" ? code : null;

            int itemOffset = e.NumberOfItems;
            int itemsCount = rcbFilterFromTable.ItemsPerRequest;
            int total = 0;

            DataTable dtFactTables = CleverUI.BusinessLogic.TCLCleverBL.Tables.ListFactTables();
             
            DataView dw = new DataView(dtFactTables);
            if (IsPostBack)
            {
                dw.RowFilter = "TableName like '" + code + "%'";
            }
            else
            {
                dw.RowFilter = "TableName like 'factInventory'";
            }
            dw.Sort = "TableName ASC";
            DataTable dt = dw.ToTable();

            total = dt.Rows.Count;

            int endOffset = itemOffset + itemsCount;
            if (endOffset > dt.Rows.Count)
            {
                endOffset = dt.Rows.Count;
            }

            rcbFilterFromTable.Items.Clear();
            rcbFilterFromTable.Text = String.Empty;
            for (int i = itemOffset; i < endOffset; i++)
            {
                rcbFilterFromTable.Items.Add(new RadComboBoxItem(dt.Rows[i][rcbFilterFromTable.DataTextField].ToString()
                                                    , dt.Rows[i][rcbFilterFromTable.DataValueField].ToString()));
            }

            if (itemOffset + itemsCount >= total)
            {
                e.EndOfItems = true;
            }
            else
            {
                e.EndOfItems = false;
            }

            if (!IsPostBack)
            {

                RadComboBoxItem item = rcbFilterFromTable.FindItemByText("factInventory");
                if (item != null)
                {
                    item.Selected = true;
                }
              
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rcbFilterFromTable_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Value))
            {
                short startTableFK = Convert.ToInt16(e.Value);
                try
                {
                    rcbFilterNodeTable.DataSource = CleverUI.BusinessLogic.TCLCleverBL.Tables.GetSecondNodeJoinTables(startTableFK);
                    rcbFilterNodeTable.DataBind();
                    rcbFilterNodeTable.Items.Insert(0, new RadComboBoxItem());
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error listing join path tables from second level with starting table: " + e.Text + ". Error details: " + ex.Message;
                    lblError.Visible = true;
                    return;
                }

                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        protected void rcbFilterToTable_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            if (IsPostBack)
            {
                string code = e.Text.Trim();
                code = code != "" ? code : null;

                int itemOffset = e.NumberOfItems;
                int itemsCount = rcbFilterToTable.ItemsPerRequest;
                int total = 0;


                DataTable dt = CleverUI.BusinessLogic.TCLCleverBL.Tables.SearchByName(code);
                total = dt.Rows.Count;

                int endOffset = itemOffset + itemsCount;
                if (endOffset > dt.Rows.Count)
                {
                    endOffset = dt.Rows.Count;
                }

                rcbFilterToTable.Items.Clear();
                rcbFilterToTable.Text = String.Empty;
                for (int i = itemOffset; i < endOffset; i++)
                {
                    rcbFilterToTable.Items.Add(new RadComboBoxItem(dt.Rows[i][rcbFilterToTable.DataTextField].ToString()
                                                        , dt.Rows[i][rcbFilterToTable.DataValueField].ToString()));
                }

                if (itemOffset + itemsCount >= total)
                {
                    e.EndOfItems = true;
                }
                else
                {
                    e.EndOfItems = false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(rcbFilterFromTable.SelectedValue))
            {
                this.FilterStartTable = Convert.ToInt16(rcbFilterFromTable.SelectedValue);
            }
            else
            {
                this.FilterStartTable = null;
            }

            if (!String.IsNullOrEmpty(rcbFilterToTable.SelectedValue))
            {
                this.FilterEndTable = Convert.ToInt16(rcbFilterToTable.SelectedValue);
            }
            else
            {
                this.FilterEndTable = null;
            }

            if (!String.IsNullOrEmpty(rcbFilterNodeTable.SelectedValue))
            {
                this.FilterFirstNodeTable = Convert.ToInt16(rcbFilterNodeTable.SelectedValue);
            }
            else
            {
                this.FilterFirstNodeTable = null;
            }


            rgJoinPaths.Rebind();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgJoinPaths_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {

            if (this.FilterStartTable.HasValue)
            {
                rgJoinPaths.Visible = true;
                try
                {
                    rgJoinPaths.DataSource = CleverUI.BusinessLogic.TCLCleverBL.JoinPath.Search(this.FilterStartTable,this.FilterFirstNodeTable,this.FilterEndTable);
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error searching join paths. Error details: " + ex.Message;
                    lblError.Visible = true;
                }

            }
            else
            {
                rgJoinPaths.Visible = false;
            }

          
          
        }

        /// <summary>
        /// Binds ranking criteria.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        /// <author>Nevena Uzunova</author>
        /// <date>01/26/2011</date>
        protected void rgJoinPaths_DetailTableDataBind(object source, GridDetailTableDataBindEventArgs e)
        {
            switch (e.DetailTableView.Name)
            {
                case "JoinPathJoins":
                    {
                        GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
                        int joinPathPK = Convert.ToInt32(dataItem.GetDataKeyValue("JoinPathPK"));
                        try
                        {
                            DataTable dt = CleverUI.BusinessLogic.TCLCleverBL.JoinPath.GetJoinPathJoins(joinPathPK);
                            
                            //Reorder From and To Tables
                            for (int i = 0; i < dt.Rows.Count - 1; i++)
                            {
                                string fromTableName = dt.Rows[i]["FromTableName"].ToString();
                                short fromTableFK = Convert.ToInt16(dt.Rows[i]["FromTableFK"]);
                                string toTableName = dt.Rows[i]["ToTableName"].ToString();
                                short toTableFK = Convert.ToInt16(dt.Rows[i]["ToTableFK"]);
                                string nextFromTableName = dt.Rows[i + 1]["FromTableName"].ToString();

                                if (fromTableName == nextFromTableName)
                                {
                                    dt.Rows[i]["FromTableName"] = toTableName;
                                    dt.Rows[i]["FromTableFK"] = toTableFK;
                                    dt.Rows[i]["ToTableName"] = fromTableName;
                                    dt.Rows[i]["ToTableFK"] = fromTableFK;
                                }
                            }

                            e.DetailTableView.DataSource = dt;
                        }
                        catch (Exception ex)
                        {
                            lblError.Text = "Error getting join path joins for join with id: " + joinPathPK.ToString()
                                    + "Error details: " + ex.Message;
                            lblError.Visible = true;
                            return;
                        }

                        break;
                    }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgJoinPaths_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.EditFormItem
                   && e.Item.IsInEditMode)
            {
                if (!e.Item.OwnerTableView.IsItemInserted)
                {
                    //item is about to be edited
                    e.Item.Edit = false;
                }
                else
                {
                    //new target will be created
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.Visible = false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgJoinPaths_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.Item.OwnerTableView.Name == "JoinPaths")
            {
                // MasterTable's command is fired

                if (e.CommandName == RadGrid.EditCommandName)
                {
                    //Edit ranking
                    //hfIsInsertRanking.Value = "False";

                    lblStartTable.Text = Convert.ToString(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["StartTableName"]);
                    lblEndTable.Visible = true;
                    rcbEndTable.Visible = false;
                    lblEndTable.Text = Convert.ToString(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["EndTableName"]);
                    rfvEndTable.Enabled = false;
                    spanEndTableValidator.Visible = false;
                    short joinPathFK = Convert.ToInt16(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["JoinPathPK"]);
                    this.JoinPathPK = joinPathFK;

                    this.JoinPathJoins = null;

                    try
                    {
                        this.JoinPathJoins = CleverUI.BusinessLogic.TCLCleverBL.JoinPath.GetJoinPathJoins(joinPathFK);
                    
                    }
                    catch (Exception ex)
                    {
                        lblFormError.Text = "Error listing join path joins of the selected join. Error details: " + ex.Message;
                        lblFormError.Visible = true;
                    }

                    if (this.JoinPathJoins != null)
                    {
                        //reorder from and to tables
                     
                        for (int i = 0; i < this.JoinPathJoins.Rows.Count - 1; i++)
                        {
                            string fromTableName = this.JoinPathJoins.Rows[i]["FromTableName"].ToString();
                            short fromTableFK = Convert.ToInt16(this.JoinPathJoins.Rows[i]["FromTableFK"]);
                            string toTableName = this.JoinPathJoins.Rows[i]["ToTableName"].ToString();
                            short toTableFK = Convert.ToInt16(this.JoinPathJoins.Rows[i]["ToTableFK"]);
                            string nextFromTableName = this.JoinPathJoins.Rows[i + 1]["FromTableName"].ToString();

                            if (fromTableName == nextFromTableName)
                            {
                                this.JoinPathJoins.Rows[i]["FromTableName"] = toTableName;
                                this.JoinPathJoins.Rows[i]["FromTableFK"] = toTableFK;
                                this.JoinPathJoins.Rows[i]["ToTableName"] = fromTableName;
                                this.JoinPathJoins.Rows[i]["ToTableFK"] = fromTableFK;
                            }
                        }


                        DataTable dt = new DataTable();
                        dt.Columns.Add("FromTableName");
                        dt.Columns.Add("FromTableKey");
                        dt.Columns.Add("ToTableName");

                        dt.Rows.Add(dt.NewRow());
                        dt.Rows.Add(dt.NewRow());
                        dt.Rows.Add(dt.NewRow());
                        dt.Rows.Add(dt.NewRow());

                        rgJoinPathJoins.DataSource = dt;
                        rgJoinPathJoins.DataBind();
                    }
                   

                    rwFormJoinPath.Visible = true;
                    rwFormJoinPath.VisibleOnPageLoad = true;
               
                    e.Canceled = true;
                }
                else if (e.CommandName == RadGrid.InitInsertCommandName)
                {
                    //Create ranking
                    //hfIsInsertRanking.Value = "True";
                    this.JoinPathPK = null;
                    this.JoinPathJoins = null;

                    lblStartTable.Text = Convert.ToString(rcbFilterFromTable.Text);
                    lblEndTable.Visible = false;
                    rcbEndTable.Visible = true;
                    if (!String.IsNullOrEmpty(rcbFilterToTable.SelectedValue))
                    {
                        rcbEndTable.Text = rcbFilterToTable.Text;
                    }
                    else
                    {
                        rcbEndTable.Text = "";
                    }
                    rfvEndTable.Enabled = false;
                    spanEndTableValidator.Visible = false;

                    CheckWhetherJoinCounldBeDefault(rcbEndTable.Text.Trim(), lblStartTable.Text.Trim());

                    DataTable dt = new DataTable();
                    dt.Columns.Add("FromTableName");
                    dt.Columns.Add("FromTableKey");
                    dt.Columns.Add("ToTableName");

                    dt.Rows.Add(dt.NewRow());
                    dt.Rows.Add(dt.NewRow());
                    dt.Rows.Add(dt.NewRow());
                    dt.Rows.Add(dt.NewRow());

                    this.JoinPathJoins = dt;
                    rgJoinPathJoins.DataSource = dt;
                    rgJoinPathJoins.DataBind();


                    rwFormJoinPath.Visible = true;
                    rwFormJoinPath.VisibleOnPageLoad = true;

                    e.Canceled = true;
                   
                }
            }
         
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgJoinPaths_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            short joinPathPK = Convert.ToInt16(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["JoinPathPK"]);
            try
            {
                CleverUI.BusinessLogic.TCLCleverBL.JoinPath.Delete(joinPathPK);
            }
            catch (Exception ex)
            {
                if (ex is SqlException && (ex as SqlException).Number == 547) // 547 is the number of the sql exception for reference dependences.
                {
                    lblError.Text = "The join path is already used and cannot be deleted. Error details: " + ex.Message;
                }
                else
                {
                    lblError.Text = "Error occured while trying to delete the join pah. Error details: " + ex.Message;
                }
               
                lblError.Visible = true;
                e.Canceled = true;
                return;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        protected void rcbEndTable_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            if (IsPostBack)
            {
                string code = e.Text.Trim();
                code = code != "" ? code : null;

                int itemOffset = e.NumberOfItems;
                int itemsCount = rcbEndTable.ItemsPerRequest;
                int total = 0;


                DataTable dt = CleverUI.BusinessLogic.TCLCleverBL.Tables.SearchByName(code);
                total = dt.Rows.Count;

                int endOffset = itemOffset + itemsCount;
                if (endOffset > dt.Rows.Count)
                {
                    endOffset = dt.Rows.Count;
                }

                rcbEndTable.Items.Clear();
                rcbEndTable.Text = String.Empty;
                for (int i = itemOffset; i < endOffset; i++)
                {
                    rcbEndTable.Items.Add(new RadComboBoxItem(dt.Rows[i][rcbEndTable.DataTextField].ToString()
                                                        , dt.Rows[i][rcbEndTable.DataValueField].ToString()));
                }

                if (itemOffset + itemsCount >= total)
                {
                    e.EndOfItems = true;
                }
                else
                {
                    e.EndOfItems = false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rcbEndTable_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //DataTable dt = new DataTable();
            //dt.Columns.Add("FromTableName");
            //dt.Columns.Add("FromTableKey");
            //dt.Columns.Add("ToTableName");

            //dt.Rows.Add(dt.NewRow());
            //dt.Rows.Add(dt.NewRow());
            //dt.Rows.Add(dt.NewRow());
            //dt.Rows.Add(dt.NewRow());

            //rgJoinPathJoins.DataSource = dt;
            //rgJoinPathJoins.DataBind();
            CheckWhetherJoinCounldBeDefault(e.Text.Trim(), lblStartTable.Text.Trim());
        }

     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgJoinPathJoins_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                //attach SelectedIndexChanged event for the dropdown FromTableName  
                RadComboBox rcbFromTableName = e.Item.FindControl("rcbFromTableName") as RadComboBox;
                rcbFromTableName.AutoPostBack = true;
                rcbFromTableName.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(rcbGridFromTableName_SelectedIndexChanged);
                rcbFromTableName.Skin = "Default";

                //attach SelectedIndexChanged event for the dropdown FromTableName  
                RadComboBox rcbFromTableKey = e.Item.FindControl("rcbFromTableKey") as RadComboBox;
                rcbFromTableKey.AutoPostBack = true;
                rcbFromTableKey.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(rcbGridFromTableKey_SelectedIndexChanged);
                rcbFromTableKey.Skin = "Default";

                //attach SelectedIndexChanged event for the dropdown ToTableName  
                RadComboBox rcbToTableName =  e.Item.FindControl("rcbToTableName") as RadComboBox;
                rcbToTableName.AutoPostBack = true;
                rcbToTableName.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(rcbGridToTableName_SelectedIndexChanged);
                rcbToTableName.Skin = "Default";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgJoinPathJoins_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                //From Table 
                RadComboBox rcbFromTableName = (RadComboBox)e.Item.FindControl("rcbFromTableName");

                //From Table Key
                RadComboBox rcbFromTableKey = (RadComboBox)e.Item.FindControl("rcbFromTableKey");

                //To Table
                RadComboBox rcbToTableName = (RadComboBox)e.Item.FindControl("rcbToTableName");


                if (e.Item.ItemIndex == 0 && !this.JoinPathPK.HasValue)
                {
                    //Insert Mode

                    //Start table of the join path (first row)
                    rcbFromTableName.Items.Insert(0, new RadComboBoxItem(lblStartTable.Text, this.FilterStartTable.Value.ToString()));

                    //Start table of the join path (first row)
                    LoadFromTableColumnKeys(rcbFromTableKey, this.FilterStartTable.Value);

                    //To Table (first row)
                    LoadToTables(rcbToTableName, this.FilterStartTable.Value, rcbFromTableKey.SelectedValue);
                }

                if (this.JoinPathPK.HasValue && this.JoinPathJoins != null)
                {
                    //Edit Mode
                    int nodeSequence = e.Item.ItemIndex + 1;
                    DataRow[] drs = this.JoinPathJoins.Select("NodeSequence=" + nodeSequence);
                    if (drs.Length > 0)
                    {
                        string fromTableName = Convert.ToString(drs[0]["FromTableName"]);
                        string toTableName = Convert.ToString(drs[0]["ToTableName"]);
                        short fromTableFK = Convert.ToInt16(drs[0]["FromTableFK"]);
                        short toTableFK = Convert.ToInt16(drs[0]["ToTableFK"]);
                        short joinPK = Convert.ToInt16(drs[0]["JoinPK"]);
                        string key1 = Convert.ToString(drs[0]["ColPrimaryKey"]);
                        string key2 = Convert.ToString(drs[0]["ColForeignKey"]);

                        //From table
                        rcbFromTableName.Items.Insert(0, new RadComboBoxItem(fromTableName, fromTableFK.ToString()));
                        
                        //From table keys
                        LoadFromTableColumnKeys(rcbFromTableKey, fromTableFK);
                        if (rcbFromTableKey.Items.FindItemByText(key1) != null)
                        {
                            rcbFromTableKey.SelectedValue = key1;
                        }
                        else
                        {
                            rcbFromTableKey.SelectedValue = key2;
                        }

                        //To Table
                        LoadToTables(rcbToTableName, fromTableFK, rcbFromTableKey.SelectedValue);
                        rcbToTableName.SelectedValue = toTableFK.ToString() + ";" + joinPK.ToString();

                        Label lblTableJoinStatement = (Label)e.Item.FindControl("lblTableJoinStatement");
                        lblTableJoinStatement.Text = drs[0]["TableJoinStatement"].ToString();

                        Label lblTableJoinAlias = (Label)e.Item.FindControl("lblTableJoinAlias");
                        lblTableJoinAlias.Text = drs[0]["JoinAlias"].ToString();

                        Label lblJoinPathStatement = (Label)e.Item.FindControl("lblJoinPathStatement");
                        lblJoinPathStatement.Text = drs[0]["JoinStatement"].ToString();

                    }
                }

            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rcbGridFromTableName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            GridItem item = ((sender as RadComboBox).NamingContainer as GridItem);
            RadComboBox rcbFromTableKey = item.FindControl("rcbFromTableKey") as RadComboBox;
 
            short fromTablePK = Convert.ToInt16(e.Value);
            LoadFromTableColumnKeys(rcbFromTableKey, fromTablePK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rcbGridFromTableKey_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //When from table column key is selected => populate to tables
            GridItem item = ((sender as RadComboBox).NamingContainer as GridItem);
            RadComboBox rcbFromTableName = item.FindControl("rcbFromTableName") as RadComboBox;
            RadComboBox rcbToTableName = item.FindControl("rcbToTableName") as RadComboBox;
            Label lblTableJoinStatement = (Label)item.FindControl("lblTableJoinStatement");
            Label lblTableJoinAlias = (Label)item.FindControl("lblTableJoinAlias");
            Label lblJoinPathStatement = (Label)item.FindControl("lblJoinPathStatement");

            string fromTableColumnKey = e.Value;
            if (!String.IsNullOrEmpty(rcbFromTableName.SelectedValue))
            {
                short fromTablePK = Convert.ToInt16(rcbFromTableName.SelectedValue);
                LoadToTables(rcbToTableName,fromTablePK, fromTableColumnKey);
                lblTableJoinStatement.Text = "";
                lblTableJoinAlias.Text = "";
                lblJoinPathStatement.Text = "";


                //Clear next join path  joins (if any)
                for (int i = item.ItemIndex + 1; i < item.OwnerTableView.Items.Count; i++)
                {
                    RadComboBox rcbRowFromTable = (RadComboBox)item.OwnerTableView.Items[i].FindControl("rcbFromTableName");
                    rcbRowFromTable.Items.Clear();

                    RadComboBox rcbRowFromTableKeys = (RadComboBox)item.OwnerTableView.Items[i].FindControl("rcbFromTableKey");
                    rcbRowFromTableKeys.Items.Clear();

                    RadComboBox rcbRowToTable = (RadComboBox)item.OwnerTableView.Items[i].FindControl("rcbToTableName");
                    rcbRowToTable.Items.Clear();

                    Label lblRowTableJoinStatement = (Label)item.OwnerTableView.Items[i]["TableJoinStatement"].FindControl("lblTableJoinStatement");
                    lblRowTableJoinStatement.Text = "";

                    Label lblRowTableJoinAlias = (Label)item.OwnerTableView.Items[i]["TableJoinStatement"].FindControl("lblTableJoinAlias");
                    lblRowTableJoinAlias.Text = "";

                    Label lblRowJoinPathStatement = (Label)item.OwnerTableView.Items[i]["JoinPathStatement"].FindControl("lblJoinPathStatement");
                    lblRowJoinPathStatement.Text = "";
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rcbGridToTableName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            GridItem item = ((sender as RadComboBox).NamingContainer as GridItem);
            RadComboBox rcbToTableName = item.FindControl("rcbToTableName") as RadComboBox;

            Label lblTableJoinStatement = (Label)item.OwnerTableView.Items[item.ItemIndex].FindControl("lblTableJoinStatement");
            Label lblTableJoinAlias = (Label)item.OwnerTableView.Items[item.ItemIndex].FindControl("lblTableJoinAlias");
            Label lblJoinPathStatement = (Label)item.OwnerTableView.Items[item.ItemIndex].FindControl("lblJoinPathStatement");

            RadComboBox rcbNextFromTableName = item.OwnerTableView.Items[item.ItemIndex + 1].FindControl("rcbFromTableName") as RadComboBox;
            RadComboBox rcbNextFromTableKey = item.OwnerTableView.Items[item.ItemIndex + 1].FindControl("rcbFromTableKey") as RadComboBox;

            string[] toTableValues = null;
            short tablePK = 0;
            short joinPK = 0;

            //1. Build join path statement
            if (!String.IsNullOrEmpty(e.Value))
            {
               
                toTableValues = e.Value.Split(';');
                tablePK = Convert.ToInt16(toTableValues[0]);
                joinPK = Convert.ToInt16(toTableValues[1]);

                CleverUI.BusinessLogic.TCLCleverBL.TableJoin objTableJoin = null;
                try
                {
                    objTableJoin = CleverUI.BusinessLogic.TCLCleverBL.TableJoin.Load(joinPK);
                }
                catch (Exception ex)
                {
                    lblFormError.Text = "Error loading table join details. Error details: " + ex.Message;
                    lblFormError.Visible = false;
                }

                if (objTableJoin != null)
                {
                    lblTableJoinStatement.Text = objTableJoin.JoinStatement;
                    lblTableJoinAlias.Text = objTableJoin.JoinAlias;
                    if (item.ItemIndex > 0)
                    {
                        Label lblPrevTableJoinAlias = (Label)item.OwnerTableView.Items[item.ItemIndex - 1].FindControl("lblTableJoinAlias");

                        string prevNodeJoinAlias = lblPrevTableJoinAlias.Text;
                        lblJoinPathStatement.Text = objTableJoin.JoinStatement.Replace(objTableJoin.TableName, prevNodeJoinAlias);
                    }
                    else
                    {
                        lblJoinPathStatement.Text = objTableJoin.JoinStatement;

                    }
                }
            }
            else
            {
                lblTableJoinStatement.Text = "";
                lblJoinPathStatement.Text = "";
            }

            //Prepare next rows in the grid
            if (item.OwnerTableView.Items.Count >= item.ItemIndex + 1)
            {
                //Clear next join path  joins (if any)
                for (int i = item.ItemIndex + 1; i < item.OwnerTableView.Items.Count; i++)
                {
                    RadComboBox rcbRowFromTable = (RadComboBox)item.OwnerTableView.Items[i].FindControl("rcbFromTableName");
                    rcbRowFromTable.Items.Clear();

                    RadComboBox rcbRowFromTableKeys = (RadComboBox)item.OwnerTableView.Items[i].FindControl("rcbFromTableKey");
                    rcbRowFromTableKeys.Items.Clear();

                    RadComboBox rcbRowToTable = (RadComboBox)item.OwnerTableView.Items[i].FindControl("rcbToTableName");
                    rcbRowToTable.Items.Clear();

                    Label lblRowTableJoinStatement = (Label)item.OwnerTableView.Items[i].FindControl("lblTableJoinStatement");
                    lblRowTableJoinStatement.Text = "";

                    Label lblRowTableJoinAlias = (Label)item.OwnerTableView.Items[i].FindControl("lblTableJoinAlias");
                    lblRowTableJoinAlias.Text = "";

                    Label lblRowJoinPathStatement = (Label)item.OwnerTableView.Items[i].FindControl("lblJoinPathStatement");
                    lblRowJoinPathStatement.Text = "";
                }

                //When select to table => populate from tables for the the next path join path.
                if (!String.IsNullOrEmpty(e.Value) 
                    && 
                    ((!this.JoinPathPK.HasValue && rcbEndTable.Text != e.Text) || (this.JoinPathPK.HasValue && lblEndTable.Text != e.Text)))
                {
                    rcbNextFromTableName.Items.Clear();
                 
                    string value = "";
                    string[] itemValues = e.Value.Split(';');
                    value = itemValues[0]; //tablePK
                    rcbNextFromTableName.Items.Add(new RadComboBoxItem(e.Text, value.ToString()));

                    rcbNextFromTableName.SelectedValue = e.Value.Split(';')[0];
                    rcbNextFromTableKey.Items.Clear();
                    LoadFromTableColumnKeys(rcbNextFromTableKey, tablePK);
                }
                else
                {
                    rcbNextFromTableName.Items.Clear();
                    rcbNextFromTableKey.Items.Clear();
                }

              
            }
        
        }  


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            rwFormJoinPath.Visible = false;
            rwFormJoinPath.VisibleOnPageLoad = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //Check whether at least one path is specified
            RadComboBox rcbFirstToTable = (RadComboBox)rgJoinPathJoins.MasterTableView.Items[0].FindControl("rcbToTableName");
            if (String.IsNullOrEmpty(rcbFirstToTable.SelectedValue))
            {
                lblFormError.Text = "At least one join path should be specified.";
                lblFormError.Visible = true;
                return;
            }

         
            List<CleverUI.BusinessLogic.TCLCleverBL.JoinPathJoin> lstJoinPathJoin = new List<CleverUI.BusinessLogic.TCLCleverBL.JoinPathJoin>();

            foreach (GridDataItem item in rgJoinPathJoins.MasterTableView.Items)
	        {
        		 RadComboBox rcbFromTable =  item.FindControl("rcbFromTableName") as RadComboBox;
                 RadComboBox rcbFromTableKey = item.FindControl("rcbFromTableKey") as RadComboBox;
                 RadComboBox rcbToTable = item.FindControl("rcbToTableName") as RadComboBox;
                 Label lblTableJoinAlias = (Label)item.FindControl("lblTableJoinAlias");
                 Label lblJoinPathStatement = (Label)item.FindControl("lblJoinPathStatement");

                 if (!String.IsNullOrEmpty(rcbToTable.SelectedValue)) //full join path join definition
                 {
                     CleverUI.BusinessLogic.TCLCleverBL.JoinPathJoin joinPathJoin = new CleverUI.BusinessLogic.TCLCleverBL.JoinPathJoin();
                     joinPathJoin.fromTableFK = Convert.ToInt16(rcbFromTable.SelectedValue);
                     joinPathJoin.fromTableName = rcbFromTable.Text;
                     joinPathJoin.fromTableColumnKey = rcbFromTableKey.SelectedValue;
                     string[] tableKeys = rcbToTable.SelectedValue.Split(';');
                     joinPathJoin.toTableFK = Convert.ToInt16(tableKeys[0]);
                     joinPathJoin.toTableName = rcbToTable.Text;
                     joinPathJoin.joinFK = Convert.ToInt16(tableKeys[1]);
                     joinPathJoin.joinAlias = lblTableJoinAlias.Text.Trim();
                     joinPathJoin.joinStatement = lblJoinPathStatement.Text.Trim();
                     joinPathJoin.nodeSequence = item.ItemIndex + 1;
                     

                     lstJoinPathJoin.Add(joinPathJoin);
                 }
	        }

            
            if (lstJoinPathJoin.Count > 0)
            {
                //Check whether the end table is reached
                string endTableName = "";
                if (this.JoinPathPK.HasValue)
                {
                    endTableName = lblEndTable.Text.Trim();
                }
                else
                {
                    endTableName = rcbEndTable.Text.Trim();
                }

                if (endTableName != lstJoinPathJoin[lstJoinPathJoin.Count - 1].toTableName)
                {
                    lblFormError.Text = "The end table specified for the join path is not part of the join nodes.";
                    lblFormError.Visible = true;
                    return;
                }

                if (this.JoinPathPK.HasValue)
                {
                    //Edit
                    try
                    {
                        CleverUI.BusinessLogic.TCLCleverBL.JoinPath.UpdateJoinPath(this.JoinPathPK.Value,lstJoinPathJoin, chkIsDefault.Checked);
                    }
                    catch (Exception ex)
                    {
                        lblFormError.Text = "The join path is already used and cannot be updated. Error details: " + ex.Message;
                        lblFormError.Visible = true;
                        return;
                    }
                }
                else
                {
                    //Create
                    try
                    {
                        CleverUI.BusinessLogic.TCLCleverBL.JoinPath.CreateJoinPath(lstJoinPathJoin, chkIsDefault.Checked);
                    }
                    catch (Exception ex)
                    {
                        lblFormError.Text = "Error creating the join path. Error details: " + ex.Message;
                        lblFormError.Visible = true;
                        return;
                    }
                }

              

                rwFormJoinPath.Visible = false;
                rwFormJoinPath.VisibleOnPageLoad = false;
                rgJoinPaths.Rebind();
            }
          
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="rcb"></param>
        /// <param name="tablePK"></param>
        private void LoadFromTableColumnKeys(RadComboBox rcb, short tablePK)
        {
            DataTable dtTableKeys = null;
            try
            {
                dtTableKeys = CleverUI.BusinessLogic.TCLCleverBL.Tables.GetKeyColumns(tablePK);
            }
            catch (Exception ex)
            {
                lblFormError.Text = "Error getting keys on start table. Error details: " + ex.Message;
                lblFormError.Visible = true;
                return;
            }

            rcb.Items.Clear();
            if (dtTableKeys != null && dtTableKeys.Rows.Count > 0)
            {
                foreach (DataRow drKey in dtTableKeys.Rows)
                {
                    rcb.Items.Add(new RadComboBoxItem(drKey["ColumnName"].ToString(), drKey["ColumnName"].ToString()));
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rcb"></param>
        /// <param name="tablePK"></param>
        private void LoadToTables(RadComboBox rcb, short fromTablePK, string columnKeyName)
        {
            
            DataTable dtToTables = null;
            try
            {
                dtToTables = CleverUI.BusinessLogic.TCLCleverBL.Tables.GetJoinTablesByColumnKey(fromTablePK, columnKeyName);
            }
            catch (Exception ex)
            {
                lblFormError.Text = "Error getting join tables. Error details: " + ex.Message;
                lblFormError.Visible = true;
                return;
            }

            rcb.Items.Clear();
            if (dtToTables != null && dtToTables.Rows.Count > 0)
            {

                foreach (DataRow drTable in dtToTables.Rows)
                {
                    rcb.Items.Add(new RadComboBoxItem(drTable["TableName"].ToString(), drTable["TablePK"].ToString() + ';' + drTable["JoinPK"].ToString()));
                }
                rcb.Items.Insert(0, new RadComboBoxItem());
                rcb.EmptyMessage = "Select To Table";

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table1PK"></param>
        /// <param name="table2PK"></param>
        private void CheckWhetherJoinCounldBeDefault(string table1Name, string table2Name)
        {
            bool existDefaultJoin = CleverUI.BusinessLogic.TCLCleverBL.JoinPath.ExistDefaultJoinPath(table1Name, table2Name);

            if (existDefaultJoin)
            {
                chkIsDefault.Checked = false;
                chkIsDefault.Enabled = false;
            }
            else
            {
                chkIsDefault.Enabled = true;
            }
        }
    }
}
