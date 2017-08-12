using System;
using System.Collections.Generic;
using System.Text;
using Remotion.Linq.Clauses;

namespace Starcounter.Linq
{
    public class QueryBuilder
    {
        public QueryBuilder()
        {
            FromParts = new List<string>();
            WhereParts = new List<string>();
            OrderByParts = new List<string>();
        }

        public string SelectPart { get; set; }
        private List<string> FromParts { get; }
        private List<string> WhereParts { get; }
        private List<string> OrderByParts { get; }

        public string FetchPart { get; set; }

        public void AddFromPart(IQuerySource querySource)
        {
            FromParts.Add($"{GetEntityName(querySource)} {querySource.ItemName()}");
        }

        public void AddWherePart(string formatString, params object[] args)
        {
            WhereParts.Add(string.Format(formatString, args));
        }

        public void AddOrderByPart(IEnumerable<string> orderings)
        {
            OrderByParts.Insert(0, SeparatedStringBuilder.Build(", ", orderings));
        }

        public string BuildSqlString()
        {
            var stringBuilder = new StringBuilder();

            if (string.IsNullOrEmpty(SelectPart) || FromParts.Count == 0)
                throw new InvalidOperationException("A query must have a select part and at least one from part.");

            stringBuilder.Append($"SELECT {SelectPart}");
            stringBuilder.Append($" FROM {SeparatedStringBuilder.Build(", ", FromParts)}");

            if (WhereParts.Count > 0)
                stringBuilder.Append($" WHERE {SeparatedStringBuilder.Build(" AND ", WhereParts)}");

            if (OrderByParts.Count > 0)
                stringBuilder.Append($" ORDER BY {SeparatedStringBuilder.Build(", ", OrderByParts)}");

            if (FetchPart != null)
                stringBuilder.Append($" {FetchPart}");

            return stringBuilder.ToString();
        }

        private static string GetEntityName(IQuerySource querySource)
        {
            return querySource.ItemType.FullName;
        }
    }

    public class SeparatedStringBuilder
    {
        public static string Build(string s, IEnumerable<string> orderings)
        {
            return string.Join(s, orderings);
        }
    }
}