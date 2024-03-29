﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailModule.Services.Interfaces;

namespace MailModule.Services.Implementations
{
    public class DefaultMailClientManager : IMailClientsManager
    {
        #region Constructor

        public DefaultMailClientManager(IEnumerable<IMailClient> mailServices)
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

        public Task<IMailClient[]> GetMailClientsAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_mailServices);
        }

        public Task<IMailClient> GetMailClientAsync(string uniqueName, CancellationToken cancellationToken = default)
        {
            var mailService = _mailServices
                .FirstOrDefault(x => x.UniqueName == uniqueName);

            return Task.FromResult(mailService);
        }

        public virtual Task<IMailClient> GetActiveMailClientAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_selectedMailService);
        }

        public virtual Task MarkMailClientAsActiveAsync(string uniqueName,
            CancellationToken cancellationToken = default)
        {
            var mailService = _mailServices.FirstOrDefault(x => x.UniqueName == uniqueName);
            if (mailService == null)
                throw new Exception($"Mail service whose name {uniqueName} cannot be found");

            _selectedMailService = mailService;
            return Task.CompletedTask;
        }

        #endregion
    }
}