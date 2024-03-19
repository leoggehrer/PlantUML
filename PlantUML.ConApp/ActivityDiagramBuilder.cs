namespace PlantUML.ConApp
{
    using System.IO;
    /// <summary>
    /// Initializes a new instance of the <see cref="ActivityDiagramBuilder"/> class.
    /// </summary>
    /// <param name="filePath">The path of the file.</param>
    /// <param name="diagramFolder">The folder where the diagram will be created.</param>
    /// <param name="force">A flag indicating whether to force the creation of the diagram.</param>
    internal partial class ActivityDiagramBuilder(string filePath, string diagramFolder, bool force) : UMLDiagramBuilder(filePath, diagramFolder, force)
    {
        public override void CreateFromFile()
        {
            var fileDirectory = Path.GetDirectoryName(FilePath!);
            var diagramsDirectory = Path.Combine(fileDirectory!, DiagramFolder!);
            var source = File.ReadAllText(FilePath!);

            Logic.DiagramCreator.CreateActivityDiagram(diagramsDirectory, source, Force);
        }
    }
}

