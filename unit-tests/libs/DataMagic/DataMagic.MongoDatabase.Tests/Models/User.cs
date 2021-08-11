using System;

namespace DataMagic.MongoDatabase.Tests.Models
{
    public class User
    {
        #region Properties

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        public Guid Id { get; private set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public decimal Balance { get; set; }

        #endregion

        #region Constructor

        public User(Guid id)
        {
            Id = id;
        }

        #endregion
    }
}