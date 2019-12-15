using MailManager.Models.Interfaces;

namespace NetCoreTest.Models
{
    public class MailAddress : IMailAddress
    {
        public string Address { get; set; }

        public string DisplayName { get; set; }

        public MailAddress()
        {
        }

        public MailAddress(string address, string displayName)
        {
            Address = address;
            DisplayName = displayName;
        }
    }
}