using CmsModulesManagement.Models.Entities;
using MediatR;

namespace CmsModulesManagement.Cqrs.Queries
{
    public class GetClientSettingsQuery : IRequest<SiteSetting[]>
    {
    }
}