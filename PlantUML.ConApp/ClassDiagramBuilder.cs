﻿using System.Text;
using CommonTool;

namespace PlantUML.ConApp
{
     /// <summary>
    /// Represents a base class for building UML diagrams.
    /// </summary>
    /// <param name="pathOrFilePath">The path or file path of the input file.</param>
    /// <param name="diagramFolder">The folder where the generated diagrams will be saved.</param>
    /// <param name="force">A flag indicating whether to overwrite existing diagrams.</param>
   internal partial class ClassDiagramBuilder(string pathOrFilePath, string diagramFolder, bool force) : UMLDiagramBuilder(pathOrFilePath, diagramFolder, force)
    {
        public override void CreateFromFile()
        {
            var fileDirectory = Path.GetDirectoryName(PathOrFilePath!);
            var diagramsDirectory = Path.Combine(fileDirectory!, DiagramFolder!);
            var source = File.ReadAllText(PathOrFilePath!);

            Logic.DiagramCreator.CreateClassDiagram(diagramsDirectory, source, Force);
        }
        public override void CreateFromPath()
        {
            StringBuilder builder = new StringBuilder();
            
            if (Directory.Exists(PathOrFilePath))
            {
                var files = Application.GetSourceCodeFiles(PathOrFilePath, [ "*.cs" ]);
                
                foreach (var file in files)
                {
                    var source = File.ReadAllText(file);

                    builder.AppendLine(source);
                }

                var diagramsDirectory = Path.Combine(PathOrFilePath!, DiagramFolder!);

                Logic.DiagramCreator.CreateClassDiagram(diagramsDirectory, builder.ToString(), Force);
            }
        }
    }
}
