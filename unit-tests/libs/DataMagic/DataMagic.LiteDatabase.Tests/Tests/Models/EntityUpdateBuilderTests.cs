using System;
using System.Collections.Generic;
using System.IO;
using CmsModules.TestDependencies.Providers.Implementations;
using CmsModules.TestDependencies.Providers.Interfaces;
using DataMagic.LiteDatabase.Tests.Models;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DataMagic.LiteDatabase.Tests.Tests.Models
{
    [TestFixture]
    public partial class FieldUpdateBuilderTests
    {
        [SetUp]
        public void SetUp()
        {
            _services.Clear();
            _tools.Clear();

            // Tool registration.
            _tools.AddSingleton<IFileProvider, FileProvider>();

            var liteDatabase = new LiteDB.LiteDatabase(new MemoryStream());
            _disposables.AddLast(liteDatabase);
            _services.AddSingleton<ILiteDatabase>(_ =>
            {
                var liteDatabase = new LiteDB.LiteDatabase(new MemoryStream());
                _disposables.AddLast(liteDatabase);
                return liteDatabase;
            });

            _services.AddScoped(provider =>
            {
                var liteDatabase = provider.GetService<ILiteDatabase>();
                return liteDatabase.GetCollection<User>();
            });
        }

        [TearDown]
        public void TearDown()
        {
            _services.Clear();
            _tools.Clear();

            foreach (var disposable in _disposables)
                disposable.Dispose();
            _disposables.Clear();
        }

        private readonly IServiceCollection _services;

        private readonly IServiceCollection _tools;

        private readonly LinkedList<IDisposable> _disposables;

        public FieldUpdateBuilderTests()
        {
            _services = new ServiceCollection();
            _tools = new ServiceCollection();
            _disposables = new LinkedList<IDisposable>();
        }
    }
}