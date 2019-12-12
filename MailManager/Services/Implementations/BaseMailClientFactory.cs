using System;
using System.Collections.Generic;
using System.Linq;
using MailManager.Services.Interfaces;

namespace MailManager.Services.Implementations
{
    public class BaseMailClientFactory : IMailClientFactory
    {
        #region Constructor

        public BaseMailClientFactory(IEnumerable<IMailClient> mailServices)
        {
            _mailServices = mailServices.ToArray();
            _selectedMailService = _mailServices.FirstOrDefault();
        }

        #endregion

        #region Properties

        // ReSharper disable once InconsistentNaming
        protected readonly IMailClient[] _mailServices;

        private IMailClient _selectedMailService;

        #endregion

        #region Methods

        public virtual IMailClient[] GetMailServices()
        {
            return _mailServices;
        }

        public virtual IMailClient GetMailService(string uniqueName)
        {
            return _mailServices
                .FirstOrDefault(mailService => mailService.UniqueName == uniqueName);
        }

        public virtual IMailClient GetActiveMailService()
        {
            return _selectedMailService;
        }

        public virtual void SetActiveMailService(string uniqueName)
        {
            var mailService = _mailServices.FirstOrDefault(x => x.UniqueName == uniqueName);
            if (mailService == null)
                throw new Exception($"Mail service whose name {uniqueName} cannot be found");

            _selectedMailService = mailService;
        }

        #endregion
    }
}