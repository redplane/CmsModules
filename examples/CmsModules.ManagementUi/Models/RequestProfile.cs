using System;
using CmsModules.ManagementUi.Models.Interfaces;

namespace CmsModules.ManagementUi.Models
{
    public class RequestProfile : IRequestProfile
    {
        #region Properties

        public Guid TenantId { get; }

        public bool IsAuthenticated { get; }

        #endregion

        #region Constructor

        public RequestProfile()
        {
        }

        public RequestProfile(Guid tenantId)
        {
            TenantId = tenantId;
            IsAuthenticated = true;
        }

        #endregion
    }
}