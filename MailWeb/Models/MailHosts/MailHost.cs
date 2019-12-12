using MailWeb.Models.Interfaces;

namespace MailWeb.Models.MailHosts
{
    public class MailHost : IMailHost
    {
        public string Type => GetType().FullName;
    }
}