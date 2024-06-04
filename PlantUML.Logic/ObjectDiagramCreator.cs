//@BaseCode
//MdStart
namespace PlantUML.Logic
{
    /// <summary>
    /// Represents a class that creates object diagrams and writes them to a file.
    /// </summary>
    public class ObjectDiagramCreator
    {
        #region properties
        /// <summary>
        /// Gets or sets the maximum depth of the object diagram.
        /// </summary>
        public int MaxDepth { get; set; } = 100;

        /// <summary>
        /// Gets or sets the path where the object diagram file will be saved.
        /// </summary>
        public string DiagramPath { get; set; }

        /// <summary>
        /// Gets or sets the name of the object diagram file.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets the full file path of the object diagram file.
        /// </summary>
        public string FilePath => Path.Combine(DiagramPath, FileName);
        #endregion properties

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectDiagramCreator"/> class with default values.
        /// The object diagram file will be saved in the user's profile directory with the default file name.
        /// </summary>
        public ObjectDiagramCreator()
            : this(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "od_object.puml")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectDiagramCreator"/> class with the specified file name.
        /// The object diagram file will be saved in the user's profile directory with the specified file name.
        /// </summary>
        /// <param name="fileName">The name of the object diagram file.</param>
        public ObjectDiagramCreator(string fileName)
            : this(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), fileName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectDiagramCreator"/> class with the specified diagram path and file name.
        /// </summary>
        /// <param name="diagramPath">The path where the object diagram file will be saved.</param>
        /// <param name="fileName">The name of the object diagram file.</param>
        public ObjectDiagramCreator(string diagramPath, string fileName)
        {
            DiagramPath = diagramPath;
            FileName = fileName;
        }
        #endregion constructors

        #region methods
        /// <summary>
        /// Creates an object diagram for the specified object.
        /// </summary>
        /// <param name="obj">The object to create the diagram for.</param>
        /// <param name="notes">Additional notes to include in the diagram.</param>
        /// <returns>A list of strings representing the object diagram.</returns>
        public List<string> Create(object obj, params string[] notes)
        {
            var diagramData = DiagramCreator.CreateObjectDiagram(MaxDepth, obj).ToList();

            if (diagramData.Any())
            {
                diagramData.Insert(0, "@start" + "uml object");
                if (notes.Length == 1)
                {
                    diagramData.Add("skinparam TitleFontColor #DarkBlue");
                    diagramData.Add("title " + notes[0]);
                }
                else
                {
                    foreach (var note in notes)
                    {
                        diagramData.Add(note);
                    }
                }
                diagramData.Add("@end" + "uml");
            }
            return diagramData;
        }

        /// <summary>
        /// Creates an object diagram and writes it to a file.
        /// </summary>
        /// <param name="obj">The object to create the diagram for.</param>
        /// <param name="notes">Additional notes to include in the diagram.</param>
        public void CreateToFile(object obj, params string[] notes)
        {
            var diagramData = Create(obj, notes);

            File.WriteAllLines(FilePath, diagramData);
        }
        #endregion methods
    }
}
//MdEnd