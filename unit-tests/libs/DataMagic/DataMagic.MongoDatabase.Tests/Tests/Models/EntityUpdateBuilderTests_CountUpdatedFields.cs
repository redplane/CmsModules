using DataMagic.Abstractions.Models;
using DataMagic.MongoDatabase.Models;
using DataMagic.MongoDatabase.Tests.Models;
using NUnit.Framework;

namespace DataMagic.MongoDatabase.Tests.Tests.Models
{
    public partial class EntityUpdateBuilderTests
    {
        [Test]
        public virtual void CountUpdatedFields_NoFieldIsUpdated_Returns_Zero()
        {
            var entityUpdateBuilder = new EntityUpdateBuilder<User>();
            entityUpdateBuilder.Update(x => x.Age, new EditableField<int>())
                .Update(x => x.Name, new EditableField<string>());

            Assert.AreEqual(0, entityUpdateBuilder.CountUpdatedFields());
        }

        [Test]
        public virtual void CountUpdatedFields_FieldsAreUpdated_Returns_NumberOfUpdatedFields()
        {
            var entityUpdateBuilder = new EntityUpdateBuilder<User>();
            entityUpdateBuilder.Update(x => x.Age, new EditableField<int>(100))
                .Update(x => x.Name, new EditableField<string>("Changed name"));

            Assert.AreEqual(2, entityUpdateBuilder.CountUpdatedFields());
        }
    }
}