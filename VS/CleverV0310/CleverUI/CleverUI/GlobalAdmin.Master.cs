using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace CleverUI
{
    public partial class GlobalAdmin : System.Web.UI.MasterPage
    {
        public bool IsAdminMenuVisible
        {
            get
            {
                return menuAdmin.Visible;
            }
            set
            {
                menuAdmin.Visible = value;
            }
        }

        public bool IsHelpMenuVisible
        {
            get
            {
                return menuHelp.Visible;
            }
            set
            {
                menuHelp.Visible = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Roles.IsUserInRole("Admin"))
                {
                    hldrAdmin.Visible = true;
                }
                else
                {
                    hldrAdmin.Visible = false;
                }
            }
        }

        /// <summary>
        ///  When the user logged out update his last activity date  in order to update hist status IsOnline.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cntrlLoginStatus_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            MembershipUser currentUser = Membership.GetUser();
            currentUser.LastActivityDate = currentUser.LastActivityDate.AddHours(-1);
            Membership.UpdateUser(currentUser);

            Session.Clear();
        }
    }
}