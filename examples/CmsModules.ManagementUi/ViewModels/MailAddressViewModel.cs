using MailModule.Models.Interfaces;

namespace CmsModules.ManagementUi.ViewModels
{
    public class MailAddressViewModel : IMailAddress
    {
        public string Address { get; set; }

        public string DisplayName { get; set; }
    }
}