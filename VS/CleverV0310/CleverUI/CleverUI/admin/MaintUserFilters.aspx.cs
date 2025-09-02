using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using CleverUI.BusinessLogic.TCLCleverBL;
using System.Web.Security;
using Telerik.Web.UI;

namespace CleverUI.Admin
{
    public partial class MaintUserFilters : System.Web.UI.Page
    {
        private DataTable dtFilters = null;


            /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            lblFormError.Visible = false;     
        }


        /// <summary>
        /// List Membership users
        /// </summary>
        /// <author>Mario Berberyan</author>
        /// <date>01/21/2011</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgUsers_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            MembershipUserCollection users = new MembershipUserCollection();

            try
            {
                users = Membership.GetAllUsers();
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error listing users. Error message: " + ex.Message;
                return;
            }

            dtFilters = new DataTable();
            try
            {
                dtFilters = Filter.ListFilterByType((short)FilterTypeEnum.UserSystem);
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error listing user filters. Error message: " + ex.Message;
                return;
            }

            DataTable dtUsers = new DataTable();
            dtUsers.Columns.Add("ProviderUserKey");
            dtUsers.Columns.Add("UserName");
            dtUsers.Columns.Add("Email");
            dtUsers.Columns.Add("IsApproved");
            dtUsers.Columns.Add("IsLockedOut");
            dtUsers.Columns.Add("CreationDate");
            dtUsers.Columns.Add("LastLoginDate");
            dtUsers.Columns.Add("FilterPK");
            dtUsers.Columns.Add("HasFilter");

            if (dtFilters.Rows.Count > 0)
            {
                foreach (MembershipUser user in users)
                {
                    DataRow dr = dtUsers.NewRow();
                    dr["ProviderUserKey"] = user.ProviderUserKey;
                    dr["UserName"] = user.UserName;
                    dr["Email"] = user.Email;
                    dr["IsApproved"] = user.IsApproved;
                    dr["IsLockedOut"] = user.IsLockedOut;
                    dr["CreationDate"] = user.CreationDate;
                    dr["LastLoginDate"] = user.LastLoginDate;
                   

                    DataRow[] rows = dtFilters.Select("UserName='" + user.UserName+ "'");
                    if (rows.Length > 0)
                    {
                        dr["FilterPK"] = rows[0]["FilterPK"].ToString();
                        dr["HasFilter"] = "Yes";
                    }
                    else
                    {
                        dr["HasFilter"] = "No";
                    }
                   
                    dtUsers.Rows.Add(dr);
                }
            }

            rgUsers.DataSource = dtUsers;            
        }

        /// <summary>
        /// Open popup window to add/edit user details
        /// </summary>
        /// <author>Mario Berberyan</author>
        /// <date>01/21/2011</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgUsers_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.EditCommandName)
            {
                if (e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FilterPK"] != DBNull.Value)
                {
                    this.ucUserFilterManagerTreeList.SelectedFilterID = int.Parse((string)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FilterPK"]);

                    this.ucUserFilterManagerTreeList.FilterType = FilterTypeEnum.UserSystem;
                    this.ucUserFilterManagerTreeList.Visible = true;
                    this.ucUserFilterManagerTreeList.ShowPopup();
                }
                else
                {
                    rcbTableName.Text = string.Empty;
                    rwAddEditUser.Visible = true;
                    rwAddEditUser.VisibleOnPageLoad = true;
                }
            }
        }

        /// <summary>
        /// Init the user add/edit form
        /// </summary>
        /// <author>Mario Berberyan</author>
        /// <date>01/21/2011</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgUsers_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.EditFormItem && e.Item.IsInEditMode)
            {
                e.Item.Edit = false;

                if (rwAddEditUser.Visible)
                {
                    txtUserName.Text = (string)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserName"];

                    try
                    {
						rcbTableName.DataSource = Tables.ListTablesForFilter();
                        rcbTableName.DataBind();
                    }
                    catch (Exception ex)
                    {
                        lblFormError.Text = "Error listing tables! Error message: " + ex.Message;
                        lblFormError.Visible = true;
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //Create system user filter
            Filter objFilter = new Filter();
            objFilter.FilterDesc = txtUserName.Text;
            objFilter.FilterType = FilterTypeEnum.UserSystem;
            objFilter.UserName = txtUserName.Text;
            objFilter.TableID = Convert.ToInt16(rcbTableName.SelectedValue);

            try
            {
                Filter.Save(objFilter);
            }
            catch (Exception ex)
            {
                lblFormError.Visible = true;
                lblFormError.Text = "Error creating user filter. Erorr message: " + ex.Message;

                rwAddEditUser.Visible = true;
                rwAddEditUser.VisibleOnPageLoad = true;
                return;
            }

            rwAddEditUser.Visible = false;
            rwAddEditUser.VisibleOnPageLoad = false;

            rgUsers.Rebind();

            DataRow[] rows = dtFilters.Select("UserName='" + txtUserName.Text + "'");
            if (rows.Length > 0)
                this.ucUserFilterManagerTreeList.SelectedFilterID = (int)rows[0]["FilterPK"];

            this.ucUserFilterManagerTreeList.Visible = true;
            this.ucUserFilterManagerTreeList.FilterType = FilterTypeEnum.UserSystem;
            this.ucUserFilterManagerTreeList.ShowPopup();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            rwAddEditUser.Visible = false;
            rwAddEditUser.VisibleOnPageLoad = false;
        }
    }
}

