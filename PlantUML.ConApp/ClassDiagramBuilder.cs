namespace PlantUML.ConApp
{
    internal partial class ClassDiagramBuilder(string filePath, string diagramFolder, bool force) : UMLDiagramBuilder(filePath, diagramFolder, force)
    {
        public override void CreateFromFile()
        {
            var fileDirectory = Path.GetDirectoryName(FilePath!);
            var diagramsDirectory = Path.Combine(fileDirectory!, DiagramFolder!);
            var source = File.ReadAllText(FilePath!);

            Logic.DiagramCreator.CreateClassDiagram(diagramsDirectory, source, Force);
        }
    }
}


