using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CleverUI
{
    public partial class Footer : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (DataAccess.AppDataAccess.ActiveDatabaseTitle != null)
                {
                    lblActiveDatabase.Text = DataAccess.AppDataAccess.ActiveDatabaseTitle + " | ";
                }
                else
                {
                    lblActiveDatabase.Text = "";
                }
            }
        }
    }
}