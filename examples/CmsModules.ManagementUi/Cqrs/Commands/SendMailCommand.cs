using System.Dynamic;
using CmsModules.ManagementUi.ViewModels;
using MediatR;

namespace CmsModules.ManagementUi.Cqrs.Commands
{
    public class SendMailCommand : IRequest<bool>
    {
        #region Properties

        public string MailServiceName { get; set; }

        public bool UseMailServiceName { get; set; }

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