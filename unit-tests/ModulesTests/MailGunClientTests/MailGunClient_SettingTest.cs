using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Autofac;
using CmsModulesManagement.Services.Implementations;
using CmsModulesShared.Models.MailHosts;
using MailModule.Models.Interfaces;
using MailModule.Services.Interfaces;
using ModulesTests.Interceptors;
using ModulesTests.Models;
using Moq;
using NUnit.Framework;

namespace ModulesTests.MailGunClientTests
{
    public class MailGunClient_SettingTest
    {
        #region Properties

        protected const string MailGunDisplayName = "MailGun-DisplayName";

        protected const string MailGunUniqueName = "MailGun-UniqueName";

        protected const string MailGunApiKey = "MailGun-ApiKey";

        protected const string MailGunDomain = "MailGun-Domain";

        public IContainer AutofacContainer { get; protected set; }

        #endregion

        #region Setup

        [SetUp]
        public void Setup()
        {
            var containerBuilder = new ContainerBuilder();

            var mailGunHost = new MailGunHost();
            mailGunHost.ApiKey = MailGunApiKey;
            mailGunHost.Domain = MailGunDomain;

            var mailGunClientSetting = new Mock<IMailClientSetting>();
            mailGunClientSetting.Setup(options => options.Timeout).Returns(30);
            mailGunClientSetting.Setup(options => options.DisplayName).Returns(MailGunDisplayName);
            mailGunClientSetting.Setup(options => options.UniqueName).Returns(MailGunUniqueName);
            mailGunClientSetting.Setup(options => options.MailHost).Returns(mailGunHost);

            containerBuilder
                .RegisterInstance(mailGunClientSetting.Object)
                .AsImplementedInterfaces()
                .SingleInstance();

            var moqHttpClientFactory = new Mock<IHttpClientFactory>();
            moqHttpClientFactory.CallBase = true;
            moqHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient(new MailGunHttpInterceptor(null, null)));

            containerBuilder
                .RegisterInstance(moqHttpClientFactory.Object)
                .AsImplementedInterfaces();

            containerBuilder.RegisterType<MailGunClient>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            AutofacContainer = containerBuilder.Build();
        }

        #endregion

        #region Methods

        [Test]
        public void ValidateMailGunSetting_HasSettingsAsDefined()
        {
            var mailClient = AutofacContainer.Resolve<IMailClient>();
            Assert.AreEqual(MailGunUniqueName, mailClient.UniqueName);
            Assert.AreEqual(MailGunDisplayName, mailClient.DisplayName);
        }

        [Test]
        public void SendMailWithValidSetting_MailIsSentSuccessfully()
        {
            var mailClient = AutofacContainer.Resolve<IMailClient>();
            var recipients = new LinkedList<IMailAddress>();
            recipients.AddLast(new MailAddress("recipient@gmail.com", "recipient@gmail.com"));
            recipients.AddLast(new MailAddress("recipient-01@gmail.com", "recipient-01@gmail.com"));

            mailClient.SendMailAsync(new MailAddress("sender@gmail.com", "sender display name"),
                    recipients.ToArray(), null, null,
                    "Subject-01", "Content-01", true, null, null, null, CancellationToken.None)
                .Wait();
        }

        #endregion
    }
}