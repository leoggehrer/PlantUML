namespace PlantUML.ConApp
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClassDiagramBuilder"/> class.
    /// </summary>
    /// <param name="filePath">The path of the file.</param>
    /// <param name="diagramFolder">The folder where the diagram will be created.</param>
    /// <param name="force">A flag indicating whether to force the creation of the diagram.</param>
    internal partial class ClassDiagramBuilder(string filePath, string diagramFolder, bool force) : UMLDiagramBuilder(filePath, diagramFolder, force)
    {
        public override void Create()
        {
            var fileDirectory = Path.GetDirectoryName(FilePath!);
            var diagramsDirectory = Path.Combine(fileDirectory!, DiagramFolder!);
            var source = File.ReadAllText(FilePath!);

            Logic.DiagramCreator.CreateClassDiagram(diagramsDirectory, source, Force);
        }
    }
}


