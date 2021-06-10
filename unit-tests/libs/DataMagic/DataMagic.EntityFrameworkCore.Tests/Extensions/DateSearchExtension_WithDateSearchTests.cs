using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using DataMagic.Abstractions.Models;
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
        public void WithDateSearch_PassNullDateFilter_ReturnFullItems()
        {
            // Arrange
            Expression<Func<User, DateTime?>> x = text => DateTime.Now;

            // Act
           var result=  this._users.WithDateSearch( x, null );

            // Assert
            result.ToList( ).Count.Should( ).Be( this._users.ToList( ).Count );
        }
    }
}