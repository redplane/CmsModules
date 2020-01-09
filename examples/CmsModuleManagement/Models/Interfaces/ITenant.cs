using System;

namespace MailWeb.Models.Interfaces
{
    public interface ITenant
    {
        #region Properties

        Guid SiteId { get; set; }

        #endregion
    }
}