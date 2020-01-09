namespace CorsModule.Models.Interfaces
{
    public interface ICorsPolicy
    {
        #region Properties

        string UniqueName { get; }

        string DisplayName { get; set; }

        string[] AllowedHeaders { get; set; }

        string[] AllowedMethods { get; set; }

        string[] AllowedOrigins { get; set; }

        string[] AllowedExposedHeaders { get; set; }

        bool AllowCredentials { get; set; }

        #endregion
    }
}