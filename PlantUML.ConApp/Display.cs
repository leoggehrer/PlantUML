//@BaseCode
//MdStart
namespace PlantUML.ConApp
{
    internal static class Display
    {
        public static ConsoleColor ForegroundColor 
        { 
            get => Console.ForegroundColor; 
            set => Console.ForegroundColor = value; 
        }

        public static int Print(string message)
        {
            Console.Write(message);
            return message.Length;
        }

        public static void PrintLine()
        {
            Console.WriteLine();
        }
        public static int PrintLine(string message)
        {
            Console.WriteLine(message);
            return message.Length;
        }
        public static int PrintLine(char chr, int count)
        {
            string message = new string(chr, count);

            Console.WriteLine(message);
            return message.Length;
        }
    }
}
//MdEnd