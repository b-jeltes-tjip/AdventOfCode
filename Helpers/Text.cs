using System.Reflection;

namespace AdventOfCode.Helpers
{
    internal static class Text
    {
        /// <summary>
        /// Retrieves all text from a file within the assembly.
        /// </summary>
        /// <param name="fileName">File name including the extension (example: file.txt).</param>
        /// <returns>All text from a file within the executing assembly.</returns>
        public static string FromFile(string folder, string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"AdventOfCode.{folder}.{fileName}";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            using StreamReader reader = new(stream);
            return reader.ReadToEnd();
        }
    }
}
