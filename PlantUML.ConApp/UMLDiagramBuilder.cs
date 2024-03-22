namespace PlantUML.ConApp
{
    /// <summary>
    /// Represents an abstract class for building UML diagrams.
    /// </summary>
    public abstract partial class UMLDiagramBuilder(string pathOrFilePath, string diagramFolder, bool createCompleteDiagram, bool force)
    {
        #region properties

        /// <summary>
        /// Gets or sets the path or file path for the UML diagram.
        /// </summary>
        public string PathOrFilePath { get; private set; } = pathOrFilePath;
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
        #endregion methods
    }
}