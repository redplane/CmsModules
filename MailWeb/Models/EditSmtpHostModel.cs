using MailWeb.Models.Interfaces;
using MailWeb.ViewModels;

namespace MailWeb.Models
{
    public class EditSmtpHostModel : IEditMailHost
    {
        #region Properties

        public string Type { get; set; }

        public EditableFieldViewModel<string> HostName { get; set; }

        public EditableFieldViewModel<int> Port { get; set; }

        public EditableFieldViewModel<bool> Ssl { get; set; }

        public EditableFieldViewModel<string> Username { get; set; }

        public EditableFieldViewModel<string> Password { get; set; }

        #endregion
    }
}