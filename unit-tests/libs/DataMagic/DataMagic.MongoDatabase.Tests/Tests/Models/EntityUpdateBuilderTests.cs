using System;
using System.Collections.Generic;
using CmsModules.TestDependencies.Providers.Implementations;
using CmsModules.TestDependencies.Providers.Interfaces;
using DataMagic.MongoDatabase.Tests.Models;
using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using MongoDB.Driver;
using NUnit.Framework;

namespace DataMagic.MongoDatabase.Tests.Tests.Models
{
    [TestFixture]
    public partial class EntityUpdateBuilderTests
    {
        #region Properties

        private readonly LinkedList<IDisposable> _disposables;

        private readonly IServiceCollection _services;

        private readonly IServiceCollection _tools;

        #endregion

        #region Constructor

        public EntityUpdateBuilderTests()
        {
            _disposables = new LinkedList<IDisposable>();
            _services = new ServiceCollection();
            _tools = new ServiceCollection();
        }

        #endregion

        #region Methods

        [SetUp]
        public virtual void SetUp()
        {
            _services.Clear();
            _tools.Clear();

            var mongoDbRunner = MongoDbRunner.Start();
            _disposables.AddLast(mongoDbRunner);

            _services.AddScoped<IMongoClient>(_ => new MongoClient(mongoDbRunner.ConnectionString));
            _services.AddScoped<IMongoDatabase>(provider =>
            {
                var mongoClient = provider.GetService<IMongoClient>();
                return mongoClient.GetDatabase(nameof(EntityUpdateBuilderTests));
            });
            _services.AddScoped<IMongoCollection<User>>(provider =>
            {
                var database = provider.GetService<IMongoDatabase>();
                return database.GetCollection<User>("Users");
            });

            _tools.AddScoped<IFileProvider, FileProvider>();
        }

        #endregion
    }
}