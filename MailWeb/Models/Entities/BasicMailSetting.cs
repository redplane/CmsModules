using System;
using MailWeb.Models.Interfaces;
using MailWeb.Models.ValueObjects;

namespace MailWeb.Models.Entities
{
    public class BasicMailSetting : BaseEntity, IBasicMailSetting
    {
        #region Constructor
        
        public BasicMailSetting(Guid id, string uniqueName) : base(id)
        {
            UniqueName = uniqueName;
        }
        
        #endregion

        #region Methods
        
        public string UniqueName { get; private set; }
        
        public string DisplayName { get; set; }
        
        public int Timeout { get; set; }
        
        public IMailHost MailHost { get; set; }
        
        #endregion
    }
}