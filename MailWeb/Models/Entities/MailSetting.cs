using System;
using MailWeb.Models.Interfaces;

namespace MailWeb.Models.Entities
{
    public class MailSetting : BaseEntity, IBasicMailSetting
    {
        #region Constructor
        
        public MailSetting(Guid id, string uniqueName) : base(id)
        {
            UniqueName = uniqueName;
        }
        
        #endregion

        #region Properties
        
        public string UniqueName { get; private set; }
        
        public string DisplayName { get; set; }
        
        public int Timeout { get; set; }
        
        public IMailHost MailHost { get; set; }
        
        #endregion
    }
}