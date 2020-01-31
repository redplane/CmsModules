namespace CorsModule.Models.Interfaces
{
    public interface ICorsPolicy
    {
        #region Properties

        string Name { get; set; }

        string[] AllowedHeaders { get; set; }

        string[] AllowedOrigins { get; set; }

        string[] AllowedMethods { get; set; }

        string[] AllowedExposedHeaders { get; set; }

        bool AllowCredential { get; set; }

        #endregion
    }
}
