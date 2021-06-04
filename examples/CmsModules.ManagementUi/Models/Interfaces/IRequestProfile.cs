using System;

namespace CmsModules.ManagementUi.Models.Interfaces
{
    public interface IRequestProfile
    {
        #region Properties

        Guid TenantId { get; }

        #endregion
    }
}