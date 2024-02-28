namespace PlantUML.ConApp;

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
    #endregion app properties

    #region overrides
    /// <summary>
    /// Creates an array of menu items for the application menu.
    /// </summary>
    /// <returns>An array of MenuItem objects representing the menu items.</returns>
    protected override MenuItem[] CreateMenuItems()
    {
        var mnuIdx = 0;
        var files = GetSourceCodeFiles(SourcePath, ["*.cs"]);
        var menuItems = new List<MenuItem>
        {
            new()
            {
                Key = $"{++mnuIdx}",
                Text = "Force...........Change force flag",
                Action = ChangeForce,
            },
            new()
            {
                Key = $"{++mnuIdx}",
                Text = "Path............Change source path",
                Action = ChangeSourcePath,
            },
            new()
            {
                Key = "---",
                Text = new string('-', 60),
                Action = () => { },
            },
            new()
            {
                Key = $"{++mnuIdx}",
                Text = "Folder..........Change diagram folder",
                Action = ChangeDiagramFolder,
            },
            new()
            {
                Key = $"{++mnuIdx}",
                Text = "Builder.........Change diagram builder",
                Action = ChangeDiagramBuilder,
            },
            new()
            {
                Key = "---",
                Text = new string('-', 60),
                Action = () => { },
            },
        };

        foreach (var file in files.OrderBy(f => f))
        {
            menuItems.Add(new ()
            {
                Key = $"{++mnuIdx}",
                OptionalKey = "a",  // it's for choose option all
                Text = $"File............{file.Replace(SourcePath, string.Empty)}",
                Action = () => {  CreateDiagram(file, Force); },
            });
        }

        menuItems.Add(new ()
        {
            Key = "x|X",
            Text = "Exit",
            Action = () => { RunApp = false; },
        });

        return [.. menuItems];
    }

    /// <summary>
    /// Prints the header for the PlantUML application.
    /// </summary>
    /// <param name="sourcePath">The path of the solution.</param>
    protected override void PrintHeader(string sourcePath)
    {
        var count = 0;
        var saveForeColor = ForegroundColor;

        ForegroundColor = ConsoleColor.Green;

        count = PrintLine(nameof(PlantUML));
        PrintLine('=', count);
        PrintLine();
        ForegroundColor = saveForeColor;
        PrintLine($"Force flag:  {Force}");
        PrintLine($"Source path: {sourcePath}");
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
