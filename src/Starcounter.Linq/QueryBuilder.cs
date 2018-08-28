using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Starcounter.Linq.Helpers;

//String interpolation, string.Join, string.Format should be avoided due to performance overhead
namespace Starcounter.Linq
{
    internal class QueryBuilder<TEntity>
    {
        private static readonly Type QueryType = UnwrapQueryType(typeof(TEntity));
        // ReSharper disable once StaticMemberInGenericType
        private static readonly string SourceAliasName = QueryType.SourceName();
        // ReSharper disable once StaticMemberInGenericType
        private static readonly string QueryTypeName = SqlHelper.EscapeIdentifiers(QueryType.FullName);

        // Precompute the from clause for this <T>
        // ReSharper disable once StaticMemberInGenericType
        private static readonly string FromAlias = $" FROM {QueryTypeName} {SourceAliasName}";
        // ReSharper disable once StaticMemberInGenericType
        private static readonly string From = $" FROM {QueryTypeName}";

        public QueryResultMethod ResultMethod { get; set; } = QueryResultMethod.FirstOrDefault;

        public Expression AllMethodExpression { get; set; }

        private StringBuilder Where { get; } = new StringBuilder();
        private StringBuilder OrderBy { get; } = new StringBuilder();
        private StringBuilder SelectPath { get; } = new StringBuilder(SourceAliasName);
        private string SelectAggregation { get; set; }

        private List<object> Variables { get; } = new List<object>();

        public string FetchPart { get; private set; }
        public string OffsetPart { get; private set; }

        private static Type UnwrapQueryType(Type type)
        {
            if (!type.IsConstructedGenericType) return type;
            return type.GetGenericArguments().First();
        }

        public void Fetch(int count) => FetchPart = " FETCH " + count;
        public void Offset(int count) => OffsetPart = " OFFSET " + count;

        public void BeginWhereSection()
        {
            WriteWhere(Where.Length > 0 ? " AND (" : "(");
        }
        public void EndWhereSection()
        {
            WriteWhere(")");
        }

        public void WriteWhere(string text) => Where.Append(text);

        public void WriteWhereObjectNo()
        {
            Where.Append(SourceAliasName);
            Where.Append(".\"ObjectNo\"");
        }

        public void WriteOrderByObjectNo()
        {
            OrderBy.Append(SourceAliasName);
            OrderBy.Append(".\"ObjectNo\"");
        }

        public void WriteOrderBy(string text) => OrderBy.Append(text);

        public void AppendSelectPath(string text)
        {
            SelectPath.Append(text);
        }

        public void SetAggregation(string @operator)
        {
            OrderBy.Clear();
            SelectAggregation = @operator;
        }

        public string GetSource()
        {
            return SelectPath.ToString();
        }

        public string BuildSqlString()
        {
            //Why this structure?
            //Formatting strings is slow, appending is fast.
            //Avoid string.Format and string interpolation
            //None of this matters much for compiled queries, but is highly relevant for standard ad-hoc linq

            var stringBuilder = new StringBuilder();

            if (ResultMethod == QueryResultMethod.Delete)
            {
                stringBuilder.Append("DELETE");
                stringBuilder.Append(From);
            }
            else
            {
                stringBuilder.Append("SELECT ");
                if (SelectAggregation != null)
                {
                    stringBuilder.Append(SelectAggregation);
                    stringBuilder.Append("(");
                }

                stringBuilder.Append(SelectPath);

                if (SelectAggregation != null)
                {
                    stringBuilder.Append(")");
                }
                stringBuilder.Append(FromAlias);
            }

            if (Where.Length > 0)
            {
                stringBuilder.Append(" WHERE ");
                stringBuilder.Append(Where);
            }

            if (OrderBy.Length > 0)
            {
                stringBuilder.Append(" ORDER BY ");
                stringBuilder.Append(OrderBy);
            }

            if (FetchPart != null)
            {
                stringBuilder.Append(FetchPart);
            }

            if (OffsetPart != null)
            {
                stringBuilder.Append(OffsetPart);
            }

            return stringBuilder.ToString();
        }

        public void AddVariable(object value) => Variables.Add(value);

        public object[] GetVariables() => Variables.ToArray();

        public void BeginOrderBySection()
        {
            if (OrderBy.Length > 0)
            {
                WriteOrderBy(", ");
            }
        }

        public void EndOrderBySection(bool asc) => WriteOrderBy(asc ? " ASC" : " DESC");
    }
}