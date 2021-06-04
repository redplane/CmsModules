namespace CmsModules.ManagementUi.ViewModels
{
    public class MailClientViewModel
    {
        #region Properties

        public string UniqueName { get; set; }

        public string DisplayName { get; set; }

        #endregion

        #region Constructor

        public MailClientViewModel()
        {
        }

        public MailClientViewModel(string uniqueName, string displayName)
        {
            UniqueName = uniqueName;
            DisplayName = displayName;
        }

        #endregion
    }
}