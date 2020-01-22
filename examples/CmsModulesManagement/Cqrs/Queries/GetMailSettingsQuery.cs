using CmsModulesManagement.ViewModels.MailSettings;
using MediatR;

namespace CmsModulesManagement.Cqrs.Queries
{
    public class GetMailSettingsQuery : IRequest<MailSettingViewModel[]>
    {
    }
}