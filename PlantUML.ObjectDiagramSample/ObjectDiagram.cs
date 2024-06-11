namespace PlantUML.ObjectDiagramSample
{
    internal class ObjectDiagram
    {
        #region fields
        private static PlantUML.Logic.ObjectDiagramCreator diagramCreator = new("od_Stack.puml");
        #endregion fields

        #region properties
        #endregion properties

        #region constructors
        #endregion constructors

        #region methods
        public static void Create(object obj, params string[] notes)
        {
            diagramCreator.CreateToFile(obj, notes);
        }
        #endregion methods
    }
}
