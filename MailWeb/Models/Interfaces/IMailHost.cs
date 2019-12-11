using System;

namespace MailWeb.Models.Interfaces
{
    public interface IMailHost
    {
        #region Properties
        
        Type Type { get; }
        
        #endregion
    }
}