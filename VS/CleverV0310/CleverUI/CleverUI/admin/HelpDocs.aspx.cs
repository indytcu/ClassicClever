using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using System.Configuration;

namespace CleverUI.Admin
{
    public partial class HelpDocs : System.Web.UI.Page
    {
        #region *** Control Properties *********

        public short? SelectedHelpTypePK
        {
            get
            {
                return (short?)ViewState["SelectedHelpTypePK"];
            }
            set
            {
                ViewState["SelectedHelpTypePK"] = value;
            }
        }

        public Guid? SelectedHelpDocPK
        {
            get
            {
                return (Guid?)ViewState["SelectedHelpDocPK"];
            }
            set
            {
                ViewState["SelectedHelpDocPK"] = value;
            }
        }

        #endregion *** Control Properties ******

        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorHelpDocs.Visible = false;
            lblErrorHelpTypes.Visible = false;
            lblUploadErr.Visible = false;
            lblDocFormError.Visible = false;

            if (!IsPostBack)
            {
                rdHelpDocs.Visible = false;
                RadUpload1.TargetFolder = ConfigurationManager.AppSettings[CleverUI.BusinessLogic.TCLCleverBL.Help.HelpDocsTargetFolderKey];
            }
        }

        protected void rgHelpTypes_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            rgHelpTypes.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.Top;
            rgHelpTypes.MasterTableView.NoMasterRecordsText = "No help categories to display.";
            try
            {
                DataTable typs = CleverUI.BusinessLogic.TCLCleverBL.Help.ListHelpTypes();
                typs.DefaultView.Sort = "HelpTypeUIOrder";
                rgHelpTypes.DataSource = typs.DefaultView;
                rgHelpTypes.Visible = true;
            }
            catch
            {
                lblErrorHelpTypes.Text = "Error retrieving help categories.";
                lblErrorHelpTypes.Visible = true;
                rgHelpTypes.Visible = false;
            }
        }

        protected void rgHelpDocs_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                rgHelpDocs.DataSource = CleverUI.BusinessLogic.TCLCleverBL.Help.ListHelpDocs(this.SelectedHelpTypePK.Value, null);
                rgHelpDocs.Visible = true;
            }
            catch
            {
                lblErrorHelpDocs.Text = "Error retrieving help documents.";
                lblErrorHelpDocs.Visible = true;
                rgHelpDocs.Visible = false;
            }
        }

        protected void rgHelpTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SelectedHelpTypePK = (short)rgHelpTypes.SelectedValue;

            rgHelpDocs.Rebind();

            string desc = rgHelpTypes.MasterTableView.DataKeyValues[rgHelpTypes.SelectedItems[0].ItemIndex]["HelpTypeDesc"].ToString();

            rdHelpDocs.Title = "Help Documents (" + desc + ")";
            rdHelpDocs.Visible = true;
            rdHelpDocs.Collapsed = false;
            rdHelpTypes.Collapsed = true;
        }

        protected void rgHelpDocs_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            Guid helpDocPK = (Guid)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HelpDocPK"];
            try
            {
                CleverUI.BusinessLogic.TCLCleverBL.Help.DeleteHelpDoc(helpDocPK);
            }
            catch (Exception ex)
            {
                lblErrorHelpDocs.Text = "Error occured while trying to delete the help document. Error details: " + ex.Message;
                lblErrorHelpDocs.Visible = true;
                e.Canceled = true;
            }
        }

        protected void rgHelpDocs_ItemCommand(object sender, GridCommandEventArgs e)
        {
     
            if (e.CommandName == RadGrid.EditCommandName || e.CommandName == RadGrid.InitInsertCommandName)
            {
                rwAddEditDoc.Visible = true;
                rwAddEditDoc.VisibleOnPageLoad = true;
                lblDocFormError.Visible = false;
            }
            else if (e.CommandName == "View")
            {
                string pk = ((Guid)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HelpDocPK"]).ToString();

                if (!System.IO.File.Exists(CleverUI.BusinessLogic.TCLCleverBL.Help.GetDocServerPath(pk)))
                {
                    lblErrorHelpDocs.Text = "File not found.";
                    lblErrorHelpDocs.Visible = true;
                }
                else
                {
                    this.Response.ClearHeaders();
                    this.Response.ClearContent();
                    this.Response.Clear();
                    this.Response.AddHeader("Content-Disposition", "attachment;filename=HelpDoc" + CleverUI.BusinessLogic.TCLCleverBL.Help.FileExtension);
                    this.Response.TransmitFile(CleverUI.BusinessLogic.TCLCleverBL.Help.GetDocURL(pk));
                    this.Response.Flush();
                    this.Response.End();
                }
            }
            else if (e.CommandName == "MoveUp")
            {
                Guid docPK = (Guid)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HelpDocPK"];
                short docOrder = (short)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HelpDocUIOrder"];

                Guid docPrevPK = (Guid)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex - 1]["HelpDocPK"];
                short docPrevOrder = (short)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex - 1]["HelpDocUIOrder"];

                try
                {
                    CleverUI.BusinessLogic.TCLCleverBL.Help.ChangeOrder(docPK, docPrevOrder);
                    CleverUI.BusinessLogic.TCLCleverBL.Help.ChangeOrder(docPrevPK, docOrder);
                }
                catch (Exception ex)
                {
                    lblErrorHelpDocs.Text = "Error moving up. Error details: " + ex.Message;
                    lblErrorHelpDocs.Visible = true;
                    return;
                }

                e.Item.OwnerTableView.Rebind();
            }
            else if (e.CommandName == "MoveDown")
            {
                Guid docPK = (Guid)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HelpDocPK"];
                short docOrder = (short)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HelpDocUIOrder"];

                Guid docNextPK = (Guid)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex + 1]["HelpDocPK"];
                short docNextOrder = (short)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex + 1]["HelpDocUIOrder"];

                try
                {
                    CleverUI.BusinessLogic.TCLCleverBL.Help.ChangeOrder(docPK, docNextOrder);
                    CleverUI.BusinessLogic.TCLCleverBL.Help.ChangeOrder(docNextPK, docOrder);
                }
                catch (Exception ex)
                {
                    lblErrorHelpDocs.Text = "Error moving down. Error details: " + ex.Message;
                    lblErrorHelpDocs.Visible = true;
                    return;
                }

                e.Item.OwnerTableView.Rebind();
            }
        }

        protected void rgHelpDocs_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                DataRowView dateItem = (DataRowView)e.Item.DataItem;

                if (e.Item.ItemIndex == 0)
                {
                    ((GridDataItem)e.Item)["MoveUp"].Controls[0].Visible = false;
                }
                else if (e.Item.ItemIndex == ((DataTable)e.Item.OwnerTableView.DataSource).Rows.Count - 1)
                {
                    ((GridDataItem)e.Item)["MoveDown"].Controls[0].Visible = false;
                }
            }
            else if (e.Item.ItemType == GridItemType.EditFormItem && e.Item.IsInEditMode)
            {
                //Init Insert/Edit Form

                rcbType.DataSource = CleverUI.BusinessLogic.TCLCleverBL.Help.ListHelpTypes();
                rcbType.DataBind();
                rcbType.Items.Insert(0, new RadComboBoxItem());

                if (e.Item.OwnerTableView.IsItemInserted)
                {
                    //**** Insert ******************
                    txtDocDesc.Text = string.Empty;
                    rcbType.SelectedValue = string.Empty;
                    chkDocIsVisible.Checked = false;
                    this.SelectedHelpDocPK = null;
                    rcbType.SelectedValue = this.SelectedHelpTypePK.Value.ToString();
          
                    lblDoc.Text = @"Document <span class=""required"">*</span>";
                    rwAddEditDoc.Title = "Add Help Document";
                    //Hide insert radgrid form
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.Visible = false;
                }
                else
                {
                    //**** Update ******************

                    //Display current settings in the form
                    DataRowView item = (DataRowView)e.Item.DataItem;
                    this.SelectedHelpDocPK = (Guid)item["HelpDocPK"];
                    txtDocDesc.Text = (string)item["HelpDocDesc"];
                    chkDocIsVisible.Checked = (bool)item["IsVisible"];
                    rcbType.SelectedValue = ((short)item["HelpTypeFK"]).ToString();
              
                    lblDoc.Text = "Document";
                    rwAddEditDoc.Title = "Update Help Document";

                    //hide edit radgrid form
                    e.Item.Edit = false;
                }

            }
        }

        protected void btnCancelDoc_Click(object sender, EventArgs e)
        {
            rwAddEditDoc.Visible = false;
            rwAddEditDoc.VisibleOnPageLoad = false;
        }

        protected void btnSaveDoc_Click(object sender, EventArgs e)
        {
            if (RadUpload1.InvalidFiles.Count > 0)
            {
                lblUploadErr.Visible = true;
                lblUploadErr.Text = "Only .PDF files are allowed";
                return;
            }

            CleverUI.BusinessLogic.TCLCleverBL.Help hlp = new CleverUI.BusinessLogic.TCLCleverBL.Help();
            hlp.DateUpdated = DateTime.Now;
            hlp.HelpDocDesc = txtDocDesc.Text.Trim();
            hlp.IsVisible = chkDocIsVisible.Checked;
            hlp.HelpTypeFK = short.Parse(rcbType.SelectedValue);
            hlp.UpdatedUserName = this.Page.User.Identity.Name;
            
            if (this.SelectedHelpDocPK != null)
            {
                //update
                hlp.HelpDockPK = this.SelectedHelpDocPK;
            }
            else
            {
                //insert
                if (RadUpload1.UploadedFiles.Count == 0)
                {
                    lblUploadErr.Visible = true;
                    lblUploadErr.Text = "Select a Document";
                    return;
                }

                hlp.HelpDockPK = Guid.NewGuid();
            }

            try
            {
                if (RadUpload1.UploadedFiles.Count > 0)
                {
                    UploadedFile fil = RadUpload1.UploadedFiles[0];
                    string destPath = CleverUI.BusinessLogic.TCLCleverBL.Help.GetDocServerPath(hlp.HelpDockPK.ToString());
                    
                    if (this.SelectedHelpDocPK != null)
                        System.IO.File.Delete(destPath);

                    System.IO.File.Move(System.IO.Path.Combine(Server.MapPath(RadUpload1.TargetFolder), fil.GetName()), destPath);
                }

                hlp.Save();
            }
            catch (Exception ex)
            {
                lblDocFormError.Text = "Error saving help document. Error details: " + ex.Message;
                lblDocFormError.Visible = true;

                return;
            }

            rgHelpDocs.MasterTableView.ClearEditItems();
            rgHelpDocs.Rebind();
            rwAddEditDoc.Visible = false;
            rwAddEditDoc.VisibleOnPageLoad = false;
        }
    }
}