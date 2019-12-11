using MailWeb.Models.Interfaces;
using MailWeb.ViewModels.BasicMailSettings;
using MediatR;

namespace MailWeb.Cqrs.Queries
{
    public class GetBasicMailSettingsQuery : IRequest<BasicMailSettingViewModel[]>
    {
        
    }
}