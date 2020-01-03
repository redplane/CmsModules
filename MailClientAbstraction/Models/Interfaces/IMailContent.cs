namespace MailClientAbstraction.Models.Interfaces
{
    public interface IMailContent
    {
        #region Properties

        string Subject { get; set; }

        string Content { get; set; }

        bool IsHtml { get; set; }

        #endregion
    }
}