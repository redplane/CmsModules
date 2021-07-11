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
            this._mailClientInterfaces.Add(googleMailClientMock.Object);

            var yahooMailClientMock = new Mock<IMailClient>();
            yahooMailClientMock.Setup(c => c.DisplayName).Returns("yahoo");
            this._mailClientInterfaces.Add(yahooMailClientMock.Object);
        }


        #endregion

        #region Methods

        [Test]
        public async Task GetMailClientsAsync_ShouldReturnAllMailClients( )
        {
            // Arrange           
            var defaultMailService = new DefaultMailClientManager( this._mailClientInterfaces );

            // Act
            var mailClients = await defaultMailService.GetMailClientsAsync( );

            // Assert
            mailClients.Length.Should( ).Be( 2 );
            mailClients.Any( c => c.DisplayName == "google" ).Should( ).BeTrue( );
        }

   
        #endregion
    }
}