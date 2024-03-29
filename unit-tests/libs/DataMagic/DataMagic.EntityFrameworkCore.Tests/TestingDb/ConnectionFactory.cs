﻿using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DataMagic.EntityFrameworkCore.Tests.TestingDb
{
    public class ConnectionFactory : IDisposable
    {
        #region Implementation of IDisposable

        private bool _disposedValue; // To detect redundant calls 
        private DbConnection _connection;

        public TestDbContext CreateContextForSQLite()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var option = new DbContextOptionsBuilder<TestDbContext>().UseSqlite(connection).Options;
            _connection = RelationalOptionsExtension.Extract(option).Connection;

            var context = new TestDbContext(option);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }

        #endregion

        #region Implementation of IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing) _connection.Dispose();

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}