using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CleverUI
{
    public partial class AboutGlobal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ((GlobalAdmin)Master).IsAdminMenuVisible = false;
        }
    }
}