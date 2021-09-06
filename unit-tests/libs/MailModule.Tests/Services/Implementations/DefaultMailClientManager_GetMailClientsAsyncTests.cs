using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MailModule.Services.Implementations;
using MailModule.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace MailModule.Tests.Services.Implementations
{
    [TestFixture]
    public class DefaultMailClientManager_GetMailClientsAsyncTests
    {
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

        private List<IMailClient> _mailClientInterfaces;

        [Test]
        public async Task GetMailClientsAsync_ShouldReturnAllMailClients()
        {
            // Arrange           
            var defaultMailService = new DefaultMailClientManager(_mailClientInterfaces);

            // Act
            var mailClients = await defaultMailService.GetMailClientsAsync();

            // Assert
            mailClients.Length.Should().Be(2);
            mailClients.Any(c => c.DisplayName == "google").Should().BeTrue();
        }
    }
}