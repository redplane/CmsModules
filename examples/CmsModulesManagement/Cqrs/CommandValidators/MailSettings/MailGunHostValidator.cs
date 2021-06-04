using CmsModulesShared.Models.MailHosts;
using FluentValidation;

namespace CmsModulesManagement.Cqrs.CommandValidators.MailSettings
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