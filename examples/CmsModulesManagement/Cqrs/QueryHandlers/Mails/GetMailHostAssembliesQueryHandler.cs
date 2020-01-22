using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CmsModulesManagement.Cqrs.Queries;
using CmsModulesShared.Models.MailHosts;
using MediatR;

namespace CmsModulesManagement.Cqrs.QueryHandlers.Mails
{
    public class GetMailHostAssembliesQueryHandler : IRequestHandler<GetMailHostAssembliesQuery, string[]>
    {
        public Task<string[]> Handle(GetMailHostAssembliesQuery request, CancellationToken cancellationToken)
        {
            var mailHosts = new LinkedList<string>();
            mailHosts.AddLast(typeof(SmtpHost).FullName);
            mailHosts.AddLast(typeof(MailGunHost).FullName);

            return Task.FromResult(mailHosts.ToArray());
        }
    }
}