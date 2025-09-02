using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CleverDAO;
using Telerik.Web.UI;
using CleverUI.InterfaceLogic;
using CleverUI.BusinessLogic.adminTCL;
using System.Web.Security;
using System.Data.SqlClient;

namespace CleverUI
{
    public partial class MaintDataSources : System.Web.UI.Page
    {


        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["app"]))
            {
                this.MasterPageFile = "~/Admin/Admin.master";
            }
            else
            {
                ((GlobalAdmin)Master).IsHelpMenuVisible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            lblFormError.Visible = false;
            lblUsersFormError.Visible = false;
        }

        protected void rgDataSources_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            rgDataSources.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.Top;
            rgDataSources.MasterTableView.NoMasterRecordsText = "No data sources to display.";
            try
            {
                rgDataSources.DataSource = DataAccess.CleverDataSource.GetAllDataSources(null);
                rgDataSources.Visible = true;
            }
            catch
            {
                lblError.Text = "Error retrieving data sources.";
                lblError.Visible = true;
                rgDataSources.Visible = false;
            }
        }

        protected void rgDataSources_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                byte dsID = (byte)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CleverDataSourcePK"];
                DataAccess.CleverDataSource.Delete(dsID);
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error deleting data source. Error message: " + ex.Message;
            }
        }

        protected void rgDataSources_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.EditCommandName)
            {
                hfIsInsert.Value = "False";
                rwAddEditDataSource.Visible = true;
                rwAddEditDataSource.VisibleOnPageLoad = true;
                rwAddEditDataSource.Title = "Update data source";
            }
            else if (e.CommandName == RadGrid.InitInsertCommandName)
            {
                hfIsInsert.Value = "True";
                rwAddEditDataSource.Visible = true;
                rwAddEditDataSource.VisibleOnPageLoad = true;
                rwAddEditDataSource.Title = "Add new data source";               
            }
            else if (e.CommandName == "AssignUsers")
            {
                rwAssignUsers.Visible = true;
                rwAssignUsers.VisibleOnPageLoad = true;
                rtvAssignUsers.Nodes.Clear();

                try
                {
                    hfDataSourceID.Value = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CleverDataSourcePK"].ToString();

                    rtvAssignUsers.Nodes.Add(new RadTreeNode((string)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Title"]));
                    rtvAssignUsers.Nodes[0].Expanded = true;

                    DataTable userDBs = UsersCleverDataSource.GetUserCleverDataSourcesByCleverDataSourceFK(byte.Parse(hfDataSourceID.Value));

                    string[] roles = Roles.GetAllRoles();
                    foreach (string r in roles)
                    {
                        RadTreeNode roleNode = new RadTreeNode(r);
                        roleNode.Expanded = true;

                        string[] users = Roles.GetUsersInRole(r);
                        foreach (string u in users)
                        {
                            RadTreeNode userNode = new RadTreeNode(u);
                            foreach (DataRow row in userDBs.Rows)
                            {
                                if ((string)row["UserName"] == u)
                                {
                                    userNode.Checked = true;
                                    userNode.Value = ((int)row["UserCleverDataSourcePK"]).ToString();
                                    break;
                                }
                            }
                            roleNode.Nodes.Add(userNode);
                        }
                        rtvAssignUsers.Nodes[0].Nodes.Add(roleNode);
                    }
                }
                catch (Exception ex)
                {
                    lblUsersFormError.Visible = true;
                    lblUsersFormError.Text = "Error getting user data sources. Error message: " + ex.Message;
                }
            }
        }

        protected void rgDataSources_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
             if (e.Item.ItemType == GridItemType.EditFormItem && e.Item.IsInEditMode)
            {
                rcbDataSourceType.DataSource = CleverDataSourceType.GetAllDataSourceTypes();
                rcbDataSourceType.DataTextField = "CleverDataSourceTypeCode";
                rcbDataSourceType.DataValueField = "CleverDataSourceTypePK";
                rcbDataSourceType.DataBind();
                
                if (!e.Item.OwnerTableView.IsItemInserted)
                {
                    e.Item.Edit = false;

                    hfID.Value = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CleverDataSourcePK"].ToString();
                    DataAccess.CleverDataSource ds = DataAccess.CleverDataSource.LoadByPK(byte.Parse(hfID.Value));

                    txtTitle.Text = ds.Title;
                    txtServerName.Text = ds.ServerName;
                    txtDescription.Text = ds.Description;
                    txtDatabasePassword.Attributes.Add("value", ds.DatabasePassowrd);
                    txtDatabaseName.Text = ds.DatabaseName;
                    txtDatabaseLogin.Text = ds.DatabaseLogin;
                    chkIsDefault.Checked = ds.IsDefault.Value;
                    rcbDataSourceType.SelectedValue = ds.CleverDataSourceTypeFK.ToString();
                }
                else
                {
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.Visible = false;

                    txtTitle.Text = string.Empty;
                    txtServerName.Text = string.Empty;
                    txtDescription.Text = string.Empty;
                    txtDatabasePassword.Attributes.Add("value", string.Empty);
                    txtDatabaseName.Text = string.Empty;
                    txtDatabaseLogin.Text = string.Empty;
                    chkIsDefault.Checked = false;
                    rcbDataSourceType.SelectedIndex = 0;
                }
            }
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            rwAddEditDataSource.Visible = false;
            rwAddEditDataSource.VisibleOnPageLoad = false;
        }

        protected void btnCancelAssignUsers_Click(object sender, EventArgs e)
        {
            rwAssignUsers.Visible = false;
            rwAssignUsers.VisibleOnPageLoad = false;
        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            SqlConnection conn = null;
            try
            {
                SqlConnectionStringBuilder bldr = new SqlConnectionStringBuilder();
                bldr.DataSource = txtServerName.Text.Trim();
                bldr.InitialCatalog = txtDatabaseName.Text.Trim();
                bldr.UserID = txtDatabaseLogin.Text.Trim();
                bldr.Password = txtDatabasePassword.Text.Trim();
                bldr.PersistSecurityInfo = true;
                conn = new SqlConnection(bldr.ConnectionString);
                conn.Open();

                lblFormError.Text = "Data Source opened successfully.";
                lblFormError.Visible = true;
            }
            catch (Exception ex)
            {
                lblFormError.Text = ex.Message;
                lblFormError.Visible = true;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        protected void btnSaveAssignUsers_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (RadTreeNode roleNode in rtvAssignUsers.Nodes[0].Nodes)
                {
                    foreach (RadTreeNode userNode in roleNode.Nodes)
                    {
                        if (userNode.Checked && string.IsNullOrEmpty(userNode.Value))
                        {
                            //insert
                            UsersCleverDataSource.InsertUserCleverDataSource(byte.Parse(hfDataSourceID.Value), userNode.Text);
                        }
                        else if (!userNode.Checked && !string.IsNullOrEmpty(userNode.Value))
                        {
                            //delete
                            UsersCleverDataSource.DeleteUserCleverDataSource(int.Parse(userNode.Value));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblUsersFormError.Visible = true;
                lblUsersFormError.Text = "Error saving user data sources. Erorr message: " + ex.Message;
                return;
            }

            rwAssignUsers.Visible = false;
            rwAssignUsers.VisibleOnPageLoad = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataAccess.CleverDataSource ds = new DataAccess.CleverDataSource();
            ds.DatabaseLogin = txtDatabaseLogin.Text.Trim();
            ds.DatabaseName = txtDatabaseName.Text.Trim();
            ds.ServerName = txtServerName.Text.Trim();
            ds.Description = txtDescription.Text.Trim();
            ds.Title = txtTitle.Text.Trim();
            ds.CleverDataSourceTypeFK = byte.Parse(rcbDataSourceType.SelectedValue);
            ds.DatabasePassowrd = txtDatabasePassword.Text.Trim();
            ds.IsDefault = chkIsDefault.Checked;

            if (Convert.ToBoolean(hfIsInsert.Value))
            {
                ds.DateCreated = DateTime.Now;
            }
            else
            {
                ds.CleverDataSourcePK = byte.Parse(hfID.Value);
            }

            try
            {
                DataAccess.CleverDataSource.Save(ds);
            }
            catch (Exception ex)
            {
                lblFormError.Visible = true;
                lblFormError.Text = "Error saving data source. Erorr message: " + ex.Message;
                return;
            }
            
            rwAddEditDataSource.Visible = false;
            rwAddEditDataSource.VisibleOnPageLoad = false;

            rgDataSources.Rebind();
        }


    }
}