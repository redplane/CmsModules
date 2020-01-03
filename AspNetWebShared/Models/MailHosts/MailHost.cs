using MailClientAbstraction.Models.Interfaces;

namespace AspNetWebShared.Models.MailHosts
{
    public abstract class MailHost : IMailHost
    {
        public abstract string Type { get; }
    }
}