using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DataMagic.EntityFrameworkCore.Tests.TestingDb
{
    public class ConnectionFactory: IDisposable
    {
        #region Implementation of IDisposable
        private bool _disposedValue = false; // To detect redundant calls 
        public TestDbContext CreateContextForSQLite()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var option = new DbContextOptionsBuilder<TestDbContext>().UseSqlite(connection).Options;

            var context = new TestDbContext(option);

            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return context;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                }

                this._disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}