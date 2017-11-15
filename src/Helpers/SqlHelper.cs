using System.Linq;
using System.Text;

namespace Starcounter.Linq.Helpers
{
    public static class SqlHelper
    {
        private static readonly string[] ReservedWords =
        {
            "ALL", "AND", "AS", "ASC", "AVG",
            "BY", "BINARY",
            "CAST", "COUNT", "CREATE", "CROSS",
            "DATE", "DATETIME", "DELETE", "DESC", "DISTINCT",
            "ESCAPE", "EXISTS",
            "FALSE", "FETCH", "FIRST", "FIXED", "FORALL", "FROM", "FULL",
            "GROUP",
            "HAVING",
            "IN", "INDEX", "INNER", "INSERT", "IS",
            "JOIN",
            "LEFT", "LIKE", "LIMIT",
            "MAX", "MIN",
            "NOT", "NULL",
            "OBJ", "OBJECT", "OFFSET", "OFFSETKEY", "ON", "ONLY",
            "OPTION", "OR", "ORDER", "OUT", "OUTER", "OUTPUT",
            "PROC", "PROCEDURE",
            "RANDOM", "RIGHT", "ROWS",
            "SELECT", "STARTS", "SUM",
            "TIME", "TIMESTAMP", "TRUE",
            "UNIQUE", "UNKNOWN", "UPDATE",
            "VALUES", "VAR", "VARIABLE",
            "WHEN", "WHERE", "WITH"
        };

        /// <summary>
        /// Escapes single identifier if applicable
        /// </summary>
        public static string EscapeIfReservedWord(string word)
        {
            return ReservedWords.Contains(word.ToUpperInvariant()) ? $"\"{word}\"" : word;
        }

        /// <summary>
        /// Escapes parts of compound identifier when applicable.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string EscapeReservedWords(string word)
        {
            var parts = word.Split('.');
            if (parts.Length == 1)
            {
                return EscapeIfReservedWord(word);
            }

            var sb = new StringBuilder();
            foreach (var identifier in parts)
            {
                if (ReservedWords.Contains(identifier.ToUpperInvariant()))
                {
                    sb.Append('"');
                    sb.Append(identifier);
                    sb.Append('"');
                }
                else
                {
                    sb.Append(identifier);
                }
                sb.Append('.');
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }
}