//@BaseCode
//MdStart
namespace PlantUML.Watcher
{
    using CommonTool.Extensions;
    public partial class UMLWatcher : FolderWatcher
    {
        #region Class-Constructors
        /// <summary>
        /// Initializes the <see cref="UMLWatcher"/> class.
        /// This static constructor sets up the necessary properties for the program.
        /// </summary>
        static UMLWatcher()
        {
            ClassConstructing();
            ClassConstructed();
        }

        /// <summary>
        /// This method is called during the construction of the <see cref="UMLWatcher"/> class.
        /// </summary>
        static partial void ClassConstructing();

        /// <summary>
        /// Represents a method that is called when the <see cref="UMLWatcher"/> class is constructed.
        /// </summary>
        static partial void ClassConstructed();
        #endregion Class-Constructors

        #region properties
        /// <summary>
        /// Gets or sets the type of diagram builder.
        /// </summary>
        public DiagramBuilderType DiagramBuilder { get; private set; }

        /// <summary>
        /// Gets the target path.
        /// </summary>
        public string TargetPath { get; private set; }
        /// <summary>
        /// Gets the maximum depth for the UML diagram.
        /// </summary>
        public int MaxSubPathDeep { get; private set; } = 3;

        /// <summary>
        /// Gets or sets the folder path for diagrams.
        /// </summary>
        public string DiagramFolder { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether to create a complete diagram.
        /// </summary>
        public bool CreateCompleteDiagram { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the force flag is enabled.
        /// </summary>
        public bool Force { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the process is currently in progress.
        /// </summary>
        private bool InProcess { get; set; } = false;
        #endregion properties

        #region Instance-Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UMLWatcher"/> class with the specified parameters.
        /// </summary>
        /// <param name="sourcePath">The path of the folder to watch.</param>
        /// <param name="maxSubPathDeep">The max source path deep.</param>
        /// <param name="targetPath">The target path.</param>
        /// <param name="diagramFolder">The folder where the diagrams will be saved.</param>
        /// <param name="diagramBuilderType">The type of diagram builder to use.</param>
        /// <param name="filter">The filter for the files to watch.</param>
        /// <param name="createCompleteDiagram">A flag indicating whether to create a complete diagram.</param>
        /// <param name="force">A flag indicating whether to force the creation of diagrams.</param>
        public UMLWatcher(string sourcePath, int maxSubPathDeep, string targetPath, string diagramFolder, DiagramBuilderType diagramBuilderType, string filter, bool createCompleteDiagram, bool force)
            : base(sourcePath, filter)
        {
            Constructing();

            TargetPath = targetPath;
            MaxSubPathDeep = maxSubPathDeep;
            DiagramFolder = diagramFolder;
            DiagramBuilder = diagramBuilderType;
            CreateCompleteDiagram = createCompleteDiagram;
            Force = force;

            Constructed();
        }

        partial void Constructing();
        partial void Constructed();
        #endregion Instance-Constructors

        #region Event-Handlers
        /// <summary>
        /// Event handler for the file creation event.
        /// </summary>
        /// <param name="source">The object that raised the event.</param>
        /// <param name="e">The event data.</param>
        protected override void OnCreated(object source, FileSystemEventArgs e)
        {
            base.OnCreated(source, e);

            if (InProcess == false)
            {
                InProcess = true;
                UpdateDiagrams(e.FullPath);
                InProcess = false;
            }
        }

        /// <summary>
        /// Event handler for the file system change event.
        /// </summary>
        /// <param name="source">The object that raised the event.</param>
        /// <param name="e">The event data.</param>
        override protected void OnChanged(object source, FileSystemEventArgs e)
        {
            base.OnChanged(source, e);

            if (InProcess == false)
            {
                InProcess = true;
                UpdateDiagrams(e.FullPath);
                InProcess = false;
            }
        }
        /// <summary>
        /// Event handler for the "Deleted" event.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        protected override void OnDeleted(object source, FileSystemEventArgs e)
        {
            base.OnDeleted(source, e);

            if (InProcess == false)
            {
                InProcess = true;
                UpdateDiagrams(e.FullPath);
                InProcess = false;
            }
        }
        #endregion Event-Handlers

        #region app-methods
        /// <summary>
        /// Updates the diagrams based on the specified file path.
        /// </summary>
        /// <param name="filePath">The path of the file that triggered the update.</param>
        private void UpdateDiagrams(string filePath)
        {
            var path = Path.GetDirectoryName(filePath);

            if (Path.Exists(path))
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    while (IsPathAccessible(path, [Filter]) == false)
                    {
                        Thread.Sleep(500);
                    }
                }

                if (Force)
                {
                    DeleteDiagrams(TargetPath, DiagramFolder);
                }

                if ((DiagramBuilder & DiagramBuilderType.Activity) > 0)
                {
                    var builder = new ActivityDiagramBuilder(path, MaxSubPathDeep, TargetPath, DiagramFolder, CreateCompleteDiagram, Force);

                    builder.CreateFromPath();
                }
                if ((DiagramBuilder & DiagramBuilderType.Class) > 0)
                {
                    var builder = new ClassDiagramBuilder(path, MaxSubPathDeep, TargetPath, DiagramFolder, CreateCompleteDiagram, Force);

                    builder.CreateFromPath();
                }
                if ((DiagramBuilder & DiagramBuilderType.Sequence) > 0)
                {
                    var builder = new SequenceDiagramBuilder(path, MaxSubPathDeep, TargetPath, DiagramFolder, Force);

                    builder.CreateFromPath();
                }
            }
        }
        /// <summary>
        /// Deletes all PlantUML diagram files in the specified target path or diagram folder.
        /// </summary>
        /// <param name="targetPath">The target path where the diagram files are located.</param>
        /// <param name="diagramFolder">The optional diagram folder within the target path.</param>
        private void DeleteDiagrams(string targetPath, string diagramFolder)
        {
            var path = diagramFolder.IsNullOrEmpty() ? targetPath : Path.Combine(targetPath, diagramFolder);

            if (Path.Exists(path))
            {
                var di = new DirectoryInfo(path);
                var files = di.GetFiles(PlantUML.Logic.DiagramCreator.PlantUMLExtension.Replace(".", "*."))
                                     .Where(p => p.Extension == PlantUML.Logic.DiagramCreator.PlantUMLExtension);

                foreach (FileInfo file in files)
                {
                    try
                    {
                        file.Attributes = FileAttributes.Normal;
                        File.Delete(file.FullName);
                    }
                    catch
                    {
                        System.Diagnostics.Debug.WriteLine($"Could not delete file: {file.FullName}");
                    }
                }
            }
        }
        #endregion app-methods
    }
}
//MdEnd