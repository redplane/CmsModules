using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using MailModule.Services.Implementations;
using MailModule.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace MailModule.Tests.Services.Implementations
{
    public class DefaultMailClientManager_GetActiveMailClientAsyncTests
    {
        #region Properties

        private List<IMailClient> _mailClientInterfaces;

        #endregion

        #region Setup

        [SetUp]
        public void Setup()
        {
            _mailClientInterfaces = new List<IMailClient>();
            var googleMailClientMock = new Mock<IMailClient>();
            googleMailClientMock.Setup(c => c.DisplayName).Returns("google");
            _mailClientInterfaces.Add(googleMailClientMock.Object);

            var yahooMailClientMock = new Mock<IMailClient>();
            yahooMailClientMock.Setup(c => c.DisplayName).Returns("yahoo");
            _mailClientInterfaces.Add(yahooMailClientMock.Object);
        }

        #endregion

        #region Methods

        [Test]
        public async Task GetActiveMailClientAsync_ShouldReturnActiveMailClient()
        {
            // Arrange           
            var defaultMailService = new DefaultMailClientManager(_mailClientInterfaces);

            // Act
            var mailClient = await defaultMailService.GetActiveMailClientAsync();

            // Assert
            mailClient.Should().NotBeNull();
            mailClient.DisplayName.Should().Be("google");
        }

        #endregion
    }
}