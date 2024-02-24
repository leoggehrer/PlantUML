namespace PlantUML.ConApp
{
    internal abstract partial class UMLDiagramBuilder(string filePath, string diagramFolder, bool force)
    {
        #region properties
        public bool Force { get; private set; } = force;
        public string FilePath { get; private set; } = filePath;
        public string DiagramFolder { get; set; } = diagramFolder;
        #endregion properties

        #region methods
        public abstract void Create();
        #endregion methods
    }
}