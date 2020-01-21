using System;
using MailWeb.Models.Interfaces;

namespace MailWeb.Models
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