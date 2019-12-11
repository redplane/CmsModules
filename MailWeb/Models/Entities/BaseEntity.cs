using System;
using MailWeb.Enums;

namespace MailWeb.Models.Entities
{
    public class BaseEntity
    {
        #region Constructor

        public BaseEntity(Guid id)
        {
            Id = id;
            Availability = MasterItemAvailabilities.Available;
        }

        #endregion

        #region Properties

        public Guid Id { get; private set; }

        public MasterItemAvailabilities Availability { get; set; }

        public double CreatedTime { get; set; }

        public double? LastModifiedTime { get; set; }

        #endregion
    }
}