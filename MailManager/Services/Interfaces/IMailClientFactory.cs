namespace MailManager.Services.Interfaces
{
    public interface IMailClientFactory
    {
        #region Methods

        /// <summary>
        ///     Get all registered mail services.
        /// </summary>
        /// <returns></returns>
        IMailClient[] GetMailServices();

        /// <summary>
        ///     Get a a registered mail service by name.
        /// </summary>
        /// <param name="uniqueName"></param>
        /// <returns></returns>
        IMailClient GetMailService(string uniqueName);

        /// <summary>
        ///     Get mail service is selected.
        /// </summary>
        /// <returns></returns>
        IMailClient GetActiveMailClient();

        /// <summary>
        ///     Set mail service as active.
        /// </summary>
        /// <param name="uniqueName"></param>
        void SetActiveMailClient(string uniqueName);

        #endregion
    }
}