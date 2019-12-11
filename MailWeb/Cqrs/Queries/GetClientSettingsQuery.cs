using MailWeb.Models.Entities;
using MediatR;

namespace MailWeb.Cqrs.Queries
{
    public class GetClientSettingsQuery : IRequest<ClientSetting[]>
    {
    }
}