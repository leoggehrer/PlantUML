//@BaseCode
//MdStart
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections;
using System.Reflection;
using System.Text;
using PlantUML.Logic.Extensions;
using CommonTool.Extensions;

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

        #region properties
        /// <summary>
        /// Gets the file filter for info files.
        /// </summary>
        public static string InfoFileFilter => "*_info.txt";
        /// <summary>
        /// Gets or sets the extension for PlantUML files.
        /// </summary>
        public static string PlantUMLExtension => ".puml";
        public static string CustomUMLLabel => "CustomUML";
        #endregion properties

        #region skinparam
        /// <summary>
        /// Gets the default skin parameters for the class diagram.
        /// </summary>
        public static IEnumerable<string> DefaultClassSkinparam => [
            "skinparam class {",
            " BackgroundColor whitesmoke",
            " ArrowColor grey",
            " BorderColor darkgrey",
            "}",
        ];
        /// <summary>
        /// Gets the default skin parameters for objects in the diagram.
        /// </summary>
        public static IEnumerable<string> DefaultObjectSkinparam => [
            "skinparam object {",
            " BackgroundColor white",
            " ArrowColor grey",
            " BorderColor darkgrey",
            "}",
        ];

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
            public static string Class { get; set; } = "#GhostWhite";
            public static string RootObject { get; set; } = "#LightBlue";
            public static string Object { get; set; } = "#LightGoldenRodYellow";
            public static string AbstractClass { get; set; } = "#White";
            public static string Interface { get; set; } = "#LightGrey";

            public static string Parameters { get; set; } = "#LightGreen";
            public static string Declaration { get; set; } = "#LightSkyBlue";

            public static string Expression { get; set; } = "#WhiteSmoke";
            public static string Return { get; set; } = "#Lavender";

            public static string Participant { get; set; } = "#LightGreen";
            public static string StartParticipant { get; set; } = "#LightYellow";

            public static string Throw { get; set; } = "#Red";
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
        /// Creates activity diagrams for the specified source code and saves them to the given path.
        /// </summary>
        /// <param name="path">The path where the activity diagrams will be saved.</param>
        /// <param name="source">The source code to generate the activity diagrams from.</param>
        /// <param name="defines">An array of preprocessor symbols to be used during parsing.</param>
        /// <param name="force">A flag indicating whether to overwrite existing diagrams.</param>
        public static void CreateActivityDiagram(string path, string source, string[] defines, bool force)
        {
            var infoFileName = "ac_info.txt";
            var fileCounter = 0;
            var infoData = new List<string>();
            var options = new CSharpParseOptions().WithPreprocessorSymbols(defines);
            var syntaxTree = CSharpSyntaxTree.ParseText(source, options);
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
                    var fileName = $"ac_{classNode.Identifier.Text}_{methodNode.Identifier.Text}";
                    var diagramData = CreateActivityDiagram(methodNode);

                    if (infoData.Contains($"{nameof(fileName)}:{fileName}{PlantUMLExtension}") == false)
                    {
                        fileName = $"{fileName}{PlantUMLExtension}";
                    }
                    else
                    {
                        fileName = $"{fileName}_{++fileCounter}{PlantUMLExtension}";
                    }

                    var filePath = Path.Combine(path, fileName);

                    diagramData.AddRange(ReadCustomUMLFromFle(filePath));
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

                    infoData.Add($"{nameof(title)}:{title} (AC)");
                    infoData.Add($"{nameof(fileName)}:{fileName}");
                    infoData.Add($"generated_on:{DateTime.UtcNow}");
                    infoData.Add($"generated_by:generated with the DiagramCreator by Prof.Gehrer");
                }
            }

            if (force || Path.Exists(Path.Combine(path, infoFileName)) == false)
            {
                File.WriteAllLines(Path.Combine(path, infoFileName), infoData);
                UpdateDiagramPath(path, InfoFileFilter);
            }
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
                diagramData.Add($"{Color.Parameters}:Params{paramsStatement};");
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
                        diagramData[^1] += $"\\n{localDeclarationStatement.Declaration}";
                    }
                }
                else
                {
                    if (islocalDeclaration)
                    {
                        islocalDeclaration = false;
                        diagramData[^1] += ";";
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
            var infoFileName = "ac_info.txt";
            var result = new List<string>();
            var umlFiles = new List<string>();
            var umlItems = new List<UMLItem>();
            var infoFilePath = Path.Combine(path, infoFileName);
            var files = Directory.GetFiles(path, PlantUMLExtension.Replace(".", "*."), SearchOption.AllDirectories)
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
            var completeInfoData = new List<string>();

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

                completeInfoData.Add($"title:All acivity diagrams (AC)");
                completeInfoData.Add($"{nameof(fileName)}:{fileName}");
                completeInfoData.Add($"generated_on:{DateTime.UtcNow}");
                completeInfoData.Add($"generated_by:generated with the DiagramCreator by Prof.Gehrer");
            }

            var comleteInfoFileName = "CompleteActivityDiagram_info.txt";

            if (force || Path.Exists(Path.Combine(path, comleteInfoFileName)) == false)
            {
                File.WriteAllLines(Path.Combine(path, comleteInfoFileName), completeInfoData);
            }
        }
        #endregion create activity diagram

        #region create class diagram
        /// <summary>
        /// Creates a class diagram from the provided source code and saves it to the specified path.
        /// </summary>
        /// <param name="path">The path where the class diagram files will be saved.</param>
        /// <param name="source">The source code from which the class diagram will be generated.</param>
        /// <param name="defines">An array of preprocessor symbols to be used during parsing of the source code.</param>
        /// <param name="force">A flag indicating whether to overwrite existing class diagram files.</param>
        public static void CreateClassDiagram(string path, string source, string[] defines, bool force)
        {
            var infoFileName = "cd_info.txt";
            var fileCounter = 0;
            var infoData = new List<string>();
            var options = new CSharpParseOptions().WithPreprocessorSymbols(defines);
            var syntaxTree = CSharpSyntaxTree.ParseText(source, options);
            var syntaxRoot = syntaxTree.GetRoot();
            var mscorlib = MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location);
            var systemCore = MetadataReference.CreateFromFile(typeof(Enumerable).GetTypeInfo().Assembly.Location);
            var compilation = CSharpCompilation.Create("ClassCompilation", syntaxTrees: [syntaxTree], references: [mscorlib, systemCore]);
            var semanticModel = compilation.GetSemanticModel(syntaxTree);
            var typeDeclarations = syntaxRoot.DescendantNodes().OfType<TypeDeclarationSyntax>();

            if (Path.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            foreach (var itemNode in typeDeclarations)
            {
                var title = $"{itemNode.Identifier.Text}";
                var fileName = $"cd_{itemNode.Identifier.Text}";
                var diagramData = new List<string>();

                if (infoData.Contains($"{nameof(fileName)}:{fileName}{PlantUMLExtension}") == false)
                {
                    fileName = $"{fileName}{PlantUMLExtension}";
                }
                else
                {
                    fileName = $"{fileName}_{++fileCounter}{PlantUMLExtension}";
                }

                AnalyzeDeclarationSyntax(semanticModel, itemNode, diagramData, 0);

                var filePath = Path.Combine(path, fileName);

                diagramData.AddRange(ReadCustomUMLFromFle(filePath));
                diagramData.Insert(0, $"@startuml {title}");
                diagramData.Insert(1, $"title {title}");
                diagramData.Add("@enduml");

                if (force || Path.Exists(filePath) == false)
                {
                    File.WriteAllLines(filePath, diagramData);
                }
                infoData.Add($"{nameof(title)}:{title} (CD)");
                infoData.Add($"{nameof(fileName)}:{fileName}");
            }

            if (force || Path.Exists(Path.Combine(path, infoFileName)) == false)
            {
                File.WriteAllLines(Path.Combine(path, infoFileName), infoData);
                UpdateDiagramPath(path, InfoFileFilter);
            }
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
            var files = Directory.GetFiles(path, $"*{PlantUMLExtension}", SearchOption.AllDirectories)
                                 .Where(f => Path.GetFileName(f).StartsWith("cd_"));

            foreach (var file in files)
            {
                var lines = File.ReadAllLines(file);

                umlItems.AddRange(ExtractUMLItems(lines));
                umlRelations.Add(ExtractUMLRelations(lines));
            }

            foreach (var item in umlItems)
            {
                var isContained = result.IsSubsequence(item);

                if (isContained == false)
                {
                    result.AddRange(item);
                }
            }

            foreach (var item in umlRelations.SelectMany(e => e).Distinct())
            {
                var itemData = item.Trim()
                                   .Split([" <|-- ", " --|> "], StringSplitOptions.RemoveEmptyEntries)
                                   .ToArray();

                foreach (var relationPart in itemData)
                {
                    if (result.Any(l => l.Contains(relationPart)) == false)
                    {
                        if (relationPart.Length > 1 && relationPart[0] == 'I' && char.IsUpper(relationPart[1]))
                        {
                            result.Add($"interface {relationPart} {Color.Interface}");
                        }
                        else
                        {
                            result.Add($"class {relationPart} {Color.Class}");
                        }
                    }
                }
                result.Add(item);
            }

            var fileName = "CompleteClassDiagram.puml";
            var filePath = Path.Combine(path, fileName);
            var diagramData = new List<string>(result);
            var completeInfoData = new List<string>();

            if (diagramData.Count > 0)
            {
                diagramData.AddRange(ReadCustomUMLFromFle(filePath));
                diagramData.Insert(0, "@startuml CompleteClassDiagram");
                diagramData.Insert(1, "title CompleteClassDiagram");
                diagramData.Add("@enduml");

                if (force || Path.Exists(filePath) == false)
                {
                    File.WriteAllLines(filePath, diagramData);
                }

                completeInfoData.Add($"title:All class diagrams (CD)");
                completeInfoData.Add($"{nameof(fileName)}:{fileName}");
                completeInfoData.Add($"generated_on:{DateTime.UtcNow}");
                completeInfoData.Add($"generated_by:generated with the DiagramCreator by Prof.Gehrer");
            }

            var comleteInfoFileName = "CompleteClassDiagram_info.txt";

            if (force || Path.Exists(Path.Combine(path, comleteInfoFileName)) == false)
            {
                File.WriteAllLines(Path.Combine(path, comleteInfoFileName), completeInfoData);
            }
        }
        #endregion create class diagram

        #region create sequence diagram
        /// <summary>
        /// Creates sequence diagrams for the specified source code and saves them to the specified path.
        /// </summary>
        /// <param name="path">The path where the sequence diagrams will be saved.</param>
        /// <param name="source">The source code to generate the sequence diagrams from.</param>
        /// <param name="defines">An array of preprocessor symbols to be used during parsing.</param>
        /// <param name="force">A flag indicating whether to overwrite existing files in the specified path.</param>
        public static void CreateSequenceDiagram(string path, string source, string[] defines, bool force)
        {
            var infoFileName = "sq_info.txt";
            var fileCounter = 0;
            var infoData = new List<string>();
            var options = new CSharpParseOptions().WithPreprocessorSymbols(defines);
            var syntaxTree = CSharpSyntaxTree.ParseText(source, options);
            var syntaxRoot = syntaxTree.GetRoot();
            var classNodes = syntaxRoot.DescendantNodes().OfType<ClassDeclarationSyntax>();
            var mscorlib = MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location);
            var systemCore = MetadataReference.CreateFromFile(typeof(Enumerable).GetTypeInfo().Assembly.Location);
            var compilation = CSharpCompilation.Create("SequenceCompilation", syntaxTrees: [syntaxTree], references: [mscorlib, systemCore]);
            var semanticModel = compilation.GetSemanticModel(syntaxTree);

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
                    var fileName = $"sq_{classNode.Identifier.Text}_{methodNode.Identifier.Text}";
                    var diagramData = CreateSequenceDiagram(semanticModel, methodNode);

                    if (diagramData.Count > 0)
                    {
                        if (infoData.Contains($"{nameof(fileName)}:{fileName}{PlantUMLExtension}") == false)
                        {
                            fileName = $"{fileName}{PlantUMLExtension}";
                        }
                        else
                        {
                            fileName = $"{fileName}_{++fileCounter}{PlantUMLExtension}";
                        }
                        
                        var filePath = Path.Combine(path, fileName);

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
                        diagramData.AddRange(ReadCustomUMLFromFle(filePath));
                        diagramData.Add("@enduml");

                        if (force || Path.Exists(filePath) == false)
                        {
                            File.WriteAllLines(filePath, diagramData);
                        }
                        infoData.Add($"{nameof(title)}:{title} (SQ)");
                        infoData.Add($"{nameof(fileName)}:{fileName}");
                        infoData.Add($"generated_on:{DateTime.UtcNow}");
                        infoData.Add($"generated_by:generated with the DiagramCreator by Prof.Gehrer");
                    }
                }
            }

            if (force || Path.Exists(Path.Combine(path, infoFileName)) == false)
            {
                File.WriteAllLines(Path.Combine(path, infoFileName), infoData);
                UpdateDiagramPath(path, InfoFileFilter);
            }
        }
        /// <summary>
        /// Creates a sequence diagram based on the provided semantic model and method declaration syntax.
        /// </summary>
        /// <param name="semanticModel">The semantic model representing the code.</param>
        /// <param name="methodNode">The method declaration syntax node.</param>
        /// <returns>A list of strings representing the sequence diagram data.</returns>
        public static List<string> CreateSequenceDiagram(SemanticModel semanticModel, MethodDeclarationSyntax methodNode)
        {
            var diagramData = new List<string>();
            var messages = new List<string>();
            var participants = new List<string>();
            var participantAliasse = new List<string>();
            var invocationExpressions = methodNode.DescendantNodes().OfType<InvocationExpressionSyntax>().ToArray() ?? [];
            var filteredInvocationExpressions = invocationExpressions.Where(ie => ie.Expression.ToString().Contains("ToString") == false
                                                                               && ie.Expression.ToString().Contains("ConfigureAwait") == false
                                                                               && ie.Expression.ToString().Contains("nameof") == false);

            participants.Add(CreateParticipant(methodNode));
            participants.AddRange(filteredInvocationExpressions.Select(ie => CreateParticipant(ie)).Distinct());

            participantAliasse.Add(CreateParticipantAlias(methodNode!));
            participantAliasse.AddRange(filteredInvocationExpressions.Select(ie => CreateParticipantAlias(ie)).Distinct());

            AnalyzeCallSequence(semanticModel, methodNode!, participantAliasse, messages, 0);

            for (var i = 0; i < participants.Count && i < participantAliasse.Count; i++)
            {
                var isReferenced = messages.Any(l => l.Contains($"{participantAliasse[i]}"));

                if (isReferenced)
                {
                    diagramData.Add($"participant \"{participants[i]}\" as {participantAliasse[i]} {(i == 0 ? Color.StartParticipant : Color.Participant)}");
                }
            }
            if (messages.Count > 0)
            {
                diagramData.Add("autonumber");
                diagramData.AddRange(messages);
            }
            return diagramData;
        }
        #endregion create sequence diagram

        #region create object and class diagrams with types
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

                    extend.Extends.ForEach(e => result.AddRange(CreateTypeHierachy([extend.Type!, e.Type!])));
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
        private static string CreateObjectName(Object obj)
        {
            var result = obj.GetType().Name.Replace("[]", "Array");

            return $"{result}_{obj.GetHashCode()}";
        }
        /// <summary>
        /// Creates a collection name for the specified object.
        /// </summary>
        /// <param name="obj">The object for which a collection name is to be created.</param>
        /// <returns>A string representing the collection name.</returns>
        private static string CreateCollectionName(object obj) => $"Colletion_{obj.GetHashCode()}";
        /// <summary>
        /// Creates an object diagram for the given objects up to a specified depth.
        /// </summary>
        /// <param name="maxDeep">The maximum depth of the object diagram.</param>
        /// <param name="objects">The objects to include in the object diagram.</param>
        /// <returns>The lines representing the object diagram.</returns>
        public static IEnumerable<string> CreateObjectDiagram(int maxDeep, params object[] objects)
        {
            var result = new List<string>();
            var createdObjects = new List<object>();
            void CreateMapStateRec(object[] objects, List<string> lines, int deep)
            {
                static void CreateMapObjectState(object obj, List<string> lines, int deep)
                {
                    string color = deep == 0 ? Color.RootObject : Color.Object;

                    lines.Add($"map {CreateObjectName(obj)}{color} " + "{");
                    lines.AddRange(CreateObjectState(obj).SetIndent(1));
                    lines.Add("}");
                }
                static void CreateMapCollectionState(IEnumerable collection, List<string> lines)
                {
                    lines.Add($"map {CreateCollectionName(collection)}{Color.Object} " + "{");
                    lines.AddRange(CreateCollectionState(collection).SetIndent(1));
                    lines.Add("}");
                }

                foreach (var obj in objects)
                {
                    if (createdObjects.Contains(obj) == false)
                    {
                        createdObjects.Add(obj);
                        CreateMapObjectState(obj, lines, deep);

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
        /// Analyzes the declaration syntax of a given semantic model and syntax node.
        /// </summary>
        /// <param name="semanticModel">The semantic model to analyze.</param>
        /// <param name="syntaxNode">The syntax node representing the declaration.</param>
        /// <param name="diagramData">The list to store the diagram data.</param>
        /// <param name="level">The indentation level for the diagram data.</param>
        public static void AnalyzeDeclarationSyntax(SemanticModel semanticModel, SyntaxNode syntaxNode, List<string> diagramData, int level)
        {
            static string GetVisibility(SyntaxTokenList modifiers, string defaultValue)
            {
                var result = defaultValue;

                if (modifiers.Any(SyntaxKind.PublicKeyword))
                {
                    result = "+";
                }
                else if (modifiers.Any(SyntaxKind.PrivateKeyword))
                {
                    result = "-";
                }
                else if (modifiers.Any(SyntaxKind.ProtectedKeyword))
                {
                    result = "#";
                }
                else if (modifiers.Any(SyntaxKind.InternalKeyword))
                {
                    result = "#";
                }
                return result;
            }
            static string ConvertModifiers(IEnumerable<SyntaxToken> modifiers)
            {
                string result = string.Empty;

                foreach (SyntaxToken modifier in modifiers)
                {
                    if (modifier.Text == "abstract")
                        result += "abstract";
                    else if (modifier.Text == "static")
                        result += "{static}";
                }
                return result;
            }
            static string ConvertFieldDeclaration(FieldDeclarationSyntax declaration)
            {
                var result = $"{GetVisibility(declaration.Modifiers, "-")}";
                var initValue = $"{declaration.Declaration.Variables.FirstOrDefault()?.Initializer?.Value}";

                if (declaration.Modifiers.Any(SyntaxKind.ConstKeyword))
                {
                    result += " {const}";
                }
                if (declaration.Modifiers.Any(SyntaxKind.AbstractKeyword))
                {
                    result += " {abstract}";
                }
                if (declaration.Modifiers.Any(SyntaxKind.StaticKeyword))
                {
                    result += " {static}";
                }
                result += $" {declaration.Declaration.Type}";
                result += $" {declaration.Declaration.Variables.First().Identifier.Text}";

                if (initValue.HasContent())
                {
                    result += $" = {initValue}";
                }
                return result;
            }
            static string[] ConvertPropertyDeclaration(PropertyDeclarationSyntax declaration)
            {
                var result = new List<string>();
                var visibility = GetVisibility(declaration.Modifiers, "-");
                var modifier = ConvertModifiers(declaration.Modifiers);

                if (declaration.AccessorList != null)
                {
                    foreach (AccessorDeclarationSyntax item in declaration.AccessorList!.Accessors)
                    {
                        var accessVisibility = GetVisibility(item.Modifiers, visibility);
                        var accessModifier = modifier;

                        if (item.Modifiers.Any())
                        {
                            accessModifier = ConvertModifiers(item.Modifiers);
                        }
                        if (item.Keyword.Text == "get")
                        {
                            result.Add($"{accessVisibility} {(accessModifier.HasContent() ? $"{accessModifier} " : string.Empty)}{declaration.Type} get{declaration.Identifier}()");
                        }
                        else if (item.Keyword.Text == "set")
                        {
                            result.Add($"{accessVisibility} {(accessModifier.HasContent() ? $"{accessModifier} " : string.Empty)}Void set{declaration.Identifier}({declaration.Type} value)");
                        }
                    }
                }
                else
                {
                    result.Add($"{visibility} {modifier} {declaration.Type} get{declaration.Identifier}()".Shrink(' '));
                }
                return [.. result];
            }
            static string ConvertMethodDeclaration(MethodDeclarationSyntax declaration)
            {
                var visibility = $"{GetVisibility(declaration.Modifiers, "-")}";
                var modifiers = ConvertModifiers(declaration.Modifiers);
                var parameterList = string.Empty;

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
                return $"{visibility} {modifiers} {declaration.ReturnType} {declaration.Identifier}({parameterList})".Shrink(' ');
            }

            if (syntaxNode is EnumDeclarationSyntax enumDeclaration)
            {
                string declaration = ConvertModifiers(enumDeclaration.Modifiers);

                declaration += $" enum {enumDeclaration.Identifier} {Color.Enum}" + " {";
                diagramData.Add(declaration);
                foreach (var member in enumDeclaration.Members)
                {
                    diagramData.Add($"{member.Identifier}");
                }
                diagramData.Add("}");
            }
            else if (syntaxNode is StructDeclarationSyntax structDeclaration)
            {
                string declaration = ConvertModifiers(structDeclaration.Modifiers);

                declaration += $" struct {structDeclaration.Identifier} {Color.Struct}" + " {";
                diagramData.Add(declaration);
                foreach (var member in structDeclaration.Members)
                {
                    AnalyzeDeclarationSyntax(semanticModel, member, diagramData, level + 1);
                }
                diagramData.Add("}");
            }
            else if (syntaxNode is InterfaceDeclarationSyntax interfaceDeclaration)
            {
                string declaration = ConvertModifiers(interfaceDeclaration.Modifiers);

                declaration += $" interface {interfaceDeclaration.Identifier} {Color.Interface}" + " {";
                diagramData.Add(declaration);
                foreach (var member in interfaceDeclaration.Members.Where(m => m is FieldDeclarationSyntax))
                {
                    AnalyzeDeclarationSyntax(semanticModel, member, diagramData, level + 1);
                }
                diagramData.Add("---");
                foreach (var member in interfaceDeclaration.Members.Where(m => m is PropertyDeclarationSyntax))
                {
                    AnalyzeDeclarationSyntax(semanticModel, member, diagramData, level + 1);
                }
                diagramData.Add("---");
                foreach (var member in interfaceDeclaration.Members.Where(m => m is MethodDeclarationSyntax))
                {
                    AnalyzeDeclarationSyntax(semanticModel, member, diagramData, level + 1);
                }
                diagramData.Add("}");
            }
            else if (syntaxNode is ClassDeclarationSyntax classDeclaration)
            {
                var declaration = ConvertModifiers(classDeclaration.Modifiers);
                var autoProperties = classDeclaration.Members.OfType<PropertyDeclarationSyntax>()
                                                     .Where(IsAutoProperty);
                var isStatic = declaration.Contains("{static}");
                var isAbstract = declaration.Contains("abstract");

                declaration = declaration.Replace("{static}", string.Empty);
                declaration += $" class {classDeclaration.Identifier}";
                declaration += isStatic ? $" << static >> " : " ";
                declaration += isAbstract ? $"{Color.AbstractClass}" : $"{Color.Class}";
                declaration += " {";
                diagramData.Add(declaration);

                foreach (var autoProperty in autoProperties)
                {
                    var modifier = ConvertModifiers(autoProperty.Modifiers);

                    diagramData.Add($"- {modifier} {autoProperty.Type} _{autoProperty.Identifier.Text.ToLower()}".Shrink(' '));
                }

                foreach (var member in classDeclaration.Members.Where(m => m is FieldDeclarationSyntax))
                {
                    AnalyzeDeclarationSyntax(semanticModel, member, diagramData, level + 1);
                }
                diagramData.Add("---");
                foreach (var member in classDeclaration.Members.Where(m => m is PropertyDeclarationSyntax))
                {
                    AnalyzeDeclarationSyntax(semanticModel, member, diagramData, level + 1);
                }
                diagramData.Add("---");
                foreach (var member in classDeclaration.Members.Where(m => m is MethodDeclarationSyntax))
                {
                    AnalyzeDeclarationSyntax(semanticModel, member, diagramData, level + 1);
                }
                diagramData.Add("}");

                if (classDeclaration.BaseList != null)
                {
                    foreach (var baseType in classDeclaration.BaseList.Types)
                    {
                        string identifierText;
                        var typeDeclaration = FindTypeDeclaration(semanticModel, baseType);

                        if (typeDeclaration != default)
                        {
                            AnalyzeDeclarationSyntax(semanticModel, typeDeclaration, diagramData, level + 1);
                        }

                        if (baseType.Type is IdentifierNameSyntax identifierName)
                        {
                            identifierText = identifierName.Identifier.Text;

                            diagramData.Add($"{classDeclaration.Identifier} <|-- {identifierText}");
                        }
                        else if (baseType.Type is GenericNameSyntax genericName)
                        {
                            identifierText = genericName.Identifier.Text;

                            diagramData.Add($"{classDeclaration.Identifier} <|-- {identifierText}");
                        }
                        else
                        {
                            identifierText = baseType.Type.ToString();

                            diagramData.Add($"{classDeclaration.Identifier} <|-- {identifierText}");
                        }

                        if (typeDeclaration != default)
                        {
                            if (identifierText.Length > 1 && identifierText[0] == 'I' && char.IsUpper(identifierText[1]))
                            {
                                diagramData.Add($"interface {identifierText} {Color.Interface}");
                            }
                            else
                            {
                                diagramData.Add($"class {identifierText} {Color.Class}");
                            }
                        }
                    }
                }
            }
            else if (syntaxNode is FieldDeclarationSyntax fieldDeclaration)
            {
                diagramData.Add(ConvertFieldDeclaration(fieldDeclaration));
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
        /// <summary>
        /// Analyzes the call sequence within a method declaration.
        /// </summary>
        /// <param name="semanticModel">The semantic model.</param>
        /// <param name="methodDeclaration">The method declaration syntax.</param>
        /// <param name="participantAliasse">The list of participant aliases.</param>
        /// <param name="messages">The list of messages.</param>
        /// <param name="level">The level of the call sequence.</param>
        public static void AnalyzeCallSequence(SemanticModel semanticModel, MethodDeclarationSyntax methodDeclaration, List<string> participantAliasse, List<string> messages, int level)
        {
            var methodResults = new Dictionary<string, string>();
            var statements = methodDeclaration?.Body?.Statements ?? [];

            foreach (StatementSyntax statement in statements!)
            {
                AnalyzeCallSequence(semanticModel, methodDeclaration!, statement, participantAliasse, messages, methodResults, level);
            }
        }
        /// <summary>
        /// Analyzes the call sequence within a method.
        /// </summary>
        /// <param name="semanticModel">The semantic model of the syntax tree.</param>
        /// <param name="methodDeclaration">The method declaration syntax node.</param>
        /// <param name="syntaxNode">The syntax node to analyze.</param>
        /// <param name="participantAliasse">The list of participant aliases.</param>
        /// <param name="messages">The list of messages.</param>
        /// <param name="methodResults">The dictionary of method results.</param>
        /// <param name="level">The current level of analysis.</param>
        public static void AnalyzeCallSequence(SemanticModel semanticModel, MethodDeclarationSyntax methodDeclaration, SyntaxNode syntaxNode, List<string> participantAliasse, List<string> messages, Dictionary<string, string> methodResults, int level)
        {
            if (syntaxNode is LocalDeclarationStatementSyntax localDeclarationStatement)
            {
                foreach (var variable in localDeclarationStatement.Declaration.Variables)
                {
                    AnalyzeCallSequence(semanticModel, methodDeclaration, variable, participantAliasse, messages, methodResults, level);
                }
            }
            else if (syntaxNode is VariableDeclaratorSyntax variableDeclarator)
            {
                if (variableDeclarator.Initializer != null)
                {
                    AnalyzeCallSequence(semanticModel, methodDeclaration, variableDeclarator.Initializer, participantAliasse, messages, methodResults, level);
                }

                foreach (var item in variableDeclarator.Initializer?.Value?.ChildNodes() ?? [])
                {
                    if (item is InvocationExpressionSyntax varInvocationExpression)
                    {
                        if (varInvocationExpression.ArgumentList?.Arguments.Count == 0)
                        {
                            foreach (var argument in varInvocationExpression.ArgumentList.Arguments)
                            {
                                AnalyzeCallSequence(semanticModel, methodDeclaration, argument, participantAliasse, messages, methodResults, level);
                            }
                        }
                    }
                }
            }
            else if (syntaxNode is InvocationExpressionSyntax invocationExpression)
            {
                var participantFrom = CreateParticipantAlias(methodDeclaration);
                var participantTo = CreateParticipantAlias(invocationExpression);

                if (participantAliasse.Contains(participantFrom)
                    && participantAliasse.Contains(participantTo))
                {
                    var argumentList = CreateArgumentList(invocationExpression, methodResults);

                    if (argumentList != "()")
                    {
                        messages.Add($"{participantFrom} -[#grey]> {participantTo} : {argumentList}".SetIndent(level));
                    }
                    else
                    {
                        messages.Add($"{participantFrom} -[#grey]> {participantTo}".SetIndent(level));
                    }

                    if (invocationExpression.Parent is AssignmentExpressionSyntax assignmentExpression)
                    {
                        var resultVariable = assignmentExpression.Left.ToString();

                        messages.Add($"{participantTo} -[#blue]-> {participantFrom} : {resultVariable}".SetIndent(level));
                    }
                    else if (invocationExpression.Parent is EqualsValueClauseSyntax equalsValueClause)
                    {
                        if (equalsValueClause.Parent is VariableDeclaratorSyntax equalsVariableDeclarator)
                        {
                            var equalsVariable = equalsVariableDeclarator.Identifier.Text;

                            messages.Add($"{participantTo} -[#blue]-> {participantFrom} : {equalsVariable}".SetIndent(level));
                        }
                    }
                    else if (invocationExpression.Parent is ReturnStatementSyntax)
                    {
                        var resultVariable = "result";

                        messages.Add($"{participantTo} -[#blue]-> {participantFrom} : {resultVariable}".SetIndent(level));
                    }
                    else
                    {
                        var symbolInfo = semanticModel.GetSymbolInfo(invocationExpression);

                        if (symbolInfo.Symbol is IMethodSymbol methodSymbol)
                        {
                            var result = methodSymbol.ReturnType.ToString();

                            if (result?.ToLower() != "void")
                            {
                                var resultVariable = "result";

                                messages.Add($"{participantTo} -[#blue]-> {participantFrom} : {resultVariable}".SetIndent(level));
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in invocationExpression.ChildNodes())
                    {
                        AnalyzeCallSequence(semanticModel, methodDeclaration, item, participantAliasse, messages, methodResults, level);
                    }
                }
            }
            else if (syntaxNode is ExpressionStatementSyntax expressionStatement)
            {
                AnalyzeCallSequence(semanticModel, methodDeclaration, expressionStatement.Expression, participantAliasse, messages, methodResults, level);
            }
            else if (syntaxNode is AssignmentExpressionSyntax assignmentExpression)
            {
                if (assignmentExpression.Right is InvocationExpressionSyntax rightExpression)
                {
                    AnalyzeCallSequence(semanticModel, methodDeclaration, rightExpression, participantAliasse, messages, methodResults, level);
                }
            }
            else if (syntaxNode is BinaryExpressionSyntax binaryExpression)
            {
                AnalyzeCallSequence(semanticModel, methodDeclaration, binaryExpression.Left, participantAliasse, messages, methodResults, level);
                AnalyzeCallSequence(semanticModel, methodDeclaration, binaryExpression.Right, participantAliasse, messages, methodResults, level);
            }
            else if (syntaxNode is DoStatementSyntax doStatement
                     && HasInvocationExpression(doStatement))
            {
                messages.Add($"loop#LightCoral {doStatement.Condition}".SetIndent(level));
                foreach (var item in doStatement.ChildNodes())
                {
                    AnalyzeCallSequence(semanticModel, methodDeclaration, item, participantAliasse, messages, methodResults, level + 1);
                }
                messages.Add("end".SetIndent(level));
            }
            else if (syntaxNode is WhileStatementSyntax whileStatement
                     && HasInvocationExpression(whileStatement))
            {
                messages.Add($"loop#LightCoral {whileStatement.Condition}".SetIndent(level));
                foreach (var item in whileStatement.ChildNodes())
                {
                    AnalyzeCallSequence(semanticModel, methodDeclaration, item, participantAliasse, messages, methodResults, level + 1);
                }
                messages.Add("end".SetIndent(level));
            }
            else if (syntaxNode is ForStatementSyntax forStatement
                     && HasInvocationExpression(forStatement))
            {
                messages.Add($"loop#LightCoral {forStatement.Condition}".SetIndent(level));
                foreach (var item in forStatement.ChildNodes())
                {
                    AnalyzeCallSequence(semanticModel, methodDeclaration, item, participantAliasse, messages, methodResults, level + 1);
                }
                messages.Add("end".SetIndent(level));
            }
            else if (syntaxNode is ForEachStatementSyntax forEachStatement
                     && HasInvocationExpression(forEachStatement))
            {
                messages.Add($"loop#LightCoral {forEachStatement.Expression}".SetIndent(level));
                foreach (var item in forEachStatement.ChildNodes())
                {
                    AnalyzeCallSequence(semanticModel, methodDeclaration, item, participantAliasse, messages, methodResults, level + 1);
                }
                messages.Add("end".SetIndent(level));
            }
            else if (syntaxNode is IfStatementSyntax ifStatement
                     && HasInvocationExpression(ifStatement))
            {
                messages.Add($"alt#LightBlue {ifStatement.Condition}".SetIndent(level));
                foreach (var item in ifStatement.ChildNodes())
                {
                    AnalyzeCallSequence(semanticModel, methodDeclaration, item, participantAliasse, messages, methodResults, level + 1);
                }
                messages.Add("end".SetIndent(level));
            }
            else if (syntaxNode is ElseClauseSyntax elseClause
                     && HasInvocationExpression(elseClause))
            {
                messages.Add($"else".SetIndent(level));
                foreach (var item in elseClause.ChildNodes())
                {
                    AnalyzeCallSequence(semanticModel, methodDeclaration, item, participantAliasse, messages, methodResults, level + 1);
                }
            }
            else
            {
                foreach (var item in syntaxNode.ChildNodes())
                {
                    AnalyzeCallSequence(semanticModel, methodDeclaration, item, participantAliasse, messages, methodResults, level);
                }
            }
        }

        private static bool HasInvocationExpression(SyntaxNode syntaxNode)
        {
            var result = syntaxNode.ChildNodes().OfType<InvocationExpressionSyntax>().Any();

            if (result == false)
            {
                var iterator = syntaxNode.ChildNodes().GetEnumerator();

                while (result == false && iterator.MoveNext())
                {
                    result = HasInvocationExpression(iterator.Current);
                }
            }
            return result;
        }
        /// <summary>
        /// Analyzes a syntax node and generates diagram data based on the type of the node.
        /// </summary>
        /// <param name="syntaxNode">The syntax node to analyze.</param>
        /// <param name="diagramData">The list to store the generated diagram data.</param>
        /// <param name="level">The indentation level for the generated diagram data.</param>
        public static void AnalyzeStatement(SyntaxNode syntaxNode, List<string> diagramData, int level)
        {
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
                string condition = string.Join(' ', ifStatement.Condition.ToString()
                                                               .Replace(Environment.NewLine, " ")
                                                               .Replace("\r", " ")
                                                               .Split(' ', StringSplitOptions.RemoveEmptyEntries));

                diagramData.Add($"if ({condition}) then ({yesLabel})".SetIndent(level));
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
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            else if (syntaxNode is BreakStatementSyntax breakStatement)
            {
                System.Diagnostics.Debug.WriteLine($"{nameof(breakStatement)} is known but not used!");
            }
#pragma warning disable IDE0059 // Unnecessary assignment of a value
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
                diagramData.Add($":{forEachStatement.Identifier} = iterator.Current();".SetIndent(level));

                AnalyzeStatement(forEachStatement.Statement, statements, level + 1);

                foreach (var statement in statements)
                {
                    diagramData.Add(statement.SetIndent(level + 1));
                }

                diagramData.Add($"endwhile ({noLabel})".SetIndent(level));
            }
            else if (syntaxNode is ReturnStatementSyntax returnStatement)
            {
                diagramData.Add($"{Color.Return}:return {returnStatement.Expression};".SetIndent(level));
            }
            else if (syntaxNode is ThrowStatementSyntax throwStatement)
            {
                diagramData.Add($"{Color.Throw}:throw {throwStatement.Expression};".SetIndent(level));
                diagramData.Add("kill".SetIndent(level));
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"{syntaxNode.GetType().Name} is unknown!");
            }
#pragma warning restore IDE0059 // Unnecessary assignment of a value
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
        public static IEnumerable<string> CreateObjectState(object obj)
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

            #region object is an array
            if (obj.GetType().IsArray)
            {
                var array = obj as Array;

                for (int i = 0; i < array!.Length; i++)
                {
                    result.Add($"{i} => {array.GetValue(i)}");
                }
            }
            #endregion object is an array

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
        /// <summary>
        /// Extracts UML items from the given lines.
        /// </summary>
        /// <param name="lines">The lines to extract UML items from.</param>
        /// <returns>An enumerable collection of UML items.</returns>
        private static List<UMLItem> ExtractUMLItems(IEnumerable<string> lines)
        {
            var result = new List<UMLItem>();
            var isItem = false;
            var umlItem = default(UMLItem);

            foreach (var line in lines)
            {
                if (isItem == false && line.Trim().EndsWith('{') && (line.Contains("class") || line.Contains("interface")))
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
                else if (isItem && umlItem != default && line.Trim().StartsWith('}') == false && line.Trim().StartsWith("stop", StringComparison.CurrentCultureIgnoreCase) == false)
                {
                    umlItem.Add(line);
                }
                else if (isItem && umlItem != default && line.Trim().StartsWith('}'))
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
        /// Updates the diagram path by deleting any diagram files that do not have a corresponding entry in the info files.
        /// </summary>
        /// <param name="path">The directory path where the diagram files and info files are located.</param>
        /// <param name="infoFileFilter">The filter pattern for the info files.</param>
        private static void UpdateDiagramPath(string path, string infoFileFilter)
        {
            var infoLines = new List<string>();
            var infoFiles = Directory.GetFiles(path, infoFileFilter, SearchOption.TopDirectoryOnly);

            foreach (var infoFile in infoFiles)
            {
                infoLines.AddRange(File.ReadAllLines(infoFile));
            }

            var files = Directory.GetFiles(path, PlantUMLExtension.Replace(".", "*."), SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);

                if (infoLines.Any(l => l.StartsWith($"fileName:{fileName}")) == false)
                {
                    File.Delete(file);
                }
            }
        }
        /// <summary>
        /// Finds the type declaration syntax node that matches the given base type syntax.
        /// </summary>
        /// <param name="semanticModel">The semantic model to use for type resolution.</param>
        /// <param name="baseTypeSyntax">The base type syntax to match.</param>
        /// <returns>The type declaration syntax node that matches the given base type syntax, or null if no match is found.</returns>
        private static TypeDeclarationSyntax? FindTypeDeclaration(SemanticModel semanticModel, BaseTypeSyntax baseTypeSyntax)
        {
            var result = default(TypeDeclarationSyntax);
            var typeDeclarations = semanticModel.SyntaxTree.GetRoot().DescendantNodes().OfType<TypeDeclarationSyntax>();

            if (baseTypeSyntax.Type is IdentifierNameSyntax identifierName)
            {
                string identifierText = identifierName.Identifier.Text;

                result = typeDeclarations.FirstOrDefault(t => t.Identifier.Text == identifierText);
            }
            return result;
        }
        /// <summary>
        /// Finds the method declaration syntax node for the given invocation expression.
        /// </summary>
        /// <param name="semanticModel">The semantic model.</param>
        /// <param name="invocation">The invocation expression syntax node.</param>
        /// <returns>The method declaration syntax node if found, otherwise null.</returns>
        private static MethodDeclarationSyntax? FindMethodDeclaration(SemanticModel semanticModel, InvocationExpressionSyntax invocation)
        {
            var result = default(MethodDeclarationSyntax);
            var symbolInfo = semanticModel.GetSymbolInfo(invocation).Symbol;

            if (symbolInfo != null && symbolInfo is IMethodSymbol methodSymbol)
            {
                var methodReferences = methodSymbol.DeclaringSyntaxReferences;

                foreach (var reference in methodReferences)
                {
                    if (reference.GetSyntax() is MethodDeclarationSyntax methodDeclaration)
                    {
                        result = methodDeclaration;
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Creates a participant string representation based on the provided method syntax.
        /// </summary>
        /// <param name="methodSyntax">The method syntax to create the participant for.</param>
        /// <returns>The participant string representation.</returns>
        private static string CreateParticipant(MethodDeclarationSyntax methodSyntax)
        {
            var parameters = methodSyntax.ParameterList?.Parameters ?? [];
            var paramsStatement = parameters.Any() ? $"({string.Join(",", parameters)})" : string.Empty;

            return $"{methodSyntax?.Identifier}{paramsStatement}".Replace($"{Environment.NewLine}", string.Empty);
//                                                                 .Replace(" ", string.Empty);
        }
        /// <summary>
        /// Creates an alias for a participant based on the given method syntax.
        /// </summary>
        /// <param name="methodSyntax">The method syntax to create the alias from.</param>
        /// <returns>The alias for the participant.</returns>
        private static string CreateParticipantAlias(MethodDeclarationSyntax methodSyntax)
        {
            var identifier = methodSyntax.Identifier;
            var parameters = methodSyntax.ParameterList?.Parameters ?? [];
            var result = $"{identifier}" + (parameters.Any() ? $"_{string.Join("_", parameters.Select((item, index) => $"p{index}"))}" : string.Empty);

            return result.Select(c => char.IsLetterOrDigit(c) ? c.ToString() : "_").Aggregate((a, b) => a + b).Shrink('_');
        }
        /// <summary>
        /// Creates a participant string based on the given invocation expression syntax.
        /// </summary>
        /// <param name="invocationSyntax">The invocation expression syntax.</param>
        /// <returns>The participant string.</returns>
        private static string CreateParticipant(InvocationExpressionSyntax invocationSyntax)
        {
            var arguments = invocationSyntax.ArgumentList?.Arguments ?? [];
            var argsStatement = arguments.Any() ? string.Join(",", arguments.Select((item, index) => $"a{index}")) : string.Empty;

            return $"{invocationSyntax?.Expression}({argsStatement})".Replace($"{Environment.NewLine}", string.Empty)
                                                                     .Replace(" ", string.Empty);
        }
        /// <summary>
        /// Creates an alias for a participant based on the given invocation expression.
        /// </summary>
        /// <param name="invocationExpression">The invocation expression to create the alias from.</param>
        /// <returns>The alias for the participant.</returns>
        private static string CreateParticipantAlias(InvocationExpressionSyntax invocationExpression)
        {
            var identifier = invocationExpression.Expression.ToString();
            var arguments = invocationExpression.ArgumentList?.Arguments ?? [];
            var result = $"{identifier}" + (arguments.Any() ? $"_{string.Join("_", arguments.Select((item, index) => $"a{index}"))}" : string.Empty);

            return result.Select(c => char.IsLetterOrDigit(c) ? c.ToString() : "_").Aggregate((a, b) => a + b).Shrink('_');
        }
        /// <summary>
        /// Creates an argument list based on the provided invocation expression and method results.
        /// </summary>
        /// <param name="invocationExpression">The invocation expression to extract arguments from.</param>
        /// <param name="methodResults">A dictionary containing method results.</param>
        /// <returns>The created argument list as a string.</returns>
        private static string CreateArgumentList(InvocationExpressionSyntax invocationExpression, Dictionary<string, string> methodResults)
        {
            var result = string.Empty;
            var arguments = invocationExpression.ArgumentList?.Arguments ?? [];

            foreach (var item in arguments)
            {
                if (result.Length > 0)
                {
                    result += ", ";
                }
                if (item.Expression is InvocationExpressionSyntax argumentInvocationExpression)
                {
                    var participant = CreateParticipantAlias(argumentInvocationExpression);

                    if (methodResults.TryGetValue(participant, out string? value))
                    {
                        result = $"{result}{value}";
                    }
                    else
                    {
                        result = $"{result}{item}";
                    }
                }
                else
                {
                    result = $"{result}{item}";
                }
            }
            return $"({result})";
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
        public static object? GetFieldValue(object obj, FieldInfo fieldInfo)
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
        public static string GetStateValue(object obj, FieldInfo fieldInfo)
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
        public static string GetStateValue(object obj, FieldInfo fieldInfo, int maxLength)
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

        /// <summary>
        /// Determines whether the specified property declaration is an auto-implemented property.
        /// </summary>
        /// <param name="propertydeclaration">The property declaration syntax.</param>
        /// <returns><c>true</c> if the property is an auto-implemented property; otherwise, <c>false</c>.</returns>
        private static bool IsAutoProperty(PropertyDeclarationSyntax propertydeclaration)
        {
            var accessorList = propertydeclaration.AccessorList;

            return accessorList != null
                && accessorList.Accessors.All(accessor => accessor.Body == null
                                              && accessor.ExpressionBody == null);
        }

        /// <summary>
        /// Reads custom UML lines from a file.
        /// </summary>
        /// <param name="filePath">The path of the file to read.</param>
        /// <returns>An array of custom UML lines read from the file.</returns>
        public static string[] ReadCustomUMLFromFle(string filePath)
        {
            var result = new List<string>();

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                var customUMLLines = ReadCustomUML(lines);

                if (customUMLLines.Any())
                {
                    customUMLLines.Insert(0, $"' {CustomUMLLabel}");
                    customUMLLines.Add($"' {CustomUMLLabel}");
                }
                result.AddRange(customUMLLines);
            }
            return result.ToArray();
        }
        /// <summary>
        /// Reads custom UML lines from the given collection of lines.
        /// </summary>
        /// <param name="lines">The collection of lines to read from.</param>
        /// <returns>A list of custom UML lines.</returns>
        private static List<string> ReadCustomUML(IEnumerable<string> lines)
        {
            var result = new List<string>();
            var counter = 0;

            foreach (var line in lines)
            {
                if (line.Contains($"{CustomUMLLabel}", StringComparison.CurrentCultureIgnoreCase))
                {
                    counter++;
                }
                else if (counter > 0 && counter % 2 > 0)
                {
                    result.Add(line);
                }
            }
            return result;
        }
        #endregion helpers
    }
}
//MdEnd
