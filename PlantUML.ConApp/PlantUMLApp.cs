﻿namespace PlantUML.ConApp
{
    public partial class PlantUMLApp : CommonTool.ConsoleApplication
    {
        #region Class-Constructors
        /// <summary>
        /// Initializes the <see cref="Program"/> class.
        /// This static constructor sets up the necessary properties for the program.
        /// </remarks>
        static PlantUMLApp()
        {
            ClassConstructing();
            DocumentsPath = SourcePath;
            ClassConstructed();
        }
        /// <summary>
        /// This method is called during the construction of the class.
        /// </summary>
        static partial void ClassConstructing();
        /// <summary>
        /// Represents a method that is called when a class is constructed.
        /// </summary>
        static partial void ClassConstructed();
        #endregion Class-Constructors

        #region Instance-Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PlantUMLApp"/> class.
        /// </summary>
        public PlantUMLApp()
        {
            Constructing();
            Constructed();
        }
        /// <summary>
        /// This method is called during the construction of the object.
        /// </summary>
        partial void Constructing();
        /// <summary>
        /// This method is called when the object is constructed.
        /// </summary>
        partial void Constructed();
        #endregion Instance-Constructors

        #region app properties
        /// <summary>
        /// Gets or sets the selected diagram type.
        /// </summary>
        public static DiagramBuilderType DiagramBuilder { get; set; } = DiagramBuilderType.Activity;
        /// <summary>
        /// Gets or sets the folder path for diagrams.
        /// </summary>
        public static string DiagramFolder { get; set; } = "diagrams";
        public static string DocumentsPath { get; set; }
        #endregion app properties

        #region overrides
        /// <summary>
        /// Creates an array of menu items for the application menu.
        /// </summary>
        /// <returns>An array of MenuItem objects representing the menu items.</returns>
        protected override MenuItem[] CreateMenuItems()
        {
            var mnuIdx = 0;
            var menuItems = new List<MenuItem>
            {
                CreateMenuSeparator(),
                new()
                {
                    Key = $"{++mnuIdx}",
                    Text = ToLabelText("Force", "Change force flag"),
                    Action = (self) => ChangeForce(),
                },
                new()
                {
                    Key = $"{++mnuIdx}",
                    Text = ToLabelText("Path", "Change source path"),
                    Action = (self) => 
                    {
                        var savePath = DocumentsPath;
                        
                        DocumentsPath = SelectOrChangeToSubPath(DocumentsPath, [ SourcePath ]);
                        if (savePath != DocumentsPath)
                        {
                            PageIndex = 0;
                        }
                    },
                },
                CreateMenuSeparator(),
                new()
                {
                    Key = $"{++mnuIdx}",
                    Text = ToLabelText("Folder", "Change diagram folder"),
                    Action = (self) => ChangeDiagramFolder(),
                },
                new()
                {
                    Key = $"{++mnuIdx}",
                    Text = ToLabelText("Builder", "Change diagram builder"),
                    Action = (self) => ChangeDiagramBuilder(),
                },
                CreateMenuSeparator(),
            };

            var files = GetSourceCodePaths(DocumentsPath, ["*.cs"]).ToArray();

            menuItems.AddRange(CreatePageMenuItems(ref mnuIdx, files, (item, menuItem) =>
            {
                menuItem.Text = ToLabelText("Path", $"{item.Replace(DocumentsPath, string.Empty)}");
                menuItem.Tag = "path";
                menuItem.Action = (self) => CreateDiagram(self, Force);
                menuItem.Params = new() { { "path", item } };
            }));
            return [.. menuItems.Union(CreateExitMenuItems())];
        }

        /// <summary>
        /// Prints the header for the PlantUML application.
        /// </summary>
        /// <param name="sourcePath">The path of the solution.</param>
        protected override void PrintHeader()
        {
            var count = 0;
            var saveForeColor = ForegroundColor;

            ForegroundColor = ConsoleColor.Green;

            count = PrintLine(nameof(PlantUML));
            PrintLine('=', count);
            PrintLine();
            ForegroundColor = saveForeColor;
            PrintLine($"Force flag:  {Force}");
            PrintLine($"Source path: {SourcePath}");
            PrintLine();
            PrintLine($"Diagram folder:  {DiagramFolder}");
            PrintLine($"Diagram builder: {DiagramBuilder} [{DiagramBuilderType.Activity}|{DiagramBuilderType.Class}|{DiagramBuilderType.Sequence}]");
            PrintLine();
        }
        #endregion overrides

        #region methods for app
        /// <summary>
        /// Changes the diagram folder name.
        /// </summary>
        private static void ChangeDiagramFolder()
        {
            DiagramFolder = ReadLine("Enter the diagram folder name: ").Trim();
        }
        /// <summary>
        /// Changes the diagram builder type based on the current value of DiagramBuilder.
        /// </summary>
        private static void ChangeDiagramBuilder()
        {
            DiagramBuilder = DiagramBuilder.ToString().ToLower() switch
            {
                "activity" => DiagramBuilderType.Class,
                "class" => DiagramBuilderType.Sequence,
                "sequence" => DiagramBuilderType.Activity,
                _ => DiagramBuilder,
            };
        }

        /// <summary>
        /// Creates a UML diagram based on the specified <paramref name="menuItem"/> and <paramref name="force"/> flag.
        /// </summary>
        /// <param name="menuItem">The menu item containing the parameters and tag for the diagram creation.</param>
        /// <param name="force">A flag indicating whether to force the creation of the diagram.</param>
        private static void CreateDiagram(MenuItem menuItem, bool force)
        {
            var pathOrFilePath = menuItem.Params[menuItem.Tag]?.ToString() ?? string.Empty;

            StartProgressBar();
            UMLDiagramBuilder diagram = DiagramBuilder switch
            {
                DiagramBuilderType.Activity => new ActivityDiagramBuilder(pathOrFilePath, DiagramFolder, force),
                DiagramBuilderType.Class => new ClassDiagramBuilder(pathOrFilePath, DiagramFolder, force),
                DiagramBuilderType.Sequence => new SequenceDiagramBuilder(pathOrFilePath, DiagramFolder, force),
                _ => throw new InvalidOperationException("Invalid diagram builder type."),
            };

            if (menuItem.Tag.ToLower() == "path")
            {
                diagram.CreateFromPath();
            }
            else if (menuItem.Tag.ToLower() == "file")
            {
                diagram.CreateFromFile();
            }
        }
        #endregion methods for app
    }
}