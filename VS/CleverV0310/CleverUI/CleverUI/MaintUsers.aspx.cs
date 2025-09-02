using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Telerik.Web.UI;
using System.Collections;
using System.Data;
using CleverUI.BusinessLogic.adminTCL;

namespace CleverUI
{
    public partial class MaintUsers : System.Web.UI.Page
    {
        private DataTable userDS = null;


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


        protected void rgDataSources_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                if (userDS != null)
                {
                    byte dsID = (byte)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CleverDataSourcePK"];

                    foreach (DataRow row in userDS.Rows)
                    {
                        if ((byte)row["CleverDataSourceFK"] == dsID)
                        {
                            CheckBox btnDS = (CheckBox)e.Item.FindControl("chkDataSource");
                            btnDS.Checked = true;
                        }
                    }
                }
            }
        }

        protected void rgDataSources_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                DataTable dsRows = DataAccess.CleverDataSource.GetAllDataSources(null);
                dsRows.DefaultView.Sort = "Title";
                rgDataSources.DataSource = dsRows.DefaultView;
            }
            catch (Exception ex)
            {
                lblFormError.Visible = true;
                lblFormError.Text = "Error listing data sources. Error message: " + ex.Message;
                return;
            } 
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
            try
            {
                rgUsers.DataSource = Membership.GetAllUsers();
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error listing users. Error message: " + ex.Message;
                return;
            }      
        }

        /// <summary>
        /// Delete selected user from system
        /// </summary>
        /// <author>Mario Berberyan</author>
        /// <date>01/21/2011</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgUsers_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            Guid guid = new Guid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ProviderUserKey"].ToString());
            MembershipUser user = Membership.GetUser(guid);

            if (user.UserName.ToLower() == this.Page.User.Identity.Name.ToLower())
            {
                lblError.Text = "Cannot delete current user!";
                lblError.Visible = true;
                return;
            }

            try
            {
                UsersCleverDataSource.DeleteUserCleverDataSourcesByUserId(guid);

                Membership.DeleteUser(user.UserName);
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error deleting user details. Error message: " + ex.Message;
                return;
            }
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
                hfIsInsert.Value = "False";
                rwAddEditUser.Visible = true;
                rwAddEditUser.VisibleOnPageLoad = true;
                rwAddEditUser.Title = "Update user details";
            }
            else if (e.CommandName == RadGrid.InitInsertCommandName)
            {
                hfIsInsert.Value = "True";
                rwAddEditUser.Visible = true;
                rwAddEditUser.VisibleOnPageLoad = true;
                  rwAddEditUser.Title = "Add new user";
            }
            else if (e.CommandName == "Unlock")
            {
                string username = e.CommandArgument.ToString();
                MembershipUser usr = Membership.GetUser(username);
                usr.UnlockUser();
                rgUsers.Rebind();
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
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                Button btnUnlock = (Button)e.Item.FindControl("btnUnlock");
                bool isLockedOut = Convert.ToBoolean(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsLockedOut"]);
                if (isLockedOut)
                {
                    btnUnlock.Visible = true;
                }
                else
                {
                    btnUnlock.Visible = false;
                }

            }
            else if (e.Item.ItemType == GridItemType.EditFormItem && e.Item.IsInEditMode)
            {
                cxblUserRoles.DataSource = Roles.GetAllRoles();
                cxblUserRoles.DataBind();

                if (!e.Item.OwnerTableView.IsItemInserted)
                {
                    //user is about to be edited
                    e.Item.Edit = false;

                    hfGuid.Value = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ProviderUserKey"].ToString();
                    Guid userId = new Guid(hfGuid.Value);
                    MembershipUser user = Membership.GetUser(userId);

                    txtUserName.Enabled = false;
                    txtUserName.Text = user.UserName;
                    txtEmail.Text = user.Email;
                    chkIsActive.Checked = user.IsApproved;                    
                    
                    // set user roles
                    foreach (string role in Roles.GetRolesForUser(user.UserName))
                    {
                        cxblUserRoles.Items.FindByText(role).Selected = true;
                    }

                    userDS = UsersCleverDataSource.GetUserCleverDataSourcesByUserId(userId);

                    rgDataSources.Rebind();

                    // disable password controls on edit
                    trPassword.Visible = false;
                    trRetypePassword.Visible = false;
                    rfvPassword.Enabled = false;
                    rfvRetypePassword.Enabled = false;
                    cvPassword.Enabled = false;
                }
                else
                {
                    //new user will be created
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.Visible = false;

                    txtUserName.Enabled = true;
                    txtUserName.Text = "";
                    txtEmail.Text = "";
                    txtPassword.Text = "";
                    txtRetypePassword.Text = "";                    
                    chkIsActive.Checked = false;

                    // enable password controls when creating new user
                    trPassword.Visible = true;
                    trRetypePassword.Visible = true;
                    rfvPassword.Enabled = true;
                    rfvRetypePassword.Enabled = true;
                    cvPassword.Enabled = true;

                    rgDataSources.Rebind();
                }
            }
        }


        /// <summary>
        /// Save user details in Membership tables
        /// </summary>
        /// <author>Mario Berberyan</author>
        /// <date>01/21/2011</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            MembershipUser memberUser;

            // Load membership user
            if (Convert.ToBoolean(hfIsInsert.Value))
            {
                #region Create User
                try
                {
                    // add membership info
                    memberUser = Membership.CreateUser(txtUserName.Text.Trim(), txtPassword.Text, txtEmail.Text.Trim());

                    // set user active status
                    memberUser.IsApproved = chkIsActive.Checked;
                    Membership.UpdateUser(memberUser);

                    // add user roles
                    foreach (ListItem item in cxblUserRoles.Items)
                    {
                        if (item.Selected)
                        {
                            Roles.AddUserToRole(memberUser.UserName, item.Value);
                        }
                    }                    
                }
                catch (MembershipCreateUserException ex)
                {                    
                    lblFormError.Visible = true;
                    lblFormError.Text = GetMembershipErrorMessage(ex.StatusCode);

                    rwAddEditUser.Visible = true;
                    rwAddEditUser.VisibleOnPageLoad = true;

                    return;
                }

   
                #endregion Create User
            }
            else
            {
                #region Update User
                memberUser = Membership.GetUser(new Guid(hfGuid.Value));
                memberUser.Email = txtEmail.Text.Trim();
                memberUser.IsApproved = chkIsActive.Checked;

                try
                {
                    // update user details
                    Membership.UpdateUser(memberUser);
                    
                    // set user roles
                    foreach(ListItem item in cxblUserRoles.Items)
                    {
                        if (item.Selected && !Roles.IsUserInRole(memberUser.UserName, item.Text))
                        {
                            Roles.AddUserToRoles(memberUser.UserName, new string[] { item.Text });
                        }

                        if (!item.Selected && Roles.IsUserInRole(memberUser.UserName, item.Text))
                        {
                            Roles.RemoveUserFromRoles(memberUser.UserName, new string[] { item.Text });
                        }
                    }

                    UsersCleverDataSource.DeleteUserCleverDataSourcesByUserId((Guid)memberUser.ProviderUserKey);         
                }
                catch (Exception ex)
                {
                    lblFormError.Visible = true;
                    lblFormError.Text = "Error saving user details. Erorr message: " + ex.Message;

                    rwAddEditUser.Visible = true;
                    rwAddEditUser.VisibleOnPageLoad = true;

                    return;
                }

                #endregion Update User
            }

            foreach (GridDataItem itm in rgDataSources.Items)
            {
                CheckBox chk = (CheckBox)itm.FindControl("chkDataSource");
                if (chk.Checked)
                    UsersCleverDataSource.InsertUserCleverDataSource((byte)itm.OwnerTableView.DataKeyValues[itm.ItemIndex]["CleverDataSourcePK"], memberUser.UserName);
            }

            rwAddEditUser.Visible = false;
            rwAddEditUser.VisibleOnPageLoad = false;

            rgUsers.Rebind();
        }

        /// <summary>
        /// Closes the popup for user add/edit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Mario Berberyan</author>
        /// <date>01/21/2011</date>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            rwAddEditUser.Visible = false;
            rwAddEditUser.VisibleOnPageLoad = false;
        }


        /// <summary>
        /// Error message list for Membership creation errors
        /// </summary>
        /// <author>Mario Berberyan</author>
        /// <date>01/21/2011</date>
        /// <param name="status"></param>
        /// <returns></returns>
        private string GetMembershipErrorMessage(MembershipCreateStatus status)
        {
            switch (status)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Username already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A username for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
    }
}
