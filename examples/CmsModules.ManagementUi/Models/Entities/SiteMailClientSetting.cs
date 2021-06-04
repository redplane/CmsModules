using System;
using MailModule.Models.Interfaces;

namespace CmsModules.ManagementUi.Models.Entities
{
    public class SiteMailClientSetting : BaseEntity, IMailClientSetting
    {
        #region Constructor

        public SiteMailClientSetting(Guid id, Guid tenantId, string uniqueName) : base(id)
        {
            TenantId = tenantId;
            UniqueName = uniqueName;
        }

        #endregion

        #region Properties

        public string UniqueName { get; }

        public string DisplayName { get; set; }

        public int Timeout { get; set; }

        public IMailAddress[] CarbonCopies { get; set; }

        public IMailAddress[] BlindCarbonCopies { get; set; }

        public IMailHost MailHost { get; set; }

        public Guid TenantId { get;}

        #endregion
    }
}