//@BaseCode
//MdStart
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PlantUML.Logic.Extensions;
using System.Collections;
using System.Reflection;
using System.Text;

namespace PlantUML.Logic
{
    /// <summary>
    /// Represents a class that is responsible for creating various types of diagrams based on source code.
    /// </summary>
    public partial class DiagramCreator
    {
        #region Class-Constructors
        /// <summary>
        /// Initializes the <see cref="Program"/> class.
        /// This static constructor sets up the necessary properties for the program.
        /// </remarks>
        static DiagramCreator()
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
        /// Initializes a new instance of the <see cref="PlantUMLApp"/> class.
        /// </summary>
        public DiagramCreator()
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

        #region skinparam
        /// <summary>
        /// Gets the default class skin parameters.
        /// </summary>
        /// <remarks>
        /// The default class skin parameters include background color, arrow color, and border color.
        /// </remarks>
        /// <returns>
        /// An enumerable collection of strings representing the default class skin parameters.
        /// </returns>
        public static IEnumerable<string> DefaultClassSkinparam => new[] {
            "skinparam class {",
            " BackgroundColor whitesmoke",
            " ArrowColor grey",
            " BorderColor darkgrey",
            "}",
        };
        /// <summary>
        /// Returns the default skin parameters for objects.
        /// </summary>
        /// <remarks>
        /// The skin parameters define the appearance of objects in a diagram.
        /// </remarks>
        /// <returns>
        /// An enumerable collection of strings representing the default skin parameters.
        /// </returns>
        public static IEnumerable<string> DefaultObjectSkinparam => new[] {
            "skinparam object {",
            " BackgroundColor white",
            " ArrowColor grey",
            " BorderColor darkgrey",
            "}",
        };
        /// <summary>
        /// Represents a class that defines color values for different elements in the diagram.
        /// </summary>
        private static partial class Color
        {
            #region Class-Constructors
            /// <summary>
            /// Initializes the <see cref="Color"/> class.
            /// This static constructor sets up the necessary properties for the program.
            /// </summary>
            static Color()
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

            public static string Enum { get; set; } = "#LightBlue";
            public static string Struct { get; set; } = "#LightYellow";
            public static string Class { get; set; } = "#White";
            public static string AbstractClass { get; set; } = "#White";
            public static string Interface { get; set; } = "#LightGrey";

            public static string Parameters { get; set; } = "#LightGreen";
            public static string Declaration { get; set; } = "#LightSkyBlue";

            public static string Expression { get; set; } = "#WhiteSmoke";
            public static string Return { get; set; } = "#Lavender";

            public static string Participant { get; set; } = "#LightGreen";
            public static string StartParticipant { get; set; } = "#LightYellow";
        }
        #endregion skinparam

        #region class definitions
        /// <summary>
        /// Represents a collection of strings that make up a UML item.
        /// </summary>
        private class UMLItem : List<string>
        {
        }
        #endregion class definitions

        #region create activity diagram
        /// <summary>
        /// Creates activity diagrams for each method in the provided source code and saves them to the specified path.
        /// </summary>
        /// <param name="path">The path where the activity diagrams will be saved.</param>
        /// <param name="source">The source code to generate the activity diagrams from.</param>
        /// <param name="force">A flag indicating whether to overwrite existing activity diagrams if they already exist.</param>
        public static void CreateActivityDiagram(string path, string source, bool force)
        {
            var infoData = new List<string>();
            var syntaxTree = CSharpSyntaxTree.ParseText(source);
            var syntaxRoot = syntaxTree.GetRoot();
            var classNodes = syntaxRoot.DescendantNodes().OfType<ClassDeclarationSyntax>();

            if (Path.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            foreach (var classNode in classNodes)
            {
                var methodNodes = classNode.DescendantNodes().OfType<MethodDeclarationSyntax>();

                foreach (var methodNode in methodNodes)
                {
                    var title = $"{classNode.Identifier.Text}.{methodNode.Identifier.Text}";
                    var fileName = $"ac_{classNode.Identifier.Text}_{methodNode.Identifier.Text}.puml";
                    var filePath = Path.Combine(path, fileName);
                    var diagramData = CreateActivityDiagram(methodNode);

                    diagramData.Insert(0, $"@startuml {title}");
                    // diagramData.Insert(1, "header");
                    // diagramData.Insert(2, $"generated on {DateTime.UtcNow}");
                    // diagramData.Insert(3, "end header");
                    diagramData.Insert(1, $"title {title}");
                    diagramData.Insert(2, "start");

                    // diagramData.Add("footer");
                    // diagramData.Add("generated with the DiagramCreator by Prof.Gehrer");
                    // diagramData.Add("end footer");
                    diagramData.Add("stop");
                    diagramData.Add("@enduml");

                    if (force || Path.Exists(filePath) == false)
                    {
                        File.WriteAllLines(filePath, diagramData);
                    }
                    infoData.Add($"{nameof(title)}:{title}");
                    infoData.Add($"{nameof(fileName)}:{fileName}");
                    infoData.Add($"generated_on:{DateTime.UtcNow}");
                    infoData.Add($"generated_by:generated with the DiagramCreator by Prof.Gehrer");
                }
            }

            if (force || Path.Exists(Path.Combine(path, "ac_info.txt")) == false)
            {
                File.WriteAllLines(Path.Combine(path, "ac_info.txt"), infoData);
            }
            CreateCompleteActivityDiagram(path, force);
        }
        /// <summary>
        /// Creates an activity diagram based on the provided method declaration syntax.
        /// </summary>
        /// <param name="methodNode">The method declaration syntax to analyze.</param>
        /// <returns>A list of strings representing the activity diagram data.</returns>
        public static List<string> CreateActivityDiagram(MethodDeclarationSyntax methodNode)
        {
            var diagramData = new List<string>();
            var islocalDeclaration = false;
            var parameters = methodNode.ParameterList?.Parameters ?? [];
            var paramsStatement = parameters.Any() ? $"({string.Join(",", parameters)})" : string.Empty;

            if (paramsStatement.HasContent())
            {
                diagramData.Add($"{Color.Parameters}:Params{paramsStatement}");
            }
            foreach (StatementSyntax statement in methodNode?.Body?.Statements ?? [])
            {
                if (statement is LocalDeclarationStatementSyntax localDeclarationStatement)
                {
                    if (islocalDeclaration == false)
                    {
                        islocalDeclaration = true;
                        diagramData.Add($"{Color.Declaration}:{localDeclarationStatement.Declaration}");
                    }
                    else
                    {
                        diagramData.Add($"{localDeclarationStatement.Declaration}");
                    }
                }
                else
                {
                    if (islocalDeclaration)
                    {
                        islocalDeclaration = false;
                        diagramData[diagramData.Count - 1] += ";";
                    }
                    AnalyzeStatement(statement, diagramData, 0);
                }
            }
            return diagramData;
        }
        /// <summary>
        /// Creates a complete activity diagram by processing all the .puml files in the specified directory and its subdirectories.
        /// The diagram is generated in PlantUML format and saved as "CompleteActivityDiagram.puml" in the specified directory.
        /// Optionally, the diagram can be forced to be created even if it already exists.
        /// </summary>
        /// <param name="path">The path to the directory containing the .puml files.</param>
        /// <param name="force">A boolean value indicating whether to force the creation of the diagram even if it already exists.</param>
        public static void CreateCompleteActivityDiagram(string path, bool force)
        {
            var result = new List<string>();
            var umlFiles = new List<string>();
            var umlItems = new List<UMLItem>();
            var infoFilePath = Path.Combine(path, "ac_info.txt");
            var files = Directory.GetFiles(path, "*.puml", SearchOption.AllDirectories)
                                 .Where(f => Path.GetFileName(f).StartsWith("ac_"));

            if (File.Exists(infoFilePath))
            {
                var infoData = File.ReadAllLines(infoFilePath);

                foreach (var infoItem in infoData.Select(l => l.Split(':'))
                                                 .Where(d => d[0].Equals("fileName", StringComparison.OrdinalIgnoreCase))
                                                 .Select(d => d[1]))
                {
                    var query = files.Where(f => Path.GetFileName(f) == infoItem).Distinct();

                    umlFiles.AddRange(query);
                }
            }
            else
            {
                umlFiles.AddRange(files);
            }

            foreach (var file in umlFiles)
            {
                var lines = File.ReadAllLines(file);
                var acItems = ExtractUMLItems(lines).ToArray();
                var acTitles = lines.Where(l => l.StartsWith("title")).ToArray();

                if (acItems.Length == acTitles.Length)
                {
                    for (int i = 0; i < acItems.Length; i++)
                    {
                        acItems[i].Insert(1, $"note right: {acTitles[i].Replace("title", string.Empty)}");
                    }
                }
                umlItems.AddRange(acItems.Where(e => e.Count > 3));
            }

            var fileName = "CompleteActivityDiagram.puml";
            var filePath = Path.Combine(path, fileName);
            var diagramData = new List<string>();

            foreach (var item in umlItems)
            {
                diagramData.AddRange(item);
            }
            if (diagramData.Count > 0)
            {
                diagramData.Insert(0, "@startuml CompleteActivityDiagram");
                diagramData.Insert(1, "header");
                diagramData.Insert(2, $"generated on {DateTime.UtcNow}");
                diagramData.Insert(3, "end header");
                diagramData.Insert(4, "title CompleteActivityDiagram");

                diagramData.Add("footer");
                diagramData.Add("generated with the DiagramCreator by Prof.Gehrer");
                diagramData.Add("end footer");
                diagramData.Add("@enduml");
                if (force || Path.Exists(filePath) == false)
                {
                    File.WriteAllLines(filePath, diagramData);
                }
            }
        }
        #endregion create activity diagram

        #region create class diagram
        /// <summary>
        /// Creates a class diagram from the provided source code and saves it to the specified path.
        /// </summary>
        /// <param name="path">The path where the class diagram files will be saved.</param>
        /// <param name="source">The source code from which the class diagram will be generated.</param>
        /// <param name="force">A flag indicating whether to overwrite existing class diagram files.</param>
        public static void CreateClassDiagram(string path, string source, bool force)
        {
            var infoData = new List<string>();
            var syntaxTree = CSharpSyntaxTree.ParseText(source);
            var syntaxRoot = syntaxTree.GetRoot();
            var typeDeclarations = syntaxRoot.DescendantNodes().OfType<TypeDeclarationSyntax>();

            if (Path.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            foreach (var itemNode in typeDeclarations)
            {
                var title = $"{itemNode.Identifier.Text}";
                var fileName = $"cd_{itemNode.Identifier.Text}.puml";
                var filePath = Path.Combine(path, fileName);
                var diagramData = new List<string>();

                AnalyzeStatement(itemNode, diagramData, 0);
                diagramData.Insert(0, $"@startuml {title}");
                diagramData.Insert(1, $"title {title}");

                diagramData.Add("@enduml");
                if (force || Path.Exists(filePath) == false)
                {
                    File.WriteAllLines(filePath, diagramData);
                }
                infoData.Add($"{nameof(title)}:{title}");
                infoData.Add($"{nameof(fileName)}:{fileName}");
            }
            if (force)
            {
                File.WriteAllLines(Path.Combine(path, "cd_info.txt"), infoData);
            }
            CreateCompleteClassDiagram(path, force);
        }

        /// <summary>
        /// Creates a complete class diagram by extracting UML items and relations from multiple PlantUML files.
        /// The diagram is saved as a PlantUML file named "CompleteClassDiagram.puml" in the specified path.
        /// </summary>
        /// <param name="path">The directory path where the PlantUML files are located.</param>
        /// <param name="force">A flag indicating whether to overwrite an existing diagram file with the same name.</param>
        public static void CreateCompleteClassDiagram(string path, bool force)
        {
            var result = new List<string>();
            var umlItems = new List<UMLItem>();
            var umlRelations = new List<UMLItem>();
            var files = Directory.GetFiles(path, "*.puml", SearchOption.AllDirectories)
                                 .Where(f => Path.GetFileName(f).StartsWith("cd_"));

            foreach (var file in files)
            {
                var lines = File.ReadAllLines(file);

                umlItems.AddRange(ExtractUMLItems(lines));
                umlRelations.Add(ExtractUMLRelations(lines));
            }

            var fileName = "CompleteClassDiagram.puml";
            var filePath = Path.Combine(path, fileName);
            var diagramData = new List<string>();

            foreach (var item in umlItems)
            {
                diagramData.AddRange(item);
            }
            foreach (var item in umlRelations)
            {
                diagramData.AddRange(item);
            }
            if (diagramData.Count > 0)
            {
                diagramData.Insert(0, "@startuml CompleteClassDiagram");
                diagramData.Insert(1, "title CompleteClassDiagram");
                diagramData.Add("@enduml");
                if (force || Path.Exists(filePath) == false)
                {
                    File.WriteAllLines(filePath, diagramData);
                }
            }
        }
        #endregion create class diagram

        #region create sequence diagram
        /// <summary>
        /// Creates sequence diagrams for each method in the provided source code and saves them to the specified path.
        /// </summary>
        /// <param name="path">The path where the sequence diagrams will be saved.</param>
        /// <param name="source">The source code to generate the sequence diagrams from.</param>
        /// <param name="force">A flag indicating whether to overwrite existing sequence diagrams.</param>
        public static void CreateSequenceDiagram(string path, string source, bool force)
        {
            var infoData = new List<string>();
            var syntaxTree = CSharpSyntaxTree.ParseText(source);
            var syntaxRoot = syntaxTree.GetRoot();
            var classNodes = syntaxRoot.DescendantNodes().OfType<ClassDeclarationSyntax>();

            if (Path.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            foreach (var classNode in classNodes)
            {
                var methodNodes = classNode.DescendantNodes().OfType<MethodDeclarationSyntax>();

                foreach (var methodNode in methodNodes)
                {
                    var title = $"{classNode.Identifier.Text}.{methodNode.Identifier.Text}";
                    var fileName = $"sq_{classNode.Identifier.Text}_{methodNode.Identifier.Text}.puml";
                    var filePath = Path.Combine(path, fileName);
                    var diagramData = CreateSequenceDiagram(methodNode);

                    diagramData.Insert(0, $"@startuml {title}");
                    // diagramData.Insert(1, "header");
                    // diagramData.Insert(2, $"generated on {DateTime.UtcNow}");
                    // diagramData.Insert(3, "end header");
                    diagramData.Insert(1, $"title {title}");
                    //diagramData.Insert(2, "start");

                    // diagramData.Add("footer");
                    // diagramData.Add("generated with the DiagramCreator by Prof.Gehrer");
                    // diagramData.Add("end footer");
                    //diagramData.Add("stop");
                    diagramData.Add("@enduml");

                    if (force || Path.Exists(filePath) == false)
                    {
                        File.WriteAllLines(filePath, diagramData);
                    }
                    infoData.Add($"{nameof(title)}:{title}");
                    infoData.Add($"{nameof(fileName)}:{fileName}");
                    infoData.Add($"generated_on:{DateTime.UtcNow}");
                    infoData.Add($"generated_by:generated with the DiagramCreator by Prof.Gehrer");
                }
            }

            if (force || Path.Exists(Path.Combine(path, "sq_info.txt")) == false)
            {
                File.WriteAllLines(Path.Combine(path, "sq_info.txt"), infoData);
            }
        }
        /// <summary>
        /// Creates a sequence diagram based on the given method declaration syntax.
        /// </summary>
        /// <param name="methodNode">The method declaration syntax to create the sequence diagram from.</param>
        /// <returns>A list of strings representing the sequence diagram data.</returns>
        public static List<string> CreateSequenceDiagram(MethodDeclarationSyntax methodNode)
        {
            var diagramData = new List<string>();
            var participants = new List<string>();
            var parameters = methodNode.ParameterList?.Parameters ?? [];
            var paramsStatement = parameters.Any() ? $"({string.Join(",", parameters)})" : string.Empty;
            var invocationExpressions = methodNode?.DescendantNodes().OfType<InvocationExpressionSyntax>().ToArray() ?? [];

            participants.Add(CreateParticipantName(methodNode!));
            diagramData.Add($"participant \"{methodNode?.Identifier}{paramsStatement}\" as {CreateParticipantName(methodNode!)} {Color.StartParticipant}");
            foreach (var invocationExpression in invocationExpressions)
            {
                var identifier = invocationExpression.Expression.ToString();

                if (identifier.Contains("ToString") == false)
                {
                    var arguments = invocationExpression.ArgumentList?.Arguments ?? [];
                    var argsStatement = arguments.Any() ? $"({string.Join(", ", arguments.Select((item, index) => $"a{index}"))})" : "()";
                    var participantName = CreateParticipantName(invocationExpression);
                    var participantDeclaration = $"participant \"{identifier}{argsStatement}\" as {participantName} {Color.Participant}";

                    if (diagramData.Contains(participantDeclaration) == false)
                    {
                        participants.Add(participantName);
                        diagramData.Add(participantDeclaration);
                    }
                }
            }

            diagramData.Add("autonumber");
            AnalyzeCallSequence(methodNode!, participants, diagramData, 0);
            return diagramData;
        }
        #endregion create sequence diagram

        #region create object and class diagrams with types
        /// <summary>
        /// Extracts UML items from the given lines.
        /// </summary>
        /// <param name="lines">The lines to extract UML items from.</param>
        /// <returns>An enumerable collection of UML items.</returns>
        private static IEnumerable<UMLItem> ExtractUMLItems(IEnumerable<string> lines)
        {
            var result = new List<UMLItem>();
            var isItem = false;
            var umlItem = default(UMLItem);

            foreach (var line in lines)
            {
                if (isItem == false && line.Trim().EndsWith("{") && (line.Contains("class") || line.Contains("interface")))
                {
                    // item is class or interface
                    isItem = true;
                    umlItem = new UMLItem
                    {
                        line
                    };
                }
                else if (isItem == false && line.Trim().StartsWith("start", StringComparison.CurrentCultureIgnoreCase))
                {
                    // item is activity 
                    isItem = true;
                    umlItem = new UMLItem
                    {
                        line
                    };
                }
                else if (isItem && umlItem != default && line.Trim().StartsWith("}") == false && line.Trim().StartsWith("stop", StringComparison.CurrentCultureIgnoreCase) == false)
                {
                    umlItem.Add(line);
                }
                else if (isItem && umlItem != default && line.Trim().StartsWith("}"))
                {
                    // end of item from class or interface
                    umlItem.Add(line);
                    result.Add(umlItem);
                    umlItem = default;
                    isItem = false;
                }
                else if (isItem && umlItem != default && line.Trim().StartsWith("stop", StringComparison.CurrentCultureIgnoreCase))
                {
                    // end of item from activity
                    umlItem.Add(line);
                    result.Add(umlItem);
                    umlItem = default;
                    isItem = false;
                }
            }
            return result;
        }
        /// <summary>
        /// Represents a UML item.
        /// </summary>
        private static UMLItem ExtractUMLRelations(IEnumerable<string> lines)
        {
            var result = new UMLItem();

            foreach (var line in lines)
            {
                if (line.Contains("<|--") || line.Contains("--|>"))
                {
                    result.Add(line);
                }
            }
            return result;
        }

        /// <summary>
        /// Creates a class diagram based on specified creation flags and types.
        /// </summary>
        /// <param name="diagramCreationFlags">The flags indicating the desired elements to include in the diagram.</param>
        /// <param name="types">The types used to generate the class diagram.</param>
        /// <returns>A collection of strings representing the class diagram.</returns>
        public static IEnumerable<string> CreateClassDiagram(DiagramCreationFlags diagramCreationFlags, params Type[] types)
        {
            var result = new List<string>();
            var allTypes = new List<Type>(types);

            if ((diagramCreationFlags & DiagramCreationFlags.TypeExtends) > 0)
            {
                foreach (var type in allTypes.Clone().Where(t => t.IsClass))
                {
                    allTypes.AddRange(type.GetClassHierarchy().Where(e => allTypes.Contains(e) == false));
                }
            }

            if ((diagramCreationFlags & DiagramCreationFlags.ImplementedInterfaces) > 0)
            {
                foreach (var type in allTypes.Clone())
                {
                    allTypes.AddRange(type.GetDeclaredInterfaces().Where(e => allTypes.Contains(e) == false));
                }
            }

            if ((diagramCreationFlags & DiagramCreationFlags.InterfaceExtends) > 0)
            {
                foreach (var type in allTypes.Clone().Where(t => t.IsInterface))
                {
                    allTypes.AddRange(type.GetClassHierarchy().Where(e => allTypes.Contains(e) == false));
                }
            }

            result.AddRange(CreateTypeDefinitions(allTypes, diagramCreationFlags));

            if ((diagramCreationFlags & DiagramCreationFlags.TypeExtends) > 0)
            {
                result.AddRange(CreateTypeHirarchies(allTypes.Where(t => t.IsClass)));
            }

            if ((diagramCreationFlags & DiagramCreationFlags.InterfaceExtends) > 0)
            {
                foreach (var type in allTypes.Where(t => t.IsInterface))
                {
                    var extend = type.GetInterfaceHierarchy();

                    extend.Extends.ForEach(e => result.AddRange(CreateTypeHierachy(new[] { extend.Type!, e.Type! })));
                }
            }

            if ((diagramCreationFlags & DiagramCreationFlags.ImplementedInterfaces) > 0)
            {
                foreach (var type in allTypes.Where(t => t.IsClass))
                {
                    result.AddRange(CreateTypeImplements(type));
                }
            }

            if ((diagramCreationFlags & DiagramCreationFlags.ClassRelations) > 0)
            {
                foreach (var type in allTypes.Where(t => t.IsClass))
                {
                    result.AddRange(CreateTypeRelations(type, 0));
                }
            }
            return result;
        }
        /// <summary>
        /// Creates the name of an object by combining the object's type name and hash code.
        /// </summary>
        /// <param name="obj">The object to create the name for.</param>
        /// <returns>The name of the object.</returns>
        private static string CreateObjectName(Object obj) => $"{obj.GetType().Name}_{obj.GetHashCode()}";
        /// <summary>
        /// Creates a collection name for the specified object.
        /// </summary>
        /// <param name="obj">The object for which a collection name is to be created.</param>
        /// <returns>A string representing the collection name.</returns>
        private static string CreateCollectionName(Object obj) => $"Colletion_{obj.GetHashCode()}";
        /// <summary>
        /// Creates an object diagram for the given objects up to a specified depth.
        /// </summary>
        /// <param name="maxDeep">The maximum depth of the object diagram.</param>
        /// <param name="objects">The objects to include in the object diagram.</param>
        /// <returns>The lines representing the object diagram.</returns>
        public static IEnumerable<string> CreateObjectDiagram(int maxDeep, params Object[] objects)
        {
            var result = new List<string>();
            var createdObjects = new List<object>();
            void CreateMapStateRec(Object[] objects, List<string> lines, int deep)
            {
                static void CreateMapObjectState(Object obj, List<string> lines)
                {
                    lines.Add($"map {CreateObjectName(obj)} " + "{");
                    lines.AddRange(CreateObjectState(obj).SetIndent(1));
                    lines.Add("}");
                }
                static void CreateMapCollectionState(IEnumerable collection, List<string> lines)
                {
                    lines.Add($"map {CreateCollectionName(collection)} " + "{");
                    lines.AddRange(CreateCollectionState(collection).SetIndent(1));
                    lines.Add("}");
                }

                foreach (var obj in objects)
                {
                    if (createdObjects.Contains(obj) == false)
                    {
                        createdObjects.Add(obj);
                        CreateMapObjectState(obj, lines);

                        if (deep + 1 <= maxDeep)
                        {
                            var relations = obj.GetType().GetRelations();
                            var relationFieldInfos = obj.GetType().GetAllClassFields()
                                                        .Where(fi => relations.Any(r => r == fi.FieldType));

                            foreach (var relFieldInfo in relationFieldInfos)
                            {
                                var value = GetFieldValue(obj, relFieldInfo);

                                if (value is IEnumerable collection)
                                {
                                    var counter = 0;

                                    CreateMapCollectionState(collection, lines);
                                    lines.Add($"{CreateObjectName(obj)}::{GetFieldName(relFieldInfo)} --> {CreateCollectionName(value)}");
                                    if (deep + 2 <= maxDeep)
                                    {
                                        foreach (var item in collection)
                                        {
                                            if (item != null)
                                            {
                                                CreateMapStateRec([item], lines, deep + 2);
                                                lines.Add($"{CreateCollectionName(collection)}::{counter} --> {CreateObjectName(item)}");
                                            }
                                            counter++;
                                        }
                                    }
                                }
                                else if (value != null)
                                {
                                    CreateMapStateRec([value], lines, deep + 1);
                                    lines.Add($"{CreateObjectName(obj)}::{GetFieldName(relFieldInfo)} --> {CreateObjectName(value)}");
                                }
                            }
                        }
                    }
                }
            }
            CreateMapStateRec(objects, result, 0);
            return result;
        }
        #endregion create object and class diagrams with types

        #region diagram helpers
        /// <summary>
        /// Analyzes the call sequence within a method declaration and populates the participants and diagram data.
        /// </summary>
        /// <param name="methodDeclaration">The method declaration syntax node.</param>
        /// <param name="participants">The list of participants in the call sequence.</param>
        /// <param name="diagramData">The list of diagram data.</param>
        /// <param name="level">The current level of the call sequence.</param>
        public static void AnalyzeCallSequence(MethodDeclarationSyntax methodDeclaration, List<string> participants, List<string> diagramData, int level)
        {
            var statements = methodDeclaration?.Body?.Statements ?? [];

            foreach (StatementSyntax statement in statements!)
            {
                AnalyzeCallSequence(methodDeclaration!, statement, participants, diagramData, level);
            }
        }
        /// <summary>
        /// Analyzes the call sequence within a method declaration and generates diagram data.
        /// </summary>
        /// <param name="methodDeclaration">The method declaration syntax node.</param>
        /// <param name="syntaxNode">The syntax node to analyze.</param>
        /// <param name="participants">The list of participants in the call sequence.</param>
        /// <param name="diagramData">The list to store the generated diagram data.</param>
        /// <param name="level">The current level of analysis.</param>
        public static void AnalyzeCallSequence(MethodDeclarationSyntax methodDeclaration, SyntaxNode syntaxNode, List<string> participants, List<string> diagramData, int level)
        {
            if (syntaxNode is LocalDeclarationStatementSyntax localDeclarationStatement)
            {
                foreach (var variable in localDeclarationStatement.Declaration.Variables)
                {
                    AnalyzeCallSequence(methodDeclaration, variable, participants, diagramData, level);
                }
            }
            else if (syntaxNode is VariableDeclaratorSyntax variableDeclarator)
            {
                foreach (var item in variableDeclarator.Initializer?.Value?.ChildNodes() ?? [])
                {
                    if (item is InvocationExpressionSyntax varInvocationExpression)
                    {
                        var participantTo = CreateParticipantName(varInvocationExpression);

                        if (participants.Contains(participantTo))
                        {
                            var participantFrom = CreateParticipantName(methodDeclaration);
                            var argumentList = CreateArgumentList(varInvocationExpression);

                            if (argumentList != "()")
                            {
                                diagramData.Add($"{participantFrom} -> {participantTo} : {argumentList}");
                            }
                            else
                            {
                                diagramData.Add($"{participantFrom} -> {participantTo}");
                            }
                            diagramData.Add($"{participantTo} --> {participantFrom} : result");
                        }
                    }
                }
            }
            else if (syntaxNode is ExpressionStatementSyntax expressionStatement)
            {
                AnalyzeCallSequence(methodDeclaration, expressionStatement.Expression, participants, diagramData, level);
            }
            if (syntaxNode is InvocationExpressionSyntax invocationExpression)
            {
                var participantTo = CreateParticipantName(invocationExpression);

                if (participants.Contains(participantTo))
                {
                    var participantFrom = CreateParticipantName(methodDeclaration);
                    var argumentList = CreateArgumentList(invocationExpression);

                    if (argumentList != "()")
                    {
                        diagramData.Add($"{participantFrom} -> {participantTo} : {argumentList}");
                    }
                    else
                    {
                        diagramData.Add($"{participantFrom} -> {participantTo}");
                    }
                }
            }
            if (syntaxNode is AssignmentExpressionSyntax assignmentExpression)
            {
                var rightExpression = assignmentExpression.Right as InvocationExpressionSyntax;
                var participantTo = rightExpression != null ? CreateParticipantName(rightExpression) : string.Empty;

                if (participants.Contains(participantTo))
                {
                    var participantFrom = CreateParticipantName(methodDeclaration);
                    var argumentList = CreateArgumentList(rightExpression!);

                    if (argumentList != "()")
                    {
                        diagramData.Add($"{participantFrom} -> {participantTo} : {argumentList}");
                        diagramData.Add($"{participantTo} --> {participantFrom} : {assignmentExpression.Left}");
                    }
                    else
                    {
                        diagramData.Add($"{participantFrom} -> {participantTo}");
                        diagramData.Add($"{participantTo} --> {participantFrom} : {assignmentExpression.Left}");
                    }
                }
            }
            if (syntaxNode is BinaryExpressionSyntax binaryExpression)
            {
                AnalyzeCallSequence(methodDeclaration, binaryExpression.Left, participants, diagramData, level);
                AnalyzeCallSequence(methodDeclaration, binaryExpression.Right, participants, diagramData, level);
            }
        }

        /// <summary>
        /// Analyzes a syntax node and generates diagram data based on the type of the node.
        /// </summary>
        /// <param name="syntaxNode">The syntax node to analyze.</param>
        /// <param name="diagramData">The list to store the generated diagram data.</param>
        /// <param name="level">The indentation level for the generated diagram data.</param>
        public static void AnalyzeStatement(SyntaxNode syntaxNode, List<string> diagramData, int level)
        {
            static string ConvertModifiers(IEnumerable<SyntaxToken> modifiers)
            {
                string result = string.Empty;

                foreach (SyntaxToken modifier in modifiers)
                {
                    if (modifier.Text == "public")
                        result = $"+{result}";
                    else if (modifier.Text == "internal")
                        result = $"#{result}";
                    else if (modifier.Text == "abstract")
                        result += "abstract";
                    else if (modifier.Text == "static")
                        result += "{static}";
                }
                return result;
            }
            static string ConvertFieldDeclaration(FieldDeclarationSyntax declaration)
            {
                var result = string.Empty;
                var data = declaration.ToString().Split(' ');

                foreach (var item in data)
                {
                    if (item == "public")
                        result = $"+{result}";
                    else if (item == "internal")
                        result = $"#{result}";
                    else if (item == "private")
                        result = $"-{result}";
                    else if (item == "abstract")
                        result += "{abstract}";
                    else if (item == "static")
                        result += "{static}";
                    else if (item != ";")
                        result += $" {item}";
                }
                return result.Replace(";", string.Empty);
            }
            static string[] ConvertPropertyDeclaration(PropertyDeclarationSyntax declaration)
            {
                var result = new List<string>();
                var modifier = ConvertModifiers(declaration.Modifiers);

                if (declaration.AccessorList != null)
                {
                    foreach (AccessorDeclarationSyntax item in declaration.AccessorList!.Accessors)
                    {
                        var accessModifier = modifier;

                        if (item.Modifiers.Any())
                        {
                            accessModifier = ConvertModifiers(item.Modifiers);
                        }
                        if (item.Keyword.Text == "get")
                        {
                            result.Add($"{accessModifier} {declaration.Type} get{declaration.Identifier}()");
                        }
                        else if (item.Keyword.Text == "set")
                        {
                            result.Add($"{accessModifier} Void set{declaration.Identifier}({declaration.Type} value)");
                        }
                    }
                }
                else
                {
                    result.Add($"{modifier} {declaration.Type} get{declaration.Identifier}()");
                }
                return result.ToArray();
            }
            static string ConvertMethodDeclaration(MethodDeclarationSyntax declaration)
            {
                var modifiers = ConvertModifiers(declaration.Modifiers);
                var parameterList = "(";

                if (declaration.ParameterList != default)
                {
                    int paramCount = 0;

                    foreach (var item in declaration.ParameterList.Parameters)
                    {
                        if (paramCount++ > 0)
                            parameterList += ", ";

                        parameterList += $"{item.Type} {item.Identifier}";
                    }
                }
                parameterList += ")";

                return $"{modifiers} {declaration.ReturnType} {declaration.Identifier}{parameterList}";
            }

            const string yesLabel = "<color:green>yes";
            const string noLabel = "<color:red>no";

            if (syntaxNode is LocalDeclarationStatementSyntax localDeclarationStatement)
            {
                diagramData.Add($"{Color.Declaration}:{localDeclarationStatement.Declaration};".SetIndent(level));
            }
            else if (syntaxNode is ExpressionStatementSyntax expressionStatement)
            {
                var expression = expressionStatement.ToString();

                expression = expression.Replace("System.Console.WriteLine", "PrintLine");
                expression = expression.Replace("Console.WriteLine", "PrintLine");
                expression = expression.Replace("System.Console.Write", "Print");
                expression = expression.Replace("Console.Write", "Print");

                expression = expression.Replace("System.Console.ReadLine", "ReadLine");
                expression = expression.Replace("Console.ReadLine", "ReadLine");
                expression = expression.Replace("System.Console.Read", "Read");
                expression = expression.Replace("Console.Read", "Read");

                diagramData.Add($"{Color.Expression}:{expression}".SetIndent(level));
            }
            else if (syntaxNode is BlockSyntax blockSyntax)
            {
                foreach (var node in blockSyntax.ChildNodes())
                {
                    if (node is StatementSyntax statementSyntax)
                    {
                        AnalyzeStatement(statementSyntax, diagramData, level + 1);
                    }
                }
            }
            else if (syntaxNode is IfStatementSyntax ifStatement)
            {
                diagramData.Add($"if ({ifStatement.Condition}) then ({yesLabel})".SetIndent(level));
                AnalyzeStatement(ifStatement.Statement, diagramData, level + 1);
                if (ifStatement.Else != null)
                    AnalyzeStatement(ifStatement.Else, diagramData, level + 1);

                diagramData.Add("endif".SetIndent(level));
            }
            else if (syntaxNode is ElseClauseSyntax elseClause)
            {
                diagramData.Add($"else ({noLabel})".SetIndent(level));
                AnalyzeStatement(elseClause.Statement, diagramData, level + 1);
            }
            else if (syntaxNode is SwitchStatementSyntax switchStatement)
            {
                diagramData.Add($"switch ({switchStatement.Expression})".SetIndent(level));
                foreach (var section in switchStatement.Sections)
                {
                    var labels = $"{section.Labels}".Replace("case", "case(");

                    if (labels.Contains("default:"))
                        labels = labels.Replace("default:", "case ( default )");
                    else
                        labels = labels.Replace(":", " )");

                    diagramData.Add($"{labels}".SetIndent(level + 1));
                    foreach (var node in section.ChildNodes())
                    {
                        if (node is StatementSyntax statementSyntax)
                        {
                            AnalyzeStatement(statementSyntax, diagramData, level + 1);
                        }
                    }
                }
                diagramData.Add("endswitch".SetIndent(level));
            }
            else if (syntaxNode is BreakStatementSyntax breakStatement)
            {
                System.Diagnostics.Debug.WriteLine($"{nameof(breakStatement)} is known but not used!");
            }
            else if (syntaxNode is ContinueStatementSyntax continueStatement)
            {
                System.Diagnostics.Debug.WriteLine($"{nameof(continueStatement)} is known but not used!");
            }
            else if (syntaxNode is DoStatementSyntax doStatement)
            {
                diagramData.Add("repeat".SetIndent(level));
                AnalyzeStatement(doStatement.Statement, diagramData, level + 1);
                diagramData.Add($"repeat while ({doStatement.Condition}) is ({yesLabel})".SetIndent(level));
            }
            else if (syntaxNode is WhileStatementSyntax whileStatement)
            {
                diagramData.Add($"while ({whileStatement.Condition}) is ({yesLabel})".SetIndent(level));
                AnalyzeStatement(whileStatement.Statement, diagramData, level + 1);
                diagramData.Add($"endwhile ({noLabel})".SetIndent(level));
            }
            else if (syntaxNode is ForStatementSyntax forStatement)
            {
                diagramData.Add($"{Color.Declaration}:{forStatement.Declaration};".SetIndent(level));
                diagramData.Add($"while ({forStatement.Condition}) is ({yesLabel})".SetIndent(level));
                AnalyzeStatement(forStatement.Statement, diagramData, level + 1);
                if (forStatement.Incrementors.Count > 0)
                    diagramData.Add($":{forStatement.Incrementors};".SetIndent(level));

                diagramData.Add($"endwhile ({noLabel})".SetIndent(level));
            }
            else if (syntaxNode is ForEachStatementSyntax forEachStatement)
            {
                var statements = new List<string>();

                diagramData.Add($":iterator = {forEachStatement.Expression}.GetIterator();".SetIndent(level));
                diagramData.Add($"while (iterator.MoveNext()) is ({yesLabel})".SetIndent(level));
                diagramData.Add($":current = iterator.Current();".SetIndent(level));

                AnalyzeStatement(forEachStatement.Statement, statements, level + 1);

                foreach (var statement in statements)
                {
                    diagramData.Add(statement.Replace(forEachStatement.Identifier.ToString(), "current").SetIndent(level + 1));
                }

                diagramData.Add($"endwhile ({noLabel})".SetIndent(level));
            }
            else if (syntaxNode is ReturnStatementSyntax returnStatement)
            {
                diagramData.Add($"{Color.Return}:return {returnStatement.Expression};".SetIndent(level));
            }
            else if (syntaxNode is EnumDeclarationSyntax enumDeclaration)
            {
                diagramData.Add($"enum {enumDeclaration.Identifier} {Color.Enum}".SetIndent(level));
                foreach (var member in enumDeclaration.Members)
                {
                    diagramData.Add($"{member.Identifier}".SetIndent(level + 1));
                }
                diagramData.Add("}".SetIndent(level));
            }
            else if (syntaxNode is StructDeclarationSyntax structDeclaration)
            {
                diagramData.Add($"struct {structDeclaration.Identifier} {Color.Struct}".SetIndent(level));
                foreach (var member in structDeclaration.Members)
                {
                    AnalyzeStatement(member, diagramData, level + 1);
                }
                diagramData.Add("}".SetIndent(level));
            }
            else if (syntaxNode is InterfaceDeclarationSyntax interfaceDeclaration)
            {
                string declaration = ConvertModifiers(interfaceDeclaration.Modifiers);

                declaration += $"interface {interfaceDeclaration.Identifier} {Color.Interface}";
                declaration += " {";
                diagramData.Add(declaration.SetIndent(level));
                foreach (var member in interfaceDeclaration.Members.Where(m => m is FieldDeclarationSyntax))
                {
                    AnalyzeStatement(member, diagramData, level + 1);
                }
                diagramData.Add("---".SetIndent(level));
                foreach (var member in interfaceDeclaration.Members.Where(m => m is PropertyDeclarationSyntax))
                {
                    AnalyzeStatement(member, diagramData, level + 1);
                }
                diagramData.Add("---".SetIndent(level));
                foreach (var member in interfaceDeclaration.Members.Where(m => m is MethodDeclarationSyntax))
                {
                    AnalyzeStatement(member, diagramData, level + 1);
                }
                diagramData.Add("}".SetIndent(level));
            }
            else if (syntaxNode is ClassDeclarationSyntax classDeclaration)
            {
                string declaration = ConvertModifiers(classDeclaration.Modifiers);

                declaration += $"class {classDeclaration.Identifier}";
                declaration += declaration.Contains("abstract") ? $" {Color.Class}" : $" {Color.AbstractClass}";
                declaration += " {";
                diagramData.Add(declaration.SetIndent(level));
                foreach (var member in classDeclaration.Members.Where(m => m is FieldDeclarationSyntax))
                {
                    AnalyzeStatement(member, diagramData, level + 1);
                }
                diagramData.Add("---".SetIndent(level));
                foreach (var member in classDeclaration.Members.Where(m => m is PropertyDeclarationSyntax))
                {
                    AnalyzeStatement(member, diagramData, level + 1);
                }
                diagramData.Add("---".SetIndent(level));
                foreach (var member in classDeclaration.Members.Where(m => m is MethodDeclarationSyntax))
                {
                    AnalyzeStatement(member, diagramData, level + 1);
                }
                diagramData.Add("}".SetIndent(level));
                if (classDeclaration.BaseList != null)
                {
                    foreach (var baseType in classDeclaration.BaseList.Types)
                    {
                        diagramData.Add($"{classDeclaration.Identifier} <|-- {baseType.Type}".SetIndent(level));
                    }
                }
            }
            else if (syntaxNode is FieldDeclarationSyntax fieldDeclaration)
            {
                diagramData.Add(ConvertFieldDeclaration(fieldDeclaration).SetIndent(level));
            }
            else if (syntaxNode is PropertyDeclarationSyntax propertyDeclaration)
            {
                diagramData.AddRange(ConvertPropertyDeclaration(propertyDeclaration));
            }
            else if (syntaxNode is MethodDeclarationSyntax methodDeclaration)
            {
                diagramData.Add(ConvertMethodDeclaration(methodDeclaration));
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"{syntaxNode.GetType().Name} is unknown!");
            }
        }

        /// Creates the XML documentation for the CreateTypeDefinition method.
        /// @param type The type for which to create the type definition.
        /// @param diagramCreationFlags The flags to determine which members to include in the type definition.
        /// @return A collection of strings representing the XML documentation for the method.
        public static IEnumerable<string> CreateTypeDefinition(Type type, DiagramCreationFlags diagramCreationFlags)
        {
            var result = new List<string>();

            if (type.IsEnum)
            {
                result.Add($"enum {type.Name} #light" +
                $"blue " + "{");
                if ((diagramCreationFlags & DiagramCreationFlags.EnumMembers) > 0)
                {
                    foreach (var item in Enum.GetValues(type))
                    {
                        result.Add($" {item}");
                    }
                }
                result.Add("}");
            }
            else if (type.IsClass)
            {
                var prefix = type.IsPublic ? "+" : string.Empty;

                if (type.IsAbstract)
                    result.Add($"{prefix}abstract class {type.Name} {Color.Class} " + "{");
                else
                    result.Add($"{prefix}class {type.Name} {Color.AbstractClass} " + "{");
                if ((diagramCreationFlags & DiagramCreationFlags.ClassMembers) > 0)
                {
                    result.AddRange(CreateItemMembers(type).SetIndent(1));
                }
                result.Add("}");
            }
            else if (type.IsInterface)
            {
                result.Add($"interface {type.Name} {Color.Interface} " + "{");
                if ((diagramCreationFlags & DiagramCreationFlags.InterfaceMembers) > 0)
                {
                    result.AddRange(CreateItemMembers(type).SetIndent(1));
                }
                result.Add("}");
            }
            return result;
        }
        /// <summary>
        /// Creates type hierarchies for the given collection of types.
        /// </summary>
        /// <param name="types">The collection of types.</param>
        /// <returns>A collection of string representing the type hierarchies.</returns>
        public static IEnumerable<string> CreateTypeHirarchies(IEnumerable<Type> types)
        {
            var result = new List<string>();

            foreach (var item in CreateDiagramHirarchies(types))
            {
                result.AddRange(CreateTypeHierachy(item));
            }
            return result.Distinct();
        }
        /// <summary>
        /// Creates a type hierarchy based on the input collection of types.
        /// </summary>
        /// <param name="types">The collection of types to create the hierarchy from.</param>
        /// <returns>An enumerable collection of strings representing the type hierarchy.</returns>
        public static IEnumerable<string> CreateTypeHierachy(IEnumerable<Type> types)
        {
            var result = new List<string>();
            var typeArray = types.ToArray();

            for (int i = 0; i < typeArray.Length - 1; i++)
            {
                if (typeArray[i + 1].IsInterface || typeArray[i].IsInterface)
                {
                    result.Add($"{typeArray[i + 1].Name} <|.. {typeArray[i].Name}");
                }
                else
                {
                    result.Add($"{typeArray[i + 1].Name} <|-- {typeArray[i].Name}");
                }
            }
            return result;
        }
        /// <summary>
        /// Retrieves a collection of type relations for the specified type up to the specified depth.
        /// </summary>
        /// <param name="type">The type for which to retrieve type relations.</param>
        /// <param name="deep">The depth up to which to retrieve type relations.</param>
        /// <returns>A collection of string representations of the type relations.</returns>
        public static IEnumerable<string> CreateTypeRelations(Type type, int deep)
        {
            var result = new List<string>();

            foreach (var item in type.GetRelations(deep))
            {
                if (item.IsArray)
                {
                    var elemType = item.UnderlyingSystemType;

                    if (item.IsNullableType())
                    {
                        result.Add($"{type.Name} --> \"0..1\" {elemType.Name}");
                    }
                    else
                    {
                        result.Add($"{type.Name} --> \"1\" {elemType.Name}");
                    }
                }
                else if (item.IsGenericCollectionType())
                {
                    var elemType = item.GetGenericArguments()[0];

                    if (item.IsNullableType())
                    {
                        result.Add($"{type.Name} \"*\" o-- \"0..1\" {elemType.Name}");
                    }
                    else
                    {
                        result.Add($"{type.Name} \"*\" *-- \"1\" {elemType.Name}");
                    }
                }
                else
                {
                    if (item.IsNullableType())
                    {
                        result.Add($"{type.Name} --> \"0..1\" {item.Name}");
                    }
                    else
                    {
                        result.Add($"{type.Name} --> \"1\" {item.Name}");
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Creates a collection of formatted string representations of interfaces implemented by the specified type.
        /// </summary>
        /// <param name="type">The type to check for implemented interfaces.</param>
        /// <returns>A collection of formatted string representations of interfaces implemented by the specified type.</returns>
        public static IEnumerable<string> CreateTypeImplements(Type type)
        {
            var result = new List<string>();
            var lolypop = "()-";

            foreach (var typeInfo in type.GetDeclaredInterfaces())
            {
                result.Add($"{typeInfo.Name} {lolypop} {type.Name}");
            }
            return result;
        }
        /// <summary>
        /// Creates type definitions for the specified collection of types.
        /// </summary>
        /// <param name="types">The collection of types to create type definitions for.</param>
        /// <param name="diagramCreationFlags">Flags specifying diagram creation options.</param>
        /// <returns>A collection of type definitions as strings.</returns>
        public static IEnumerable<string> CreateTypeDefinitions(IEnumerable<Type> types, DiagramCreationFlags diagramCreationFlags)
        {
            var result = new List<string>();

            foreach (var type in types)
            {
                result.AddRange(CreateTypeDefinition(type, diagramCreationFlags));
            }
            return result;
        }
        /// <summary>
        /// Retrieves a collection of item members for the specified type.
        /// </summary>
        /// <param name="type">The type to retrieve item members from.</param>
        /// <returns>A collection of item members.</returns>
        public static IEnumerable<string> CreateItemMembers(Type type)
        {
            var counter = 0;
            var result = new List<string>();

            #region fields
            BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (var item in type.GetFields(bindingFlags))
            {
                counter++;
                result.Add($"{(item.IsPublic ? "+" : "-")}" + " {static}" + $"{item.FieldType.GetSourceTypeName()} {GetFieldName(item)}");
            }
            if (counter > 0)
                result.Add("---");

            counter = 0;
            bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            foreach (var item in type.GetFields(bindingFlags))
            {
                var prefix = item.IsPrivate ? "-" : "+";

                counter++;
                result.Add($"{prefix}" + " " + $"{item.FieldType.GetSourceTypeName()} {GetFieldName(item)}");
            }
            if (counter > 0)
                result.Add("---");
            #endregion fields

            #region properties
            counter = 0;
            bindingFlags = BindingFlags.Static | BindingFlags.Public;
            foreach (var item in type.GetProperties(bindingFlags))
            {
                if (item.CanRead)
                {
                    counter++;
                    result.Add(" + {static}" + $"{item.PropertyType.GetSourceTypeName()} get{item.Name}()");
                }
                if (item.CanWrite)
                {
                    counter++;
                    result.Add(" + {static}" + $"set{item.Name}({item.PropertyType.GetSourceTypeName()} value)");
                }
            }
            if (counter > 0)
                result.Add("---");

            counter = 0;
            bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
            foreach (var item in type.GetProperties(bindingFlags))
            {
                if (item.CanRead)
                {
                    counter++;
                    result.Add($" + {item.PropertyType.GetSourceTypeName()} get{item.Name}()");
                }
                if (item.CanWrite)
                {
                    counter++;
                    result.Add($" + set{item.Name}({item.PropertyType.GetSourceTypeName()} value)");
                }
            }
            if (counter > 0)
                result.Add("---");
            #endregion

            #region methoden
            counter = 0;
            bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (var item in type.GetMethods(bindingFlags))
            {
                var prefix = item.IsPrivate ? "-" : "+";

                counter++;
                result.Add($"{prefix} " + "{static}" + $"{item.ReturnType.GetSourceTypeName()} {item.Name}({GetParameters(item)})");
            }
            if (counter > 0)
                result.Add("---");

            counter = 0;
            bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            foreach (var item in type.GetMethods(bindingFlags))
            {
                var prefix = item.IsPrivate ? "-" : "+";

                counter++;
                result.Add($"{prefix} {item.ReturnType.GetSourceTypeName()} {item.Name}({GetParameters(item)})");
            }
            //if (counter > 0)
            //    result.Add("---");
            #endregion methoden
            return result;
        }
        /// <summary>
        /// Creates an object state by retrieving the fields and their values of the specified object.
        /// </summary>
        /// <param name="obj">The object for which to create the state.</param>
        /// <returns>An enumerable collection of string representations of the object state.</returns>
        public static IEnumerable<string> CreateObjectState(Object obj)
        {
            var counter = 0;
            var result = new List<string>();

            #region fields
            BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (var item in obj.GetType().GetFields(bindingFlags))
            {
                counter++;
                // result.Add("{static}" + $"{item.FieldType.Name} {GetFieldName(item)} => {GetStateValue(obj, item)}");
                result.Add("{static}" + $"{GetFieldName(item)} => {GetStateValue(obj, item)}");
            }
            if (counter > 0)
                result.Add("---");

            counter = 0;
            foreach (var item in obj.GetType().GetAllClassFields())
            {
                counter++;
                //                result.Add($"{item.FieldType.Name} {GetFieldName(item)} => {GetStateValue(obj, item)}");
                result.Add($"{GetFieldName(item)} => {GetStateValue(obj, item)}");
            }
            //if (counter > 0)
            //    result.Add("---");
            #endregion fields

            return result;
        }
        /// <summary>
        /// Creates a collection state by iterating through the given collection and generating a string representation of each item in the collection.
        /// </summary>
        /// <param name="collection">The collection to iterate through.</param>
        /// <returns>An enumerable collection of strings representing the state of items in the collection.</returns>
        public static IEnumerable<string> CreateCollectionState(IEnumerable collection)
        {
            var counter = 0;
            var result = new List<string>();

            foreach (var item in collection)
            {
                if (item != null)
                {
                    result.Add($"{counter++} => {item.GetType().Name}_{item.GetHashCode()}");
                }
                else
                {
                    result.Add($"{counter++} => null");
                }
            }
            return result;
        }
        #endregion diagram helpers

        #region helpers
        private static string CreateParticipantName(MethodDeclarationSyntax methodSyntax)
        {
            var identifier = methodSyntax.Identifier;
            var parameters = methodSyntax.ParameterList?.Parameters ?? [];

            return $"{identifier}" + (parameters.Any() ? $"_{string.Join("_", parameters.Select((item, index) => $"p{index}"))}" : string.Empty);
        }
        private static string CreateParticipantName(InvocationExpressionSyntax invocationExpression)
        {
            var identifier = invocationExpression.Expression.ToString();
            var arguments = invocationExpression.ArgumentList?.Arguments ?? [];

            return $"{identifier}" + (arguments.Any() ? $"_{string.Join("_", arguments.Select((item, index) => $"a{index}"))}" : string.Empty);
        }
        private static string CreateArgumentList(InvocationExpressionSyntax invocationExpression)
        {
            var result = "()";
            var arguments = invocationExpression.ArgumentList?.Arguments ?? [];

            if (arguments.Any())
            {
                result = $"({string.Join(" ,", arguments.Select((item, index) => $"{item}"))})";
            }
            return result;
        }
        /// <summary>
        /// Creates diagram hierarchies for the given types.
        /// </summary>
        /// <param name="types">The types for which to create diagram hierarchies.</param>
        /// <returns>An enumerable of enumerables of types representing the diagram hierarchies.</returns>
        public static IEnumerable<IEnumerable<Type>> CreateDiagramHirarchies(IEnumerable<Type> types)
        {
            var result = new List<IEnumerable<Type>>();

            foreach (var type in types)
            {
                var classHirarchy = type.GetClassHierarchy();

                if (classHirarchy.Count() > 1)
                {
                    result.Add(classHirarchy);
                }
            }
            var calculatedHirarchies = new List<IEnumerable<Type>>();

            for (int i = 0; i < result.Count - 1; i++)
            {
                var commonSet = result[i];

                for (int j = i + 1; j < result.Count; j++)
                {
                    commonSet = commonSet.Intersect(result[j]);
                }

                if (commonSet.Count() > 1)
                {
                    for (int j = 0; j < result.Count; j++)
                    {
                        var currentSet = result[j];
                        var exceptSet = currentSet.Except(commonSet);

                        if (commonSet.All(e => currentSet.Any(c => e == c)) && exceptSet.Any())
                        {
                            var intersectSet = result[j].Intersect(new[] { commonSet.First() });
                            var createSet = exceptSet.Union(intersectSet);

                            calculatedHirarchies.Add(createSet);
                        }
                        else
                        {
                            calculatedHirarchies.Add(result[j]);
                        }
                    }
                }
                else
                {
                    calculatedHirarchies.AddRange(result);
                }
                result.Clear();
                result.AddRange(calculatedHirarchies);
                calculatedHirarchies.Clear();
            }
            return result.Distinct();
        }
        /// <summary>
        /// Returns the name of the field.
        /// </summary>
        /// <param name="fieldInfo">The fieldInfo object representing the field.</param>
        /// <returns>The name of the field.</returns>
        public static string GetFieldName(FieldInfo fieldInfo)
        {
            string? result;

            if (fieldInfo.Name.Contains("k__BackingField"))
            {
                result = "_" + fieldInfo.Name.Betweenstring("<", ">");
                result = string.Concat(result[..2].ToLower(), result.AsSpan(2));
            }
            else
            {
                result = fieldInfo.Name;
            }
            return result;
        }
        /// <summary>
        /// Retrieves the parameters of a method and returns them as a string.
        /// </summary>
        /// <param name="methodInfo">The <see cref="MethodInfo"/> object representing the method.</param>
        /// <returns>A string representation of the parameters in the format "ParameterType parameterName, ..." (comma separated).</returns>
        public static string GetParameters(MethodInfo methodInfo)
        {
            var counter = 0;
            var result = new StringBuilder();

            foreach (var item in methodInfo.GetParameters())
            {
                if (counter++ > 0)
                    result.Append(", ");

                result.Append($"{item.ParameterType.Name} {item.Name}");
            }
            return result.ToString();
        }
        /// <summary>
        /// Retrieves the value of a specified field.
        /// </summary>
        /// <param name="obj">The object whose field value will be retrieved.</param>
        /// <param name="fieldInfo">The FieldInfo object representing the field.</param>
        /// <returns>The value of the specified field.</returns>
        public static Object? GetFieldValue(Object obj, FieldInfo fieldInfo)
        {
            object? value;

            if (fieldInfo.IsStatic)
            {
                value = fieldInfo.GetValue(null);
            }
            else
            {
                value = fieldInfo.GetValue(obj);
            }
            return value;
        }
        /// <summary>
        /// Retrieves the state value of an object's field using a specified FieldInfo.
        /// </summary>
        /// <param name="obj">The object whose state value is to be retrieved.</param>
        /// <param name="fieldInfo">The FieldInfo of the field to retrieve the state value from.</param>
        /// <returns>The state value of the object's field.</returns>
        public static string GetStateValue(Object obj, FieldInfo fieldInfo)
        {
            return GetStateValue(obj, fieldInfo, 15);
        }
        /// <summary>
        /// Retrieves the state value of a specified field.
        /// </summary>
        /// <param name="obj">The object to retrieve the state value from.</param>
        /// <param name="fieldInfo">The field information of the desired field.</param>
        /// <param name="maxLength">The maximum length of the returned value.</param>
        /// <returns>The state value of the specified field as a string.</returns>
        public static string GetStateValue(Object obj, FieldInfo fieldInfo, int maxLength)
        {
            string? result;
            var value = GetFieldValue(obj, fieldInfo);

            if (fieldInfo.FieldType.IsValueType)
            {
                result = value?.ToString() ?? string.Empty;
            }
            else if (fieldInfo.FieldType == typeof(string))
            {
                result = $"\"{value}\"";
            }
            else if (value == null)
            {
                result = "null";
            }
            else
            {
                result = $"_{value.GetHashCode()}";
            }
            return result.Length > maxLength - 3 ? result[..(maxLength - 2)] + "..." : result;
        }
        #endregion helpers
    }
}
//MdEnd