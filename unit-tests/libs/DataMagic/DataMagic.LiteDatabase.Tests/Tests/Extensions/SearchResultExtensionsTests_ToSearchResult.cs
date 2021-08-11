using System.Linq;
using System.Threading.Tasks;
using CmsModules.TestDependencies.Providers.Interfaces;
using DataMagic.Abstractions.Interfaces;
using DataMagic.LiteDatabase.Extensions;
using DataMagic.LiteDatabase.Tests.Models;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace DataMagic.LiteDatabase.Tests.Tests.Extensions
{
    public partial class SearchResultExtensionsTests
    {
        #region Methods

        /// <summary>
        ///     Test code: bb119e69ac0a918112c19712ec0b2dac
        /// Pre-condition:
        /// - Items exist in database
        /// Actions:
        /// - Call ToSearchResult with pager is null
        /// Expects:
        /// - All data will be returned.
        /// </summary>
        [Test]
        public virtual async Task ToSearchResult_PagerIsNull_Expects_AllDataWillBeReturned()
        {
            var toolProvider = _tools.BuildServiceProvider();
            var serviceProvider = _services.BuildServiceProvider();

            try
            {
                var fileProvider = toolProvider.GetService<IFileProvider>()!;
                var users = serviceProvider.GetService<ILiteCollection<User>>();

                // Read mock file.
                var mockUsers = await fileProvider.ReadJsonFromFileAsync<User[]>(new[]
                    { "Resources", "MockItems", "bb119e69ac0a918112c19712ec0b2dac", "Users.json" });
                users.InsertBulk(mockUsers);

                // Find the user.
                var loadUsersResult = await
                    users.Query()
                        .ToSearchResultAsync(null);

                Assert.AreEqual(mockUsers.Length, loadUsersResult.TotalRecords);

                foreach (var mockUser in mockUsers)
                {
                    // Find the loaded user.
                    var loadedUser = loadUsersResult.Items.First(x => x.Id == mockUser.Id);
                    Assert.AreEqual(mockUser.Name, loadedUser.Name);
                    Assert.AreEqual(mockUser.Age, loadedUser.Age);
                    Assert.AreEqual(mockUser.Balance, loadedUser.Balance);
                }
            }
            finally
            {
                toolProvider?.Dispose();
                serviceProvider?.Dispose();
            }
        }

        /// <summary>
        ///     Test code: f2be8e62c29ed26c2bdbb59ca283296e
        /// Pre-condition:
        /// - Items exist in database
        /// Actions
        /// - Call ToSearchResult with pager which contains page & records to be taken.
        /// Expects:
        /// - Specified records will be returned.
        /// </summary>
        [Test]
        public virtual async Task ToSearchResult_PagerIsDefine_Expects_OnlyTakeTheSpecifiedRecords()
        {
            var toolProvider = _tools.BuildServiceProvider();
            var serviceProvider = _services.BuildServiceProvider();

            try
            {
                var fileProvider = toolProvider.GetService<IFileProvider>()!;
                var users = serviceProvider.GetService<ILiteCollection<User>>();

                // Read mock file.
                var mockUsers = await fileProvider.ReadJsonFromFileAsync<User[]>(new[]
                    { "Resources", "MockItems", "f2be8e62c29ed26c2bdbb59ca283296e", "Users.json" });
                users.InsertBulk(mockUsers);

                // Find the user.
                var mockPager = new Mock<IPager>();
                mockPager.Setup(x => x.ShouldItemsQueried()).Returns(true);
                mockPager.Setup(x => x.ShouldItemsCounted()).Returns(true);
                mockPager.Setup(x => x.GetSkippedRecords()).Returns(0);
                mockPager.Setup(x => x.GetTotalRecords()).Returns(2);

                var loadUsersResult = await
                    users.Query()
                        .ToSearchResultAsync(mockPager.Object);

                Assert.AreEqual(mockUsers.Length, loadUsersResult.TotalRecords);

                for (var i = 0; i < loadUsersResult.Items.Length; i++)
                {
                    var mockUser = mockUsers[i];
                    var actualUser = loadUsersResult.Items[i];

                    Assert.AreEqual(mockUser.Name, actualUser.Name);
                    Assert.AreEqual(mockUser.Age, actualUser.Age);
                    Assert.AreEqual(mockUser.Balance, actualUser.Balance);
                }
            }
            finally
            {
                toolProvider?.Dispose();
                serviceProvider?.Dispose();
            }
        }

        /// <summary>
        /// Test code: c043b10c63d180e9c888d2886630758f
        /// Pre-condition:
        /// - Items are available in database
        /// Action:
        /// - Call ToSearchResult with ShouldItemsCounter is false
        /// Expects:
        /// - Total record in SearchResult instance will be 0
        /// </summary>
        [Test]
        public virtual async Task ToSearchResult_ShouldItemsCountedSetToFalse_TotalRecordWillBeZero()
        {
            var toolProvider = _tools.BuildServiceProvider();
            var serviceProvider = _services.BuildServiceProvider();

            try
            {
                var fileProvider = toolProvider.GetService<IFileProvider>()!;
                var users = serviceProvider.GetService<ILiteCollection<User>>();

                // Read mock file.
                var mockUsers = await fileProvider.ReadJsonFromFileAsync<User[]>(new[]
                    { "Resources", "MockItems", "c043b10c63d180e9c888d2886630758f", "Users.json" });
                users.InsertBulk(mockUsers);

                // Find the user.
                var mockPager = new Mock<IPager>();
                mockPager.Setup(x => x.ShouldItemsQueried()).Returns(true);
                mockPager.Setup(x => x.ShouldItemsCounted()).Returns(false);
                mockPager.Setup(x => x.GetSkippedRecords()).Returns(0);

                var loadUsersResult = await
                    users.Query()
                        .ToSearchResultAsync(mockPager.Object);

                Assert.AreEqual(0, loadUsersResult.TotalRecords);

                for (var i = 0; i < loadUsersResult.Items.Length; i++)
                {
                    var mockUser = mockUsers[i];
                    var actualUser = loadUsersResult.Items[i];

                    Assert.AreEqual(mockUser.Name, actualUser.Name);
                    Assert.AreEqual(mockUser.Age, actualUser.Age);
                    Assert.AreEqual(mockUser.Balance, actualUser.Balance);
                }
            }
            finally
            {
                toolProvider?.Dispose();
                serviceProvider?.Dispose();
            }
        }

        /// <summary>
        /// Test code: e27036c3430a0b1a8e87a06e4682df7f
        /// Pre-condition:
        /// - Items are available in database
        /// Action:
        /// - Call ToSearchResult with ShouldItemsQueried is false
        /// Expects:
        /// - Items returned to outer component is empty.
        /// </summary>
        [Test]
        public virtual async Task ToSearchResult_ShouldItemsQueriedSetToFalse_ItemsWillBeEmpty()
        {
            var toolProvider = _tools.BuildServiceProvider();
            var serviceProvider = _services.BuildServiceProvider();

            try
            {
                var fileProvider = toolProvider.GetService<IFileProvider>()!;
                var users = serviceProvider.GetService<ILiteCollection<User>>();

                // Read mock file.
                var mockUsers = await fileProvider.ReadJsonFromFileAsync<User[]>(new[]
                    { "Resources", "MockItems", "e27036c3430a0b1a8e87a06e4682df7f", "Users.json" });
                users.InsertBulk(mockUsers);

                // Find the user.
                var mockPager = new Mock<IPager>();
                mockPager.Setup(x => x.ShouldItemsQueried()).Returns(false);
                mockPager.Setup(x => x.ShouldItemsCounted()).Returns(true);
                mockPager.Setup(x => x.GetSkippedRecords()).Returns(0);

                var loadUsersResult = await
                    users.Query()
                        .ToSearchResultAsync(mockPager.Object);

                Assert.AreEqual(mockUsers.Length, loadUsersResult.TotalRecords);
                Assert.IsEmpty(loadUsersResult.Items);
            }
            finally
            {
                toolProvider?.Dispose();
                serviceProvider?.Dispose();
            }
        }

        #endregion
    }
}