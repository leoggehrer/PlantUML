namespace PlantUML.ConApp
{
    using System.IO;
    using System.Text;
    using CommonTool;

    /// <summary>
    /// Represents a base class for building UML diagrams.
    /// </summary>
    /// <param name="pathOrFilePath">The path or file path of the input file.</param>
    /// <param name="diagramFolder">The folder where the generated diagrams will be saved.</param>
    /// <param name="createCompleteDiagram">A flag indicating whether to create complete diagrams.</param>
    /// <param name="force">A flag indicating whether to overwrite existing diagrams.</param>
    public partial class ActivityDiagramBuilder(string pathOrFilePath, string diagramFolder, bool createCompleteDiagram, bool force) : UMLDiagramBuilder(pathOrFilePath, diagramFolder, createCompleteDiagram, force)
    {
        public override void CreateFromFile()
        {
            var fileDirectory = Path.GetDirectoryName(PathOrFilePath!);
            var diagramsDirectory = Path.Combine(fileDirectory!, DiagramFolder!);
            var source = File.ReadAllText(PathOrFilePath!);
            var defines = ReadDefinesFromProjectFiles(fileDirectory!);

            Logic.DiagramCreator.CreateActivityDiagram(diagramsDirectory, source, defines, Force);
            if (CreateCompleteDiagram)
            {
                Logic.DiagramCreator.CreateCompleteActivityDiagram(diagramsDirectory, Force);
            }
        }
        public override void CreateFromPath()
        {
            StringBuilder builder = new();

            if (Directory.Exists(PathOrFilePath))
            {
                var soureFiles = Application.GetSourceCodeFiles(PathOrFilePath, ["*.cs"]);

                foreach (var file in soureFiles)
                {
                    var source = File.ReadAllText(file);

                    builder.AppendLine(source);
                }

                var diagramsDirectory = Path.Combine(PathOrFilePath!, DiagramFolder!);
                var defines = ReadDefinesFromProjectFiles(PathOrFilePath!);

                Logic.DiagramCreator.CreateActivityDiagram(diagramsDirectory, builder.ToString(), defines, Force);
                if (CreateCompleteDiagram)
                {
                    Logic.DiagramCreator.CreateCompleteActivityDiagram(diagramsDirectory, Force);
                }
            }
        }
    }
}

