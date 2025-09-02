<%@ Application Language="C#" %>
<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown
    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
        try
        {
            Exception ex = Server.GetLastError();

            #region Validate

            if (ex == null)
                return;

            if (Request.Url.ToString().ToLower().Contains("scriptresource.axd") ||
                Request.Url.ToString().ToLower().Contains("webresource.axd"))
                return;

            #endregion


            CleverUI.BusinessLogic.LogManager logManager = new CleverUI.BusinessLogic.LogManager();
            logManager.OnErrorEvent("Application_Error", new CleverUI.BusinessLogic.ErrorArgs("Global.asax", "Application_Error", null, null, ex));
            
           //If no connection to the database
            if (ex.InnerException is System.Data.SqlClient.SqlException)
            {
                System.Data.SqlClient.SqlException sqlEx = (System.Data.SqlClient.SqlException)ex.InnerException;
                
                if (sqlEx.Number == 53)
                {
                    Response.Redirect("~/ConnectionLost.aspx");
                }
            }

            //If session was expired
            if (Context.Session["UserConnectionString"] == null)
            {
                Response.Redirect("~/ConnectionLost.aspx");
            }
        }
        catch (Exception eX)
        {
            throw eX;
        }
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started
    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
    }

    
    void Application_AuthenticateRequest(object sender, EventArgs e)
    {
        if (this.Request.Path.EndsWith("aspx")
            && !this.Request.Path.EndsWith("ProcessRunningInfo.aspx")
            && !String.IsNullOrEmpty(DataAccess.AppDataAccess.UserConnectionString))
           // && !this.Request.Path.EndsWith("SelectDataSource.aspx"))
        {
            if (this.User != null &&
                !Roles.IsUserInRole("Admin"))
            {
                //check for running process.
                System.Data.DataTable dt = CleverUI.BusinessLogic.TCLCleverBL.Solution.GetRunningSolutions();
                if (dt.Rows.Count > 0)
                {
                    Response.Redirect("~/ProcessRunningInfo.aspx");
                }
            }
        }
    }
           
</script>
