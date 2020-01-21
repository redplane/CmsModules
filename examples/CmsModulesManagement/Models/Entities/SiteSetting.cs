using System;

namespace MailWeb.Models.Entities
{
    public class SiteSetting
    {
        #region Constructor

        public SiteSetting(Guid id)
        {
            Id = id;
        }

        #endregion

        #region Properties

        public Guid Id { get; }

        public string Name { get; set; }

        public string ActiveMailClient { get; set; }

        /// <summary>
        /// List of cors policies that are activated.
        /// </summary>
        public string[] InUseCorsPolicies { get; set; }

        #endregion
    }
}