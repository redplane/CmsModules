using CorsModule.Models.Interfaces;

namespace CorsModule.Models.Implementations
{
    public class CorsPolicy : ICorsPolicy
    {
        public string Name { get; set; }

        public string[] AllowedHeaders { get; set; }

        public string[] AllowedOrigins { get; set; }

        public string[] AllowedMethods { get; set; }

        public string[] AllowedExposedHeaders { get; set; }

        public bool AllowCredential { get; set; }
    }
}