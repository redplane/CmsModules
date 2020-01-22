using System;
using System.Threading;
using System.Threading.Tasks;
using CorsModule.Models.Interfaces;
using CorsModule.Services.Interfaces;
using MailWeb.Enums;
using MailWeb.ViewModels;

namespace MailWeb.Services.Interfaces
{
    public interface ISiteCorsPolicyService : ICorsPoliciesManager
    {
        #region Methods

        Task<ICorsPolicy> UpdateSiteCorsPolicyAsync(Guid id, EditableFieldViewModel<string[]> allowedHeaders, EditableFieldViewModel<string[]> allowedOrigins,
            EditableFieldViewModel<string[]> allowedMethods,
            EditableFieldViewModel<string[]> allowedExposedHeaders, EditableFieldViewModel<bool> allowCredential,
            EditableFieldViewModel<MasterItemAvailabilities> availability,
            CancellationToken cancellationToken = default);

        #endregion
    }
}
