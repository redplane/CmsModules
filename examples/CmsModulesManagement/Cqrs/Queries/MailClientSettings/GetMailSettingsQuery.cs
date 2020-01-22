using CmsModulesManagement.ViewModels.MailSettings;
using MediatR;

namespace CmsModulesManagement.Cqrs.Queries.MailClientSettings
{
    public class GetMailSettingsQuery : IRequest<MailSettingViewModel[]>
    {
    }
}