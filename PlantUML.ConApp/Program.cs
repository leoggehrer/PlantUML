//@BaseCode
//MdStart
namespace PlantUML.ConApp
{
    /// <summary>
    /// Represents the entry point of the application.
    /// </summary>
    public partial class Program
    {
        public static void Main(string[] args)
        {
            PlantUMLApp app = new();

            app.Run(args);
        }
    }
}
//MdEnd