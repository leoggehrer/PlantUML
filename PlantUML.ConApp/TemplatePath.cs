using System.Diagnostics;
using System.Reflection;

namespace PlantUML.ConApp
{
    public partial class TemplatePath
    {
        /// <summary>
        /// Retrieves the sub-paths within the specified start path.
        /// </summary>
        /// <param name="startPath">The starting path.</param>
        /// <returns>An array of sub-paths.</returns>
        public static string[] GetSubPaths(string startPath)
        {
            return QueryDirectoryStructure(startPath, n => n.Contains('.') == false, "bin", "obj", "node_modules");
        }
        /// <summary>
        /// Retrieves an array of quick template projects from the specified starting path.
        /// </summary>
        /// <param name="startPath">The starting path to search for quick template projects.</param>
        /// <returns>An array of quick template projects.</returns>
        /// <remarks>
        /// Quick template projects are identified by their name starting with "QT" and will exclude
        /// directories with names "bin", "obj", and "node_modules" from the search results.
        /// </remarks>
        public static string[] GetTemplatePaths(string startPath)
        {
            return QueryDirectoryStructure(startPath, n => n.StartsWith("QT") || n.Equals("QuickTemplate"), "bin", "obj", "node_modules");
        }
        /// <summary>
        /// Retrieves an array of string values representing the paths to QuickTemplate solutions within a specified directory.
        /// </summary>
        /// <param name="startPath">The starting directory path in which to search for QuickTemplate solutions.</param>
        /// <returns>
        /// An array of string value representing the paths to QuickTemplate solutions found in the specified directory.
        /// </returns>
        public static string[] GetTemplateSolutions(string startPath)
        {
            var result = new List<string>();
            var qtPaths = GetTemplatePaths(startPath);

            foreach (var qtPath in qtPaths)
            {
                var di = new DirectoryInfo(qtPath);

                if (di.GetFiles().Any(f => Path.GetExtension(f.Name).Equals(".sln", StringComparison.CurrentCultureIgnoreCase)))
                {
                    result.Add(qtPath);
                }
            }
            return [.. result];
        }
        /// <summary>
        /// Retrieves the directory structure of a specified path.
        /// </summary>
        /// <param name="path">The path to the root directory.</param>
        /// <param name="filter">The optional filter function to determine which directories to include.</param>
        /// <param name="excludeFolders">The optional list of folder names to exclude from the directory structure.</param>
        /// <returns>An array of strings representing the full paths of the directories in the directory structure.</returns>
        public static string[] QueryDirectoryStructure(string path, Func<string, bool>? filter, params string[] excludeFolders)
        {
            return QueryDirectoryStructure(path, filter, 3, excludeFolders);
        }

        public static string[] QueryDirectoryStructure(string path, Func<string, bool>? filter, int nmaxDeep, params string[] excludeFolders)
        {
            static void GetDirectoriesWithoutHidden(Func<string, bool>? filter, DirectoryInfo directoryInfo, List<string> list, int maxDeep, int deep, params string[] excludeFolders)
            {
                try
                {
                    if (directoryInfo.Attributes.HasFlag(FileAttributes.Hidden) == false)
                    {
                        if ((filter == null || filter(directoryInfo.Name)))
                        {
                            list.Add(directoryInfo.FullName);
                        }
                        if (deep < maxDeep)
                        {
                            foreach (var di in directoryInfo.GetDirectories())
                            {
                                if (excludeFolders.Any(e => e.Equals(di.Name, StringComparison.CurrentCultureIgnoreCase)) == false)
                                {
                                    GetDirectoriesWithoutHidden(filter, di, list, maxDeep, deep + 1, excludeFolders);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in {MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
                }
            }
            var result = new List<string>();
            var directoryInfo = new DirectoryInfo(path);

            GetDirectoriesWithoutHidden(filter, directoryInfo, result, Math.Max(nmaxDeep, 0), 0, excludeFolders);
            return [.. result];
        }
    }
}
