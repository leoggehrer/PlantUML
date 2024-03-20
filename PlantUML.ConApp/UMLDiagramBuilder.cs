namespace PlantUML.ConApp
{
    public abstract partial class UMLDiagramBuilder(string pathOrFilePath, string diagramFolder, bool force)
    {
        #region properties
        public bool Force { get; private set; } = force;
        public string PathOrFilePath { get; private set; } = pathOrFilePath;
        public string DiagramFolder { get; set; } = diagramFolder;
        #endregion properties

        #region methods
        public abstract void CreateFromFile();
        public abstract void CreateFromPath();
        #endregion methods
    }
}