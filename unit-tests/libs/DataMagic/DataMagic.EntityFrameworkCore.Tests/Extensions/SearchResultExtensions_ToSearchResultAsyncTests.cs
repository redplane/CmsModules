using System;
using System.Collections.Generic;
using System.Linq;
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
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class SearchResultExtensions_ToSearchResultAsyncTests
    {
        #region Private

        private IQueryable<User> _users;
        private ConnectionFactory _connectionFactory;

        #endregion

        #region Public

        [SetUp]
        public void Setup()
        {
            _connectionFactory = new ConnectionFactory();
            var dbContext = _connectionFactory.CreateContextForSQLite();
            var users = new List<User>()
            {
                new() { Id = 1, Name = "Name1", Birthday = Convert.ToDateTime("1-1-2015"), DeathTime = null },
                new()
                {
                    Id = 2, Name = "Name2", Birthday = Convert.ToDateTime("1-1-2016"),
                    DeathTime = Convert.ToDateTime("1-1-2076")
                },
                new()
                {
                    Id = 3, Name = "Name3", Birthday = Convert.ToDateTime("1-1-2016"),
                    DeathTime = Convert.ToDateTime("1-1-2096")
                },
                new()
                {
                    Id = 4, Name = "Name4", Birthday = Convert.ToDateTime("1-1-2017"),
                    DeathTime = Convert.ToDateTime("1-1-2086")
                },
                new()
                {
                    Id = 5, Name = "Name5", Birthday = Convert.ToDateTime("1-1-2018"),
                    DeathTime = Convert.ToDateTime("1-1-2086")
                }
            };
            dbContext.Users.AddRange(users);
            dbContext.SaveChanges();

            _users = dbContext.Users.AsQueryable();
        }

        [TearDown]
        public void TearDown()
        {
            _connectionFactory.Dispose();
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
            var actualUsers = (SearchResult<User>)await _users.ToSearchResultAsync(pagerMock.Object);

            // Assert
            actualUsers.Items.Length.Should().Be(0);
            actualUsers.TotalRecords.Should().Be(0);
        }

        [Test]
        public async Task ToSearchResultAsync_PassNullPager_ShouldReturnAllItems()
        {
            // Act
            var actualUsers = (SearchResult<User>)await _users.ToSearchResultAsync(null);

            // Assert
            actualUsers.Items.Should().BeEquivalentTo(_users.AsQueryable().ToList());
            actualUsers.TotalRecords.Should().Be(_users.Count());
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
            var actualUsers = (SearchResult<User>)await _users.ToSearchResultAsync(pagerMock.Object);

            // Assert
            actualUsers.Items.Length.Should().Be(2);
            actualUsers.TotalRecords.Should().Be(5);
        }

        #endregion
    }
}