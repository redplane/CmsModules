using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataMagic.Abstractions.Enums;
using DataMagic.EntityFrameworkCore.Extensions;
using DataMagic.EntityFrameworkCore.Tests.TestingDb;
using FluentAssertions;
using NUnit.Framework;

namespace DataMagic.EntityFrameworkCore.Tests.Extensions
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class SortingExtensions_WithPropertySortTests
    {
        [SetUp]
        public void Setup()
        {
            _connectionFactory = new ConnectionFactory();
            var dbContext = _connectionFactory.CreateContextForSQLite();
            var users = new List<User>
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

        private ConnectionFactory _connectionFactory;

        private IQueryable<User> _users;

        [Test]
        public void WithPropertySort_PassAscendingSortOfId_ShouldReturnAscendingItems()
        {
            // Arrange
            Expression<Func<User, int>> idExpression = user => user.Id;

            // Act
            var actualUsers = _users.WithPropertySort(idExpression, SortDirections.Ascending);

            // Assert
            actualUsers.Should().BeInAscendingOrder(idExpression);
            actualUsers.Count().Should().Be(_users.Count());
        }

        [Test]
        public void WithPropertySort_PassDecendingSortOfId_ShouldReturnDecendingItems()
        {
            // Arrange
            Expression<Func<User, int>> idExpression = user => user.Id;

            // Act
            var actualUsers = _users.WithPropertySort(idExpression, SortDirections.Descending);

            // Assert
            actualUsers.Should().BeInDescendingOrder(idExpression);
            actualUsers.Count().Should().Be(_users.Count());
        }

        [Test]
        public void WithPropertySort_PassNoneSortOfId_ShouldReturnOriginalItems()
        {
            // Arrange
            Expression<Func<User, DateTime?>> deathDayExpression = user => user.DeathTime;

            // Act
            var actualUsers = _users.WithPropertySort(deathDayExpression);

            // Assert
            actualUsers.Should().NotBeInAscendingOrder(deathDayExpression);
            actualUsers.Should().NotBeInDescendingOrder(deathDayExpression);
            actualUsers.Count().Should().Be(_users.Count());
        }
    }
}