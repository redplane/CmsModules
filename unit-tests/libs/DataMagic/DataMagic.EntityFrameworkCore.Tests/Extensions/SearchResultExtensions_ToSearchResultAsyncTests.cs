using System.Threading.Tasks;
using DataMagic.Abstractions.Interfaces;
using DataMagic.Abstractions.Models;
using DataMagic.EntityFrameworkCore.Extensions;
using DataMagic.EntityFrameworkCore.Tests.TestingDb;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DataMagic.EntityFrameworkCore.Tests.Extensions
{
    // ReSharper disable once InconsistentNaming
    public class SearchResultExtensions_ToSearchResultAsyncTests
    {
        private TestDbContext _dbContext;
        [SetUp]
        public void Setup()
        {
            var factory = new ConnectionFactory();
            this._dbContext = factory.CreateContextForSQLite();
        }
        [Test]
        public async Task ToSearchResultAsync_PassTrueOfShouldItemsCountedAndQueried_ShouldReturnItemsMatch()
        {
            // Arrange
            var pagerMock = new Mock<IPager>();
            pagerMock.Setup(c => c.ShouldItemsCounted()).Returns(true);
            pagerMock.Setup(c => c.ShouldItemsQueried()).Returns(true);
            pagerMock.Setup(c => c.GetTotalRecords()).Returns(2);
            pagerMock.Setup(c => c.GetSkippedRecords()).Returns(2);

            // Act
            var actualUsers = (SearchResult<User>)await this._dbContext.Users.ToSearchResultAsync(pagerMock.Object);

            // Assert
           actualUsers.Items.Length.Should().Be(2);
           actualUsers.TotalRecords.Should( ).Be( 5 );

        }

        [Test]
        public async Task ToSearchResultAsync_PassFalseOfShouldItemsCountedAndQueried_ShouldReturnEmpty()
        {
            // Arrange
            var pagerMock = new Mock<IPager>();
            pagerMock.Setup(c => c.ShouldItemsCounted()).Returns(false);
            pagerMock.Setup(c => c.ShouldItemsQueried()).Returns(false);
            pagerMock.Setup(c => c.GetTotalRecords()).Returns(2);
            pagerMock.Setup(c => c.GetSkippedRecords()).Returns(2);

            // Act
            var actualUsers = (SearchResult<User>)await this._dbContext.Users.ToSearchResultAsync(pagerMock.Object);

            // Assert
            actualUsers.Items.Length.Should().Be(0);
            actualUsers.TotalRecords.Should().Be(0);

        }


    }

}