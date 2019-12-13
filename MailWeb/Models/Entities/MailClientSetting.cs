using System;
using MailManager.Models.Interfaces;
using MailWeb.Models.Interfaces;

namespace MailWeb.Models.Entities
{
    public class MailClientSetting : BaseEntity, IMailClientSetting
    {
        #region Constructor
        
        public MailClientSetting(Guid id, Guid clientId, string uniqueName, string type) : base(id)
        {
            ClientId = clientId;
            UniqueName = uniqueName;
            Type = type;
        }
        
        #endregion

        #region Properties
        
        public Guid ClientId { get; }
        
        public string UniqueName { get; private set; }
        
        public string DisplayName { get; set; }
        
        public string Type { get; }

        public int Timeout { get; set; }
        
        public IMailAddress[] CarbonCopies { get; set; }
        
        public IMailAddress[] BlindCarbonCopies { get; set; }

        public IMailHost MailHost { get; set; }
        
        #endregion
    }
}