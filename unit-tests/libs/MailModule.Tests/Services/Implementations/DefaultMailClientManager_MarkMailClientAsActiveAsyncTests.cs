using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using MailModule.Services.Implementations;
using MailModule.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace MailModule.Tests.Services.Implementations
{
    public class DefaultMailClientManager_MarkMailClientAsActiveAsyncTests
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
            googleMailClientMock.Setup(c => c.UniqueName).Returns("google");
            _mailClientInterfaces.Add(googleMailClientMock.Object);

            var yahooMailClientMock = new Mock<IMailClient>();
            yahooMailClientMock.Setup(c => c.DisplayName).Returns("yahoo");
            yahooMailClientMock.Setup(c => c.UniqueName).Returns("yahoo");
            _mailClientInterfaces.Add(yahooMailClientMock.Object);
        }

        #endregion

        #region Methods

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        [TestCase("123")]
        public void MarkMailClientAsActiveAsync_SendInvalidUniqueName_ShouldReturnException(string uniqueName)
        {
            // Arrange
            var defaultMailService = new DefaultMailClientManager(_mailClientInterfaces);

            //Act
            Func<Task> mailClientTask = async () => await defaultMailService.MarkMailClientAsActiveAsync(uniqueName);

            //Assert
            mailClientTask.Should().ThrowExactly<Exception>()
                .WithMessage($"Mail service whose name {uniqueName} cannot be found");
        }


        [Test]
        public async Task MarkMailClientAsActiveAsync_SendRightValidUniqueName_ShouldReturnActiveMailClient()
        {
            // Arrange           
            var defaultMailService = new DefaultMailClientManager(_mailClientInterfaces);

            // Act
            await defaultMailService.MarkMailClientAsActiveAsync("yahoo");
            var mailClient = await defaultMailService.GetActiveMailClientAsync();

            // Assert
            mailClient.Should().NotBeNull();
            mailClient.UniqueName.Should().Be("yahoo");
        }

        #endregion
    }
}