using System;
using CmsModulesManagement.Enums;
using CorsModule.Models.Interfaces;

namespace CmsModulesManagement.Models.Entities
{
    public class SiteCorsPolicy : BaseEntity, ICorsPolicy
    {
        #region Constructor

        public SiteCorsPolicy(Guid id) : base(id)
        {
        }

        public SiteCorsPolicy(Guid id, ICorsPolicy initialPolicy) : base(id)
        {
            Name = initialPolicy.Name;
            AllowedHeaders = initialPolicy.AllowedHeaders;
            AllowedOrigins = initialPolicy.AllowedOrigins;
            AllowedMethods = initialPolicy.AllowedMethods;
            AllowedExposedHeaders = initialPolicy.AllowedExposedHeaders;
            AllowCredential = initialPolicy.AllowCredential;
            Availability = MasterItemAvailabilities.Available;
        }

        #endregion

        #region Properties

        public string Name { get; set; }

        public string[] AllowedHeaders { get; set; }

        public string[] AllowedOrigins { get; set; }

        public string[] AllowedMethods { get; set; }

        public string[] AllowedExposedHeaders { get; set; }

        public bool AllowCredential { get; set; }

        #endregion
    }
}