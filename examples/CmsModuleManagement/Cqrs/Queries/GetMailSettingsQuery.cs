using MailWeb.ViewModels.MailSettings;
using MediatR;

namespace MailWeb.Cqrs.Queries
{
    public class GetMailSettingsQuery : IRequest<MailSettingViewModel[]>
    {
    }
}