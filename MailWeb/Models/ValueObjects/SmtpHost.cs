using System;
using MailWeb.Models.Interfaces;

namespace MailWeb.Models.ValueObjects
{
    public class SmtpHost : IMailHost
    {
        #region Properties

        public Type Type => typeof(SmtpHost);

        public string HostName { get; set; }

        public int Port { get; set; }

        public bool Ssl { get; set; }
        
        public string Username { get; set; }
        
        public string Password { get; set; }

        #endregion

        #region Constructor

        

        #endregion
    }
}