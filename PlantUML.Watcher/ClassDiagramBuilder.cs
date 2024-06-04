﻿//@BaseCode
//MdStart
namespace PlantUML.Watcher
{
    using System.IO;
    using System.Text;
    using CommonTool;
    using CommonTool.Extensions;

    /// <summary>
    /// Represents a base class for building UML diagrams.
    /// </summary>
    /// <param name="sourcePath">The path or file path of the input file.</param>
    /// <param name="maxSubPathDeep">The max source path deep.</param>
    /// <param name="targetPath">The path or for uml-diagrams.</param>
    /// <param name="diagramFolder">The folder where the generated diagrams will be saved.</param>
    /// <param name="createCompleteDiagram">A flag indicating whether to create complete diagrams.</param>
    /// <param name="force">A flag indicating whether to overwrite existing diagrams.</param>
    public partial class ClassDiagramBuilder(string sourcePath, int maxSubPathDeep, string targetPath, string diagramFolder, bool createCompleteDiagram, bool force) : UMLDiagramBuilder(sourcePath, maxSubPathDeep, targetPath, diagramFolder, createCompleteDiagram, force)
    {
        public override void CreateFromFile()
        {
            var fileDirectory = Path.GetDirectoryName(SourcePath!) ?? string.Empty;
            var defines = ReadDefinesFromProjectFiles(fileDirectory!);
            var diagramsDirectory = DiagramFolder.HasContent() ? Path.Combine(fileDirectory, DiagramFolder) : fileDirectory;
            var source = File.ReadAllText(SourcePath!);

            Logic.DiagramCreator.CreateClassDiagram(diagramsDirectory, source, defines, Force);
            if (CreateCompleteDiagram)
            {
                Logic.DiagramCreator.CreateCompleteClassDiagram(diagramsDirectory, Force);
            }
        }
        public override void CreateFromPath()
        {
            StringBuilder builder = new();
            
            if (Directory.Exists(SourcePath))
            {
                var defines = ReadDefinesFromProjectFiles(SourcePath!);
                var sourceFiles = Application.GetSourceCodeFiles(SourcePath, ["*.cs"], MaxSubPathDeep);
                var diagramsDirectory = TargetPath;
                
                foreach (var file in sourceFiles)
                {
                    var source = File.ReadAllText(file);

                    builder.AppendLine(source);
                }

                if (DiagramFolder.HasContent())
                {
                    diagramsDirectory = Path.Combine(TargetPath, DiagramFolder);
                }

                Logic.DiagramCreator.CreateClassDiagram(diagramsDirectory, builder.ToString(), defines, Force);
                if (CreateCompleteDiagram)
                {
                    Logic.DiagramCreator.CreateCompleteClassDiagram(diagramsDirectory, Force);
                }
            }
        }
    }
}
//MdEnd