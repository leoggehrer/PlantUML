//@BaseCode
//MdStart

using PlantUML.Logic.Extensions;

namespace PlantUML.ConApp
{
    /// <summary>
    /// Represents an abstract console application.
    /// </summary>
    public abstract partial class ConsoleApplication : Application
    {
        #region menuitem
        /// Represents a menu item.
        /// Gets or sets the unique key of the menu item.
        /// Gets or sets the non-unique optional key of the menu item.
        /// Gets or sets the displayed text of the menu item.
        /// Gets or sets the action to be performed when the menu item is selected.
        protected partial class MenuItem
        {
            /// <summary>
            /// Gets or sets the key.
            /// </summary>
            public required string Key { get; set; }
            /// <summary>
            /// Gets or sets the optional key.
            /// </summary>
            public string OptionalKey  { get; set; } = string.Empty;
            /// <summary>
            /// Gets or sets the text.
            /// </summary>
            public required string Text { get; set; }
            /// <summary>
            /// Gets or sets the action associated with the property.
            /// </summary>
            public required Action Action { get; set; }
        }
        #endregion menuitem

        #region console-properties
        /// <summary>
        /// Gets or sets the foreground color of the console.
        /// </summary>
        public static ConsoleColor ForegroundColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }
        /// <summary>
        /// Clears the console screen.
        /// </summary>
        public static void Clear() => Console.Clear();
        /// <summary>
        /// Prints the specified message to the console.
        /// </summary>
        /// <param name="message">The message to be printed.</param>
        /// <returns>The length of the printed message.</returns>
        public static int Print(string message)
        {
            Console.Write(message);
            return message.Length;
        }

        /// <summary>
        /// Prints a message with a specified character and length.
        /// </summary>
        /// <param name="chr">The character to print.</param>
        /// <param name="message">The message to print.</param>
        /// <param name="length">The desired length of the output.</param>
        public static void Print(char chr, string message, int length)
        {
            for (int i = message.Length; i < length; i++)
            {
                Console.Write(chr);
            }
            Console.Write(message);
        }
        /// <summary>
        /// Prints a new line to the console.
        /// </summary>
        public static void PrintLine()
        {
            Console.WriteLine();
        }
        /// <summary>
        /// Prints a message to the console and returns the length of the message.
        /// </summary>
        /// <param name="message">The message to be printed.</param>
        /// <returns>The length of the message.</returns>
        public static int PrintLine(string message)
        {
            Console.WriteLine(message);
            return message.Length;
        }
        /// <summary>
        /// Prints a line consisting of a specified character repeated a specified number of times.
        /// </summary>
        /// <param name="chr">The character to be repeated.</param>
        /// <param name="count">The number of times the character should be repeated.</param>
        /// <returns>The length of the printed line.</returns>
        public static int PrintLine(char chr, int count)
        {
            string message = new string(chr, count);

            Console.WriteLine(message);
            return message.Length;
        }
        /// <summary>
        /// Reads a line of input from the console.
        /// </summary>
        /// <returns>The line of input read from the console, or an empty string if no input is available.</returns>
        public static string ReadLine()
        {
            return Console.ReadLine() ?? string.Empty;
        }
        /// <summary>
        /// Reads a line of input from the console with the specified message.
        /// </summary>
        /// <param name="message">The message to display before reading the input.</param>
        /// <returns>The line of input read from the console.</returns>
        public static string ReadLine(string message)
        {
            Print(message);
            return ReadLine();
        }
        /// <summary>
        /// Gets the current cursor position in the console.
        /// </summary>
        /// <returns>A tuple containing the left and top position of the cursor.</returns>
        public static (int Left, int Top) GetCursorPosition()
        {
            return (Console.CursorLeft, Console.CursorTop);
        }
        /// <summary>
        /// Sets the position of the cursor in the console window.
        /// </summary>
        /// <param name="left">The column position of the cursor.</param>
        /// <param name="top">The row position of the cursor.</param>
        public static void SetCursorPosition(int left, int top)
        {
            Console.SetCursorPosition(left, top);
        }
        #endregion console-properties

        #region progressbar-properties
        /// <summary>
        /// Gets or sets the foreground color of the console.
        /// </summary>
        /// <value>
        /// The foreground color of the console.
        /// </value>
        public static ConsoleColor ProgressBarForegroundColor { get; set; } = ForegroundColor;
        /// <summary>
        /// Gets or sets a value indicating whether the RunBusyProgress is active or not.
        /// </summary>
        private static bool RunProgressBar { get; set; }
        /// <summary>
        /// Indicates whether printing is allowed when the application is busy.
        /// </summary>
        public static bool CanProgressBarPrint { get; set; } = true;
        #endregion progressbar-properties

        #region app-properties
        /// <summary>
        /// Gets or sets a value indicating whether the application should continue running.
        /// </summary>
        protected static bool RunApp { get; set; } = false;
        #endregion app-properties

        #region progressbar-methods
        /// <summary>
        /// Prints a busy progress indicator in the console.
        /// </summary>
        public static void StartProgressBar()
        {
            static void Write(int cursorLeft, int cursorTop, string output)
            {
                var saveCursorTop = Console.CursorTop;
                var saveCursorLeft = Console.CursorLeft;
                var saveForeColor = Console.ForegroundColor;

                ForegroundColor = ConsoleColor.Green;
                SetCursorPosition(cursorLeft, cursorTop);
                Print(output);
                SetCursorPosition(saveCursorLeft, saveCursorTop);
                ForegroundColor = saveForeColor;
            }
            if (RunProgressBar == false)
            {
                var head = '>';
                var runSign = '=';
                var counter = 0;

                RunProgressBar = true;
                PrintLine();

                var (Left, Top) = GetCursorPosition();

                PrintLine();
                PrintLine();
                Task.Factory.StartNew(async () =>
                {
                    while (RunProgressBar)
                    {
                        if (CanProgressBarPrint)
                        {
                            if (Left > 60)
                            {
                                var timeInSec = counter / 5;

                                for (int i = 0; i <= Left; i++)
                                {
                                    Write(i, Top, " ");
                                }
                                Left = 0;
                            }
                            else
                            {
                                Write(Left++, Top, $"{runSign}{head}");
                            }

                            if (counter % 5 == 0)
                            {
                                Write(65, Top, $"{counter / 5,5} [sec]");
                            }
                        }
                        counter++;
                        await Task.Delay(200);
                    }
                });
            }
        }

        /// <summary>
        /// Stops the execution of the busy progress.
        /// </summary>
        public static void StopProgressBar()
        {
            RunProgressBar = false;
        }
        #endregion progressbar-methods

        #region abstract-methods
        protected abstract void PrintHeader(string sourcePath);
        protected abstract MenuItem[] CreateMenuItems();
        protected abstract void PrintFooter();
        #endregion abstract-methods

        #region main-method
        public virtual void Run(string[] args)
        {
            var choose = default(string[]);
            var saveForeColor = Console.ForegroundColor;

            RunApp = true;
            do
            {
                var menuItems = CreateMenuItems();

                Clear();
                ForegroundColor = ProgressBarForegroundColor;
                PrintHeader(SolutionPath);
                menuItems.ForEach(m => PrintLine($"[{m.Key,3}] {m.Text}"));
                PrintFooter();

                choose = ReadLine().ToLower().Split(',', StringSplitOptions.RemoveEmptyEntries);
                var chooseIterator = choose.GetEnumerator();

                ForegroundColor = saveForeColor;
                while (RunApp && chooseIterator.MoveNext())
                {
                    var actions = menuItems.Where(m => m.Key.Equals(chooseIterator.Current) || m.OptionalKey.Equals(chooseIterator.Current));
                    var actioIterator = actions.GetEnumerator();

                    while (RunApp && actioIterator.MoveNext())
                    {
                        actioIterator.Current?.Action();
                    }
                    RunApp = chooseIterator.Current.Equals("x") == false;
                    StopProgressBar();
                }
            } while (RunApp);
        }
        #endregion main-method

        #region file and path methods
        /// <summary>
        /// Changes the source path for the solution.
        /// </summary>
        public static void ChangeSolutionPath()
        {
            PrintLine();
            Print("Enter solution path: ");
            var newPath = ReadLine();

            if (Directory.Exists(newPath))
            {
                SolutionPath = newPath;
            }
        }
        /// <summary>
        /// Changes the source path for the application.
        /// </summary>
        public static void ChangeSourcePath()
        {
            PrintLine();
            Print("Enter source path: ");
            var newPath = ReadLine();

            if (Directory.Exists(newPath))
            {
                SourcePath = newPath;
            }
        }
        #endregion file and path methods
    }
}
//MdEnd
