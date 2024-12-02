using System.Reflection;

namespace AdventOfCodeTest.Helpers
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
            var resourceName = $"AdventOfCodeTest.{folder}.{fileName}";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            using StreamReader reader = new(stream);
            return reader.ReadToEnd();
        }
    }
}
