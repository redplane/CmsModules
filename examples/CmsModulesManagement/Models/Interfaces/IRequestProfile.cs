using System;

namespace CmsModulesManagement.Models.Interfaces
{
    public interface IRequestProfile
    {
        #region Properties

        Guid TenantId { get; }

        #endregion
    }
}