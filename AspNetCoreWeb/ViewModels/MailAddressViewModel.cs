using MailManager.Models.Interfaces;

namespace MailWeb.ViewModels
{
    public class MailAddressViewModel : IMailAddress
    {
        public string Address { get; set; }

        public string DisplayName { get; set; }
    }
}