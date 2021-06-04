using System;
using CmsModulesManagement.Models.Interfaces;

namespace CmsModulesManagement.Models
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