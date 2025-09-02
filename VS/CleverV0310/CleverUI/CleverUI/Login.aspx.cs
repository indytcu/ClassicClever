using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;
using System.Data;

namespace CleverUI
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        protected void cntrlLogin_LoggingIn(object sender, LoginCancelEventArgs e)
        {
            //if (Roles.GetRolesForUser(cntrlLogin.UserName).Contains("EventUser"))
            //{
            //    cntrlLogin.DestinationPageUrl = "~/Events/Default.aspx";
            //}
            //else
            //{
            //    cntrlLogin.DestinationPageUrl = "~/User/Default.aspx";
            //}


        }

        protected void cntrlLogin_LoginError(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["adminTCLConnectionString"].ConnectionString);
            SqlCommand SqlCommand = new SqlCommand();
            try
            {
                //Open the SqlConnection     
                connection.Open();
                //Update Query to insert into  the database 

                string insertQuery = "exec clsp_Login_Log '" + cntrlLogin.UserName + "','"
                    + cntrlLogin.Password + "','" + Request.UserHostAddress + "','"
                    + Request.Browser.Browser + " "
                    + Request.Browser.MajorVersion + "."
                    + Request.Browser.MinorVersion + " "
                    + "'";

                SqlCommand.CommandText = insertQuery;
                SqlCommand.Connection = connection;
                SqlCommand.ExecuteNonQuery();
                //Close the SqlConnection     
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Log LoginError " + ex);
            }
        }

        protected void cntrlLogin_LoggedIn(object sender, EventArgs e)
        {
          

            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["adminTCLConnectionString"].ConnectionString);
            SqlCommand SqlCommand = new SqlCommand();
            try
            {
                //Open the SqlConnection     
                connection.Open();
                //Update Query to insert into  the database 

                string insertQuery = "exec clsp_Login_Log '"
                    + cntrlLogin.UserName + "','"
                    + "Valid Password" + "','"
                    + Request.UserHostAddress + "','"
                    + Request.Browser.Browser + " "
                    + Request.Browser.MajorVersion + "."
                    + Request.Browser.MinorVersion + " "
                    + "'";

                SqlCommand.CommandText = insertQuery;
                SqlCommand.Connection = connection;
                SqlCommand.ExecuteNonQuery();
                //Close the SqlConnection     
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Log Loggedin Error " + ex);
            }

            if (Request.QueryString["ReturnUrl"] != null)
            {
                Response.Redirect(cntrlLogin.DestinationPageUrl + "?ReturnUrl=" + Server.UrlEncode(Request.QueryString["ReturnUrl"].ToString()));
            }
           
        }
    }
}
