using MailModule.Models.Interfaces;

namespace CmsModulesShared.Models.MailHosts
{
    public abstract class MailHost : IMailHost
    {
        public abstract string Type { get; }
    }
}