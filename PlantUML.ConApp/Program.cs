﻿//@BaseCode
//MdStart
namespace PlantUML.ConApp
{
    internal partial class Program
    {
        #region Class-Constructors
        /// <summary>
        /// Initializes the <see cref="Program"/> class.
        /// </summary>
        /// <remarks>
        /// This static constructor sets up the necessary properties for the program.
        /// </remarks>
        static Program()
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

        #region properties
        /// <summary>
        /// Gets or sets the home path.
        /// </summary>
        /// <value>The home path.</value>
        internal static string? HomePath { get; set; }
        /// <summary>
        /// Gets or sets the user path.
        /// </summary>
        /// <value>
        /// The user path.
        /// </value>
        internal static string UserPath { get; set; }
        /// <summary>
        /// Gets or sets the source path.
        /// </summary>
        /// <value>
        /// The user path.
        /// </value>
        internal static string SourcePath { get; set; }
        /// <summary>
        /// Gets or sets the solution path.
        /// </summary>
        /// <value>
        /// The source path.
        /// </value>
        internal static string SolutionPath { get; set; }
        #endregion properties

        #region file and path methods
        /// <summary>
        /// Retrieves the current solution path.
        /// </summary>
        /// <returns>
        /// The current solution path as a string.
        /// </returns>
        internal static string GetCurrentSolutionPath()
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
        #endregion file and path methods

        #region print methods
        /// <summary>
        /// Prints the header for the Template Tools.
        /// </summary>
        private static void PrintHeader(string sourcePath)
        {
            var count = 0;
            var saveForeColor = Display.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Green;

            count = Display.PrintLine(nameof(PlantUML));
            Display.PrintLine('=', count);
            Display.PrintLine();
            Display.ForegroundColor = saveForeColor;
            Display.PrintLine($"Solution path: {sourcePath}");
            Display.PrintLine();
        }
        /// <summary>
        /// Prints a footer in the console.
        /// </summary>
        private static void PrintFooter()
        {
            Display.PrintLine();
            Display.Print("Choose: ");
        }
        #endregion print methods

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }
}
//MdEnd