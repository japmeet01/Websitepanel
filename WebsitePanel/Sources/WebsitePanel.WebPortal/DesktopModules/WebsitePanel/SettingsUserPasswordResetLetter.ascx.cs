﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Portal
{
    public partial class SettingsUserPasswordResetLetter : WebsitePanelControlBase, IUserSettingsEditorControl
    {
        public void BindSettings(UserSettings settings)
        {
            txtFrom.Text = settings["From"];
            txtSubject.Text = settings["Subject"];
            Utils.SelectListItem(ddlPriority, settings["Priority"]);
            txtHtmlBody.Text = settings["HtmlBody"];
            txtTextBody.Text = settings["TextBody"];
            txtLogoUrl.Text = settings["LogoUrl"];

            txtBodyPasswordResetLinkSmsBody.Text = settings["PasswordResetLinkSmsBody"];
        }

        public void SaveSettings(UserSettings settings)
        {
            settings["From"] = txtFrom.Text;
            settings["Subject"] = txtSubject.Text;
            settings["Priority"] = ddlPriority.SelectedValue;
            settings["HtmlBody"] = txtHtmlBody.Text;
            settings["TextBody"] = txtTextBody.Text;
            settings["LogoUrl"] = txtLogoUrl.Text;

            settings["PasswordResetLinkSmsBody"]= txtBodyPasswordResetLinkSmsBody.Text;
        }
    }
}