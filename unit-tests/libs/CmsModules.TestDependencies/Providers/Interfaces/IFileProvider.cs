using System.Threading.Tasks;

namespace CmsModules.TestDependencies.Providers.Interfaces
{
    public interface IFileProvider
    {
        #region Methods

        /// <summary>
        /// Load data from a text file & serialize it to object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paths"></param>
        /// <returns></returns>
        Task<T> ReadJsonFromFileAsync<T>(string[] paths);

        /// <summary>
        /// Load forgery data from a file.
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        Task<string> ReadTextFromFileAsync(string[] paths);

        /// <summary>
        /// Load binary data from path.
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        Task<byte[]> ReadBytesAsync(string[] paths);

        #endregion
    }
}