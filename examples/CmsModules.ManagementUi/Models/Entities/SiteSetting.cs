﻿using System;

namespace CmsModules.ManagementUi.Models.Entities
{
    public class SiteSetting
    {
        #region Constructor

        public SiteSetting(Guid id)
        {
            Id = id;
        }

        #endregion

        #region Properties

        public Guid Id { get; }

        public string Name { get; set; }

        #endregion
    }
}