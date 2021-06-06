using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MailModule.Models.Interfaces;
using MailModule.Services.Implementations;
using Moq;
using NUnit.Framework;

namespace MailModule.Tests.Services.Implementations
{
    public class DefaultSmtpMailClient_SendMailAsyncTests
    {
        #region Properties

        private Mock<IMailClientSetting> _mailClientSettingMock;
        private Mock<DefaultSmtpMailClient> _defaultSmtpMailClientMock;
        private Mock<IMailAddress> _mailAddressMock;

        #endregion

        #region Setup

        [SetUp]
        public void Setup()
        {
            // Mock mail client setting.
            _mailClientSettingMock = new Mock<IMailClientSetting>();
            _mailClientSettingMock.Setup(c => c.DisplayName).Returns("mailClientSetting");

            // Mock default smtp mail client.
            _defaultSmtpMailClientMock = new Mock<DefaultSmtpMailClient>(MockBehavior.Loose, _mailClientSettingMock.Object)
            {
                CallBase = true
            };
            _defaultSmtpMailClientMock.Setup(c => c.DisplayName).Returns("smtp");
        

            // Mock mail address.
            _mailAddressMock = new Mock<IMailAddress>();
            _mailAddressMock.Setup(c => c.DisplayName).Returns("address");
        }

        #endregion

        #region Methods

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void SendMailAsync_PassInvalidSenderName_ShouldReturnException(string sender)
        {
            // Act
          Func<Task> result= async () => await  _defaultSmtpMailClientMock.Object.SendMailAsync(sender,
                new[] {_mailAddressMock.Object}, new[] {_mailAddressMock.Object}, new[] {_mailAddressMock.Object},
                It.IsAny<string>());
          
            // Assert
            result.Should().ThrowExactly<ArgumentException>().And.ParamName.Should().Be(nameof(sender));
        }
     [Test]
        public void SendMailAsync_PassValidWrongSenderName_ShouldReturnException()
        {
            // Arrange
            _defaultSmtpMailClientMock.Setup(c => c.GetSenderAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IMailAddress>(null));
            
            // Act
            Func<Task> result= async () => await  _defaultSmtpMailClientMock.Object.SendMailAsync("wrong_name",
                new[] {_mailAddressMock.Object}, new[] {_mailAddressMock.Object}, new[] {_mailAddressMock.Object},
                "name");
          
            // Assert
            result.Should().ThrowExactly<Exception>().WithMessage($"Mail sender whose name is wrong_name is not found.");
        }

        [Test]
        public void SendMailAsync_PassNullRecipients_ShouldReturnException()
        {
            // Arrange
            _defaultSmtpMailClientMock.Setup(c => c.GetSenderAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IMailAddress>(_mailAddressMock.Object));
            
            // Act
            Func<Task> result= async () => await  _defaultSmtpMailClientMock.Object.SendMailAsync("sender",
                null, new[] {_mailAddressMock.Object}, new[] {_mailAddressMock.Object},
                "name");
          
            // Assert
            result.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("recipients");
        }

        #endregion
    }
}