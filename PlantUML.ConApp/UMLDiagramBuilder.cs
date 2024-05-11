namespace PlantUML.ConApp
{
    using CommonTool;
    using CommonTool.Extensions;
    using System.Text;

    /// <summary>
    /// Represents an abstract class for building UML diagrams.
    /// </summary>
    public abstract partial class UMLDiagramBuilder(string sourcePath, int maxSubPathDeep, string targetPath, string diagramFolder, bool createCompleteDiagram, bool force)
    {
        #region properties

        /// <summary>
        /// Gets or sets the source path.
        /// </summary>
        public string SourcePath { get; private set; } = sourcePath;
        /// <summary>
        /// Gets the maximum depth for the UML diagram.
        /// </summary>
        public int MaxSubPathDeep { get; private set; } = maxSubPathDeep;
        /// <summary>
        /// Gets or sets the target path for the UML diagram.
        /// </summary>
        public string TargetPath { get; private set; } = targetPath;
        /// <summary>
        /// Gets or sets the folder where the diagrams are stored.
        /// </summary>
        public string DiagramFolder { get; set; } = diagramFolder;
        /// <summary>
        /// Gets or sets a value indicating whether to create a complete diagram.
        /// </summary>
        public bool CreateCompleteDiagram { get; set; } = createCompleteDiagram;
        /// <summary>
        /// Gets or sets a value indicating whether the diagram generation should be forced.
        /// </summary>
        public bool Force { get; private set; } = force;
        #endregion properties

        #region methods
        /// <summary>
        /// Creates a UML diagram from a file.
        /// </summary>
        public abstract void CreateFromFile();
        /// <summary>
        /// Creates a UML diagram from a specified path.
        /// </summary>
        public abstract void CreateFromPath();

        /// <summary>
        /// Reads the define constants from project files located in the specified path.
        /// </summary>
        /// <param name="path">The path to the directory containing the project files.</param>
        /// <returns>An array of define constants extracted from the project files.</returns>
        public static string[] ReadDefinesFromProjectFiles(string path)
        {
            List<string> result = [];

            if (Directory.Exists(path))
            {
                var files = Application.FindFilesFromPathAndParentPath(path, "*.csproj");

                foreach (var file in files)
                {
                    var lines = File.ReadAllLines(file, Encoding.Default);

                    foreach (var line in lines)
                    {
                        var defines = line.ExtractBetween("<DefineConstants>", "</DefineConstants>");

                        if (defines.HasContent())
                        {
                            foreach (var define in defines.Split(';', StringSplitOptions.RemoveEmptyEntries))
                            {
                                result.Add(define);
                            }
                        }
                    }
                }
            }
            return result.Distinct().ToArray();
        }
        #endregion methods
    }
}