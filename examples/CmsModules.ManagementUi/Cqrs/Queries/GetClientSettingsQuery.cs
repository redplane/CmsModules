using CmsModules.ManagementUi.Models.Entities;
using MediatR;

namespace CmsModules.ManagementUi.Cqrs.Queries
{
    public class GetClientSettingsQuery : IRequest<SiteSetting[]>
    {
    }
}