//@BaseCode
//MdStart

namespace PlantUML.ConApp
{
    /// <summary>
    /// Represents the base application class.
    /// </summary>
    /// <remarks>
    /// This class provides common properties and methods for the application.
    /// </remarks>
    public abstract partial class Application
    {
        #region Class-Constructors
        /// <summary>
        /// Initializes the <see cref="Application"/> class.
        /// </summary>
        /// <remarks>
        /// This static constructor sets up the necessary properties for the program.
        /// </remarks>
        static Application()
        {
            ClassConstructing();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            HomePath = (Environment.OSVersion.Platform == PlatformID.Unix ||
            Environment.OSVersion.Platform == PlatformID.MacOSX)
            ? Environment.GetEnvironmentVariable("HOME")
            : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");

            UserPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            SourcePath = Path.Combine(UserPath, "source");
            if (Directory.Exists(SourcePath) == false)
            {
                SourcePath = UserPath;
            }
            SolutionPath = GetCurrentSolutionPath();
            ClassConstructed();
        }
        /// <summary>
        /// This method is called during the construction of the class.
        /// </summary>
        static partial void ClassConstructing();
        /// <summary>
        /// Represents a method that is called when a class is constructed.
        /// </summary>
        static partial void ClassConstructed();
        #endregion Class-Constructors

        #region Instance-Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        public Application()
        {
            Constructing();
            Constructed();
        }
        /// <summary>
        /// This method is called during the construction of the object.
        /// </summary>
        partial void Constructing();
        /// <summary>
        /// This method is called when the object is constructed.
        /// </summary>
        partial void Constructed();
        #endregion Instance-Constructors

        #region properties
        /// <summary>
        /// Gets or sets a value indicating whether the operation should be forced.
        /// </summary>
        public static bool Force { get; set; } = false;
        /// <summary>
        /// Gets or sets the home path.
        /// </summary>
        public static string? HomePath { get; set; }
        /// <summary>
        /// Gets or sets the user path.
        /// </summary>
        public static string UserPath { get; set; }
        /// <summary>
        /// Gets or sets the source path.
        /// </summary>
        public static string SourcePath { get; set; }
        /// <summary>
        /// Gets or sets the solution path.
        /// </summary>
        public static string SolutionPath { get; set; }
        #endregion properties

        #region methods for changing the properties
        /// <summary>
        /// Changes the value of the Force property.
        /// </summary>
        public static void ChangeForce() => Force = !Force;
        /// <summary>
        /// Gets the current solution path.
        /// </summary>
        /// <returns>The current solution path.</returns>
        public static string GetCurrentSolutionPath()
        {
            int endPos = AppContext.BaseDirectory
                                   .IndexOf($"{nameof(PlantUML)}", StringComparison.CurrentCultureIgnoreCase);
            var result = AppContext.BaseDirectory[..endPos];

            while (result.EndsWith(Path.DirectorySeparatorChar))
            {
                result = result[0..^1];
            }
            return result;
        }
        /// <summary>
        /// Gets the parent directory of the specified path.
        /// </summary>
        /// <param name="path">The path to get the parent directory of.</param>
        /// <returns>The parent directory of the specified path.</returns>
        public static string GetParentDirectory(string path)
        {
            var result = Directory.GetParent(path);

            return result != null ? result.FullName : path;
        }
        /// <summary>
        /// Retrieves a collection of source code files in the specified directory and its subdirectories based on the given search patterns.
        /// </summary>
        /// <param name="path">The directory path to search for source code files.</param>
        /// <param name="searchPatterns">An array of search patterns to match against the names of files in the specified directory and its subdirectories.</param>
        /// <returns>A collection of source code file paths.</returns>
        public static List<string> GetSourceCodeFiles(string path, string[] searchPatterns)
        {
            var result = new List<string>();
            var ignoreFolders = new string[] { $"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", $"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}" };

            foreach (var searchPattern in searchPatterns)
            {
                result.AddRange(Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories)
                      .Where(f => ignoreFolders.Any(e => f.Contains(e)) == false)
                      .OrderBy(i => i));
            }
            return result;
        }
        #endregion methods for changing the properties
    }
}
//MdEnd