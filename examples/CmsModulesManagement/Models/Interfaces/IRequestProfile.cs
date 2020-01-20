using System;

namespace MailWeb.Models.Interfaces
{
    public interface IRequestProfile
    {
        #region Properties

        Guid TenantId { get; }

        #endregion
    }
}