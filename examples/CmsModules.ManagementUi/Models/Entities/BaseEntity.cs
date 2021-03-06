﻿using System;
using CmsModules.ManagementUi.Enums;
using CmsModules.ManagementUi.Extensions;

namespace CmsModules.ManagementUi.Models.Entities
{
    public class BaseEntity
    {
        #region Constructor

        public BaseEntity(Guid id)
        {
            Id = id;
            Availability = MasterItemAvailabilities.Available;
            CreatedTime = DateTime.UtcNow.ToUnixTime();
        }

        #endregion

        #region Properties

        public Guid Id { get; }

        public MasterItemAvailabilities Availability { get; set; }

        public double CreatedTime { get; set; }

        public double? LastModifiedTime { get; set; }

        #endregion
    }
}