using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using Telerik.Web.UI;
using System.Text;
using CleverUI.ControlsUtil;

namespace CleverUI.Admin
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RadMenuItem menuItem = menuAdmin.FindItemByUrl(Request.Url.AbsolutePath);

                if (menuItem != null)
                {
                    if (menuItem.Level > 0)
                    {
                        //select the parent item
                        ((RadMenuItem)menuItem.Owner).Selected = true;
                    }
                    else
                    {
                        menuItem.Selected = true;
                    }

                    //set breadcrumb
                    menuAdmin.FindItemByValue("Breadcrumbs").Text = RadMenuUtils.GetBreadcrumbPath(menuItem);
                }
            }
        }
    }
}
