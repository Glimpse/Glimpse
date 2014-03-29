using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Generates a hash based on a supplied instance
    /// </summary>
    internal static class HashGenerator
    {
        /// <summary>
        /// Generates a hash based on the supplied instance
        /// </summary>
        /// <param name="instance">The instance</param>
        /// <returns>The resulting hash</returns>
        public static string Generate(object instance)
        {
            Guard.ArgumentNotNull("instance", instance);

            var crc32 = new Crc32();
            var hashContent = new StringBuilder();
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, instance);
                memoryStream.Position = 0;

                var computeHash = crc32.ComputeHash(memoryStream);

                foreach (var b in computeHash)
                {
                    hashContent.Append(b.ToString("x2"));
                }
            }

            return hashContent.ToString();
        }
    }
}