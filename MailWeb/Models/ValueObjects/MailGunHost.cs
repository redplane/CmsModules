using System;
using MailWeb.Models.Entities;
using MailWeb.Models.Interfaces;

namespace MailWeb.Models.ValueObjects
{
    public class MailGunHost : IMailHost
    {
        #region Constructor

        public MailGunHost()
        {
            
        }
        
        #endregion
        
        #region Properties

        public Type Type => typeof(MailGunHost);
        
        public string ApiKey { get; set; }
        
        public string Domain { get; set; }
        
        #endregion
    }
}