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
    // ReSharper disable once InconsistentNaming
    public class NumericSearchExtensions_WithNumericRangeSearchTests
    {
        #region Static

        #region - Public

        public static IEnumerable<TestCaseData> ValidParamsTestCaseData
        {
            get
            {
                // For from number is null.
                yield return new TestCaseData( null,
                                               new NumericFilter<float>( ( float ) 5.4, NumericComparisonOperators.Equal ),
                                               new List<Number>
                                                   {
                                                       new() { Id = 3, Int = 2, Float = ( float ) 5.4 },
                                                       new() { Id = 4, Int = 3, Float = ( float ) 5.4 }
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( null,
                                               new NumericFilter<float>( ( float ) 5.4, NumericComparisonOperators.GreaterThan ),
                                               new List<Number>
                                                   {
                                                       new() { Id = 2, Int = 2, Float = ( float ) 7.3 }
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( null,
                                               new NumericFilter<float>( ( float ) 5.4, NumericComparisonOperators.SmallerThan ),
                                               new List<Number>
                                                   {
                                                       new() { Id = 1, Int = 1, Float = ( float ) 2.6 }
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( null,
                                               new NumericFilter<float>( ( float ) 5.4, NumericComparisonOperators.SmallerThanEqualTo ),
                                               new List<Number>
                                                   {
                                                       new() { Id = 1, Int = 1, Float = ( float ) 2.6 },
                                                       new() { Id = 3, Int = 2, Float = ( float ) 5.4 },
                                                       new() { Id = 4, Int = 3, Float = ( float ) 5.4 }
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( null,
                                               new NumericFilter<float>( ( float ) 5.4, NumericComparisonOperators.GreaterThanEqualTo ),
                                               new List<Number>
                                                   {
                                                       new() { Id = 2, Int = 2, Float = ( float ) 7.3 },
                                                       new() { Id = 3, Int = 2, Float = ( float ) 5.4 },
                                                       new() { Id = 4, Int = 3, Float = ( float ) 5.4 }
                                                   }.AsQueryable( ) );

                // For to number is null.
                // For from number is null.
                yield return new TestCaseData( new NumericFilter<float>( ( float ) 5.4, NumericComparisonOperators.Equal ),
                                               null,
                                               new List<Number>
                                                   {
                                                       new() { Id = 3, Int = 2, Float = ( float ) 5.4 },
                                                       new() { Id = 4, Int = 3, Float = ( float ) 5.4 }
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( new NumericFilter<float>( ( float ) 5.4, NumericComparisonOperators.GreaterThan ),
                                               null,
                                               new List<Number>
                                                   {
                                                       new() { Id = 2, Int = 2, Float = ( float ) 7.3 }
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( new NumericFilter<float>( ( float ) 5.4, NumericComparisonOperators.SmallerThan ),
                                               null,
                                               new List<Number>
                                                   {
                                                       new() { Id = 1, Int = 1, Float = ( float ) 2.6 }
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( new NumericFilter<float>( ( float ) 5.4, NumericComparisonOperators.SmallerThanEqualTo ),
                                               null,
                                               new List<Number>
                                                   {
                                                       new() { Id = 1, Int = 1, Float = ( float ) 2.6 },
                                                       new() { Id = 3, Int = 2, Float = ( float ) 5.4 },
                                                       new() { Id = 4, Int = 3, Float = ( float ) 5.4 }
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( new NumericFilter<float>( ( float ) 5.4, NumericComparisonOperators.GreaterThanEqualTo ),
                                               null,
                                               new List<Number>
                                                   {
                                                       new() { Id = 2, Int = 2, Float = ( float ) 7.3 },
                                                       new() { Id = 3, Int = 2, Float = ( float ) 5.4 },
                                                       new() { Id = 4, Int = 3, Float = ( float ) 5.4 }
                                                   }.AsQueryable( ) );

                // For from operator is equal
                yield return new TestCaseData( new NumericFilter<float>( ( float ) 2.6, NumericComparisonOperators.Equal ),
                                               new NumericFilter<float>( ( float ) 7.3, NumericComparisonOperators.Equal ),
                                               new List<Number>( ).AsQueryable( ) );

                yield return new TestCaseData( new NumericFilter<float>( ( float ) 2.6, NumericComparisonOperators.Equal ),
                                               new NumericFilter<float>( ( float ) 7.3, NumericComparisonOperators.GreaterThan ),
                                               new List<Number>( ).AsQueryable( ) );

                yield return new TestCaseData( new NumericFilter<float>( ( float ) 2.6, NumericComparisonOperators.Equal ),
                                               new NumericFilter<float>( ( float ) 7.3, NumericComparisonOperators.SmallerThan ),
                                               new List<Number>
                                                   {
                                                       new() { Id = 1, Int = 1, Float = ( float ) 2.6 },
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( new NumericFilter<float>( ( float ) 2.6, NumericComparisonOperators.Equal ),
                                               new NumericFilter<float>( ( float ) 7.3, NumericComparisonOperators.SmallerThanEqualTo ),
                                               new List<Number>
                                                   {
                                                       new() { Id = 1, Int = 1, Float = ( float ) 2.6 },
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( new NumericFilter<float>( ( float ) 2.6, NumericComparisonOperators.Equal ),
                                               new NumericFilter<float>( ( float ) 7.3, NumericComparisonOperators.GreaterThanEqualTo ),
                                               new List<Number>( ).AsQueryable( ) );

                // For from operator is greater than

                yield return new TestCaseData( new NumericFilter<float>( ( float ) 2.6, NumericComparisonOperators.GreaterThan ),
                                               new NumericFilter<float>( ( float ) 7.3, NumericComparisonOperators.Equal ),
                                               new List<Number>
                                                   {
                                                       new() { Id = 2, Int = 2, Float = ( float ) 7.3 }
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( new NumericFilter<float>( ( float ) 2.6, NumericComparisonOperators.GreaterThan ),
                                               new NumericFilter<float>( ( float ) 7.3, NumericComparisonOperators.GreaterThan ),
                                               new List<Number>( ).AsQueryable( ) );

                yield return new TestCaseData( new NumericFilter<float>( ( float ) 2.6, NumericComparisonOperators.GreaterThan ),
                                               new NumericFilter<float>( ( float ) 7.3, NumericComparisonOperators.SmallerThan ),
                                               new List<Number>
                                                   {
                                                       new() { Id = 3, Int = 2, Float = ( float ) 5.4 },
                                                       new() { Id = 4, Int = 3, Float = ( float ) 5.4 }
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( new NumericFilter<float>( ( float ) 2.6, NumericComparisonOperators.GreaterThan ),
                                               new NumericFilter<float>( ( float ) 7.3, NumericComparisonOperators.SmallerThanEqualTo ),
                                               new List<Number>
                                                   {
                                                       new() { Id = 2, Int = 2, Float = ( float ) 7.3 },
                                                       new() { Id = 3, Int = 2, Float = ( float ) 5.4 },
                                                       new() { Id = 4, Int = 3, Float = ( float ) 5.4 }
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( new NumericFilter<float>( ( float ) 2.6, NumericComparisonOperators.GreaterThan ),
                                               new NumericFilter<float>( ( float ) 7.3, NumericComparisonOperators.GreaterThanEqualTo ),
                                               new List<Number>
                                                   {
                                                       new() { Id = 2, Int = 2, Float = ( float ) 7.3 }
                                                   }.AsQueryable( ) );

                // For from operator is smaller than
                yield return new TestCaseData( new NumericFilter<float>( ( float ) 2.6, NumericComparisonOperators.SmallerThan ),
                                               new NumericFilter<float>( ( float ) 7.3, NumericComparisonOperators.Equal ),
                                               new List<Number>( ).AsQueryable( ) );

                yield return new TestCaseData( new NumericFilter<float>( ( float ) 2.6, NumericComparisonOperators.SmallerThan ),
                                               new NumericFilter<float>( ( float ) 7.3, NumericComparisonOperators.GreaterThan ),
                                               new List<Number>( ).AsQueryable( ) );

                yield return new TestCaseData( new NumericFilter<float>( ( float ) 2.6, NumericComparisonOperators.SmallerThan ),
                                               new NumericFilter<float>( ( float ) 7.3, NumericComparisonOperators.SmallerThan ),
                                               new List<Number>( ).AsQueryable( ) );

                yield return new TestCaseData( new NumericFilter<float>( ( float ) 2.6, NumericComparisonOperators.SmallerThan ),
                                               new NumericFilter<float>( ( float ) 7.3, NumericComparisonOperators.SmallerThanEqualTo ),
                                               new List<Number>( ).AsQueryable( ) );

                yield return new TestCaseData( new NumericFilter<float>( ( float ) 2.6, NumericComparisonOperators.SmallerThan ),
                                               new NumericFilter<float>( ( float ) 7.3, NumericComparisonOperators.GreaterThanEqualTo ),
                                               new List<Number>( ).AsQueryable( ) );

                // For from operator is greater than equal
                yield return new TestCaseData( new NumericFilter<float>( ( float ) 2.6, NumericComparisonOperators.GreaterThanEqualTo ),
                                               new NumericFilter<float>( ( float ) 7.3, NumericComparisonOperators.Equal ),
                                               new List<Number>
                                                   {
                                                       new() { Id = 2, Int = 2, Float = ( float ) 7.3 }
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( new NumericFilter<float>( ( float ) 2.6, NumericComparisonOperators.GreaterThanEqualTo ),
                                               new NumericFilter<float>( ( float ) 7.3, NumericComparisonOperators.GreaterThan ),
                                               new List<Number>( ).AsQueryable( ) );

                yield return new TestCaseData( new NumericFilter<float>( ( float ) 2.6, NumericComparisonOperators.GreaterThanEqualTo ),
                                               new NumericFilter<float>( ( float ) 7.3, NumericComparisonOperators.SmallerThan ),
                                               new List<Number>
                                                   {
                                                       new() { Id = 3, Int = 2, Float = ( float ) 5.4 },
                                                       new() { Id = 4, Int = 3, Float = ( float ) 5.4 },
                                                       new() { Id = 1, Int = 1, Float = ( float ) 2.6 }
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( new NumericFilter<float>( ( float ) 2.6, NumericComparisonOperators.GreaterThanEqualTo ),
                                               new NumericFilter<float>( ( float ) 7.3, NumericComparisonOperators.SmallerThanEqualTo ),
                                               new List<Number>
                                                   {
                                                       new() { Id = 2, Int = 2, Float = ( float ) 7.3 },
                                                       new() { Id = 1, Int = 1, Float = ( float ) 2.6 },
                                                       new() { Id = 3, Int = 2, Float = ( float ) 5.4 },
                                                       new() { Id = 4, Int = 3, Float = ( float ) 5.4 }
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( new NumericFilter<float>( ( float ) 2.6, NumericComparisonOperators.GreaterThanEqualTo ),
                                               new NumericFilter<float>( ( float ) 7.3, NumericComparisonOperators.GreaterThanEqualTo ),
                                               new List<Number>
                                                   {
                                                       new() { Id = 2, Int = 2, Float = ( float ) 7.3 }
                                                   }.AsQueryable( ) );

                // For from operator is smaller than equal 
                yield return new TestCaseData( new NumericFilter<float>( ( float ) 2.6, NumericComparisonOperators.SmallerThanEqualTo ),
                                               new NumericFilter<float>( ( float ) 7.3, NumericComparisonOperators.Equal ),
                                               new List<Number>( ).AsQueryable( ) );

                yield return new TestCaseData( new NumericFilter<float>( ( float ) 2.6, NumericComparisonOperators.SmallerThanEqualTo ),
                                               new NumericFilter<float>( ( float ) 7.3, NumericComparisonOperators.GreaterThan ),
                                               new List<Number>( ).AsQueryable( ) );

                yield return new TestCaseData( new NumericFilter<float>( ( float ) 2.6, NumericComparisonOperators.SmallerThanEqualTo ),
                                               new NumericFilter<float>( ( float ) 7.3, NumericComparisonOperators.SmallerThan ),
                                               new List<Number>
                                                   {
                                                       new() { Id = 1, Int = 1, Float = ( float ) 2.6 }
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( new NumericFilter<float>( ( float ) 2.6, NumericComparisonOperators.SmallerThanEqualTo ),
                                               new NumericFilter<float>( ( float ) 7.3, NumericComparisonOperators.SmallerThanEqualTo ),
                                               new List<Number>
                                                   {
                                                       new() { Id = 1, Int = 1, Float = ( float ) 2.6 }
                                                   }.AsQueryable( ) );

                yield return new TestCaseData( new NumericFilter<float>( ( float ) 2.6, NumericComparisonOperators.SmallerThanEqualTo ),
                                               new NumericFilter<float>( ( float ) 7.3, NumericComparisonOperators.GreaterThanEqualTo ),
                                               new List<Number>( ).AsQueryable( ) );
            }
        }

        #endregion

        #endregion

        #region Private

        private IQueryable<Number> _numbers;

        #endregion

        #region Public

        [SetUp]
        public void Setup( )
        {
            var factory = new ConnectionFactory( );
            var context = factory.CreateContextForSQLite( );

            this._numbers = context.Numbers.AsQueryable( );
        }

        [Test]
        public void WithNumericRangeSearch_PassNullProperty_ShouldReturnAllItems( )
        {
            // Arrange
            var range = new NumericRangeFilter<int, int>( new NumericFilter<int>( 2, NumericComparisonOperators.Equal ),
                                                          new NumericFilter<int>( 2, NumericComparisonOperators.Equal ) );

            // Act
            var actualNumbers = this._numbers.WithNumericRangeSearch( null, range );

            // Assert
            actualNumbers.Should( ).BeEquivalentTo( this._numbers );
        }

        [Test]
        public void WithNumericRangeSearch_PassNullRange_ShouldReturnAllItems( )
        {
            // Arrange
            Expression<Func<Number, int>> intField = number => number.Int;

            // Act
            var actualNumbers = this._numbers.WithNumericRangeSearch( intField, null );

            // Assert
            actualNumbers.Should( ).BeEquivalentTo( this._numbers );
        }

        [TestCaseSource( nameof( ValidParamsTestCaseData ) )]
        public void WithNumericRangeSearch_PassValidParams_ShouldReturnMatchedItems( NumericFilter<float> fromNumberFilter, NumericFilter<float> toNumericFilter, IQueryable<Number> expectedNumbers )
        {
            // Arrange
            Expression<Func<Number, float>> property = number => number.Float;
            var range = new NumericRangeFilter<float, float>( fromNumberFilter, toNumericFilter );

            // Act
            var actualNumbers = this._numbers.WithNumericRangeSearch( property, range );

            // Assert
            actualNumbers.Should( ).BeEquivalentTo( expectedNumbers );
        }

        #endregion
    }
}