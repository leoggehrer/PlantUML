using System.Text;

namespace PlantUML.Logic.Extensions
{
    public static partial class StringExtensions
    {
        private static readonly string Indent = "  ";
        /// <summary>
        /// Indicates whether the specified string has a content.
        /// </summary>
        /// <param name="source">The string to test.</param>
        /// <returns>true if the value parameter is not null and not empty; otherwise, false.</returns>
        public static bool HasContent(this string? source)
        {
            return !string.IsNullOrEmpty(source);
        }

        /// <summary>
        /// Diese Methode fuegt einem Text n Einzuege hinzu.
        /// </summary>
        /// <param name="text">Text, dem die Einzuege hinzugefuegt werden.</param>
        /// <param name="count">Anzahl der Einzuege die dem Text hinzugefuegt werden.</param>
        /// <returns>Text mit Anzahl von Einzuegen.</returns>
        public static string SetIndent(this string text, int count)
        {
            StringBuilder sb = new();

            if (text != null)
            {
                for (int i = 0; i < count; i++)
                    sb.Append(Indent);        // spaces for one indent.
            }
            sb.Append(text);
            return sb.ToString();
        }

        /// <summary>
        /// This method adds n indents to a string array.
        /// </summary>
        /// <param name="lines">String array to which the indents are added.</param>
        /// <param name="count">Number of indents that are added to the string array.</param>
        /// <returns>String array with number of indents.</returns>
        public static string[] SetIndent(this string[] lines, int count)
        {
            if (lines != null)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    lines[i] = lines[i].SetIndent(count);
                }
            }
            return lines ?? [];
        }
        /// <summary>
        /// Sets the indent of each line in the specified collection of strings.
        /// </summary>
        /// <param name="lines">The collection of strings to set the indent for.</param>
        /// <param name="count">The indent count for each line.</param>
        /// <returns>A new collection of strings with the specified indent applied.</returns>
        public static IEnumerable<string> SetIndent(this IEnumerable<string> lines, int count)
        {
            return lines.ToArray().SetIndent(count);
        }

        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at a specified character position and ends before the first occurrence of a specified substring.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <param name="index">The zero-based starting character position of the substring.</param>
        /// <param name="text">A <see cref="string"/> used as the delimiter for the end of the substring.</param>
        /// <returns>A <see cref="string"/> that is equivalent to the substring that begins at <paramref name="index"/> and ends before the first occurrence of <paramref name="text"/>, or <see cref="string.Empty"/> if <paramref name="text"/> is not found.</returns>
        public static string Substring(this string source, int index, string text)
        {
            var result = string.Empty;
            var ofIdx = source.IndexOf(text);

            if (ofIdx >= 0 && ofIdx - index >= 0)
            {
                result = source.Substring(index, ofIdx);
            }
            return result;
        }

        /// <summary>
        /// Extracts a substring from a string (excludes from to).
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="from">Starttext</param>
        /// <param name="to">Endtext</param>
        /// <returns>The substring.</returns>
        public static string Partialstring(this string text, string from, string to)
        {
            var result = default(string);

            if (text.HasContent())
            {
                int f = text.IndexOf(from);
                int t = text.IndexOf(to, f + 1) + to.Length - 1;

                result = text.Partialstring(f, t);
            }
            return result ?? String.Empty;
        }

        /// <summary>
        /// Extracts a substring from a string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="from">Startposition</param>
        /// <param name="to">Endposition</param>
        /// <returns>The substring.</returns>
        public static string Partialstring(this string text, int from, int to)
        {
            StringBuilder sb = new();

            if (string.IsNullOrEmpty(text) == false)
            {
                for (int i = from; i >= 0 && i <= to && i < text.Length; i++)
                {
                    sb.Append(text[i]);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Extracts a substring from a string (includes from to).
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="from">Starttext</param>
        /// <param name="to">Endtext</param>
        /// <returns>The substring.</returns>
        public static string Betweenstring(this string text, string from, string to)
        {
            var result = default(string);

            if (text.HasContent())
            {
                int f = text.IndexOf(from) + from.Length;
                int t = text.IndexOf(to, f + 1) - 1;

                result = text.Partialstring(f, t);
            }
            return result ?? String.Empty;
        }
    }
}
