using System;
using System.Threading;
using System.Threading.Tasks;
using MailServices.Services.Interfaces;
using MailWeb.Cqrs.Commands;
using MailWeb.Models;
using MailWeb.Models.Entities;
using MailWeb.Models.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MailWeb.Cqrs.CommandHandlers
{
    public class UpdateActiveMailServiceCommandHandler : IRequestHandler<UpdateActiveMailServiceCommand, bool>
    {
        #region Properties

        private readonly IMailManagerService _mailManagerService;

        #endregion

        #region Constructor

        public UpdateActiveMailServiceCommandHandler(IMailManagerService mailManagerService)
        {
            _mailManagerService = mailManagerService;
        }

        #endregion

        #region Methods

        public virtual Task<bool> Handle(UpdateActiveMailServiceCommand command, CancellationToken cancellationToken)
        {
             _mailManagerService
                .SetActiveMailService(command.MailServiceUniqueName);

             return Task.FromResult(true);
        }

        #endregion
    }
}