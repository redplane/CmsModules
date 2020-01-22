using MediatR;

namespace CmsModulesManagement.Cqrs.Queries
{
    public class GetMailHostAssembliesQuery : IRequest<string[]>
    {
    }
}