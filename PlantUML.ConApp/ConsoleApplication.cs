//@BaseCode
//MdStart

using PlantUML.Logic.Extensions;

namespace PlantUML.ConApp
{
    public abstract partial class ConsoleApplication : Application
    {
        #region menuitem
        /// Represents a menu item.
        /// Gets or sets the unique key of the menu item.
        /// Gets or sets the displayed text of the menu item.
        /// Gets or sets the action to be performed when the menu item is selected.
        protected partial class MenuItem
        {
            /// <summary>
            /// Gets or sets the key.
            /// </summary>
            /// <value>
            /// The key value.
            /// </value>
            public required string Key { get; set; }
            /// <summary>
            /// Gets or sets the text.
            /// </summary>
            /// <value>
            /// The text.
            /// </value>
            public required string Text { get; set; }
            /// <summary>
            /// Gets or sets the action associated with the property.
            /// </summary>
            /// <value>
            /// The action associated with the property.
            /// </value>
            public required Action Action { get; set; }
        }
        #endregion menuitem

        #region console-properties
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
        #endregion console-properties

        #region progressbar-properties
        /// <summary>
        /// Gets or sets a value indicating whether the RunBusyProgress is active or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the RunBusyProgress is active; otherwise, <c>false</c>.
        /// </value>
        private static bool RunBusyProgress { get; set; }
        /// <summary>
        /// Indicates whether printing is allowed when the application is busy.
        /// </summary>
        /// <value>
        /// true if printing is allowed when the application is busy; otherwise, false.
        /// </value>
        public static bool CanBusyPrint { get; set; } = true;
        /// <summary>
        /// Gets or sets the foreground color of the console.
        /// </summary>
        /// <value>
        /// The foreground color of the console.
        /// </value>
        public static ConsoleColor ProgressBarForegroundColor { get; set; } = ForegroundColor;
        #endregion progressbar-properties

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

                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(cursorLeft, cursorTop);
                Console.Write(output);
                Console.SetCursorPosition(saveCursorLeft, saveCursorTop);
                Console.ForegroundColor = saveForeColor;
            }
            if (RunBusyProgress == false)
            {
                var head = '>';
                var runSign = '=';
                var counter = 0;

                RunBusyProgress = true;
                Console.WriteLine();

                var (Left, Top) = Console.GetCursorPosition();

                Console.WriteLine();
                Console.WriteLine();
                Task.Factory.StartNew(async () =>
                {
                    while (RunBusyProgress)
                    {
                        if (CanBusyPrint)
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
            RunBusyProgress = false;
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
            var running = true;
            var input = default(string?);
            var saveForeColor = Console.ForegroundColor;

            do
            {
                var menuItems = CreateMenuItems();

                Console.Clear();
                ForegroundColor = ProgressBarForegroundColor;
                PrintHeader(SolutionPath);
                menuItems.ForEach(m => Console.WriteLine($"[{m.Key}] {m.Text}"));
                PrintFooter();

                input = Console.ReadLine()?.ToLower() ?? String.Empty;
                ForegroundColor = saveForeColor;
                menuItems.FirstOrDefault(m => m.Key.Equals(input))?.Action();
                running = input.Equals("x") == false;
                StopProgressBar();
            } while (running);
        }
        #endregion main-method
    }
}
//MdEnd