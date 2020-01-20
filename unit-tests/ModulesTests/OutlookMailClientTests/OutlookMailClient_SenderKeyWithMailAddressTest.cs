using System;
using System.Threading;
using Autofac;
using CmsModulesShared.Models.MailHosts;
using CmsModulesShared.Services;
using MailModule.Models.Interfaces;
using MailModule.Services.Interfaces;
using Moq;
using netDumbster.smtp;
using NUnit.Framework;

namespace ModulesTests.OutlookMailClientTests
{
    public class OutlookMailClient_SenderKeyWithMailAddressTest
    {
        #region Properties

        public const string OutlookUniqueName = "mc-outlook";

        public const string OutlookDisplayName = "Outlook";

        public IContainer AutofacContainer { get; protected set; }

        private const string ValidSenderMailAddress = "sender@gmail.com";

        #endregion

        #region Setup

        [SetUp]
        public void Setup()
        {
            var containerBuilder = new ContainerBuilder();

            const int port = 587;
            const string hostName = "localhost";

            var smtpHost = new SmtpHost();
            smtpHost.HostName = hostName;
            smtpHost.Password = "";
            smtpHost.Port = port;
            smtpHost.Ssl = true;
            smtpHost.Username = "";

            var simpleSmtpServer = SimpleSmtpServer.Start(port);


            var outlookMailClientSettingMock = new Mock<IMailClientSetting>();
            outlookMailClientSettingMock.Setup(options => options.Timeout).Returns(30);
            outlookMailClientSettingMock.Setup(options => options.DisplayName).Returns(OutlookDisplayName);
            outlookMailClientSettingMock.Setup(options => options.UniqueName).Returns(OutlookUniqueName);
            outlookMailClientSettingMock.Setup(options => options.MailHost).Returns(smtpHost);

            containerBuilder.RegisterInstance(outlookMailClientSettingMock.Object).AsImplementedInterfaces()
                .SingleInstance();
            containerBuilder.RegisterType<OutlookMailClient>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterInstance(simpleSmtpServer)
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
        public void MailClientHasValidNames()
        {
            var smtpMailClient = AutofacContainer.Resolve<IMailClient>();
            Assert.AreEqual(smtpMailClient.UniqueName, OutlookUniqueName);
            Assert.AreEqual(smtpMailClient.DisplayName, OutlookDisplayName);
        }

        [Test]
        public void SendMailWithValidSenderMailAddress_MailSentSuccessfully()
        {
            var mailClient = AutofacContainer.Resolve<IMailClient>();

            var senderMailAddress = new Mock<IMailAddress>();
            senderMailAddress.Setup(x => x.Address).Returns(ValidSenderMailAddress);
            senderMailAddress.Setup(x => x.DisplayName).Returns(ValidSenderMailAddress);

            var recipientMailAddress = new Mock<IMailAddress>();
            recipientMailAddress.Setup(x => x.Address).Returns("recipient@gmail.com");
            recipientMailAddress.Setup(x => x.DisplayName).Returns("recipient@gmail.com");

            var simpleSmtpServer = AutofacContainer.Resolve<SimpleSmtpServer>();

            simpleSmtpServer.MessageReceived += (sender, args) =>
            {
                Assert.AreEqual(args.Message.FromAddress.Address, ValidSenderMailAddress);
                Assert.AreEqual(args.Message.ToAddresses[0].Address, "recipient@gmail.com");
                Assert.AreEqual(args.Message.Headers["Subject"], "This is subject");
                Assert.AreEqual(args.Message.MessageParts[0].BodyData, "This is content");
            };

            mailClient
                .SendMailAsync(senderMailAddress.Object, new[] {recipientMailAddress.Object},
                    null, null,
                    "This is subject", "This is content",
                    false, null, null,
                    null, CancellationToken.None)
                .Wait();
        }
        
        [Test]
        public void SendMailWithInvalidSenderMailAddress_MailSentFailed()
        {
            var mailClient = AutofacContainer.Resolve<IMailClient>();
            
            var recipientMailAddress = new Mock<IMailAddress>();
            recipientMailAddress.Setup(x => x.Address).Returns("recipient@gmail.com");
            recipientMailAddress.Setup(x => x.DisplayName).Returns("recipient@gmail.com");

            Assert.ThrowsAsync<Exception>(() => mailClient
                .SendMailAsync(null, new[] {recipientMailAddress.Object},
                    null, null,
                    "This is subject", "This is content",
                    false, null, null,
                    null, CancellationToken.None));
        }
        
        [Test]
        public void SendMailWithNoRecipientMailAddress_MailSentFailed()
        {
            var mailClient = AutofacContainer.Resolve<IMailClient>();
            
            Assert.ThrowsAsync<Exception>(() => mailClient
                .SendMailAsync(null, null,
                    null, null,
                    "This is subject", "This is content",
                    false, null, null,
                    null, CancellationToken.None));
        }

        #endregion
    }
}