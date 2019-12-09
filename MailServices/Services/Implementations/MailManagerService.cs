using System.Collections.Generic;
using System.Linq;
using MailServices.Services.Interfaces;

namespace MailServices.Services.Implementations
{
    public class MailManagerService : IMailManagerService
    {
        #region Properties

        private readonly IMailService[] _mailServices;

        #endregion

        #region Constructor

        public MailManagerService(IEnumerable<IMailService> mailServices)
        {
            _mailServices = mailServices.ToArray();
        }

        #endregion

        #region Methods

        public virtual IMailService[] GetMailServices()
        {
            return _mailServices;
        }

        public virtual IMailService GetMailService(string uniqueName)
        {
            return _mailServices
                .FirstOrDefault(mailService => mailService.UniqueName == uniqueName);
        }

        public virtual IMailService GetActiveMailService()
        {
            throw new System.NotImplementedException();
        }

        public virtual void SetActiveMailService(string uniqueName)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}