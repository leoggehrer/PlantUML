//@BaseCode
//MdStart
namespace PlantUML.Watcher
{
    using System.Diagnostics;
    /// <summary>
    /// Represents a folder watcher that monitors changes in a specified directory.
    /// </summary>
    public abstract partial class FolderWatcher : IDisposable
    {
        #region Class-Constructors
        /// <summary>
        /// Initializes the class.
        /// This static constructor sets up the necessary properties for the program.
        /// </remarks>
        static FolderWatcher()
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

        #region properties
        /// <summary>
        /// Gets the path being watched.
        /// </summary>
        public string WatchPath { get; private set; }
        /// <summary>
        /// Gets or sets the filter used to determine which files are monitored in the folder.
        /// </summary>
        public string Filter { get; private set; }
        private FileSystemWatcher Watcher { get; set; }
        #endregion properties

        #region Instance-Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FolderWatcher"/> class with the specified parameters.
        /// </summary>
        /// <param name="path">The path of the folder to watch.</param>
        /// <param name="filter">The filter for the files to watch.</param>
        public FolderWatcher(string path, string filter)
        {
            Constructing();

            WatchPath = path;
            Filter = filter;
            Watcher = new FileSystemWatcher(WatchPath)
            {
                NotifyFilter = NotifyFilters.Attributes
                            | NotifyFilters.CreationTime
                            | NotifyFilters.DirectoryName
                            | NotifyFilters.FileName
                            | NotifyFilters.LastAccess
                            | NotifyFilters.LastWrite
                            | NotifyFilters.Security
                            | NotifyFilters.Size
            };

            Watcher.Changed += OnChanged;
            Watcher.Created += OnCreated;
            Watcher.Deleted += OnDeleted;
            Watcher.Renamed += OnRenamed;
            Watcher.Error += OnError;

            Watcher.Filter = Filter;
            Watcher.IncludeSubdirectories = true;
            Watcher.EnableRaisingEvents = true;

            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
        #endregion Instance-Constructors

        #region Event-Handlers
        /// <summary>
        /// Event handler for the FileSystemWatcher's Changed event.
        /// </summary>
        /// <param name="source">The object that raised the event.</param>
        /// <param name="e">The event data.</param>
        protected virtual void OnChanged(object source, FileSystemEventArgs e)
        {
            Debug.WriteLine($"File: {e.FullPath} {e.ChangeType}");
        }
        /// <summary>
        /// Event handler for the file creation event.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="FileSystemEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCreated(object source, FileSystemEventArgs e)
        {
            Debug.WriteLine($"File: {e.FullPath} {e.ChangeType}");
        }
        protected virtual void OnDeleted(object source, FileSystemEventArgs e)
        {
            Debug.WriteLine($"File: {e.FullPath} {e.ChangeType}");
        }
        /// <summary>
        /// Event handler for the "Renamed" event.
        /// </summary>
        /// <param name="source">The object that raised the event.</param>
        /// <param name="e">The event arguments containing information about the renamed file.</param>
        protected virtual void OnRenamed(object source, RenamedEventArgs e)
        {
            Debug.WriteLine($"File: {e.OldFullPath} renamed to {e.FullPath}");
        }
        /// <summary>
        /// Event handler for handling errors that occur during the folder watching process.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">An <see cref="ErrorEventArgs"/> object that contains the event data.</param>
        protected virtual void OnError(object source, ErrorEventArgs e)
        {
            Debug.WriteLine($"Error: {e.GetException()}");
        }
        #endregion Event-Handlers

        #region helpers
        /// <summary>
        /// Checks if the specified path is accessible and none of the files matching the search pattern are in use.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <param name="searchPattern">The search pattern to match files.</param>
        /// <returns>True if the path is accessible and none of the files are in use; otherwise, false.</returns>
        public static bool IsPathAccessible(string path, string[] searchPattern)
        {
            var result = true;
            var files = CommonTool.Application.GetSourceCodeFiles(path, searchPattern);

            for (var i = 0; i < files.Count && result; i++)
            {
                var file = files[i];

                if (IsFileInUse(file))
                {
                    result = false;
                }
            }
            return result;
        }
        /// <summary>
        /// Checks if a file is currently in use by another process.
        /// </summary>
        /// <param name="filePath">The path of the file to check.</param>
        /// <returns>True if the file is in use, false otherwise.</returns>
        public static bool IsFileInUse(string filePath)
        {
            try
            {
                // Versuch, die Datei exklusiv zu öffnen
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                {
                    // Wenn es gelingt, schließen wir die Datei wieder
                }
                return false;
            }
            catch (IOException)
            {
                // Eine IOException bedeutet, dass die Datei von einem anderen Prozess verwendet wird
                return true;
            }
        }
        #endregion helpers

        #region dispose
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Watcher.Dispose();
        }
        #endregion dispose
    }
}
//MdEnd