using System;
using CmsModules.ManagementUi.Models.Interfaces;

namespace CmsModules.ManagementUi.Models
{
    public class Tenant : ITenant
    {
        #region Properties

        public Guid Id { get; }

        #endregion

        #region Constructor

        public Tenant(Guid id)
        {
            Id = id;
        }

        #endregion
    }
}