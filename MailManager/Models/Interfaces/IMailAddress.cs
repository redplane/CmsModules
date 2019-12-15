namespace MailClients.Models.Interfaces
{
    public interface IMailAddress
    {
        #region Properties

        string Address { get; set; }

        string DisplayName { get; set; }

        #endregion
    }
}