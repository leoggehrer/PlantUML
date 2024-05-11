using CommonTool;
using CommonTool.Extensions;

namespace PlantUML.ConApp
{
    public partial class PlantUMLApp : ConsoleApplication
    {
        #region Class-Constructors
        /// <summary>
        /// Initializes the <see cref="Program"/> class.
        /// This static constructor sets up the necessary properties for the program.
        /// </remarks>
        static PlantUMLApp()
        {
            ClassConstructing();
            TargetPath = ProjectsPath = SourcePath;
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
            PageSize = 15;
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
        /// Gets or sets the path to the projects.
        /// </summary>
        public static string ProjectsPath { get; set; }
        /// <summary>
        /// Gets or sets the target path.
        /// </summary>
        public static string TargetPath { get; set; }
        /// <summary>
        /// Gets or sets the selected diagram type.
        /// </summary>
        public DiagramBuilderType DiagramBuilder { get; set; } = DiagramBuilderType.All;
        /// <summary>
        /// Gets or sets the folder path for diagrams.
        /// </summary>
        public static string DiagramFolder { get; set; } = "diagrams";
        /// <summary>
        /// Gets or sets a value indicating whether to create a complete diagram.
        /// </summary>
        public static bool CreateCompleteDiagram { get; set; } = true;
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
                    Text = ToLabelText("Depth", "Change max sub path depth"),
                    Action = (self) => ChangeMaxSubPathDepth(),
                },
                new()
                {
                    Key = $"{++mnuIdx}",
                    Text = ToLabelText("Projects path", "Change projects path"),
                    Action = (self) =>
                    {
                        var savePath = ProjectsPath;

                        ProjectsPath = SelectOrChangeToSubPath(ProjectsPath, MaxSubPathDepth, [ SourcePath ]);
                        if (savePath != ProjectsPath)
                        {
                            PageIndex = 0;
                        }
                        if (savePath == TargetPath)
                        {
                            TargetPath = ProjectsPath;
                        }
                    },
                },
                new()
                {
                    Key = $"{++mnuIdx}",
                    Text = ToLabelText("Target path", "Change target path"),
                    Action = (self) =>
                    {
                        var savePath = TargetPath;

                        TargetPath = SelectOrChangeToSubPath(TargetPath, MaxSubPathDepth, [ SourcePath ]);
                        if (savePath != TargetPath)
                        {
                            PageIndex = 0;
                        }
                    },
                },
                CreateMenuSeparator(),
                new()
                {
                    Key = $"{++mnuIdx}",
                    Text = ToLabelText("Create", "Change create complete diagram"),
                    Action = (self) => CreateCompleteDiagram = !CreateCompleteDiagram,
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
                CreateMenuSeparator(),
            };

            if (mnuIdx % 10 != 0)
            {
                mnuIdx += 10 - (mnuIdx % 10);
            }

            var paths = new [] { ProjectsPath }.Union(TemplatePath.GetSubPaths(ProjectsPath, MaxSubPathDepth + 1))
                                               .Where(p => TemplatePath.ContainsFiles(p, "*.cs"))
                                               .OrderBy(p => p)
                                               .ToArray();

            menuItems.AddRange(CreatePageMenuItems(ref mnuIdx, paths, (item, menuItem) =>
            {
                var subPath = item.Replace(ProjectsPath, string.Empty);
                var targetPath = item.Replace(ProjectsPath, TargetPath);

                if (subPath.IsNullOrEmpty())
                {
                    subPath = $"{Path.DirectorySeparatorChar}{ProjectsPath.Split(Path.DirectorySeparatorChar).LastOrDefault()}";
                }
                menuItem.Text = ToLabelText("Path", $"{subPath}");
                menuItem.Tag = "paths";
                menuItem.Action = (self) => CreateDiagram(self, Force);
                menuItem.Params = new() { { "sourcePath", item }, { "targetPath", targetPath } };
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
            PrintLine($"Force flag:       {Force}");
            PrintLine($"Max. path depth:  {MaxSubPathDepth}");
            PrintLine($"Projets path:     {ProjectsPath}");
            PrintLine();
            PrintLine($"Target path:      {TargetPath}");
            PrintLine($"Diagram folder:   {DiagramFolder}");
            PrintLine($"Diagram complete: {CreateCompleteDiagram}");
            PrintLine($"Diagram builder:  {DiagramBuilder} [{DiagramBuilderType.All}|{DiagramBuilderType.Activity}|{DiagramBuilderType.Class}|{DiagramBuilderType.Sequence}]");
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
        private void ChangeDiagramBuilder()
        {
            DiagramBuilder = DiagramBuilder.ToString().ToLower() switch
            {
                "all" => DiagramBuilderType.Activity,
                "activity" => DiagramBuilderType.Class,
                "class" => DiagramBuilderType.Sequence,
                "sequence" => DiagramBuilderType.All,
                _ => DiagramBuilder,
            };
        }

        /// <summary>
        /// Creates a UML diagram based on the specified <paramref name="menuItem"/> and <paramref name="force"/> flag.
        /// </summary>
        /// <param name="menuItem">The menu item containing the parameters and tag for the diagram creation.</param>
        /// <param name="force">A flag indicating whether to force the creation of the diagram.</param>
        private void CreateDiagram(MenuItem menuItem, bool force)
        {
            var sourcePath = menuItem.Params["sourcePath"]?.ToString() ?? string.Empty;
            var targetPath = menuItem.Params["targetPath"]?.ToString() ?? string.Empty;

            static void execute(UMLDiagramBuilder builder, MenuItem menuItem)
            {
                if (menuItem.Tag.Equals("paths", StringComparison.CurrentCultureIgnoreCase))
                    builder.CreateFromPath();
                else if (menuItem.Tag.Equals("file", StringComparison.CurrentCultureIgnoreCase))
                    builder.CreateFromFile();
            }

            StartProgressBar();
            if ((DiagramBuilder & DiagramBuilderType.Activity) > 0)
            {
                var builder = new ActivityDiagramBuilder(sourcePath, MaxSubPathDepth, targetPath, DiagramFolder, CreateCompleteDiagram, force);

                execute(builder, menuItem);
            }
            if ((DiagramBuilder & DiagramBuilderType.Class) > 0)
            {
                var builder = new ClassDiagramBuilder(sourcePath, MaxSubPathDepth, targetPath, DiagramFolder, CreateCompleteDiagram, force);

                execute(builder, menuItem);
            }
            if ((DiagramBuilder & DiagramBuilderType.Sequence) > 0)
            {
                var builder = new SequenceDiagramBuilder(sourcePath, MaxSubPathDepth, targetPath, DiagramFolder, force);

                execute(builder, menuItem);
            }
        }
        #endregion methods for app
    }
}