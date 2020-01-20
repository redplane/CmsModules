using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using Autofac;
using CmsModulesShared.Models.MailHosts;
using CmsModulesShared.Services;
using MailModule.Models.Interfaces;
using MailModule.Services.Interfaces;
using ModulesTests.Interceptors;
using ModulesTests.Models;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ModulesTests.MailGunClientTests
{
    public class MailGunClient_SendMailWithSenderMailAddress
    {
        #region Properties

        protected const string MailGunDisplayName = "MailGun-DisplayName";

        protected const string MailGunUniqueName = "MailGun-UniqueName";

        protected const string MailGunApiKey = "MailGun-ApiKey";

        protected const string MailGunDomain = "MailGun-Domain";

        protected const string ValidSenderMailAddress = "valid_mail_sender@gmail.com";

        protected const string ValidRecipientMailAddress = "valid_recipient@gmail.com";

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
                .Returns(new HttpClient(new MailGunHttpInterceptor(ValidSenderMailAddress, ValidRecipientMailAddress)));

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
        public void SendMailWithValidSetting_MailIsSentSuccessfully()
        {
            var mailClient = AutofacContainer.Resolve<IMailClient>();
            var recipients = new LinkedList<IMailAddress>();
            recipients.AddLast(new MailAddress(ValidRecipientMailAddress, ValidRecipientMailAddress));

            ((MailGunClient) mailClient).OnMailSent += httpResponseMessage =>
            {
                var content = httpResponseMessage.Content.ReadAsStringAsync().Result;
                var mailGunResponse = JsonConvert.DeserializeObject<MailGunResponse>(content);

                Assert.AreEqual(httpResponseMessage.StatusCode, HttpStatusCode.OK);
                Assert.AreNotEqual(mailGunResponse.Id,  string.Empty);
            };

            // Send mail successfully.
            mailClient
                .SendMailAsync(new MailAddress(ValidSenderMailAddress, ValidSenderMailAddress),
                    recipients.ToArray(), null, null,
                    "Subject-01", "Content-01", true, null, null, null, CancellationToken.None)
                .Wait();
        }

        #endregion
    }
}