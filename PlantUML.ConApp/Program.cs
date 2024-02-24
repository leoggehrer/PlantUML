//@BaseCode
//MdStart
namespace PlantUML.ConApp
{
    /// <summary>
    /// Represents the entry point of the application.
    /// </summary>
    internal partial class Program
    {
        static void Main(string[] args)
        {
            PlantUMLApp app = new();

            app.Run(args);
        }
    }
}
//MdEnd