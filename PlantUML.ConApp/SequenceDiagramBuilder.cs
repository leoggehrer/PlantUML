namespace PlantUML.ConApp
{
    using System.IO;
    /// <summary>
    /// Initializes a new instance of the <see cref="SequenceDiagramBuilder"/> class.
    /// </summary>
    /// <param name="filePath">The path to the file containing the sequence diagram.</param>
    /// <param name="diagramFolder">The folder where the generated diagram will be saved.</param>
    /// <param name="force">A flag indicating whether to overwrite existing diagrams.</param>
    internal partial class SequenceDiagramBuilder(string filePath, string diagramFolder, bool force) : UMLDiagramBuilder(filePath, diagramFolder, force)
    {
        public override void Create()
        {
            var fileDirectory = Path.GetDirectoryName(FilePath!);
            var diagramsDirectory = Path.Combine(fileDirectory!, DiagramFolder!);
            var source = File.ReadAllText(FilePath!);

            Logic.DiagramCreator.CreateSequenceDiagram(diagramsDirectory, source, Force);
        }
    }
}

