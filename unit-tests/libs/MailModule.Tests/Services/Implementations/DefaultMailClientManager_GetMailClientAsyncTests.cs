using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MailModule.Services.Implementations;
using MailModule.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace MailModule.Tests.Services.Implementations
{
    public class DefaultMailClientManager_GetMailClientAsyncTests
    {
        #region Properties

        private List<IMailClient> _mailClientInterfaces;

        #endregion

        #region Setup

        [SetUp]
        public void Setup()
        {
            this._mailClientInterfaces = new List<IMailClient>();
            var googleMailClientMock = new Mock<IMailClient>();
            googleMailClientMock.Setup(c => c.DisplayName).Returns("google");
            googleMailClientMock.Setup(c => c.UniqueName).Returns("google");
            this._mailClientInterfaces.Add(googleMailClientMock.Object);

            var yahooMailClientMock = new Mock<IMailClient>();
            yahooMailClientMock.Setup(c => c.DisplayName).Returns("yahoo");
            yahooMailClientMock.Setup(c => c.UniqueName).Returns("yahoo");
            this._mailClientInterfaces.Add(yahooMailClientMock.Object);
        }

        #endregion

        #region Methods

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public async Task GetMailClientAsync_SendInvalidUniqueName_ShouldReturnNull(string uniqueName)
        {
            // Arrange
            var defaultMailService = new DefaultMailClientManager(_mailClientInterfaces);

            //Act
            var mailClient = await defaultMailService.GetMailClientAsync(uniqueName);

            //Assert
            mailClient.Should().BeNull();
        }

        [Test]
        public async Task GetMailClientAsync_SendWrongValidUniqueName_ShouldReturnNull()
        {
            // Arrange           
            var defaultMailService = new DefaultMailClientManager(this._mailClientInterfaces);

            // Act
            var mailClient = await defaultMailService.GetMailClientAsync("uniqueName");

            // Assert
            mailClient.Should().BeNull();
        }

        [Test]
        public async Task GetMailClientAsync_SendRightValidUniqueName_ShouldReturnMailClient()
        {
            // Arrange           
            var defaultMailService = new DefaultMailClientManager(this._mailClientInterfaces);

            // Act
            var mailClient = await defaultMailService.GetMailClientAsync("google");

            // Assert
            mailClient.Should().NotBeNull();
            mailClient.UniqueName.Should().Be("google");
        }

        #endregion
    }
}