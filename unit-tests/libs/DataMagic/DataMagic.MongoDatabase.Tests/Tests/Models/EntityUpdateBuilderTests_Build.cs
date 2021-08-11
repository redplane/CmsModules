using System.Threading;
using System.Threading.Tasks;
using CmsModules.TestDependencies.Providers.Interfaces;
using DataMagic.Abstractions.Models;
using DataMagic.MongoDatabase.Models;
using DataMagic.MongoDatabase.Tests.Models;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using NUnit.Framework;

namespace DataMagic.MongoDatabase.Tests.Tests.Models
{
    public partial class EntityUpdateBuilderTests
    {
        #region Methods

        [Test]
        public virtual void Build_NoFieldIsUpdated_Returns_Null()
        {
            var entityUpdateBuilder = new EntityUpdateBuilder<User>()
                .Update(x => x.Age, new EditableField<int>())
                .Update(x => x.Balance, new EditableField<decimal>());

            var updateDefinition = entityUpdateBuilder.Build();
            Assert.IsNull(updateDefinition);
        }

        /// <summary>
        /// Test code: 62bbccd2607f708023a35326355adc39
        /// </summary>
        [Test]
        public virtual async Task Build_FieldsAreUpdated_Expects_EntityWillBeUpdatedSuccessfully()
        {
            var serviceProvider = _services.BuildServiceProvider();
            var users = serviceProvider.GetService<IMongoCollection<User>>();

            var toolProvider = _tools.BuildServiceProvider();
            var fileProvider = toolProvider.GetService<IFileProvider>();
            var user = await fileProvider.ReadJsonFromFileAsync<User>(new[]
                { "Resources", "62bbccd2607f708023a35326355adc39", "User.json" });
            await users.InsertOneAsync(user);

            // Find the added user.
            var addedUser = await users.Find(x => x.Id == user.Id).FirstOrDefaultAsync();

            // Build the entity update.
            var entityUpdateBuilder = new EntityUpdateBuilder<User>()
                .Update(x => x.Age, new EditableField<int>(10))
                .Update(x => x.Balance, new EditableField<decimal>(2500))
                .Update(x => x.Name, new EditableField<string>("Changed name"));

            var updateDefinition = entityUpdateBuilder.Build();
            var editedUser = await users.FindOneAndUpdateAsync<User>(x => x.Id == user.Id, updateDefinition,
                new FindOneAndUpdateOptions<User>()
                {
                    ReturnDocument = ReturnDocument.After
                }, CancellationToken.None);

            Assert.AreEqual(10, editedUser.Age);
            Assert.AreEqual(2500, editedUser.Balance);
            Assert.AreEqual("Changed name", editedUser.Name);
        }

        #endregion
    }
}