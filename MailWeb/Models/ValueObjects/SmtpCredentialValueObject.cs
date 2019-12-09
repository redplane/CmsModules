namespace MailWeb.Models.ValueObjects
{
    public class SmtpCredentialValueObject
    {
        #region Properties

        public string Username { get; private set; }

        public string Password { get; private set; }

        #endregion

        #region Constructor

        public SmtpCredentialValueObject(string username, string password)
        {
            Username = username;
            Password = password;
        }

        #endregion
    }
}