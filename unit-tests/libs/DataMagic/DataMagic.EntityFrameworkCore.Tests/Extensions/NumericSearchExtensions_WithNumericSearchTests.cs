using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataMagic.Abstractions.Enums.Operators;
using DataMagic.Abstractions.Models.Filters;
using DataMagic.EntityFrameworkCore.Extensions;
using DataMagic.EntityFrameworkCore.Tests.TestingDb;
using FluentAssertions;
using NUnit.Framework;

namespace DataMagic.EntityFrameworkCore.Tests.Extensions
{
    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class NumericSearchExtensions_WithNumericSearchTests
    {
        #region Static

        #region - Public

        public static IEnumerable<TestCaseData> ValidParamsTestCaseData
        {
            get
            {
                yield return new TestCaseData(NumericComparisonOperators.Equal, new List<Number>
                {
                    new() { Id = 2, Int = 2, Float = (float)7.3 },
                    new() { Id = 3, Int = 2, Float = (float)5.4 }
                }.AsQueryable());

                yield return new TestCaseData(NumericComparisonOperators.GreaterThan, new List<Number>
                {
                    new() { Id = 4, Int = 3, Float = (float)5.4 }
                }.AsQueryable());

                yield return new TestCaseData(NumericComparisonOperators.SmallerThan, new List<Number>
                {
                    new() { Id = 1, Int = 1, Float = (float)2.6 }
                }.AsQueryable());

                yield return new TestCaseData(NumericComparisonOperators.GreaterThanEqualTo, new List<Number>
                {
                    new() { Id = 2, Int = 2, Float = (float)7.3 },
                    new() { Id = 3, Int = 2, Float = (float)5.4 },
                    new() { Id = 4, Int = 3, Float = (float)5.4 }
                }.AsQueryable());

                yield return new TestCaseData(NumericComparisonOperators.SmallerThanEqualTo, new List<Number>
                {
                    new() { Id = 1, Int = 1, Float = (float)2.6 },
                    new() { Id = 2, Int = 2, Float = (float)7.3 },
                    new() { Id = 3, Int = 2, Float = (float)5.4 }
                }.AsQueryable());
            }
        }

        #endregion

        #endregion

        #region Private

        private ConnectionFactory _connectionFactory;

        private IQueryable<Number> _numbers;

        #endregion

        #region Public

        [SetUp]
        public void Setup()
        {
            _connectionFactory = new ConnectionFactory();
            var dbContext = _connectionFactory.CreateContextForSQLite();
            var numbers = new List<Number>
            {
                new() { Id = 1, Int = 1, Float = (float)2.6 },
                new() { Id = 2, Int = 2, Float = (float)7.3 },
                new() { Id = 3, Int = 2, Float = (float)5.4 },
                new() { Id = 4, Int = 3, Float = (float)5.4 }
            };
            dbContext.Numbers.AddRange(numbers);
            dbContext.SaveChanges();

            _numbers = dbContext.Numbers.AsQueryable();
        }

        [TearDown]
        public void TearDown()
        {
            _connectionFactory.Dispose();
        }

        [Test]
        public void WithNumericSearch_PassInvalidValueType_ShouldThrowException()
        {
            // Arrange
            Expression<Func<Number, string>> property = number => "number.Text";
            var range = new NumericFilter<string>("2", NumericComparisonOperators.Equal);

            // Act
            Action actualResult = () => _numbers.WithNumericSearch(property, range);

            // Assert
            actualResult.Should().ThrowExactly<Exception>().And.Message.Should().Be("TValueType is not supported.");
        }

        [Test]
        public void WithNumericSearch_PassNullBodyOfProperty_ShouldThrowException()
        {
            // Arrange
            Expression<Func<Number, int>> intField = number => 1;
            var range = new NumericFilter<int>(2, NumericComparisonOperators.Equal);

            // Act
            Action actualResult = () => _numbers.WithNumericSearch(intField, range);

            // Assert
            actualResult.Should().ThrowExactly<ArgumentException>().And.Message.Should()
                .Be("Property expected (Parameter 'property')");
        }

        [Test]
        public void WithNumericSearch_PassNullProperty_ShouldReturnAllItems()
        {
            // Arrange
            var range = new NumericFilter<int>(2, NumericComparisonOperators.Equal);

            // Act
            var actualNumbers = _numbers.WithNumericSearch(null, range);

            // Assert
            actualNumbers.Should().BeEquivalentTo(_numbers);
        }

        [Test]
        public void WithNumericSearch_PassNullRange_ShouldReturnAllItems()
        {
            // Arrange
            Expression<Func<Number, int>> intField = number => number.Int;

            // Act
            var actualNumbers = _numbers.WithNumericSearch(intField, null);

            // Assert
            actualNumbers.Should().BeEquivalentTo(_numbers);
        }

        [TestCaseSource(nameof(ValidParamsTestCaseData))]
        public void WithNumericSearch_PassValidParams_ShouldReturnMatchedItems(
            NumericComparisonOperators numericComparisonOperators, IQueryable<Number> expectedNumbers)
        {
            // Arrange
            Expression<Func<Number, int>> property = number => number.Int;
            var range = new NumericFilter<int>(2, numericComparisonOperators);

            // Act
            var actualNumbers = _numbers.WithNumericSearch(property, range);

            // Assert
            actualNumbers.Should().BeEquivalentTo(expectedNumbers);
        }

        [Test]
        public void WithNumericSearch_PassWrongOperator_ThrowException()
        {
            // Arrange
            Expression<Func<Number, int>> property = number => number.Int;
            var range = new NumericFilter<int>(2, NumericComparisonOperators.None);

            // Act
            Action result = () => _numbers.WithNumericSearch(property, range);

            // Assert
            result.Should().ThrowExactly<ArgumentException>().And.Message.Should().Be("Not supported comparison mode");
        }

        #endregion
    }
}