using CorsModule.Models.Interfaces;

namespace CorsModule.Models.Implementations
{
    public class CorsPolicy : ICorsPolicy
    {
        #region Properties

        public string UniqueName { get; }

        public string DisplayName { get; set; }

        public string[] AllowedHeaders { get; set; }

        public string[] AllowedMethods { get; set; }

        public string[] AllowedOrigins { get; set; }

        public string[] AllowedExposedHeaders { get; set; }

        public bool AllowCredentials { get; set; }

        #endregion

        #region Constructor

        public CorsPolicy(string uniqueName)
        {
            UniqueName = uniqueName;
        }

        #endregion
    }
}