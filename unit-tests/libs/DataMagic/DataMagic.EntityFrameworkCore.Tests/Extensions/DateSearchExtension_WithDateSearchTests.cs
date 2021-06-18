using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using DataMagic.Abstractions.Enums.Operators;
using DataMagic.Abstractions.Models;
using DataMagic.Abstractions.Models.Filters;
using DataMagic.EntityFrameworkCore.Extensions;
using DataMagic.EntityFrameworkCore.Tests.TestingDb;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DataMagic.EntityFrameworkCore.Tests.Extensions
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class DateSearchExtension_WithDateSearchTests
    {
        #region Static

        #region - Public

        public static IEnumerable<TestCaseData> UserBirthdayMatchedOperatorTestCases
        {
            get
            {
                yield return new TestCaseData(DateComparisonOperators.Equal, new List<User>
                    {
                        new() { Id = 2, Name = "Name2", Birthday = Convert.ToDateTime("1-1-2016"), DeathTime = Convert.ToDateTime("1-1-2076") },
                        new() { Id = 3, Name = "Name3", Birthday = Convert.ToDateTime("1-1-2016"), DeathTime = Convert.ToDateTime("1-1-2096") }
                    }.AsQueryable());

                yield return new TestCaseData(DateComparisonOperators.SmallerThan, new List<User>
                    {
                        new() { Id = 1, Name = "Name1", Birthday = Convert.ToDateTime("1-1-2015") , DeathTime = null}
                    }.AsQueryable());

                yield return new TestCaseData(DateComparisonOperators.GreaterThan, new List<User>
                    {
                        new() { Id = 4, Name = "Name4", Birthday = Convert.ToDateTime("1-1-2017"), DeathTime = Convert.ToDateTime("1-1-2086") },
                        new() { Id = 5, Name = "Name5", Birthday = Convert.ToDateTime("1-1-2018"), DeathTime = Convert.ToDateTime("1-1-2086") }
                    }.AsQueryable());

                yield return new TestCaseData(DateComparisonOperators.SmallerThanEqualTo, new List<User>
                    {
                        new() { Id = 1, Name = "Name1", Birthday = Convert.ToDateTime("1-1-2015") , DeathTime = null},
                        new() { Id = 2, Name = "Name2", Birthday = Convert.ToDateTime("1-1-2016"), DeathTime = Convert.ToDateTime("1-1-2076") },
                        new() { Id = 3, Name = "Name3", Birthday = Convert.ToDateTime("1-1-2016"), DeathTime = Convert.ToDateTime("1-1-2096") }
                    }.AsQueryable());

                yield return new TestCaseData(DateComparisonOperators.GreaterThanEqualTo, new List<User>
                    {
                        new() { Id = 2, Name = "Name2", Birthday = Convert.ToDateTime("1-1-2016"), DeathTime = Convert.ToDateTime("1-1-2076") },
                        new() { Id = 3, Name = "Name3", Birthday = Convert.ToDateTime("1-1-2016"), DeathTime = Convert.ToDateTime("1-1-2096") },
                        new() { Id = 4, Name = "Name4", Birthday = Convert.ToDateTime("1-1-2017"), DeathTime = Convert.ToDateTime("1-1-2086") },
                        new() { Id = 5, Name = "Name5", Birthday = Convert.ToDateTime("1-1-2018"), DeathTime = Convert.ToDateTime("1-1-2086") }
                    }.AsQueryable());
            }
        }

        public static IEnumerable<TestCaseData> UserDeathDayMatchedOperatorTestCases
        {
            get
            {
                yield return new TestCaseData(DateComparisonOperators.Equal, new List<User>
                    {
                        new() { Id = 4, Name = "Name4", Birthday = Convert.ToDateTime("1-1-2017"), DeathTime = Convert.ToDateTime("1-1-2086") },
                        new() { Id = 5, Name = "Name5", Birthday = Convert.ToDateTime("1-1-2018"), DeathTime = Convert.ToDateTime("1-1-2086") }
                    }.AsQueryable());

                yield return new TestCaseData(DateComparisonOperators.SmallerThan, new List<User>
                    {
                        new() { Id = 2, Name = "Name2", Birthday = Convert.ToDateTime("1-1-2016"), DeathTime = Convert.ToDateTime("1-1-2076") }
                    }.AsQueryable());

                yield return new TestCaseData(DateComparisonOperators.GreaterThan, new List<User>
                    {
                        new() { Id = 3, Name = "Name3", Birthday = Convert.ToDateTime("1-1-2016"), DeathTime = Convert.ToDateTime("1-1-2096") }
                    }.AsQueryable());

                yield return new TestCaseData(DateComparisonOperators.SmallerThanEqualTo, new List<User>
                    {
                        new() { Id = 2, Name = "Name2", Birthday = Convert.ToDateTime("1-1-2016"), DeathTime = Convert.ToDateTime("1-1-2076") },
                        new() { Id = 4, Name = "Name4", Birthday = Convert.ToDateTime("1-1-2017"), DeathTime = Convert.ToDateTime("1-1-2086") },
                        new() { Id = 5, Name = "Name5", Birthday = Convert.ToDateTime("1-1-2018"), DeathTime = Convert.ToDateTime("1-1-2086") }
                    }.AsQueryable());

                yield return new TestCaseData(DateComparisonOperators.GreaterThanEqualTo, new List<User>
                    {
                        new() { Id = 3, Name = "Name3", Birthday = Convert.ToDateTime("1-1-2016"), DeathTime = Convert.ToDateTime("1-1-2096") },
                        new() { Id = 4, Name = "Name4", Birthday = Convert.ToDateTime("1-1-2017"), DeathTime = Convert.ToDateTime("1-1-2086") },
                        new() { Id = 5, Name = "Name5", Birthday = Convert.ToDateTime("1-1-2018"), DeathTime = Convert.ToDateTime("1-1-2086") }
                    }.AsQueryable());
            }
        }

        #endregion

        #endregion

        #region Private

        private IQueryable<User> _users;

        #endregion

        #region Public

        [SetUp]
        public void Setup()
        {
            var factory = new ConnectionFactory();
            var context = factory.CreateContextForSQLite();
           
            this._users = context.Users.AsQueryable();
        }
        [Test]
        public void WithDateSearchAllowDatetimeNull_PassWrongOperator_ThrowException()
        {
            // Arrange
            Expression<Func<User, DateTime?>> x = user => user.DeathTime;
            var dateFilter = new DateFilter(new Date(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), DateComparisonOperators.None);

            // Act
            Action result = () => this._users.WithDateSearch(x, dateFilter);

            // Assert
            result.Should().ThrowExactly<ArgumentException>().And.Message.Should().Be("Not supported operator (Parameter 'Operator')");
        }

        [Test]
        public void WithDateSearchAllowDatetimeNull_PassWrongPropertyBody_ThrowException()
        {
            // Arrange
            Expression<Func<User, DateTime?>> x = user => new DateTime();
            var dateFilter = new DateFilter(new Date(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), DateComparisonOperators.Equal);

            // Act
            Action result = () => this._users.WithDateSearch(x, dateFilter);

            // Assert
            result.Should().ThrowExactly<ArgumentException>().And.Message.Should().Be("Property expected (Parameter 'property')");
        }

        [Test]
        public void WithDateSearchNotAllowDatetimeNull_PassWrongOperator_ThrowException()
        {
            // Arrange
            Expression<Func<User, DateTime>> x = user => user.Birthday;
            var dateFilter = new DateFilter(new Date(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), DateComparisonOperators.None);

            // Act
            Action result = () => this._users.WithDateSearch(x, dateFilter);

            // Assert
            result.Should().ThrowExactly<ArgumentException>().And.Message.Should().Be("Not supported operator (Parameter 'Operator')");
        }

        [Test]
        public void WithDateSearchNotAllowDatetimeNull_PassWrongPropertyBody_ThrowException()
        {
            // Arrange
            Expression<Func<User, DateTime>> x = user => new DateTime();
            var dateFilter = new DateFilter(new Date(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), DateComparisonOperators.Equal);

            // Act
            Action result = () => this._users.WithDateSearch(x, dateFilter);

            // Assert
            result.Should().ThrowExactly<ArgumentException>().And.Message.Should().Be("Property expected (Parameter 'property')");
        }

        [Test]
        public void WithDateSearchWithAllowDatetimeNull_PassDateFilterWithNullValue_ReturnFullItems()
        {
            // Arrange
            Expression<Func<User, DateTime?>> x = user => user.DeathTime;
            var dateFilter = new DateFilter(null, DateComparisonOperators.Equal);

            // Act
            var result = this._users.WithDateSearch(x, dateFilter);

            // Assert
            result.Should().BeEquivalentTo(this._users);
        }

        [TestCaseSource(nameof(UserDeathDayMatchedOperatorTestCases))]
        public void WithDateSearchWithAllowDatetimeNull_PassEqualOperator_ReturnMatchedItems(DateComparisonOperators dateComparisonOperator, IQueryable<User> expectedUsers)
        {
            // Arrange
            Expression<Func<User, DateTime?>> x = user => user.DeathTime;
            var dateFilter = new DateFilter(new Date(2086, 1, 1), dateComparisonOperator);
            
            // Act
            var result = this._users.WithDateSearch(x, dateFilter);

            // Assert
            result.Should().BeEquivalentTo(expectedUsers);
        }

        [Test]
        public void WithDateSearchWithAllowDatetimeNull_PassNullDateFilter_ReturnFullItems()
        {
            // Arrange
            Expression<Func<User, DateTime?>> x = text => DateTime.Now;

            // Act
            var result = this._users.WithDateSearch(x, null);

            // Assert
            result.Should().BeEquivalentTo(this._users);
        }

        [Test]
        public void WithDateSearchWithAllowDatetimeNull_PassNullProperty_ReturnFullItems()
        {
            // Act
            var result = this._users.WithDateSearch((Expression<Func<User, DateTime?>>)null, It.IsAny<DateFilter>());

            // Assert
            result.Should().BeEquivalentTo(this._users);
        }

        [Test]
        public void WithDateSearchWithNotAllowDatetimeNull_PassDateFilterWithNullValue_ReturnFullItems()
        {
            // Arrange
            Expression<Func<User, DateTime>> x = user => user.Birthday;
            var dateFilter = new DateFilter(null, DateComparisonOperators.Equal);

            // Act
            var result = this._users.WithDateSearch(x, dateFilter);

            // Assert
            result.Should().BeEquivalentTo(this._users);
        }

        [TestCaseSource(nameof(UserBirthdayMatchedOperatorTestCases))]
        public void WithDateSearchWithNotAllowDatetimeNull_PassEqualOperator_ReturnMatchedItems(DateComparisonOperators dateComparisonOperator, IQueryable<User> expectedUsers)
        {
            // Arrange
            Expression<Func<User, DateTime>> x = user => user.Birthday;
            var dateFilter = new DateFilter(new Date(2016, 1, 1), dateComparisonOperator);

            // Act
            var result = this._users.WithDateSearch(x, dateFilter);

            // Assert
            result.Should().BeEquivalentTo(expectedUsers);
        }

        [Test]
        public void WithDateSearchWithNotAllowDatetimeNull_PassNullDateFilter_ReturnFullItems()
        {
            // Arrange
            Expression<Func<User, DateTime>> x = user => user.Birthday;

            // Act
            var result = this._users.WithDateSearch(x, null);

            // Assert
            result.Should().BeEquivalentTo(this._users);
        }

        [Test]
        public void WithDateSearchWithNotAllowDatetimeNull_PassNullProperty_ReturnFullItems()
        {
            // Act
            var result = this._users.WithDateSearch((Expression<Func<User, DateTime>>)null, It.IsAny<DateFilter>());

            // Assert
            result.Should().BeEquivalentTo(this._users);
        }

        #endregion
    }
}