using System;
using System.Collections.Generic;
using System.Linq;
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
    public class SearchResultExtensions_ToSearchResultTests
    {
        #region Private

        private ConnectionFactory _connectionFactory;

        private IEnumerable<User> _users;

        #endregion

        #region Public

        [SetUp]
        public void Setup( )
        {
            this._connectionFactory = new ConnectionFactory( );
            var dbContext = this._connectionFactory.CreateContextForSQLite( );
            var users = new List<User>
                {
                    new() { Id = 1, Name = "Name1", Birthday = Convert.ToDateTime( "1-1-2015" ), DeathTime = null },
                    new() { Id = 2, Name = "Name2", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2076" ) },
                    new() { Id = 3, Name = "Name3", Birthday = Convert.ToDateTime( "1-1-2016" ), DeathTime = Convert.ToDateTime( "1-1-2096" ) },
                    new() { Id = 4, Name = "Name4", Birthday = Convert.ToDateTime( "1-1-2017" ), DeathTime = Convert.ToDateTime( "1-1-2086" ) },
                    new() { Id = 5, Name = "Name5", Birthday = Convert.ToDateTime( "1-1-2018" ), DeathTime = Convert.ToDateTime( "1-1-2086" ) }
                };
            dbContext.Users.AddRange( users );
            dbContext.SaveChanges( );

            this._users = dbContext.Users.AsQueryable( );
        }

        [TearDown]
        public void TearDown( )
        {
            this._connectionFactory.Dispose( );
        }

        [Test]
        public void ToSearchResult_PassFalseOfShouldItemsCountedAndQueried_ShouldReturnEmpty( )
        {
            // Arrange
            var pagerMock = new Mock<IPager>( );
            pagerMock.Setup( c => c.ShouldItemsCounted( ) ).Returns( false );
            pagerMock.Setup( c => c.ShouldItemsQueried( ) ).Returns( false );
            pagerMock.Setup( c => c.GetTotalRecords( ) ).Returns( 2 );
            pagerMock.Setup( c => c.GetSkippedRecords( ) ).Returns( 2 );

            // Act
            var actualUsers = ( SearchResult<User> ) this._users.ToSearchResult( pagerMock.Object );

            // Assert
            actualUsers.Items.Length.Should( ).Be( 0 );
            actualUsers.TotalRecords.Should( ).Be( 0 );
        }

        [Test]
        public void ToSearchResult_PassNullPager_ShouldReturnAllItems( )
        {
            // Act
            var actualUsers = ( SearchResult<User> ) this._users.ToSearchResult( null );

            // Assert
            actualUsers.Items.Should( ).BeEquivalentTo( this._users );
            actualUsers.TotalRecords.Should( ).Be( this._users.Count( ) );
        }

        [Test]
        public void ToSearchResult_PassNullSource_ShouldReturnNull( )
        {
            // Act
            var actualUsers = ( ( IEnumerable<User> ) null ).ToSearchResult( It.IsAny<IPager>( ) );

            // Assert
            actualUsers.Should( ).BeNull( );
        }

        [Test]
        public void ToSearchResult_PassTrueOfShouldItemsCountedAndQueried_ShouldReturnItemsMatch( )
        {
            // Arrange
            var pagerMock = new Mock<IPager>( );
            pagerMock.Setup( c => c.ShouldItemsCounted( ) ).Returns( true );
            pagerMock.Setup( c => c.ShouldItemsQueried( ) ).Returns( true );
            pagerMock.Setup( c => c.GetTotalRecords( ) ).Returns( 2 );
            pagerMock.Setup( c => c.GetSkippedRecords( ) ).Returns( 2 );

            // Act
            var actualUsers = ( SearchResult<User> ) this._users.ToSearchResult( pagerMock.Object );

            // Assert
            actualUsers.Items.Length.Should( ).Be( 2 );
            actualUsers.TotalRecords.Should( ).Be( 5 );
        }

        #endregion
    }
}