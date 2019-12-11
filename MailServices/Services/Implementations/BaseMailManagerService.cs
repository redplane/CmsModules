using System;
using System.Collections.Generic;
using System.Linq;
using MailServices.Services.Interfaces;

namespace MailServices.Services.Implementations
{
    public class BaseMailManagerService : IMailManagerService
    {
        #region Constructor

        public BaseMailManagerService(IEnumerable<IMailService> mailServices)
        {
            _mailServices = mailServices.ToArray();
            _selectedMailService = _mailServices.FirstOrDefault();
        }

        #endregion

        #region Properties

        // ReSharper disable once InconsistentNaming
        protected readonly IMailService[] _mailServices;

        private IMailService _selectedMailService;

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