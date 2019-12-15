namespace MailManager.Models.Interfaces
{
    public interface IMailClientSetting
    {
        #region Properties

        string UniqueName { get; }

        string DisplayName { get; }

        IMailHost MailHost { get; set; }

        int Timeout { get; set; }

        IMailAddress[] CarbonCopies { get; set; }

        IMailAddress[] BlindCarbonCopies { get; set; }

        #endregion
    }
}