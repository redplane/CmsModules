using MailModule.Models.Interfaces;

namespace CmsModuleShared.Models.MailHosts
{
    public abstract class MailHost : IMailHost
    {
        public abstract string Type { get; }
    }
}