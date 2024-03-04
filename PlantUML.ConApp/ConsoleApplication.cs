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
        /// <summary>
        /// Represents a menu item in a console application.
        /// </summary>
        /// Represents a menu item.
        /// <remarks
        /// Gets or sets the unique key of the menu item.
        /// Gets or sets the non-unique optional key of the menu item.
        /// Gets or sets the displayed text of the menu item.
        /// Gets or sets the action to be performed when the menu item is selected.
        /// </remarks>
        protected partial class MenuItem
        {
            /// <summary>
            /// Gets or sets a value indicating whether the item is displayed.
            /// </summary>
            public bool IsDisplayed { get; set; } = true;
            /// <summary>
            /// Gets or sets the key.
            /// </summary>
            public required string Key { get; set; }

            /// <summary>
            /// Gets or sets the optional key.
            /// </summary>
            public string OptionalKey { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the text.
            /// </summary>
            public required string Text { get; set; }

            /// <summary>
            /// Gets or sets the action associated with the property.
            /// </summary>
            public required Action<MenuItem> Action { get; set; }

            /// <summary>
            /// Gets or sets the parameters for the console application.
            /// </summary>
            public Dictionary<string, object> Params { get; set; } = new Dictionary<string, object>();

            /// <summary>
            /// Gets or sets the foreground color of the console.
            /// </summary>
            public ConsoleColor ForegroundColor { get; set; } = ConsoleApplication.ForegroundColor;
        }
        #endregion menuitem

        #region properties
        /// <summary>
        /// Gets or sets the foreground color of the console.
        /// </summary>
        public static ConsoleColor ForegroundColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }
        #endregion properties

        #region console-methods
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
            string message = new(chr, count);

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

        /// <summary>
        /// Converts the given label and text into a formatted label text.
        /// </summary>
        /// <param name="label">The label to be displayed.</param>
        /// <param name="text">The text to be displayed.</param>
        /// <returns>The formatted label text.</returns>
        public static string ToLabelText(string label, string text)
        {
            return ToLabelText(label, text, 20, '.');
        }
        /// <summary>
        /// Formats a label and text into a single string with a specified width and padding character.
        /// </summary>
        /// <param name="label">The label to be displayed.</param>
        /// <param name="text">The text to be displayed.</param>
        /// <param name="width">The total width of the resulting string.</param>
        /// <param name="chr">The character used for padding.</param>
        /// <returns>A formatted string with the label and text.</returns>
        public static string ToLabelText(string label, string text, int width, char chr)
        {
            var diff = width - label.Length;
            var space = new string(chr, Math.Max(0, diff));

            return $"{label}{space}{text}";
        }
        #endregion console-methods

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
        /// Gets or sets the menu items.
        /// </summary>
        protected MenuItem[] MenuItems { get; set; } = [];
        /// <summary>
        /// Gets or sets a value indicating whether the application should continue running.
        /// </summary>
        protected bool RunApp { get; set; } = false;
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
                            if (Left > 65)
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
        /// <summary>
        /// Prints the header.
        /// </summary>
        protected abstract void PrintHeader();
        /// <summary>
        /// Creates an array of menu items.
        /// </summary>
        /// <returns>An array of <see cref="MenuItem"/> objects.</returns>
        protected abstract MenuItem[] CreateMenuItems();
        /// <summary>
        /// Creates the exit menu items for the application.
        /// </summary>
        /// <returns>An array of MenuItem objects representing the exit menu items.</returns>
        protected virtual MenuItem[] CreateExitMenuItems()
        {
            return [ new()
                     {
                        Key = "===",
                        Text = new string('=', 65),
                        Action = (self) => { },
                        ForegroundColor = ConsoleColor.DarkGreen,
                     },

                     new()
                     {
                        Key = "x|X",
                        Text = ToLabelText("Exit", "Exits the application"),
                        Action = (self) => { RunApp = false; },
                     },
            ];
        }
        /// <summary>
        /// Prints the footer.
        /// </summary>
        protected abstract void PrintFooter();
        /// <summary>
        /// Prints the screen with the menu items.
        /// </summary>
        protected virtual void PrintScreen()
        {
            var saveForegrondColor = ForegroundColor;

            MenuItems = CreateMenuItems();
            Clear();
            ForegroundColor = saveForegrondColor;
            PrintHeader();
            MenuItems.Where(mi => mi.IsDisplayed).ForEach(m =>
            {
                ForegroundColor = m.ForegroundColor;
                PrintLine($"[{m.Key,3}] {m.Text}");
            });
            ForegroundColor = saveForegrondColor;
            PrintFooter();
        }
        #endregion abstract-methods

        #region main-method
        /// <summary>
        /// This method is called before the execution of the program's main logic.
        /// </summary>
        /// <param name="args">The command-line arguments passed to the program.</param>
        protected virtual void BeforeRun(string[] args) { }
        /// <summary>
        /// This method is called before the execution of the main logic.
        /// </summary>
        protected virtual void BeforeExecution() { }
        /// <summary>
        /// Runs the console application.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public virtual void Run(string[] args)
        {
            var choose = default(string[]);
            var saveForegrondColor = ForegroundColor;

            BeforeRun(args);
            RunApp = true;
            do
            {
                PrintScreen();

                choose = ReadLine().ToLower().Split(',', StringSplitOptions.RemoveEmptyEntries);
                var chooseIterator = choose.GetEnumerator();

                BeforeExecution();
                ForegroundColor = saveForegrondColor;
                while (RunApp && chooseIterator.MoveNext())
                {
                    var actions = MenuItems.Where(m => m.Key.Equals(chooseIterator.Current) || m.OptionalKey.Equals(chooseIterator.Current));
                    var actionIterator = actions.GetEnumerator();

                    while (RunApp && actionIterator.MoveNext())
                    {
                        actionIterator.Current?.Action(actionIterator.Current);
                    }
                    RunApp = RunApp && chooseIterator.Current.Equals("x") == false;
                    StopProgressBar();
                }
                AfterExecution();
            } while (RunApp);
            AfterRun();
        }
        /// <summary>
        /// This method is called after the execution of the main logic in the derived class.
        /// </summary>
        protected virtual void AfterExecution() { }
        /// <summary>
        /// This method is called after the execution of the Run method.
        /// </summary>
        protected virtual void AfterRun() { }
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
        /// <summary>
        /// Changes the path for the application.
        /// </summary>
        public static string ChangePath(string path)
        {
            return ChangePath("Enter path: ", path);
        }
        /// <summary>
        /// Changes the path for the application.
        /// </summary>
        public static string ChangePath(string title, string path)
        {
            PrintLine();
            Print(title);
            var newPath = ReadLine();

            if (Directory.Exists(newPath))
            {
                path = newPath;
            }
            return path;
        }
        /// <summary>
        /// Changes the template solution path based on the current path and a list of query paths.
        /// </summary>
        /// <param name="currentPath">The current path.</param>
        /// <param name="queryPaths">The query paths.</param>
        /// <returns>The updated solution path.</returns>
        public static string ChangeTemplateSolutionPath(string currentPath, params string[] queryPaths)
        {
            var result = currentPath;
            var solutionPath = GetCurrentSolutionPath();
            var qtSolutionPaths = new List<string>();
            var saveForeColor = ForegroundColor;

            queryPaths.ForEach(qp => TemplatePath.GetTemplateSolutions(qp).ForEach(s => qtSolutionPaths.Add(s)));

            if (qtSolutionPaths.Contains(solutionPath) == false && solutionPath != currentPath)
            {
                qtSolutionPaths.Add(solutionPath);
            }

            var qtSelectSolutions = qtSolutionPaths.Distinct().OrderBy(e => e).ToArray();

            for (int i = 0; i < qtSelectSolutions.Length; i++)
            {
                if (i == 0)
                    PrintLine();

                ForegroundColor = i % 2 == 0 ? ConsoleColor.DarkYellow : saveForeColor;
                PrintLine($"[{i + 1,3}] Change path to: {qtSelectSolutions[i]}");
            }
            ForegroundColor = saveForeColor;
            PrintLine();
            Print("Select or enter source path: ");
            var selectOrPath = ReadLine();

            if (int.TryParse(selectOrPath, out int number))
            {
                if ((number - 1) >= 0 && (number - 1) < qtSelectSolutions.Length)
                {
                    result = qtSelectSolutions[number - 1];
                }
            }
            else if (string.IsNullOrEmpty(selectOrPath) == false
                     && Directory.Exists(selectOrPath))
            {
                result = selectOrPath;
            }
            return result;
        }
        /// <summary>
        /// Selects or changes the current path to a subpath based on the provided query paths.
        /// </summary>
        /// <param name="currentPath">The current path.</param>
        /// <param name="queryPaths">The query paths.</param>
        /// <returns>The selected or changed path.</returns>
        public static string SelectOrChangeToSubPath(string currentPath, params string[] queryPaths)
        {
            var result = currentPath;
            var solutionPath = GetCurrentSolutionPath();
            var qtSolutionPaths = new List<string>();
            var saveForeColor = ForegroundColor;

            queryPaths.ForEach(qp => TemplatePath.GetSubPaths(qp).ForEach(s => qtSolutionPaths.Add(s)));

            if (qtSolutionPaths.Contains(solutionPath) == false && solutionPath != currentPath)
            {
                qtSolutionPaths.Add(solutionPath);
            }

            var qtSelectSolutions = qtSolutionPaths.Distinct().OrderBy(e => e).ToArray();

            for (int i = 0; i < qtSelectSolutions.Length; i++)
            {
                if (i == 0)
                    PrintLine();

                ForegroundColor = i % 2 == 0 ? ConsoleColor.DarkYellow : saveForeColor;
                PrintLine($"[{i + 1,3}] Change path to: {qtSelectSolutions[i]}");
            }
            ForegroundColor = saveForeColor;
            PrintLine();
            Print("Select or enter target path: ");
            var selectOrPath = ReadLine();

            if (Int32.TryParse(selectOrPath, out int number))
            {
                if ((number - 1) >= 0 && (number - 1) < qtSelectSolutions.Length)
                {
                    result = qtSelectSolutions[number - 1];
                }
            }
            else if (string.IsNullOrEmpty(selectOrPath) == false
                     && Directory.Exists(selectOrPath))
            {
                result = selectOrPath;
            }
            return result;
        }
        #endregion file and path methods
    }
}
//MdEnd
