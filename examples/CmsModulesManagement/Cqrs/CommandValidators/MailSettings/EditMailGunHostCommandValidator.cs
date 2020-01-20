using FluentValidation;
using MailWeb.Models;

namespace MailWeb.Cqrs.CommandValidators.MailSettings
{
    public class EditMailGunHostCommandValidator : AbstractValidator<EditMailGunHostModel>
    {
        #region Constructor

        public EditMailGunHostCommandValidator()
        {
            RuleFor(command => command.ApiKey.Value)
                .NotEmpty()
                .When(command => command.ApiKey != null && command.ApiKey.HasModified);

            RuleFor(command => command.Domain.Value)
                .NotEmpty()
                .When(command => command.Domain != null && command.Domain.HasModified);
        }

        #endregion
    }
}