using System.Text;

namespace Starcounter.Linq.Helpers
{
    public static class SqlHelper
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
    }
}