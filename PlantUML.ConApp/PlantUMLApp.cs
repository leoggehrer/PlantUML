namespace PlantUML.ConApp
{
    internal partial class PlantUMLApp : ConsoleApplication
    {
        #region Class-Constructors
        /// <summary>
        /// Initializes the <see cref="Program"/> class.
        /// This static constructor sets up the necessary properties for the program.
        /// </remarks>
        static PlantUMLApp()
        {
            ClassConstructing();
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
        /// Initializes a new instance of the <see cref="Application"/> class.
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

        /// <summary>
        /// Represents the type of diagram.
        /// </summary>
        public enum DiagramBuilderType
        {
            Activity,
            Class,
        }

        #region app properties
        /// <summary>
        /// Gets or sets the selected diagram type.
        /// </summary>
        public static DiagramBuilderType DiagramBuilder { get; set; } = DiagramBuilderType.Activity;
        /// <summary>
        /// Gets or sets the folder path for diagrams.
        /// </summary>
        public static string DiagramFolder { get; set; } = "Diagrams";
        /// <summary>
        /// Gets or sets the current page index.
        /// </summary>
        private int PageIndex { get; set; } = 0;
        /// <summary>
        /// Gets or sets the page size for pagination.
        /// </summary>
        private int PageSize { get; set; } = 10;
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
                    Action = (self) => ChangeSourcePath(),
                },
                new()
                {
                    Key = "---",
                    Text = new string('-', 65),
                    Action = (self) => { },
                    ForegroundColor = ConsoleColor.DarkGreen,
                },
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
                new()
                {
                    Key = "---",
                    Text = new string('-', 65),
                    Action = (self) => { },
                    ForegroundColor = ConsoleColor.DarkGreen,
                },
            };

            var files = GetSourceCodeFiles(SourcePath, ["*.cs"]).ToArray();

            for (int i = PageIndex * PageSize; i < files.Length && i < (PageIndex + 1) * PageSize; i++)
            {
                var file = files[i];
                var text = file;

                menuItems.Add(new MenuItem
                {
                    Key = (++mnuIdx).ToString(),
                    OptionalKey = "a", // it's for choose option all
                    Text = ToLabelText("File", $"{file.Replace(SourcePath, string.Empty)}"),
                    Action = (self) =>
                    {
                        var path = self.Params["file"]?.ToString() ?? string.Empty;

                        CreateDiagram(file, Force);
                    },
                    Params = new() { { "file", file } },
                });
            }

            var pageLabel = $"{PageIndex * PageSize}..{Math.Min((PageIndex + 1) * PageSize, files.Length)}/{files.Length}";

            menuItems.Add(new()
            {
                Key = "---",
                Text = ToLabelText(pageLabel, string.Empty, 20, ' '),
                Action = (self) => { },
                ForegroundColor = ConsoleColor.DarkGreen,
            });
            menuItems.Add(new()
            {
                Key = "+",
                Text = ToLabelText("Next", "Load next path page"),
                Action = (self) =>
                {
                    PageIndex = (PageIndex + 1) * PageSize <= files.Length ? PageIndex + 1 : PageIndex;
                    PrintScreen();
                },
                ForegroundColor = ConsoleColor.DarkGreen,
            });

            menuItems.Add(new()
            {
                Key = "-",
                Text = ToLabelText("Previous", "Load previous path page"),
                Action = (self) =>
                {
                    PageIndex = Math.Max(0, PageIndex - 1);
                    PrintScreen();
                },
                ForegroundColor = ConsoleColor.DarkGreen,
            });

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
            PrintLine($"Diagram builder: {DiagramBuilder} [{DiagramBuilderType.Activity}|{DiagramBuilderType.Class}]");
            PrintLine();
        }
        /// <summary>
        /// Prints the footer of the application.
        /// </summary>
        protected override void PrintFooter()
        {
            PrintLine();
            Print("Choose [n|n,n|a...all|x|X]: ");
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
                "class" => DiagramBuilderType.Activity,
                _ => DiagramBuilder,
            };
        }
        /// <summary>
        /// Creates a diagram based on the specified file path and diagram builder type.
        /// </summary>
        /// <param name="filePath">The file path of the diagram.</param>
        /// <param name="force">A flag indicating whether to force the creation of the diagram.</param>
        private static void CreateDiagram(string filePath, bool force)
        {
            StartProgressBar();
            UMLDiagramBuilder diagram = DiagramBuilder switch
            {
                DiagramBuilderType.Activity => new ActivityDiagramBuilder(filePath, DiagramFolder, force),
                DiagramBuilderType.Class => new ClassDiagramBuilder(filePath, DiagramFolder, force),
                _ => throw new InvalidOperationException("Invalid diagram builder type."),
            };

            diagram.Create();
        }
        #endregion methods for app
    }
}