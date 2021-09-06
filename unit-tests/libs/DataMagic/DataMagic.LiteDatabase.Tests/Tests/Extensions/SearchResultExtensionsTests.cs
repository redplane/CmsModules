using System.IO;
using CmsModules.TestDependencies.Interfaces;
using CmsModules.TestDependencies.Providers.Implementations;
using CmsModules.TestDependencies.Providers.Interfaces;
using DataMagic.LiteDatabase.Tests.Models;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DataMagic.LiteDatabase.Tests.Tests.Extensions
{
    [TestFixture]
    public partial class SearchResultExtensionsTests : IUnitTest
    {
        [SetUp]
        public void SetUp()
        {
            _services.Clear();
            _tools.Clear();

            // Add tools list.
            _tools.AddScoped<IFileProvider, FileProvider>();

            // Service registration.
            _services.AddScoped<ILiteDatabase>(provider => new LiteDB.LiteDatabase(new MemoryStream()));
            _services.AddScoped(provider =>
            {
                var liteDatabase = provider.GetService<ILiteDatabase>();
                return liteDatabase.GetCollection<User>(nameof(User));
            });
        }

        [TearDown]
        public void TearDown()
        {
            // Do nothing/
        }

        private readonly IServiceCollection _services;

        private readonly IServiceCollection _tools;

        public SearchResultExtensionsTests()
        {
            _services = new ServiceCollection();
            _tools = new ServiceCollection();
        }
    }
}