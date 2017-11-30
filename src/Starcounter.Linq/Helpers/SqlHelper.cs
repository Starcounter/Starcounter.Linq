using System;
using System.Text;

namespace Starcounter.Linq.Helpers
{
    internal static class SqlHelper
    {
        /// <summary>
        /// Escape single identifier.
        /// </summary>
        public static string EscapeSingleIdentifier(string word)
        {
            return $"\"{word}\"";
        }

        /// <summary>
        /// Escapes parts of compound identifier.
        /// </summary>
        public static string EscapeIdentifiers(string word)
        {
            var parts = word.Split('.');
            if (parts.Length == 1)
            {
                return EscapeSingleIdentifier(word);
            }

            var sb = new StringBuilder();
            foreach (var identifier in parts)
            {
                sb.Append('"');
                sb.Append(identifier);
                sb.Append('"');
                sb.Append('.');
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        //This returns the source alias for a type.
        //e.g FROM Person P
        public static string SourceName(this Type self) => self.Name[0].ToString();
    }
}