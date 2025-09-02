using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using Telerik.Web.UI;

namespace CleverUI
{
    public partial class SelectDataSource : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorMessage.Visible = false;

            if (!IsPostBack)
            {
                CleverUI.StateManagers.SessionManager.ClearPortals();

                ((GlobalAdmin)Master).IsAdminMenuVisible = false;
                ((GlobalAdmin)Master).IsHelpMenuVisible = false;

                MembershipUser user = Membership.GetUser();
                if (user != null)
                {
                    //1. Get databases
                    DataTable dtDataSources = null;
                    try
                    {
                        dtDataSources = DataAccess.CleverDataSource.GetUserAllowedDataSources(user.UserName);
                    }
                    catch (Exception ex)
                    {
                        lblErrorMessage.Text = "Error listing user allowed Clever data sources. Error details: " + ex.Message;
                        lblErrorMessage.Visible = true;
                        return;
                    }


                    if (dtDataSources == null || dtDataSources.Rows.Count == 0)
                    {
                        lblErrorMessage.Text = "There are no data sources available for the currently logged user.";
                        lblErrorMessage.Visible = true;

                        rgDataSources.Visible = false;
                        btnGo.Visible = false;
                    }
                    else 
                    {
                        byte? selectedDataSourcePK = null;
                        string dsTitle = null;

                        //just logged in
                        if (string.IsNullOrEmpty(DataAccess.AppDataAccess.UserConnectionString))
                        {
                            if (dtDataSources.Rows.Count == 1)
                            {
                                selectedDataSourcePK = (byte)dtDataSources.Rows[0]["CleverDataSourcePK"];
                                dsTitle = (string)dtDataSources.Rows[0]["Title"];
                            }
                            else
                            {
                                foreach (DataRow row in dtDataSources.Rows)
                                {
                                    if ((bool)row["IsDefault"])
                                    {
                                        selectedDataSourcePK = (byte)row["CleverDataSourcePK"];
                                        dsTitle = (string)row["Title"];
                                        break;
                                    }
                                }
                            }
                        }
                                                
                        if (selectedDataSourcePK == null)
                        {
                            rgDataSources.Visible = true;
                            rgDataSources.DataSource = dtDataSources;
                            rgDataSources.DataBind();
                        }
                        else
                        {
                            rgDataSources.Visible = false;
                            string connString = "";
                            try
                            {
                                connString = DataAccess.CleverDataSource.GetDataSourceConnectionString(selectedDataSourcePK.Value);
                            }
                            catch (Exception ex)
                            {
                                lblErrorMessage.Text = "Error listing user allowed Clever data sources. Error details: " + ex.Message;
                                lblErrorMessage.Visible = true;
                                return;
                            }

                            if (!String.IsNullOrEmpty(connString))
                            {
                                DataAccess.AppDataAccess.ActiveDatabaseTitle = dsTitle;
                                DataAccess.AppDataAccess.UserConnectionString = connString;
                                DataAccess.AppDataAccess.UserConnectionPK = selectedDataSourcePK;
                            }
                            else
                            {
                                lblErrorMessage.Text = "Error listing user allowed Clever data sources. Error details: ";
                                lblErrorMessage.Visible = true;
                                return;
                            }


                            //Redirect to start page
                            if (Request.QueryString["ReturnUrl"] != null)
                            {
                                Response.Redirect(Request.QueryString["ReturnUrl"].ToString());
                            }
                            else
                            {
                                if (Roles.GetRolesForUser(Membership.GetUser().UserName).Contains("EventUser"))
                                {
                                    Response.Redirect("~/Events/Default.aspx");
                                }
                                else
                                {
                                    Response.Redirect("~/User/Default.aspx");
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgDataSources_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
          
            try
            {
                rgDataSources.DataSource = DataAccess.CleverDataSource.GetUserAllowedDataSources(this.Page.User.Identity.Name);
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = "Error listing user allowed Clever data sources. Error details: " + ex.Message;
                lblErrorMessage.Visible = true;
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgDataSources_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                RadioButton rdoButton = (RadioButton)item.FindControl("rbSelected");
                rdoButton.Attributes.Add("onclick", "SelectRadioButtonInGrid('" + rgDataSources.ClientID + "',this,'" + item.ItemIndex + "');");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgDataSources_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (!String.IsNullOrEmpty(DataAccess.AppDataAccess.ActiveDatabaseTitle))
            {
                if (e.Item.ItemType == GridItemType.Item || (e.Item.ItemType == GridItemType.AlternatingItem))
                {
                    if (((DataRowView)e.Item.DataItem)["Title"].ToString() == DataAccess.AppDataAccess.ActiveDatabaseTitle)
                    {
                        RadioButton rdoButton = (RadioButton)e.Item.FindControl("rbSelected");
                        rdoButton.Checked = true;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGo_Click(object sender, EventArgs e)
        {

            //1. Get the selected datasource
            byte selectedDataSourcePK = 0;
            string selectedDataSourceTitle = "";
            foreach (GridDataItem item in rgDataSources.MasterTableView.Items)
            {
                RadioButton rbSelected = (RadioButton)item["Selected"].FindControl("rbSelected");
                if (rbSelected != null && rbSelected.Checked)
                {
                    selectedDataSourcePK = (byte)rgDataSources.MasterTableView.DataKeyValues[item.ItemIndex]["CleverDataSourcePK"];
                    selectedDataSourceTitle = (string)rgDataSources.MasterTableView.DataKeyValues[item.ItemIndex]["Title"];
                    break;
                }
            }

            if (selectedDataSourcePK > 0)
            {
                //Set in session the connection string of the selected database

                string connString = "";
                try
                {
                    connString = DataAccess.CleverDataSource.GetDataSourceConnectionString(selectedDataSourcePK);
                }
                catch (Exception ex)
                {
                    lblErrorMessage.Text = "Error listing user allowed Clever data sources. Error details: " + ex.Message;
                    lblErrorMessage.Visible = true;
                    return;
                }

                if (!String.IsNullOrEmpty(connString))
                {
                    DataAccess.AppDataAccess.ActiveDatabaseTitle = selectedDataSourceTitle;
                    DataAccess.AppDataAccess.UserConnectionString = connString;
                    DataAccess.AppDataAccess.UserConnectionPK = selectedDataSourcePK;
                    CleverUI.StateManagers.SessionManager.ClearCurrentPlanDate();
                }
                else
                {
                    lblErrorMessage.Text = "Error listing user allowed Clever data sources. Error details: ";
                    lblErrorMessage.Visible = true;
                    return;
                }


                //2. Redirect to strart page
                if (Request.QueryString["ReturnUrl"] != null)
                {
                    Response.Redirect(Request.QueryString["ReturnUrl"].ToString());
                }
                else
                {
                    if (Roles.GetRolesForUser(Membership.GetUser().UserName).Contains("EventUser"))
                    {
                        Response.Redirect("~/Events/Default.aspx");
                    }
                    else
                    {
                        Response.Redirect("~/User/Default.aspx");
                    }
                }
            }
            else
            {
                lblErrorMessage.Text = "Please, select data source.";
                lblErrorMessage.Visible = true;
            }

           
        }


    }
}