using System.Dynamic;

namespace MailWeb.ViewModels
{
    public class SendMailViewModel
    {
        #region Properties

        public string ServiceUniqueName { get; set; }

        public MailAddressViewModel Sender { get; set; }

        public MailAddressViewModel[] Recipients { get; set; }

        public MailAddressViewModel[] CarbonCopies { get; set; }

        public MailAddressViewModel[] BlindCarbonCopies { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public bool IsHtml { get; set; }

        public ExpandoObject AdditionalSubjectData { get; set; }

        public ExpandoObject AdditionalContentData { get; set; }

        #endregion
    }
}