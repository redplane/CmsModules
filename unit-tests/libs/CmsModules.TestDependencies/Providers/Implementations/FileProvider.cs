using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CmsModules.TestDependencies.Providers.Interfaces;
using Newtonsoft.Json;

namespace CmsModules.TestDependencies.Providers.Implementations
{
    public class FileProvider : IFileProvider
    {
        #region Methods

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paths"></param>
        /// <returns></returns>
        public virtual async Task<T> ReadJsonFromFileAsync<T>(string[] paths)
        {
            var executingDirectory = Directory.GetCurrentDirectory();

            var allPaths = new LinkedList<string>();
            allPaths.AddLast(executingDirectory);

            foreach (var path in paths)
                allPaths.AddLast(path);

            var filePath = Path.Combine(allPaths.ToArray());
            var szContent = await File.ReadAllTextAsync(filePath);
            return JsonConvert.DeserializeObject<T>(szContent);
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public virtual async Task<string> ReadTextFromFileAsync(string[] paths)
        {
            var executingDirectory = Directory.GetCurrentDirectory();

            var allPaths = new LinkedList<string>();
            allPaths.AddLast(executingDirectory);

            foreach (var path in paths)
                allPaths.AddLast(path);

            var finalPath = Path.Combine(allPaths.ToArray());
            var szContent = await File.ReadAllTextAsync(finalPath);
            return szContent;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public virtual async Task<byte[]> ReadBytesAsync(string[] paths)
        {
            var executingDirectory = Directory.GetCurrentDirectory();

            var allPaths = new LinkedList<string>();
            allPaths.AddLast(executingDirectory);

            foreach (var path in paths)
                allPaths.AddLast(path);

            var finalPath = Path.Combine(allPaths.ToArray());
            var szContent = await File.ReadAllBytesAsync(finalPath);
            return szContent;
        }

        #endregion
    }
}