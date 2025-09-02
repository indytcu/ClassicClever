using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.SessionState;
using System.Web.Security;
using Telerik.Web.UI;
using CleverUI.InterfaceLogic;
using System.Data;
using CleverUI.BusinessLogic.TCLCleverBL;
using CleverUI.BusinessLogic;
using CleverUI.InterfaceLogic.Web;
using CleverUI.InterfaceLogic.Web.Extensions;
using System.ComponentModel;
using System.Configuration;

namespace CleverUI
{
    public partial class CleverApp : System.Web.UI.MasterPage
    {
        #region *** Control Properties *********

        /// <summary>
        /// Username of currently logged user.
        /// </summary>
        private string UserName
        {
            get
            {
                return this.Page.User.Identity.Name;
            }
        }

		public bool MenuProcessVisible
		{
			get
			{
				return menuProcess.Visible;
			}
		}

		public bool MenuSetupVisible
		{
			get
			{
				return menuSetup.Visible;
			}
		}

        #endregion


        /// <summary>
        /// Selects a specified portal tab.
        /// </summary>
        /// <param name="tab"></param>
        public void SelectTab(short portalPK)
        {
            if (RadMenuPortal.FindItemByValue(portalPK.ToString()) != null)
                RadMenuPortal.FindItemByValue(portalPK.ToString()).Selected = true;
        }


        /// <summary>
        /// Attaches event handlers for events - create and change portal filter.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (this.Page is PortalPage)
            {
                ((PortalPage)this.Page).PortalFilterCreate += new PortalFilterCreateHandler(CleverApp_PortalFilterCreate);
                ((PortalPage)this.Page).PortalFilterChange += new PortalFilterChangeHandler(CleverApp_PortalFilterChange);
            }
            
            BindRadMenuPortal();

			//manage visibility of the links based on the user role
			string[] roles = Roles.GetRolesForUser();
			foreach (string role in roles)
			{
				switch (role.ToLower())
				{
					case "admin":
						menuEvents.Visible = true;
						menuSetup.Visible = true;
						menuProcess.Visible = true;
						menuAdmin.Visible = true;
						break;
					case "manager":
						menuSetup.Visible = true;
						menuProcess.Visible = true;
                        menuEvents.Visible = true;
						break;
					case "eventuser":
						menuEvents.Visible = true;

						RadMenuPortal.Visible = false;
						//rdsPortalTabs.Visible = false;
						rcbFilter.Visible = false;

						break;
					case "planner":
						menuProcess.Visible = true;
                        menuSetup.Visible = true;
                        menuEvents.Visible = true;
                        break;
					case "buyer":
                        menuSetup.Visible = true;
                        break;
					case "sales":
					default:
						break;
				}
				if (role.ToLower() == "admin")
				{
					break;
				}
			}
        }

        /// <summary>
        /// Set visibility of menu items depending on the user role.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Nevena Uzunova</author>
        /// <date>11/26/2010</date>
        protected void Page_Load(object sender, EventArgs e)
        {
            //RadClientExportManager1.PdfSettings.ProxyURL = ResolveUrl(ConfigurationManager.AppSettings[CleverUI.BusinessLogic.TCLCleverBL.Help.HelpDocsTargetFolderKey]);

            if (!IsPostBack)
            {
                lblCurrentDate.Text = CleverUI.BusinessLogic.TCLCleverBL.Utils.GetCurrentPlanDate();

                //manage css class of the active top link 
                if (this.Page.Master != null)
                {
                    hlHelp.CssClass = "";
                    hlSetup.CssClass = "";
                    hlProcess.CssClass = "";
                    hlAdmin.CssClass = "";

                    if (this.Page.Master is CleverUI.Help.Help)
                    {
                        hlHelp.CssClass = "active";
                    }
                    else if (this.Page.Master is CleverUI.Events.Events)
                    {
                        hlEvents.CssClass = "active";
                    }
                    else if (this.Page.Master is CleverUI.Setup.Setup)
                    {
                        hlSetup.CssClass = "active";
                    }
                    else if (this.Page.Master is CleverUI.Process.MPProcess)
                    {
                        hlProcess.CssClass = "active";
                    }
                    else if (this.Page.Master is CleverUI.Admin.Admin)
                    {
                        hlAdmin.CssClass = "active";
                    }
                }

                if (this.Page is PortalPage && ((PortalPage)this.Page).PortalID > 0)
                {
                    ListPortalFilters();
                }
                else
                {
                    ListUserFilters();
                }

                lbLoginName.Text = this.UserName; 
            }
        }

        protected void lbLoginName_Click(object sender, EventArgs e)
        {
            DataTable dtFilter = null;
            try
            {
                dtFilter = Filter.ListFilterByTypeAndUsername((short)FilterTypeEnum.UserSystem, this.UserName);
            }
            catch (Exception ex)
            {
                lblError.Text = "Error ocurred while trying to retrieve filters. Error details: " + ex.Message;
                lblError.Visible = true;
                return;
            }        

            if (dtFilter.Rows.Count > 0)
            {
                this.ucLoggedinUserFMTL.SelectedFilterID = (int)dtFilter.Rows[0][0];
                this.ucLoggedinUserFMTL.FilterType = FilterTypeEnum.UserSystem;
                this.ucLoggedinUserFMTL.Visible = true;
                this.ucLoggedinUserFMTL.ShowPopup();
            }
            else
            {
                lblError.Text = "No data found";
            }
        }

        /// <summary>
        /// Change the selected portal filter on the portal page.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        protected void rcbFilter_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

            int filterId = Convert.ToInt32(e.Value);

            if (this.Page is PortalPage)
            {
                //raise event for that the portal filter is changed
                FilterChangeEventArgs args = new FilterChangeEventArgs(filterId);
                ((PortalPage)this.Page).OnPortalFilterChange(sender, args);
            }
            else
            {
                // Set the default User System Filter
                Filter.SetDefaultUserSystemFilter(filterId);
            }
        }

        /// <summary>
        /// Handles click event of link button btnFilters and shows a popup screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Nevena Uzunova</author>
        /// <date>10/08/2010</date>
        protected void lbFilter_Click(object sender, EventArgs e)
        {
            if (rcbFilter.SelectedItem != null)
            {
                if (this.Page is PortalPage && ((PortalPage)this.Page).PortalID > 0)
                {
                    ucFilterManager.FilterType = FilterTypeEnum.Portal;
                }
                else
                {
                    ucFilterManager.FilterType = FilterTypeEnum.UserSystem;
                }
                this.ucFilterManager.SelectedFilterID = Convert.ToInt32(rcbFilter.SelectedValue);
                this.ucFilterManager.Visible = true;
                this.ucFilterManager.ShowPopup();
            }

        }

        /// <summary>
        /// Handles click event of link button btnFilters and shows a popup screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Nevena Uzunova</author>
        /// <date>10/08/2010</date>
        protected void lbFilterTreeList_Click(object sender, EventArgs e)
        {
            if (rcbFilter.SelectedItem != null)
            {
                if (this.Page is PortalPage && ((PortalPage)this.Page).PortalID > 0)
                {
                    ucSelectFilterManagerTreeList.FilterType = FilterTypeEnum.Portal;
                }
                else
                {
                    ucSelectFilterManagerTreeList.FilterType = FilterTypeEnum.UserSystem;
                }
                this.ucSelectFilterManagerTreeList.SelectedFilterID = Convert.ToInt32(rcbFilter.SelectedValue);
                this.ucSelectFilterManagerTreeList.Visible = true;
                this.ucSelectFilterManagerTreeList.ShowPopup();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void CleverApp_PortalFilterChange(object sender, FilterChangeEventArgs args)
        {
            rcbFilter.SelectedValue = args.NewFilterID.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void CleverApp_PortalFilterCreate(object sender, FilterCreateEventArgs args)
        {
            if (rcbFilter.FindItemByValue(args.NewFilterID.ToString()) != null)
            {
                return;
            }

            short portalID = ((PortalPage)this.Page).PortalID;
            DataTable dtFilter = null;
            try
            {
                dtFilter = Portal.ListUserFilters(portalID, ((PortalPage)this.Page).PortalUser);
            }
            catch (Exception ex)
            {
                lblError.Text = "Error ocurred while trying to retrieve filters. Error details: " + ex.Message;
                lblError.Visible = true;
                return;
            }

            rcbFilter.DataSource = dtFilter;
            rcbFilter.DataBind();

            if (dtFilter.Rows.Count > 0)
            {
                rcbFilter.SelectedValue = portalID.ToString();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void ListPortalFilters()
        {
            //populate the filter in the dropdown next to button "Filters".
            rcbFilter.DataTextField = "FilterDesc";
            rcbFilter.DataValueField = "FilterPK";

            short portalID = ((PortalPage)this.Page).PortalID;
            DataTable dtFilter = null;
            try
            {
                dtFilter = Portal.ListUserFilters(portalID, this.UserName);
            }
            catch (Exception ex)
            {
                lblError.Text = "Error ocurred while trying to retrieve filters. Error details: " + ex.Message;
                lblError.Visible = true;
                return;
            }

            rcbFilter.DataSource = dtFilter;
            rcbFilter.DataBind();

            if (dtFilter.Rows.Count > 0)
            {
                int portalFilterID = ((PortalPage)this.Page).PortalFilterID;
                if ((portalFilterID > 0) &&
                    (rcbFilter.Items.FindItemByValue(portalFilterID.ToString()) != null))
                {
                    rcbFilter.SelectedValue = portalFilterID.ToString();
                }
                else
                {
                    ((PortalPage)this.Page).PortalFilterID = Convert.ToInt32(dtFilter.Rows[0]["FilterPK"]);
                }
            }
        }

        private void ListUserFilters()
        {
            //populate the filter in the dropdown next to button "Filters".
            rcbFilter.DataTextField = "FilterDesc";
            rcbFilter.DataValueField = "FilterPK";

            DataTable dtFilter = null;
            try
            {
                dtFilter = Filter.ListFilterByTypeAndUsername((short)FilterTypeEnum.UserSystem, this.UserName);
            }
            catch (Exception ex)
            {
                lblError.Text = "Error ocurred while trying to retrieve filters. Error details: " + ex.Message;
                lblError.Visible = true;
                return;
            }

            rcbFilter.DataSource = dtFilter;
            rcbFilter.DataBind();
        }

        #region DataBind

        private void BindRadMenuPortal()
        {
            DataTable portals = Portal.ListAll().Copy();

            foreach (DataRow row in portals.Rows)
            {
                if (row["NavigateURL"] != DBNull.Value)
                    row["NavigateURL"] = (string)row["NavigateURL"] + "?" + NavigateQueryStings.PortalID + "=" + ((byte)row["PortalPK"]).ToString();
            }

			portals.DefaultView.RowFilter = "IsVisible = 1 AND portalIsUserVisible = 1";
            
            RadMenuPortal.DataNavigateUrlField = "NavigateURL";
            RadMenuPortal.DataValueField = "PortalPK";
            RadMenuPortal.DataTextField = "PortalDesc";
            RadMenuPortal.DataSource = portals.DefaultView;
            RadMenuPortal.DataBind();
        }

        #endregion
    }
}
