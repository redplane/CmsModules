using System;
using System.Net.Mail;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MailModule.Models.Interfaces;
using MailModule.Services.Implementations;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace MailModule.Tests.Services.Implementations
{
    public class DefaultSmtpMailClient_SendMailAsyncTests
    {
        #region Properties

        private Mock<IMailClientSetting> _mailClientSettingMock;
        private Mock<DefaultSmtpMailClient> _defaultSmtpMailClientMock;
        private Mock<IMailAddress> _mailAddressMock;
        private Mock<IMailContent> _mailContentMock;
        private Mock<SmtpClient> _smtpClientMock;

        #endregion

        #region Setup

        [SetUp]
        public void Setup()
        {
            // Mock mail client setting.
            _mailClientSettingMock = new Mock<IMailClientSetting>();
            _mailClientSettingMock.Setup(c => c.DisplayName).Returns("mailClientSetting");

            // Mock default smtp mail client.
            _defaultSmtpMailClientMock =
                new Mock<DefaultSmtpMailClient>(MockBehavior.Loose, _mailClientSettingMock.Object)
                {
                    CallBase = true
                };
            _defaultSmtpMailClientMock.Setup(c => c.DisplayName).Returns("smtp");

            // Mock mail address.
            _mailAddressMock = new Mock<IMailAddress>();
            _mailAddressMock.Setup(c => c.DisplayName).Returns("DisplayName");
            _mailAddressMock.Setup(c => c.Address).Returns("address@gmail.com");

            // Mock for mail content.
            _mailContentMock = new Mock<IMailContent>();
            _mailContentMock.Setup(c => c.Subject).Returns("subject");
            _mailContentMock.Setup(c => c.Content).Returns("content");
            
        }

        #endregion

        #region Methods

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void SendMailAsyncWithSenderName_PassInvalidSenderName_ShouldReturnException(string sender)
        {
            // Act
            Func<Task> result = async () => await _defaultSmtpMailClientMock.Object.SendMailAsync(sender,
                new[] {_mailAddressMock.Object}, new[] {_mailAddressMock.Object}, new[] {_mailAddressMock.Object},
                It.IsAny<string>());

            // Assert
            result.Should().ThrowExactly<ArgumentException>().And.ParamName.Should().Be(nameof(sender));
        }

        [Test]
        public void SendMailAsyncWithSenderName_PassValidWrongSenderName_ShouldReturnException()
        {
            // Arrange
            _defaultSmtpMailClientMock.Setup(c => c.GetSenderAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IMailAddress>(null));

            // Act
            Func<Task> result = async () => await _defaultSmtpMailClientMock.Object.SendMailAsync("wrong_name",
                new[] {_mailAddressMock.Object}, new[] {_mailAddressMock.Object}, new[] {_mailAddressMock.Object},
                "name");

            // Assert
            result.Should().ThrowExactly<Exception>()
                .WithMessage($"Mail sender whose name is wrong_name is not found.");
        }

        [Test]
        public void SendMailAsyncWithSenderName_PassNullRecipients_ShouldReturnException()
        {
            // Arrange
            _defaultSmtpMailClientMock.Setup(c => c.GetSenderAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IMailAddress>(_mailAddressMock.Object));

            // Act
            Func<Task> result = async () => await _defaultSmtpMailClientMock.Object.SendMailAsync("sender",
                null, new[] {_mailAddressMock.Object}, new[] {_mailAddressMock.Object},
                "name");

            // Assert
            result.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("recipients");
        }

        [Test]
        public void SendMailAsyncWithSenderName_PassEmptyRecipients_ShouldReturnException()
        {
            // Arrange
            _defaultSmtpMailClientMock.Setup(c => c.GetSenderAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IMailAddress>(_mailAddressMock.Object));

            // Act
            Func<Task> result = async () => await _defaultSmtpMailClientMock.Object.SendMailAsync("sender",
                new IMailAddress[] { }, new[] {_mailAddressMock.Object}, new[] {_mailAddressMock.Object},
                "name");

            // Assert
            result.Should().ThrowExactly<Exception>().And.Message.Should().Be("No recipient has been defined.");
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void SendMailAsyncWithSenderName_PassInvalidTemplateName_ShouldReturnException(string templateName)
        {
            // Arrange 
            _defaultSmtpMailClientMock.Setup(c => c.GetSenderAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IMailAddress>(_mailAddressMock.Object));

            // Act
            Func<Task> result = async () => await _defaultSmtpMailClientMock.Object.SendMailAsync("sender",
                new[] {_mailAddressMock.Object}, new[] {_mailAddressMock.Object}, new[] {_mailAddressMock.Object},
                templateName);

            // Assert
            result.Should().ThrowExactly<ArgumentException>().And.ParamName.Should().Be(nameof(templateName));
        }

        [Test]
        public void SendMailAsyncWithSenderName_PassWrongValidTemplateName_ShouldReturnException()
        {
            // Arrange 
            _defaultSmtpMailClientMock
                .Setup(c => c.GetSenderAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IMailAddress>(_mailAddressMock.Object));

            _defaultSmtpMailClientMock
                .Setup(c => c.GetMailContentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IMailContent>(null));

            // Act
            Func<Task> result = async () => await _defaultSmtpMailClientMock.Object.SendMailAsync("sender",
                new[] {_mailAddressMock.Object}, new[] {_mailAddressMock.Object}, new[] {_mailAddressMock.Object},
                "wrong_name");

            // Assert
            result.Should().ThrowExactly<Exception>().And.Message.Should()
                .Be("Mail content whose name is wrong_name is not found.");
        }

        [Test]
        public async Task SendMailAsyncWithSenderName_PassValidParams_ShouldSendMail()
        {
            // Arrange 
            var attachment = new Attachment(Assembly.GetEntryAssembly()?.Location ?? string.Empty);
            
            _defaultSmtpMailClientMock
                .Setup(c => c.GetSenderAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IMailAddress>(_mailAddressMock.Object));

            _defaultSmtpMailClientMock
                .Setup(c => c.GetMailContentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(_mailContentMock.Object));
            
            _defaultSmtpMailClientMock.Protected()
                .Setup<SmtpClient>("GetSmtpClient", _mailClientSettingMock.Object)
                .Returns(_smtpClientMock.Object)
                .Verifiable();
            
          
            // Act
              await _defaultSmtpMailClientMock.Object.SendMailAsync("sender",
                new[] {_mailAddressMock.Object}, new[] {_mailAddressMock.Object}, new[] {_mailAddressMock.Object},
                "templateName", "additionalSubjectData", "additionalContent", new Attachment[]{ attachment});

            // Assert
           // _smtpClientMock.Verify(c=>c.SendMailAsync(It.IsAny<MailMessage>()), Times.Once);
        }

        #endregion
    }
}
