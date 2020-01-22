using CmsModulesShared.Models.MailHosts;
using FluentValidation;
using MailWeb.Models;

namespace MailWeb.Cqrs.CommandValidators.MailSettings
{
    public class MailGunHostValidator : AbstractValidator<MailGunHost>
    {
        #region Constructor

        public MailGunHostValidator()
        {
            RuleFor(command => command.ApiKey)
                .NotEmpty();

            RuleFor(command => command.Domain)
                .NotEmpty();
        }

        #endregion
    }
}