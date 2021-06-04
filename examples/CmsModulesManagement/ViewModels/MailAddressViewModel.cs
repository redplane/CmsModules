using MailModule.Models.Interfaces;

namespace CmsModulesManagement.ViewModels
{
    public class MailAddressViewModel : IMailAddress
    {
        public string Address { get; set; }

        public string DisplayName { get; set; }
    }
}