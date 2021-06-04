using CmsModules.ManagementUi.ViewModels.MailSettings;
using MediatR;

namespace CmsModules.ManagementUi.Cqrs.Queries.MailClientSettings
{
    public class GetMailSettingsQuery : IRequest<MailSettingViewModel[]>
    {
    }
}