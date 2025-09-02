using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;
using System.Web.UI.HtmlControls;
using CleverUI.BusinessLogic.TCLCleverBL;

namespace CleverUI.Admin
{
    public partial class etlTransformationSpec : System.Web.UI.Page
    {
        #region Fields & Properties

        protected short? FilterCleverTable
        {
            get
            {
                short? val = null;
                if (ViewState["FilterCleverTable"] != null)
                {
                    val = Convert.ToInt16(ViewState["FilterCleverTable"]);
                }
                return val;
            }
            set
            {
                ViewState["FilterCleverTable"] = value;
            }
        }

        protected short? FilterCustTable
        {
            get
            {
                short? val = null;
                if (ViewState["FilterCustTable"] != null)
                {
                    val = Convert.ToInt16(ViewState["FilterCustTable"]);
                }
                return val;
            }
            set
            {
                ViewState["FilterCustTable"] = value;
            }
        }

        //Selected for edit transformation
        protected short? TransID
        {
            get
            {
                short? val = null;
                if (ViewState["TransID"] != null)
                {
                    val = Convert.ToInt16(ViewState["TransID"]);
                }
                return val;
            }
            set
            {
                ViewState["TransID"] = value;
            }
        }
        protected short? TransDetailsID
        {
            get
            {
                short? val = null;
                if (ViewState["TransDetailsID"] != null)
                {
                    val = Convert.ToInt16(ViewState["TransDetailsID"]);
                }
                return val;
            }
            set
            {
                ViewState["TransDetailsID"] = value;
            }
        }
        protected DataTable dtCustTables;
        protected DataTable dtOneToManyDtls;


        protected string SelectedBusUnit
        {
            get
            {
                string val = "BIDS";
                if (!String.IsNullOrEmpty(rcbBusUnit.SelectedValue))
                {
                    val = rcbBusUnit.SelectedValue;
                }
                return val;
            }
           
        }

        #endregion Fields & Properties

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
                rcbBusUnit.DataSource = CleverUI.BusinessLogic.TCLCleverBL.Ref.BusUnit.List(false);
                rcbBusUnit.DataBind();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        protected void rcbFilterCleverTable_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            if (IsPostBack)
            {
                string code = e.Text.Trim();
                code = code != "" ? code : null;

                int itemOffset = e.NumberOfItems;
                int itemsCount = rcbFilterCleverTable.ItemsPerRequest;
                int total = 0;


                DataTable dtTables = CleverUI.BusinessLogic.TCLCleverBL.Tables.ListAll();
                DataView dw = new DataView(dtTables);
                dw.RowFilter = "TableName like '" + code + "%'";
                dw.Sort = "TableName ASC";
                DataTable dt = dw.ToTable();

                total = dt.Rows.Count;

                int endOffset = itemOffset + itemsCount;
                if (endOffset > dt.Rows.Count)
                {
                    endOffset = dt.Rows.Count;
                }

                rcbFilterCleverTable.Items.Clear();
                rcbFilterCleverTable.Text = String.Empty;
                for (int i = itemOffset; i < endOffset; i++)
                {
                    rcbFilterCleverTable.Items.Add(new RadComboBoxItem(dt.Rows[i][rcbFilterCleverTable.DataTextField].ToString()
                                                        , dt.Rows[i][rcbFilterCleverTable.DataValueField].ToString()));
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
        /// <param name="o"></param>
        /// <param name="e"></param>
        protected void rcbFilterCustTable_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            if (IsPostBack)
            {
                string code = e.Text.Trim();
                code = code != "" ? code : null;

                int itemOffset = e.NumberOfItems;
                int itemsCount = rcbFilterCustTable.ItemsPerRequest;
                int total = 0;

                DataTable dtTables = CleverUI.BusinessLogic.TCLogic.CustTable.ListAll(this.SelectedBusUnit);
                DataView dw = new DataView(dtTables);
                dw.RowFilter = "TableName like '" + code + "%'";
                dw.Sort = "TableName ASC";
                DataTable dt = dw.ToTable();

                total = dt.Rows.Count;

                int endOffset = itemOffset + itemsCount;
                if (endOffset > dt.Rows.Count)
                {
                    endOffset = dt.Rows.Count;
                }

                rcbFilterCustTable.Items.Clear();
                rcbFilterCustTable.Text = String.Empty;
                for (int i = itemOffset; i < endOffset; i++)
                {
                    rcbFilterCustTable.Items.Add(new RadComboBoxItem(dt.Rows[i][rcbFilterCustTable.DataTextField].ToString()
                                                        , dt.Rows[i][rcbFilterCustTable.DataValueField].ToString()));
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
            if (!String.IsNullOrEmpty(rcbFilterCleverTable.SelectedValue))
            {
                this.FilterCleverTable = Convert.ToInt16(rcbFilterCleverTable.SelectedValue);
            }
            else
            {
                this.FilterCleverTable = null;
            }

            if (!String.IsNullOrEmpty(rcbFilterCustTable.SelectedValue))
            {
                this.FilterCustTable = Convert.ToInt16(rcbFilterCustTable.SelectedValue);
            }
            else
            {
                this.FilterCustTable = null;
            }

            rgTransformations.Rebind();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgTransformations_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            //if (this.FilterCleverTable.HasValue || this.FilterCustTable.HasValue)
            //{
            try
            {
                rgTransformations.DataSource = CleverUI.BusinessLogic.ETL.TransformCustClvr.Search(this.SelectedBusUnit, this.FilterCleverTable, this.FilterCustTable);
                rgTransformations.Visible = true;
            }
            catch
            {
                lblError.Text = "Error retrieving the transformation notes.";
                lblError.Visible = true;
                rgTransformations.Visible = false;
            }
            //}

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgTransformations_ItemDataBound(object sender, GridItemEventArgs e)
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
                    //new item will be created
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.Visible = false;
                }
            }

            else if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                DataRowView dataItem = (DataRowView)e.Item.DataItem;
                string currentTransTypeCode = Convert.ToString(dataItem["TransTypeCode"]);
                short transID = Convert.ToInt16(dataItem["transformdataPK"]);

                HtmlGenericControl divOneToOne = (HtmlGenericControl)e.Item.FindControl("divOneToOne");
                HtmlGenericControl divOneToMany = (HtmlGenericControl)e.Item.FindControl("divOneToMany");
                HtmlGenericControl divRefValue = (HtmlGenericControl)e.Item.FindControl("divRefValue");
                HtmlGenericControl divOther = (HtmlGenericControl)e.Item.FindControl("divOther");

                divOneToOne.Visible = false;
                divOneToMany.Visible = false;
                divRefValue.Visible = false;
                divOther.Visible = false;

                switch (currentTransTypeCode)
                {

                    case "OneToMany":
                        divOneToMany.Visible = true;
                        Repeater repRelations = (Repeater)e.Item.FindControl("repRelations");

                        DataTable dtTransDetails = CleverUI.BusinessLogic.ETL.TransformCustClvr.GetOneToManyTransDetails(this.SelectedBusUnit,transID);
                        repRelations.DataSource = dtTransDetails;
                        repRelations.DataBind();

                        break;
                    case "RefValue":
                        divRefValue.Visible = true;
                        Label refTableName = (Label)e.Item.FindControl("refTableName");
                        refTableName.Text = ((DataRowView)e.Item.DataItem)["refTableName"].ToString();
                        Label refValue = (Label)e.Item.FindControl("refValue");
                        refValue.Text = ((DataRowView)e.Item.DataItem)["refTableCode"].ToString();
                        break;
                    case "RefTable":
                        divOneToOne.Visible = true;
                        Label lblRefCustTable = (Label)e.Item.FindControl("lblCustTable");
                        Label lblRefCustColumn = (Label)e.Item.FindControl("lblCustColumn");

                        DataRow drRefTransDetails = CleverUI.BusinessLogic.ETL.TransformCustClvr.GetOneToOneTransDetails(this.SelectedBusUnit,transID);
                        lblRefCustTable.Text = drRefTransDetails["custTableName"].ToString();
                        lblRefCustColumn.Text = drRefTransDetails["custColumnName"].ToString();

                        break;
                    case "OneToOne":
                        divOneToOne.Visible = true;
                        Label lblCustTable = (Label)e.Item.FindControl("lblCustTable");
                        Label lblCustColumn = (Label)e.Item.FindControl("lblCustColumn");

                        DataRow drTransDetails = CleverUI.BusinessLogic.ETL.TransformCustClvr.GetOneToOneTransDetails(this.SelectedBusUnit,transID);
                        lblCustTable.Text = drTransDetails["custTableName"].ToString();
                        lblCustColumn.Text = drTransDetails["custColumnName"].ToString();

                        break;
                    default:
                        divOther.Visible = true;
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgTransformations_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.EditCommandName || e.CommandName == RadGrid.InitInsertCommandName)
            {
                //Init trans details form
                rcbTransType.DataSource = CleverUI.BusinessLogic.ETL.TransformCustClvr.ListTransTypes(this.SelectedBusUnit,true);
                rcbTransType.DataBind();
                rcbTransType.SelectedValue = "2"; //Initial Value: OneToOne transformation

            }

            if (e.CommandName == RadGrid.EditCommandName)
            {
                this.TransID = Convert.ToInt16(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["transformdataPK"]);
                this.TransDetailsID = null;
                rwTransDetails.Title = "Update transformation";
                rwTransDetails.Visible = true;
                rwTransDetails.VisibleOnPageLoad = true;

                //Fill common details 
                CleverUI.BusinessLogic.ETL.TransformCustClvr objTrans = CleverUI.BusinessLogic.ETL.TransformCustClvr.Load(this.SelectedBusUnit,this.TransID.Value);
                //  Trans Type
                rcbTransType.SelectedValue = objTrans.TransTypeFK.Value.ToString();
                //  Clever Table
                rcbCleverTable.Text = objTrans.clvrTableName;
                rcbCleverTable.SelectedValue = objTrans.clvrTableFK.Value.ToString();
                //  Clever Column
                if (objTrans.clvrColumnFK.HasValue)
                {
                    rcbCleverColumn.Text = objTrans.clvrColumnName;
                    rcbCleverColumn.SelectedValue = objTrans.clvrColumnFK.Value.ToString();
                }
                else
                {
                    rcbCleverColumn.Text = "";
                    rcbCleverColumn.SelectedValue = "";
                }
                //  Desc & Memo
                txtTransDesc.Text = objTrans.TransformdataDesc;
                txtTransMemo.Text = objTrans.TransformMemo;


                //Fill specific details
                InitTransTypeForm(objTrans.TransTypeCode);

                switch (objTrans.TransTypeCode)
                {
                    case "Unassigned":
                        break;
                    case "Excluded":
                        break;
                    case "OneToMany":

                        dtOneToManyDtls = CleverUI.BusinessLogic.ETL.TransformCustClvr.GetOneToManyTransDetails(this.SelectedBusUnit,this.TransID.Value);


                        break;
                    case "RefValue":
                        if (objTrans.refTableFK.HasValue)
                        {
                            rcbRefTable.Text = objTrans.RefTableName;
                            rcbRefTable.SelectedValue = objTrans.refTableFK.Value.ToString();
                        }
                        if (objTrans.refKeyPK.HasValue)
                        {
                            rcbRefValue.SelectedValue = objTrans.refKeyPK.Value.ToString();
                        }
                        txtRefValue.Text = "";
                        break;
                    default:

                        DataRow drTransDetails = CleverUI.BusinessLogic.ETL.TransformCustClvr.GetOneToOneTransDetails(this.SelectedBusUnit,this.TransID.Value);
                        if (drTransDetails != null)
                        {
                            rcbCustTable.Text = drTransDetails["custTableName"].ToString();
                            rcbCustTable.SelectedValue = drTransDetails["custTableFK"].ToString();

                            short custTablePK = Convert.ToInt16(rcbCustTable.SelectedValue);
                            rcbCustColumn.DataSource = CleverUI.BusinessLogic.TCLogic.CustTable.ListColumns(this.SelectedBusUnit,custTablePK);
                            rcbCustColumn.DataBind();

                            rcbCustColumn.Text = drTransDetails["custColumnName"].ToString();
                            rcbCustColumn.SelectedValue = drTransDetails["custColumnFK"].ToString();
                            this.TransDetailsID = Convert.ToInt16(drTransDetails["transformDetailPK"]);
                        }

                        break;
                }

            }
            else if (e.CommandName == RadGrid.InitInsertCommandName)
            {
                this.TransID = null;
                this.TransDetailsID = null;

                rwTransDetails.Title = "Create transformation";
                rwTransDetails.Visible = true;
                rwTransDetails.VisibleOnPageLoad = true;

                //Clear form
                InitTransTypeForm(rcbTransType.SelectedValue);

                rcbCleverTable.SelectedValue = "";
                rcbCleverTable.Text = "";

                rcbCleverColumn.SelectedValue = "";
                rcbCleverColumn.Text = "";

                rcbCustTable.Text = "";
                rcbCustTable.SelectedValue = "";

                rcbCustColumn.Text = "";
                rcbCustColumn.SelectedValue = "";

                rcbRefTable.Text = "";
                rcbRefValue.SelectedValue = "";

                rcbRefValue.SelectedValue = "";
                rcbRefValue.Text = "";
                rcbRefValue.Items.Clear();

                txtTransDesc.Text = "";
                txtTransMemo.Text = "";


            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgTransformations_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            this.TransID = Convert.ToInt16(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["transformdataPK"]);

            try
            {
                CleverUI.BusinessLogic.ETL.TransformCustClvr.Delete(this.SelectedBusUnit,this.TransID.Value);
            }
            catch (Exception ex)
            {
                lblError.Text = "Error deleting the transformation. Error details: " + ex.Message;
                lblError.Visible = true;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rcbTransType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            InitTransTypeForm(e.Text.Trim());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rcbCleverTable_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            if (IsPostBack)
            {
                string code = e.Text.Trim();
                code = code != "" ? code : null;

                int itemOffset = e.NumberOfItems;
                int itemsCount = rcbCleverTable.ItemsPerRequest;
                int total = 0;

                DataTable dtTables = null;
                if (rcbTransType.Text == "RefTable")
                {
                    dtTables = CleverUI.BusinessLogic.TCLCleverBL.Tables.ListRefTables();
                }
                else
                {
                    dtTables = CleverUI.BusinessLogic.TCLCleverBL.Tables.ListAll();
                }
                DataView dw = new DataView(dtTables);
                dw.RowFilter = "TableName like '" + code + "%'";
                dw.Sort = "TableName ASC";
                DataTable dt = dw.ToTable();

                total = dt.Rows.Count;

                int endOffset = itemOffset + itemsCount;
                if (endOffset > dt.Rows.Count)
                {
                    endOffset = dt.Rows.Count;
                }

                rcbCleverTable.Items.Clear();
                rcbCleverTable.Text = String.Empty;
                for (int i = itemOffset; i < endOffset; i++)
                {
                    rcbCleverTable.Items.Add(new RadComboBoxItem(dt.Rows[i][rcbCleverTable.DataTextField].ToString()
                                                        , dt.Rows[i][rcbCleverTable.DataValueField].ToString()));
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
        protected void rcbCleverColumn_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            if (IsPostBack && !String.IsNullOrEmpty(rcbCleverTable.SelectedValue))
            {
                string code = e.Text.Trim();
                code = code != "" ? code : null;

                int itemOffset = e.NumberOfItems;
                int itemsCount = rcbCleverTable.ItemsPerRequest;
                int total = 0;

                short tablePK = Convert.ToInt16(rcbCleverTable.SelectedValue);
                DataTable dtColumns = CleverUI.BusinessLogic.TCLCleverBL.Tables.GetColumns(tablePK);

                DataView dw = new DataView(dtColumns);
                dw.RowFilter = "ColumnName like '" + code + "%'";
                dw.Sort = "ColumnName ASC";
                DataTable dt = dw.ToTable();

                total = dt.Rows.Count;

                int endOffset = itemOffset + itemsCount;
                if (endOffset > dt.Rows.Count)
                {
                    endOffset = dt.Rows.Count;
                }

                rcbCleverColumn.Items.Clear();
                rcbCleverColumn.Text = String.Empty;
                for (int i = itemOffset; i < endOffset; i++)
                {
                    rcbCleverColumn.Items.Add(new RadComboBoxItem(dt.Rows[i][rcbCleverColumn.DataTextField].ToString()
                                                        , dt.Rows[i][rcbCleverColumn.DataValueField].ToString()));
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
        protected void rcbRefTable_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            if (IsPostBack)
            {
                string code = e.Text.Trim();
                code = code != "" ? code : null;

                int itemOffset = e.NumberOfItems;
                int itemsCount = rcbRefTable.ItemsPerRequest;
                int total = 0;

                DataTable dtTables = CleverUI.BusinessLogic.TCLCleverBL.Tables.ListRefTables();

                DataView dw = new DataView(dtTables);
                dw.RowFilter = "TableName like '" + code + "%'";
                dw.Sort = "TableName ASC";
                DataTable dt = dw.ToTable();

                total = dt.Rows.Count;

                int endOffset = itemOffset + itemsCount;
                if (endOffset > dt.Rows.Count)
                {
                    endOffset = dt.Rows.Count;
                }

                rcbRefTable.Items.Clear();
                rcbRefTable.Text = String.Empty;
                for (int i = itemOffset; i < endOffset; i++)
                {
                    rcbRefTable.Items.Add(new RadComboBoxItem(dt.Rows[i][rcbRefTable.DataTextField].ToString()
                                                        , dt.Rows[i][rcbRefTable.DataValueField].ToString()));
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
        protected void rcbRefTable_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Value))
            {
                short tablePK = Convert.ToInt16(e.Value);
                ListRefTableValues(tablePK);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rcbCustTable_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Value))
            {
                short tablePK = Convert.ToInt16(e.Value);
                rcbCustColumn.Items.Clear();
                rcbCustColumn.DataSource = CleverUI.BusinessLogic.TCLogic.CustTable.ListColumns(this.SelectedBusUnit,tablePK);
                rcbCustColumn.DataBind();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgCustColumns_PreRender(object sender, System.EventArgs e)
        {
            foreach (GridItem item in rgCustColumns.MasterTableView.Items)
            {
                if (item is GridEditableItem)
                {
                    GridEditableItem editableItem = item as GridDataItem;
                    editableItem.Edit = true;
                }
            }
           rgCustColumns.Rebind();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgCustColumns_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                //attach SelectedIndexChanged event for the dropdown FromTableName  
                RadComboBox rcCustTableName = (e.Item as GridEditableItem)["CustTableName"].Controls[0] as RadComboBox;
                rcCustTableName.AutoPostBack = true;
                rcCustTableName.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(GridCustTableName_SelectedIndexChanged);
                rcCustTableName.Skin = "Default";


                RadComboBox rcCustColumnName = (e.Item as GridEditableItem)["CustColumnName"].Controls[0] as RadComboBox;
                rcCustColumnName.Skin = "Default";

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgCustColumns_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && (e.Item as GridEditableItem).IsInEditMode)
            {
                GridEditableItem editedItem = e.Item as GridEditableItem;
                GridEditManager editMan = editedItem.EditManager;

                //Cust Table 
                GridDropDownListColumnEditor editorCustTable = editMan.GetColumnEditor("CustTableName") as GridDropDownListColumnEditor;
                RadComboBox rcbCustTableName = editorCustTable.ComboBoxControl;
                rcbCustTableName.DataTextField = "TableName";
                rcbCustTableName.DataValueField = "TablePK";
                rcbCustTableName.DataSource = this.dtCustTables;
                rcbCustTableName.DataBind();
                rcbCustTableName.Items.Insert(0, new RadComboBoxItem());

                //Cust Column
                GridDropDownListColumnEditor editorCustColumn = editMan.GetColumnEditor("CustColumnName") as GridDropDownListColumnEditor;
                RadComboBox rcbCustColumn = editorCustColumn.ComboBoxControl;
                rcbCustColumn.DataTextField = "ColumnName";
                rcbCustColumn.DataValueField = "ColumnPK";


                if (this.dtOneToManyDtls != null
                    && this.dtOneToManyDtls.Rows.Count > e.Item.ItemIndex)
                {

                    DataRow dr = this.dtOneToManyDtls.Rows[e.Item.ItemIndex];

                    rcbCustTableName.SelectedValue = dr["CustTableFK"].ToString();
                    short tableID = Convert.ToInt16(dr["CustTableFK"]);
                    rcbCustColumn.DataSource = CleverUI.BusinessLogic.TCLogic.CustTable.ListColumns(this.SelectedBusUnit,tableID);
                    rcbCustColumn.DataBind();
                    rcbCustColumn.SelectedValue = dr["CustColumnFK"].ToString();


                    ((TextBox)e.Item.FindControl("txtDesc")).Text = dr["transformdataDesc"].ToString();
                    ((TextBox)e.Item.FindControl("txtMemo")).Text = dr["transformMemo"].ToString();

                    ((HiddenField)e.Item.FindControl("hfPK")).Value = dr["transformDetailPK"].ToString();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridCustTableName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            GridEditableItem editedItem = ((sender as RadComboBox).NamingContainer as GridEditableItem);
            RadComboBox rcbCustTable = editedItem["CustTableName"].Controls[0] as RadComboBox;
            RadComboBox rcbCustColumn = editedItem["CustColumnName"].Controls[0] as RadComboBox;

            if (!String.IsNullOrEmpty(rcbCustTable.SelectedValue))
            {
                short tableID = Convert.ToInt16(rcbCustTable.SelectedValue);

                rcbCustColumn.DataSource = CleverUI.BusinessLogic.TCLogic.CustTable.ListColumns(this.SelectedBusUnit,tableID);
                rcbCustColumn.DataBind();
                rcbCustColumn.Items.Insert(0, new RadComboBoxItem());
            }
            else
            {
                rcbCustColumn.Items.Clear();
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string refValueCode = txtRefValue.Text.Trim();
            string refTableName = rcbCleverTable.Text.Trim();

            if (!String.IsNullOrEmpty(refValueCode) && !String.IsNullOrEmpty(refTableName))
            {

                short refTableId = Convert.ToInt16(rcbCleverTable.SelectedValue);

                //Add the entered value in the reference table
                short key = 0;
                try
                {
                    key = Tables.AddValueToRefTable(refTableName, refValueCode);
                }
                catch (Exception ex)
                {
                    lblFormError.Text = "Error adding a value to the reference table. Error details: " + ex.Message;
                    lblFormError.Visible = true;
                    return;
                }

                ListRefTableValues(refTableId);
                rcbRefValue.SelectedValue = key.ToString();

                txtRefValue.Text = "";
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            rwTransDetails.Visible = false;
            rwTransDetails.VisibleOnPageLoad = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            CleverUI.BusinessLogic.ETL.TransformCustClvr objTrans = null;
            if (!this.TransID.HasValue)
            {
                //Create trnasformation
                objTrans = new CleverUI.BusinessLogic.ETL.TransformCustClvr();
            }
            else
            {
                //Edit transformation
                try
                {
                    objTrans = CleverUI.BusinessLogic.ETL.TransformCustClvr.Load(this.SelectedBusUnit,this.TransID.Value);
                }
                catch (Exception ex)
                {
                    lblFormError.Text = "Error loading transformation details. Error details: " + ex.Message;
                    lblFormError.Visible = true;
                    rwTransDetails.Visible = true;
                    rwTransDetails.VisibleOnPageLoad = true;
                    return;
                }

                if (objTrans == null)
                {
                    lblFormError.Text = "The transformation doesn't exist anymore on the database.";
                    lblFormError.Visible = true;
                    rwTransDetails.Visible = true;
                    rwTransDetails.VisibleOnPageLoad = true;
                    return;
                }
            }

            //Get entered settings
            objTrans.clvrTableFK = Convert.ToInt16(rcbCleverTable.SelectedValue);
            objTrans.clvrTableName = rcbCleverTable.Text.Trim();

            objTrans.TransTypeFK = Convert.ToInt16(rcbTransType.SelectedValue);
            objTrans.TransformdataDesc = txtTransDesc.Text.Trim();
            objTrans.TransformMemo = txtTransMemo.Text.Trim();

            if (!String.IsNullOrEmpty(rcbCleverColumn.SelectedValue))
            {
                objTrans.clvrColumnFK = Convert.ToInt16(rcbCleverColumn.SelectedValue);
                objTrans.clvrColumnName = rcbCleverColumn.Text.Trim();
            }
            else
            {
                objTrans.clvrColumnFK = null;
                objTrans.clvrColumnName = null;
            }

            if (!String.IsNullOrEmpty(rcbRefTable.SelectedValue))
            {
                objTrans.refTableFK = Convert.ToInt16(rcbRefTable.SelectedValue);
                objTrans.RefTableName = rcbRefTable.Text.Trim();
            }
            else
            {
                objTrans.refTableFK = null;
                objTrans.RefTableName = null;
            }

            if (!String.IsNullOrEmpty(rcbRefValue.SelectedValue))
            {
                objTrans.refKeyPK = Convert.ToInt16(rcbRefValue.SelectedValue);
                objTrans.refTableCode = rcbRefValue.Text.Trim();
            }
            else
            {
                objTrans.refKeyPK = null;
                objTrans.refTableCode = null;
            }

            using (IDbConnection conn = DataAccess.ETLDataAccess.OpenConnection(this.SelectedBusUnit))
            {
                IDbTransaction tran = conn.BeginTransaction();
                //Save
                try
                {
                    string transType = rcbTransType.Text.Trim();
                    CleverUI.BusinessLogic.ETL.TransformCustClvr.Save(objTrans, tran);
                    this.TransID = objTrans.TransformdataPK;

                    if (objTrans.TransformdataPK.HasValue
                        && (transType != "RefValue"))
                    {

                        switch (transType)
                        {
                            case "OneToMany":

                                foreach (GridDataItem item in rgCustColumns.MasterTableView.Items)
                                {
                                    RadComboBox rcbCustTableName = item["CustTableName"].Controls[0] as RadComboBox;
                                    RadComboBox rcbCustColumnName = item["CustColumnName"].Controls[0] as RadComboBox;
                                    TextBox txtDesc = (TextBox)item["Desc"].FindControl("txtDesc");
                                    TextBox txtMemo = (TextBox)item["Memo"].FindControl("txtMemo");
                                    HiddenField hfPK = (HiddenField)item["Desc"].FindControl("hfPK");

                                    if (!String.IsNullOrEmpty(rcbCustTableName.SelectedValue))
                                    {
                                        CleverUI.BusinessLogic.ETL.TransformCustClvrDetail transDetail = new CleverUI.BusinessLogic.ETL.TransformCustClvrDetail();
                                        transDetail.TransformdataFK = this.TransID;

                                        if (!String.IsNullOrEmpty(hfPK.Value))
                                        {
                                            transDetail.TransformDetailPK = Convert.ToInt16(hfPK.Value);
                                        }

                                        transDetail.CustTableFK = Convert.ToInt16(rcbCustTableName.SelectedValue);
                                        transDetail.CustTableName = rcbCustTableName.Text.Trim();

                                        transDetail.CustColumnFK = Convert.ToInt16(rcbCustColumnName.SelectedValue);
                                        transDetail.CustColumnName = rcbCustColumnName.Text.Trim();

                                        transDetail.TransformdataDesc = txtDesc.Text.Trim();
                                        transDetail.TransformMemo = txtMemo.Text.Trim();

                                        CleverUI.BusinessLogic.ETL.TransformCustClvrDetail.Save(transDetail, tran);
                                    }
                                }

                                break;
                            default:
                                CleverUI.BusinessLogic.ETL.TransformCustClvrDetail objTransDetail = null;

                                if (this.TransDetailsID.HasValue)
                                {
                                    objTransDetail = CleverUI.BusinessLogic.ETL.TransformCustClvrDetail.Load(this.SelectedBusUnit,this.TransDetailsID.Value);
                                }
                                else
                                {
                                    objTransDetail = new CleverUI.BusinessLogic.ETL.TransformCustClvrDetail();
                                }
                                if (objTransDetail != null)
                                {
                                    objTransDetail.TransformdataFK = objTrans.TransformdataPK;

                                    if (!String.IsNullOrEmpty(rcbCustTable.SelectedValue))
                                    {
                                        objTransDetail.CustTableFK = Convert.ToInt16(rcbCustTable.SelectedValue);
                                        objTransDetail.CustTableName = rcbCustTable.Text.Trim();
                                    }
                                    if (!String.IsNullOrEmpty(rcbCustColumn.SelectedValue))
                                    {
                                        objTransDetail.CustColumnFK = Convert.ToInt16(rcbCustColumn.SelectedValue);
                                        objTransDetail.CustColumnName = rcbCustColumn.Text.Trim();
                                    }
                                }

                                CleverUI.BusinessLogic.ETL.TransformCustClvrDetail.Save(objTransDetail, tran);

                                break;
                        }
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();

                    lblFormError.Text = "Error saving transformation details. Error details: " + ex.Message;
                    lblFormError.Visible = true;
                    rwTransDetails.Visible = true;
                    rwTransDetails.VisibleOnPageLoad = true;

                    return;
                }
                finally
                {
                    conn.Close();
                }
            }

            //close popup after successful save and rebind grid
            rwTransDetails.Visible = false;
            rwTransDetails.VisibleOnPageLoad = false;

            rgTransformations.Rebind();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="transTypeCode"></param>
        private void InitTransTypeForm(string transTypeCode)
        {
            if (transTypeCode == "RefTable")
            {
                trCleverColumn.Visible = false;
            }
            else
            {
                trCleverColumn.Visible = true;
            }
            phOneToOne.Visible = false;
            phOneToMany.Visible = false;
            phRefValue.Visible = false;

            switch (transTypeCode)
            {
                case "OneToMany":
                    phOneToMany.Visible = true;

                    this.dtCustTables = CleverUI.BusinessLogic.TCLogic.CustTable.ListAll(this.SelectedBusUnit);

                    DataTable dt = new DataTable();
                    dt.Columns.Add("CustTableName");
                    dt.Columns.Add("CustColumnName");
                    dt.Columns.Add("TransDesc");
                    dt.Columns.Add("TransMemo");

                    dt.Rows.Add(dt.NewRow());
                    dt.Rows.Add(dt.NewRow());
                    dt.Rows.Add(dt.NewRow());
                    dt.Rows.Add(dt.NewRow());
                    rgCustColumns.DataSource = dt;
                    rgCustColumns.DataBind();

                    break;
                case "RefValue":
                    phRefValue.Visible = true;
                    if (!String.IsNullOrEmpty(rcbRefTable.SelectedValue))
                    {
                        int refTableId = Convert.ToInt32(rcbRefTable.SelectedValue);
                        ListRefTableValues(refTableId);
                    }
                    break;
                case "Unassigned":
                    break;
                case "Excluded":
                    break;
                default:
                    phOneToOne.Visible = true;
                    rcbCustTable.DataSource = CleverUI.BusinessLogic.TCLogic.CustTable.ListAll(this.SelectedBusUnit);
                    rcbCustTable.DataBind();
                    rcbCustTable.Items.Insert(0, new RadComboBoxItem());
                    rcbCustColumn.Items.Clear();
                    break;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refTableId"></param>
        private void ListRefTableValues(int refTableId)
        {
            DataTable dtUpdateValues = null;
            try
            {
                dtUpdateValues = Tables.ListRefTableColumnValues(refTableId);
            }
            catch (Exception ex)
            {
                lblError.Text = "Error loading update value options. Error details: " + ex.Message;
                lblError.Visible = true;
            }

            rcbRefValue.Items.Clear();
            rcbRefValue.DataTextField = "ColDesc";
            rcbRefValue.DataValueField = "ColPK";
            rcbRefValue.DataSource = dtUpdateValues;
            rcbRefValue.DataBind();
            rcbRefValue.Items.Insert(0, new RadComboBoxItem("", ""));
        }
    }
}
