namespace MailManager.Models.Interfaces
{
    public interface IMailServiceSetting
    {
        #region Properties

        string UniqueName { get; }

        string DisplayName { get; }

        string Type { get; }

        int Timeout { get; set; }

        IMailAddress[] CarbonCopies { get; set; }

        IMailAddress[] BlindCarbonCopies { get; set; }

        #endregion
    }
}