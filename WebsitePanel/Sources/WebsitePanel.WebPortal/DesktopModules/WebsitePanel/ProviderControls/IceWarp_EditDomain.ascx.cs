// Copyright (c) 2014, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

﻿using System;
﻿using System.Collections.Generic;
﻿using System.Collections.Specialized;
﻿using System.Linq;
﻿using System.Web.UI.WebControls;
using WebsitePanel.EnterpriseServer;
using WebsitePanel.Providers.Mail;

namespace WebsitePanel.Portal.ProviderControls
{
    public partial class IceWarp_EditDomain : WebsitePanelControlBase, IMailEditDomainControl
    {
        private StringDictionary _serviceSettings;

        private StringDictionary ServiceSettings
        {
            get
            {
                if (_serviceSettings != null)
                    return _serviceSettings;

                _serviceSettings = new StringDictionary();
                var domain = ES.Services.MailServers.GetMailDomain(PanelRequest.ItemID);

                var settings = ES.Services.Servers.GetServiceSettings(domain.ServiceId);

                foreach (var settingPair in settings.Select(setting => setting.Split('=')))
                {
                    _serviceSettings.Add(settingPair[0], settingPair[1]);
                }

                return _serviceSettings;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AdvancedSettingsPanel.Visible = PanelSecurity.EffectiveUser.Role == UserRole.Administrator;
            MaxDomainDiskSpaceValidator.MaximumValue = int.MaxValue.ToString();
            MaxDomainUsersValidator.MaximumValue = int.MaxValue.ToString();
            txtLimitNumberValidator.MaximumValue = int.MaxValue.ToString();
            txtLimitVolumeValidator.MaximumValue = int.MaxValue.ToString();
            txtDefaultUserMaxMessageSizeMegaByteValidator.MaximumValue = int.MaxValue.ToString();
            txtDefaultUserMegaByteSendLimitValidator.MaximumValue = int.MaxValue.ToString();
            txtDefaultUserQuotaInMBValidator.MaximumValue = int.MaxValue.ToString();
            txtDefaultUserNumberSendLimitValidator.MaximumValue = int.MaxValue.ToString();
        }

        public void BindItem(MailDomain item)
        {
            // Hide/show controls when not enabled on service level
            rowMaxDomainDiskSpace.Visible = ServiceSettings.ContainsKey("UseDomainDiskQuota") && Convert.ToBoolean(ServiceSettings["UseDomainDiskQuota"]);
            rowDomainLimits.Visible = ServiceSettings.ContainsKey("UseDomainLimits") && Convert.ToBoolean(ServiceSettings["UseDomainLimits"]);
            rowUserLimits.Visible = ServiceSettings.ContainsKey("UseUserLimits") && Convert.ToBoolean(ServiceSettings["UseUserLimits"]);

            txtMaxDomainDiskSpace.Text = item.MaxDomainSizeInMB.ToString();
            txtMaxDomainUsers.Text = item.MaxDomainUsers.ToString();
            txtLimitVolume.Text = item.MegaByteSendLimit.ToString();
            txtLimitNumber.Text = item.NumberSendLimit.ToString();
            txtDefaultUserQuotaInMB.Text = item.DefaultUserQuotaInMB.ToString();
            txtDefaultUserMaxMessageSizeMegaByte.Text = item.DefaultUserMaxMessageSizeMegaByte.ToString();
            txtDefaultUserMegaByteSendLimit.Text = item.DefaultUserMegaByteSendLimit.ToString();
            txtDefaultUserNumberSendLimit.Text = item.DefaultUserNumberSendLimit.ToString();

            if (!IsPostBack)
            {
                var accounts = ES.Services.MailServers.GetMailAccounts(item.PackageId, false);
                ddlCatchAllAccount.DataSource = accounts;
                ddlCatchAllAccount.DataBind();
                ddlPostMasterAccount.DataSource = accounts;
                ddlPostMasterAccount.DataBind();
            }

            Utils.SelectListItem(ddlCatchAllAccount, item.CatchAllAccount);
            Utils.SelectListItem(ddlPostMasterAccount, item.PostmasterAccount);

        }

        public void SaveItem(MailDomain item)
        {
            item.CatchAllAccount = ddlCatchAllAccount.SelectedValue;
            item.PostmasterAccount = ddlPostMasterAccount.SelectedValue;
            item.MaxDomainSizeInMB = Convert.ToInt32(txtMaxDomainDiskSpace.Text);
            item.MaxDomainUsers = Convert.ToInt32(txtMaxDomainUsers.Text);
            item.NumberSendLimit = Convert.ToInt32(txtLimitNumber.Text);
            item.MegaByteSendLimit = Convert.ToInt32(txtLimitVolume.Text);
            item.DefaultUserQuotaInMB = Convert.ToInt32(txtDefaultUserQuotaInMB.Text);
            item.DefaultUserMaxMessageSizeMegaByte = Convert.ToInt32(txtDefaultUserMaxMessageSizeMegaByte.Text);
            item.DefaultUserMegaByteSendLimit = Convert.ToInt32(txtDefaultUserMegaByteSendLimit.Text);
            item.DefaultUserNumberSendLimit = Convert.ToInt32(txtDefaultUserNumberSendLimit.Text);
        }
    }
}