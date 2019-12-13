using MailWeb.Models.Interfaces;

namespace MailWeb.Models.MailHosts
{
    public abstract class MailHost : IMailHost
    {
        public abstract string Type { get; }
    }
}