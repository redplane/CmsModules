using MailModule.Models.Interfaces;

namespace CmsModules.Shares.Models.MailHosts
{
    public abstract class MailHost : IMailHost
    {
        public abstract string Type { get; }
    }
}