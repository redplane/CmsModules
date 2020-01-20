using System;
using CorsModule.Models.Interfaces;
using MailWeb.Enums;

namespace MailWeb.Models.Entities
{
    public class CorsPolicy : BaseEntity, ICorsPolicy
    {
        #region Constructor

        public CorsPolicy(Guid id) : base(id)
        {
        }

        public CorsPolicy(Guid id, ICorsPolicy initialPolicy) : base(id)
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