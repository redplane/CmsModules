﻿using System;
using System.Threading;
using System.Threading.Tasks;
using CmsModules.ManagementUi.Enums;
using CmsModules.ManagementUi.ViewModels;
using MailModule.Models.Interfaces;
using MailModule.Services.Interfaces;

namespace CmsModules.ManagementUi.Services.Interfaces
{
    public interface ISiteMailClientsService : IMailClientsManager
    {
        #region Properties

        Task<IMailClient[]> GetMailClientsAsync(Guid? tenantId, 
            string uniqueName, MasterItemAvailabilities? availability,
            CancellationToken cancellationToken = default);
        
        Task<IMailClientSetting> AddSiteMailClientSettingAsync(
            Guid tenantId,
            string uniqueName, 
            string displayName, 
            IMailHost mailHost, int timeout, IMailAddress[] carbonCopies, 
            IMailAddress[] blindCarbonCopies, 
            CancellationToken cancellationToken = default);

        Task<IMailClientSetting> EditSiteMailClientSettingAsync(Guid id, Guid? tenantId,
            EditableFieldViewModel<string> displayName,
            IMailHost mailHost,
            EditableFieldViewModel<int> timeout,
            EditableFieldViewModel<IMailAddress[]> carbonCopies, 
            EditableFieldViewModel<IMailAddress[]> blindCarbonCopies, 
            CancellationToken cancellationToken = default);

        #endregion
    }
}
