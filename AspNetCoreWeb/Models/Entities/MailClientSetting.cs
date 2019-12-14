using System;
using MailManager.Models.Interfaces;

namespace MailWeb.Models.Entities
{
    public class MailClientSetting : BaseEntity, IMailClientSetting
    {
        #region Constructor

        public MailClientSetting(Guid id, string uniqueName) : base(id)
        {
            UniqueName = uniqueName;
        }

        #endregion

        #region Properties

        public Guid ClientId { get; set; }

        public string UniqueName { get; }

        public string DisplayName { get; set; }

        public int Timeout { get; set; }

        public IMailAddress[] CarbonCopies { get; set; }

        public IMailAddress[] BlindCarbonCopies { get; set; }

        public IMailHost MailHost { get; set; }

        #endregion
    }
}