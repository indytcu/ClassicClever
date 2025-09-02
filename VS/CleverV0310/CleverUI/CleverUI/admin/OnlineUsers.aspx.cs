using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;

namespace CleverUI.Admin
{
    /// <summary>
    /// Interface for displaying the users which are currently online.
    /// </summary>
    public partial class OnlineUsers : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
        }

        /// <summary>
        /// Displays online users in the grid.
        /// </summary>
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


            DataTable dtUsers = new DataTable();
            dtUsers.Columns.Add("ProviderUserKey");
            dtUsers.Columns.Add("UserName");
            dtUsers.Columns.Add("Email");
            dtUsers.Columns.Add("LastLoginDate");
            dtUsers.Columns.Add("LastActivityDate");

            foreach (MembershipUser user in users)
            {
                if (user.IsOnline)
                {
                    DataRow dr = dtUsers.NewRow();
                    dr["ProviderUserKey"] = user.ProviderUserKey;
                    dr["UserName"] = user.UserName;
                    dr["Email"] = user.Email;
                    dr["LastLoginDate"] = user.LastLoginDate;
                    dr["LastActivityDate"] = user.LastActivityDate;

                    dtUsers.Rows.Add(dr);
                }
            }

            rgUsers.DataSource = dtUsers;        
        }
    }
}
