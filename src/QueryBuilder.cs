using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Starcounter.Linq
{
    public class QueryBuilder<T>
    {
        public static Type QueryType = UnwrapQueryType(typeof(T));

        private static Type UnwrapQueryType(Type type)
        {
            if (type.IsConstructedGenericType)
            {
                var t = type.GetGenericArguments().First();
                return t;
            }
            return type;
        }

        // ReSharper disable once StaticMemberInGenericType
        public static string SourceName = QueryType.SourceName();
        // ReSharper disable once StaticMemberInGenericType
        public static string QueryTypeName = QueryType.FullName;

        private StringBuilder WhereParts { get; } = new StringBuilder();
        private List<string> OrderByParts { get; } = new List<string>();

        private List<object> Variables { get; } = new List<object>();


        public string FetchPart { get; set; }

        public void Fetch(int count) => FetchPart = $" FETCH {count}";

        public void BeginWhereSection()
        {
            if (WhereParts.Length > 0)
            {
                WriteWhere("AND (");
            }
            else
            {
                WriteWhere("(");
            }
        }

        public void EndWhereSection()
        {
            WriteWhere(")");
        }
        public void WriteWhere(string text) => WhereParts.Append(text);

        public void WriteWhere(string formatString, params object[] args) => WhereParts.AppendFormat(formatString, args);


        public void AddOrderByPart(IEnumerable<string> orderings)
        {
            OrderByParts.Insert(0, SeparatedStringBuilder.Build(", ", orderings));
        }

        public string BuildSqlString()
        {
            //Why this structure?
            //Formatting strings is slow, appending is fast.
            //Avoid string.Format and string interpolation
            //None of this matters much for compiled queries, but is highly relevant for standard ad-hoc linq

            var stringBuilder = new StringBuilder();

            stringBuilder.Append("SELECT ");
            stringBuilder.Append(SourceName);
            stringBuilder.Append(" FROM ");
            stringBuilder.Append(QueryTypeName);
            stringBuilder.Append(" ");
            stringBuilder.Append(SourceName);

            if (WhereParts.Length > 0)
            {
                stringBuilder.Append(" WHERE ");
                stringBuilder.Append(WhereParts);
            }

            if (OrderByParts.Count > 0)
            {
                stringBuilder.Append($" ORDER BY {SeparatedStringBuilder.Build(", ", OrderByParts)}");
            }

            //if (FetchPart != null)
            //{
            //    stringBuilder.Append(FetchPart);
            //}

            return stringBuilder.ToString();
        }

        public void AddVariable(object value) => Variables.Add(value);

        public object[] GetVariables() => Variables.ToArray();
    }

    public class SeparatedStringBuilder
    {
        public static string Build(string s, IEnumerable<string> orderings) => string.Join(s, orderings);
    }
}