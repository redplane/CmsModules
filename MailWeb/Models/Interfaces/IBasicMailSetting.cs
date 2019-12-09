namespace MailWeb.Models.Interfaces
{
    public interface IBasicMailSetting
    {
        string UniqueName { get; }

        string DisplayName { get; }

        int Timeout { get; set; }
    }
}