using System;
using System.Threading;
using AspNetWebShared.Models.MailHosts;
using AspNetWebShared.Services;
using Autofac;
using MailClients.Models.Interfaces;
using MailClients.Services.Interfaces;
using Moq;
using netDumbster.smtp;
using NUnit.Framework;

namespace NetCoreTest
{
    public class OutlookMailClient_SenderKeyWithMailTemplateTest
    {
        #region Properties

        private const string OutlookUniqueName = "mc-outlook";

        private const string OutlookDisplayName = "Outlook";

        private const string ValidSenderKey = "validSenderKey";

        private const string InvalidSenderKey = "invalidSenderKey";
        
        private const string ValidSenderAddress = "sender-01@gmail.com";

        private const string InvalidMailTemplateName = "invalidTemplateName";
        
        private const string ValidMailTemplateName = "validTemplateName";

        private const string ValidMailContent = "valid-mail-content";

        private const string ValidMailSubject = "valid-mail-subject";

        private const bool IsMailHtml = true;

        private const string InvalidSenderExceptionMessage = "INVALID_SENDER";

        private const string InvalidMailTemplateExceptionMessage = "INVALID_MAIL_TEMPLATE";


        public IContainer AutofacContainer { get; protected set; }

        #endregion

        #region Setup

        [SetUp]
        public void Setup()
        {
            var containerBuilder = new ContainerBuilder();

            const int port = 587;
            const string hostName = "localhost";

            // Simple SMTP mail server.
            var simpleSmtpServer = SimpleSmtpServer.Start(port);

            var smtpHost = new SmtpHost();
            smtpHost.HostName = hostName;
            smtpHost.Password = "";
            smtpHost.Port = port;
            smtpHost.Ssl = true;
            smtpHost.Username = "";

            var outlookMailClientSettingMock = new Mock<IMailClientSetting>();
            outlookMailClientSettingMock.Setup(options => options.Timeout).Returns(30);
            outlookMailClientSettingMock.Setup(options => options.DisplayName).Returns(OutlookDisplayName);
            outlookMailClientSettingMock.Setup(options => options.UniqueName).Returns(OutlookUniqueName);
            outlookMailClientSettingMock.Setup(options => options.MailHost).Returns(smtpHost);

            containerBuilder
                .RegisterInstance(outlookMailClientSettingMock.Object)
                .AsImplementedInterfaces()
                .SingleInstance();

            containerBuilder
                .RegisterType<OutlookMailClient>()
                .AsImplementedInterfaces()
                .OnActivating(context =>
                {
                    var mailClientSetting = context.Context.Resolve<IMailClientSetting>();
                    var moqOutlookMailClient = new Mock<OutlookMailClient>(mailClientSetting);
                    moqOutlookMailClient.CallBase = true;
                    
                    var moqSenderMailAddress = new Mock<IMailAddress>();
                    moqSenderMailAddress.Setup(moqSender => moqSender.Address).Returns(ValidSenderAddress);
                    moqSenderMailAddress.Setup(moqSender => moqSender.DisplayName).Returns(ValidSenderAddress);

                    var moqMailContent = new Mock<IMailContent>();
                    moqMailContent.Setup(template => template.Content).Returns(ValidMailContent);
                    moqMailContent.Setup(template => template.Subject).Returns(ValidMailSubject);
                    moqMailContent.Setup(template => template.IsHtml).Returns(IsMailHtml);

                    moqOutlookMailClient
                        .Setup(moqClient =>
                            moqClient.GetMailContentAsync(ValidMailTemplateName, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(moqMailContent.Object);
                    
                    moqOutlookMailClient
                        .Setup(moqClient =>
                            moqClient.GetMailContentAsync(InvalidMailTemplateName, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(() => throw new Exception(InvalidMailTemplateExceptionMessage));

                    moqOutlookMailClient
                        .Setup(moqClient =>
                            moqClient.GetSenderAsync(ValidSenderKey, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(moqSenderMailAddress.Object);
                    
                    moqOutlookMailClient
                        .Setup(moqClient =>
                            moqClient.GetSenderAsync(InvalidSenderKey, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(() => throw new Exception(InvalidSenderExceptionMessage));

                    context.ReplaceInstance(moqOutlookMailClient.Object);
                })
                .InstancePerLifetimeScope();

            containerBuilder
                .RegisterInstance(simpleSmtpServer)
                .SingleInstance();

            AutofacContainer = containerBuilder.Build();
        }

        [TearDown]
        public void TearDown()
        {
            var simpleSmtpServer = AutofacContainer.Resolve<SimpleSmtpServer>();
            simpleSmtpServer?.Stop();
        }

        #endregion

        #region Tests

        [Test]
        public void SendMailWithValidSenderKey_MailSentSuccessfully()
        {
            var mailClient = AutofacContainer.Resolve<IMailClient>();

            var moqRecipientMailAddress = new Mock<IMailAddress>();
            moqRecipientMailAddress.Setup(x => x.Address).Returns("recipient@gmail.com");
            moqRecipientMailAddress.Setup(x => x.DisplayName).Returns("recipient@gmail.com");

            var moqMailContent = new Mock<IMailContent>();
            moqMailContent.Setup(x => x.Content).Returns("Rendered content");
            moqMailContent.Setup(x => x.Subject).Returns("Mail subject");
            moqMailContent.Setup(x => x.IsHtml).Returns(true);

            // Simple SMTP server listening.
            var simpleSmtpServer = AutofacContainer.Resolve<SimpleSmtpServer>();

            simpleSmtpServer.MessageReceived += (sender, args) =>
            {
                Assert.AreEqual(args.Message.FromAddress.Address, ValidSenderAddress);
                Assert.AreEqual(args.Message.ToAddresses[0].Address, "recipient@gmail.com");
                Assert.AreEqual(args.Message.Headers["Subject"], ValidMailSubject);
                Assert.AreEqual(args.Message.MessageParts[0].BodyData, ValidMailContent);
            };

            mailClient
                .SendMailAsync(ValidSenderKey, new[] {moqRecipientMailAddress.Object}, null, null,
                    ValidMailTemplateName, null, null, null,
                    CancellationToken.None)
                .Wait();
        }

        [Test]
        public void SendMailWithInvalidSenderKey_MailSentFailed()
        {
            var mailClient = AutofacContainer.Resolve<IMailClient>();

            var moqRecipientMailAddress = new Mock<IMailAddress>();
            moqRecipientMailAddress.Setup(x => x.Address).Returns("recipient@gmail.com");
            moqRecipientMailAddress.Setup(x => x.DisplayName).Returns("recipient@gmail.com");

            Assert.ThrowsAsync<Exception>(async () => await mailClient.SendMailAsync(InvalidSenderKey,
                    new[] {moqRecipientMailAddress.Object}, null, null,
                    ValidMailTemplateName, null, null, null,
                    CancellationToken.None), InvalidSenderExceptionMessage);
        }

        [Test]
        public void SendMailWithInvalidTemplateKey_MailSentFailed()
        {
            var mailClient = AutofacContainer.Resolve<IMailClient>();

            var moqRecipientMailAddress = new Mock<IMailAddress>();
            moqRecipientMailAddress.Setup(x => x.Address).Returns("recipient@gmail.com");
            moqRecipientMailAddress.Setup(x => x.DisplayName).Returns("recipient@gmail.com");

            Assert.ThrowsAsync<Exception>(async () => await mailClient.SendMailAsync(ValidSenderKey,
                new[] {moqRecipientMailAddress.Object}, null, null,
                InvalidMailTemplateName, null, null, null,
                CancellationToken.None), InvalidMailTemplateExceptionMessage);
        }
        
        #endregion
    }
}