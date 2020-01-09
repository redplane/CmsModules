namespace MailWeb.ViewModels
{
    public class MailServiceViewModel
    {
        #region Properties

        public string UniqueName { get; set; }

        public string DisplayName { get; set; }

        #endregion

        #region Constructor

        public MailServiceViewModel()
        {
        }

        public MailServiceViewModel(string uniqueName, string displayName)
        {
            UniqueName = uniqueName;
            DisplayName = displayName;
        }

        #endregion
    }
}