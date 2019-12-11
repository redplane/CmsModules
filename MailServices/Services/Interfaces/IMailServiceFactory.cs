namespace MailServices.Services.Interfaces
{
    public interface IMailServiceFactory
    {
        #region Methods

        /// <summary>
        ///     Get all registered mail services.
        /// </summary>
        /// <returns></returns>
        IMailService[] GetMailServices();

        /// <summary>
        ///     Get a a registered mail service by name.
        /// </summary>
        /// <param name="uniqueName"></param>
        /// <returns></returns>
        IMailService GetMailService(string uniqueName);

        /// <summary>
        ///     Get mail service is selected.
        /// </summary>
        /// <returns></returns>
        IMailService GetActiveMailService();

        /// <summary>
        ///     Set mail service as active.
        /// </summary>
        /// <param name="uniqueName"></param>
        void SetActiveMailService(string uniqueName);

        #endregion
    }
}