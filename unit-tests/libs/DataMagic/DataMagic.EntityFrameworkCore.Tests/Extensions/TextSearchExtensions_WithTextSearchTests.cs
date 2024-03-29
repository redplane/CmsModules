﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataMagic.Abstractions.Enums.Operators;
using DataMagic.Abstractions.Models.Filters;
using DataMagic.EntityFrameworkCore.Extensions;
using DataMagic.EntityFrameworkCore.Tests.TestingDb;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DataMagic.EntityFrameworkCore.Tests.Extensions
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class TextSearchExtensions_WithTextSearchTests
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

        public static IEnumerable<TestCaseData> UserNameMatchOperatorTestCaseData
        {
            get
            {
                yield return new TestCaseData(TextFilterModes.StartWith, "N", new List<User>
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
                }.AsQueryable());

                yield return new TestCaseData(TextFilterModes.EndWith, "1", new List<User>
                {
                    new() { Id = 1, Name = "Name1", Birthday = Convert.ToDateTime("1-1-2015"), DeathTime = null }
                }.AsQueryable());

                yield return new TestCaseData(TextFilterModes.Contains, "5", new List<User>
                {
                    new()
                    {
                        Id = 5, Name = "Name5", Birthday = Convert.ToDateTime("1-1-2018"),
                        DeathTime = Convert.ToDateTime("1-1-2086")
                    }
                }.AsQueryable());

                yield return new TestCaseData(TextFilterModes.Equal, "Name3", new List<User>
                {
                    new()
                    {
                        Id = 3, Name = "Name3", Birthday = Convert.ToDateTime("1-1-2016"),
                        DeathTime = Convert.ToDateTime("1-1-2096")
                    }
                }.AsQueryable());

                yield return new TestCaseData(TextFilterModes.None, "Name3", new List<User>
                {
                    new()
                    {
                        Id = 3, Name = "Name3", Birthday = Convert.ToDateTime("1-1-2016"),
                        DeathTime = Convert.ToDateTime("1-1-2096")
                    }
                }.AsQueryable());
            }
        }

        private ConnectionFactory _connectionFactory;

        private IQueryable<User> _users;

        [Test]
        public void WithTextSearch_PassInvalidPropertyBody_ShouldThrowException()
        {
            // Arrange
            Expression<Func<User, string>> x = user => new string("");
            var textFilter = new TextFilter();
            textFilter.Value = "name";

            // Act
            Action result = () => _users.WithTextSearch(x, textFilter);

            // Assert
            result.Should().ThrowExactly<ArgumentException>().And.Message.Should()
                .Be("Property expected (Parameter 'property')");
        }

        [Test]
        public void WithTextSearch_PassNullFilter_ShouldReturnAllItems()
        {
            // Act
            var actualUser = _users.WithTextSearch(It.IsAny<Expression<Func<User, string>>>(), null);

            // Assert
            actualUser.Should().BeEquivalentTo(_users);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void WithTextSearch_PassNullOrEmptyFilterValue_ShouldReturnOriginalItems(string filterValue)
        {
            // Arrange
            Expression<Func<User, string>> x = user => user.Name;
            var textFilter = new TextFilter();
            textFilter.Value = filterValue;
            textFilter.Mode = TextFilterModes.Equal;

            // Act
            var actualUsers = _users.WithTextSearch(x, textFilter);

            // Assert
            actualUsers.Should().BeEquivalentTo(_users);
        }

        [Test]
        public void WithTextSearch_PassNullProperty_ShouldThrowException()
        {
            // Act
            Action result = () => _users.WithTextSearch(null, new TextFilter());

            // Assert
            result.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("property");
        }

        [TestCaseSource(nameof(UserNameMatchOperatorTestCaseData))]
        public void WithTextSearch_PassValidParams_ShouldReturnMatchItems(
            TextFilterModes textFilterModes, string value, IQueryable<User> expectedUsers)
        {
            // Arrange
            Expression<Func<User, string>> x = user => user.Name;
            var textFilter = new TextFilter();
            textFilter.Value = value;
            textFilter.Mode = textFilterModes;

            // Act
            var actualUsers = _users.WithTextSearch(x, textFilter);

            // Assert
            actualUsers.Should().BeEquivalentTo(expectedUsers);
        }
    }
}