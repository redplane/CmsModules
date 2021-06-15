using System;
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
        private IQueryable<User> _users;
        [SetUp]
        public void Setup()
        {
            var factory = new ConnectionFactory();
            var context = factory.CreateContextForSQLite();
            _users = context.Users.AsQueryable();
        }

        [Test]
        public void WithDateSearchWithAllowDatetimeNull_PassNullDateFilter_ReturnFullItems()
        {
            // Arrange
            Expression<Func<User, DateTime?>> x = text => DateTime.Now;

            // Act
           var result=  this._users.WithDateSearch( x, null );

            // Assert
            result.Should( ).BeEquivalentTo( this._users );
        }
        [Test]
        public void WithDateSearchWithNotAllowDatetimeNull_PassNullDateFilter_ReturnFullItems()
        {
            // Arrange
            Expression<Func<User, DateTime>> x = text => DateTime.Now;

            // Act
            var result = this._users.WithDateSearch(x, null);

            // Assert
            result.Should().BeEquivalentTo(this._users);
        }


        [Test]
        public void WithDateSearchWithAllowDatetimeNull_PassNullProperty_ReturnFullItems()
        {
          
            // Act
            var result = this._users.WithDateSearch(null, It.IsAny<DateFilter>(  ));

            // Assert
            result.Should().BeEquivalentTo(this._users);
        }

        [Test]
        public void WithDateSearchWithAllowDatetimeNull_PassDateFilterWithNullValue_ReturnFullItems()
        {
            // Arrange
            Expression<Func<User, DateTime>> x = text => DateTime.Now;
            var dateFilter = new DateFilter( null, DateComparisonOperators.Equal );

            // Act
            var result = this._users.WithDateSearch(x, dateFilter);

            // Assert
            result.Should().BeEquivalentTo(this._users);
        }

        [Test]
        public void WithDateSearchWithAllowDatetimeNull_PassEqualOperator_ReturnMatchedItems()
        {
            // Arrange
            Expression<Func<User, DateTime>> x = text => DateTime.Now.Date;
            var dateFilter = new DateFilter(new Date(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), DateComparisonOperators.Equal);

            // Act
            var result = this._users.WithDateSearch(x, dateFilter);

            // Assert
            result.Should().BeEquivalentTo(this._users);
        }
    }
}