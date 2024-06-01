//@BaseCode
//MdStart
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection;

namespace PlantUML.Logic
{
    /// <summary>
    /// Represents a C# analyzer that provides various methods to query type, class, and method declarations in a C# source code.
    /// </summary>
    public partial class CSharpAnalyzer
    {
        #region properties
        /// <summary>
        /// Gets or sets the parse options for C# code.
        /// </summary>
        public CSharpParseOptions ParseOptions { get; private set; }
        /// <summary>
        /// Gets or sets the syntax tree associated with the C# code.
        /// </summary>
        public SyntaxTree SyntaxTree { get; private set; }
        /// <summary>
        /// Gets or sets the reference to the Portable Executable (PE) file that contains the core library (mscorlib.dll).
        /// </summary>
        public PortableExecutableReference MSCoreLibReference { get; private set; }
        /// <summary>
        /// Gets or sets the reference to the Portable Executable (PE) file.
        /// </summary>
        public PortableExecutableReference SystemCoreLibReference { get; private set; }
        /// <summary>
        /// Gets or sets the C# compilation used by the analyzer.
        /// </summary>
        public CSharpCompilation Compilation { get; private set; }
        /// <summary>
        /// Gets or sets the semantic model associated with the C# analyzer.
        /// </summary>
        public SemanticModel SemanticModel { get; private set; }
        #endregion properties

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpAnalyzer"/> class.
        /// </summary>
        /// <param name="source">The C# source code to analyze.</param>
        /// <param name="defines">Optional preprocessor symbols.</param>
        public CSharpAnalyzer(string source, params string[] defines)
        {
            ParseOptions = new CSharpParseOptions().WithPreprocessorSymbols(defines);
            SyntaxTree = CSharpSyntaxTree.ParseText(source, ParseOptions);
            MSCoreLibReference = MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location);
            SystemCoreLibReference = MetadataReference.CreateFromFile(typeof(Enumerable).GetTypeInfo().Assembly.Location);
            Compilation = CSharpCompilation.Create("Compilation", syntaxTrees: [SyntaxTree], references: [MSCoreLibReference, SystemCoreLibReference]);
            SemanticModel = Compilation.GetSemanticModel(SyntaxTree);
        }
        #endregion constructors

        #region methods
        /// <summary>
        /// Queries and returns an array of TypeDeclarationSyntax nodes from the syntax tree.
        /// </summary>
        /// <returns>An array of TypeDeclarationSyntax nodes.</returns>
        public TypeDeclarationSyntax[] QueryTypeDeclarations()
        {
            var syntaxRoot = SyntaxTree.GetRoot();
            var result = syntaxRoot.DescendantNodes().OfType<TypeDeclarationSyntax>();

            return result.ToArray();
        }

        /// <summary>
        /// Queries and returns an array of ClassDeclarationSyntax objects from the syntax tree.
        /// </summary>
        /// <returns>An array of ClassDeclarationSyntax objects.</returns>
        public ClassDeclarationSyntax[] QueryClassDeclarations()
        {
            var syntaxRoot = SyntaxTree.GetRoot();
            var result = syntaxRoot.DescendantNodes().OfType<ClassDeclarationSyntax>();

            return result.ToArray();
        }

        /// <summary>
        /// Queries and returns an array of all method declarations in the code.
        /// </summary>
        /// <returns>An array of <see cref="MethodDeclarationSyntax"/> representing the method declarations.</returns>
        public MethodDeclarationSyntax[] QueryMethodDeclarations()
        {
            var result = new List<MethodDeclarationSyntax>();
            var classNodes = QueryClassDeclarations();

            foreach (var classNode in classNodes)
            {
                var methodNodes = classNode.DescendantNodes().OfType<MethodDeclarationSyntax>();

                result.AddRange(methodNodes);
            }
            return result.ToArray();
        }
        #endregion methods
    }
}
//MdEnd