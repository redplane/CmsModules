using System.Linq;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using CmsModules.TestDependencies.Providers.Interfaces;
using DataMagic.Abstractions.Models;
using DataMagic.LiteDatabase.Models;
using DataMagic.LiteDatabase.Tests.Models;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DataMagic.LiteDatabase.Tests.Tests.Models
{
    public partial class FieldUpdateBuilderTests
    {
        #region Methods

        /// <summary>
        /// Test code: 574029cf370d7010a3a7a8f4be15ee01
        /// </summary>
        [Test]
        public virtual async Task Update_FieldIsMarkedNotChanged_Expects_NothingIsDone()
        {
            var toolProvider = _tools.BuildServiceProvider();

            var fileProvider = toolProvider.GetService<IFileProvider>();
            var user = await fileProvider.ReadJsonFromFileAsync<User>(new[]
                { "Resources", "MockItems", "574029cf370d7010a3a7a8f4be15ee01", "User.json" });
            var originalUser = await fileProvider.ReadJsonFromFileAsync<User>(new[]
                { "Resources", "MockItems", "574029cf370d7010a3a7a8f4be15ee01", "User.json" });

            var entityUpdateBuilder = new EntityUpdateBuilder<User>()
                .Update(x => x.Name, new EditableField<string>())
                .Update(x => x.Age, new EditableField<int>())
                .Update(x => x.Balance, new EditableField<decimal>());

            entityUpdateBuilder.Build(user);
            Assert.AreEqual(originalUser.Name, user.Name);
            Assert.AreEqual(originalUser.Age, user.Age);
            Assert.AreEqual(originalUser.Balance, user.Balance);
        }

        /// <summary>
        /// Test code: c4d37eb457d22c7645f8c7ed9a78c4e8
        /// </summary>
        [Test]
        public virtual async Task Update_FieldIsMarkedAsChanged_Expects_EntityIsUpdated()
        {
            var serviceProvider = _services.BuildServiceProvider();
            var toolProvider = _tools.BuildServiceProvider();

            var fileProvider = toolProvider.GetService<IFileProvider>();
            var user = await fileProvider.ReadJsonFromFileAsync<User>(new[]
                { "Resources", "MockItems", "c4d37eb457d22c7645f8c7ed9a78c4e8", "User.json" });

            // Add a new user into database.
            var users = serviceProvider.GetService<ILiteCollection<User>>();
            users.Insert(user);

            // Verify the added user.
            var addedUserId = user.Id;
            var addedUser = users.Find(x => x.Id == addedUserId).FirstOrDefault();
            Assert.NotNull(addedUser);

            var entityUpdateBuilder = new EntityUpdateBuilder<User>()
                .Update(x => x.Name, new EditableField<string>("Changed name"))
                .Update(x => x.Balance, new EditableField<decimal>(2500))
                .Update(x => x.Age, new EditableField<int>(200));

            // Do update on entity.
            entityUpdateBuilder.Build(user);
            users.Update(user);

            // Get the edited user.
            var editedUser = users.Find(x => x.Id == addedUserId).FirstOrDefault();
            Assert.NotNull(editedUser);
            Assert.AreEqual("Changed name", editedUser.Name);
            Assert.AreEqual(2500, editedUser.Balance);
            Assert.AreEqual(200, editedUser.Age);
        }

        #endregion
    }
}