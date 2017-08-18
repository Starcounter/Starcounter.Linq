using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Starcounter.Linq
{
    public class QueryBuilder<T>
    {
        private static readonly Type QueryType = UnwrapQueryType(typeof(T));
        // ReSharper disable once StaticMemberInGenericType
        private static readonly string SourceName = QueryType.SourceName();
        // ReSharper disable once StaticMemberInGenericType
        private static readonly string QueryTypeName = QueryType.FullName;
        // Precompute the from clause for this <T>
        // ReSharper disable once StaticMemberInGenericType
        private static readonly string From = $" FROM {QueryTypeName} {QueryType.SourceName()}";



        private static Type UnwrapQueryType(Type type)
        {
            if (!type.IsConstructedGenericType) return type;
            return type.GetGenericArguments().First();
        }

        private StringBuilder WhereParts { get; } = new StringBuilder();
        private StringBuilder OrderByParts { get; } = new StringBuilder();

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

        public void WriteOrderBy(string text) => OrderByParts.Append(text);


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
            stringBuilder.Append(From);

            if (WhereParts.Length > 0)
            {
                stringBuilder.Append(" WHERE ");
                stringBuilder.Append(WhereParts);
            }

            if (OrderByParts.Length > 0)
            {
                stringBuilder.Append(" ORDER BY ");
                stringBuilder.Append(OrderByParts);
            }

            //if (FetchPart != null)
            //{
            //    stringBuilder.Append(FetchPart);
            //}

            return stringBuilder.ToString();
        }

        public void AddVariable(object value) => Variables.Add(value);

        public object[] GetVariables() => Variables.ToArray();

        public void BeginOrderBySection()
        {
            if (OrderByParts.Length > 0)
            {
                WriteOrderBy(", ");
            }
        }

        public void EndOrderBySection(bool asc) => WriteOrderBy(asc ? " ASC" : " DESC");
    }

    public class SeparatedStringBuilder
    {
        public static string Build(string s, IEnumerable<string> orderings) => string.Join(s, orderings);
    }
}