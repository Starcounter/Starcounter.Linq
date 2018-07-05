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
        private static readonly string SourceName = QueryType.SourceName();
        // ReSharper disable once StaticMemberInGenericType
        private static readonly string QueryTypeName = SqlHelper.EscapeIdentifiers(QueryType.FullName);
        // Precompute the from clause for this <T>
        // ReSharper disable once StaticMemberInGenericType
        private static readonly string FromAlias = $" FROM {QueryTypeName} {QueryType.SourceName()}";
        private static readonly string From = $" FROM {QueryTypeName}";

        private static Type UnwrapQueryType(Type type)
        {
            if (!type.IsConstructedGenericType) return type;
            return type.GetGenericArguments().First();
        }

        public QueryResultMethod ResultMethod { get; set; } = QueryResultMethod.FirstOrDefault;

        public Expression AllMethodExpression { get; set; }

        private StringBuilder Select { get; } = new StringBuilder();
        private StringBuilder Where { get; } = new StringBuilder();
        private StringBuilder OrderBy { get; } = new StringBuilder();

        private List<object> Variables { get; } = new List<object>();

        public string FetchPart { get; private set; }
        public string OffsetPart { get; private set; }

        public void Fetch(int count) => FetchPart = " FETCH " + count;
        public void Offset(int count) => OffsetPart = " OFFSET " + count;

        public void BeginWhereSection()
        {
            if (Where.Length > 0)
            {
                WriteWhere(" AND (");
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

        public void WriteWhere(string text) => Where.Append(text);

        public void WriteOrderBy(string text) => OrderBy.Append(text);

        public void WriteSelect(string text) => Select.Append(text);

        public void WriteAggregationSelect(string aggregation)
        {
            OrderBy.Clear();
            Select.Append(aggregation);
        }

        public void WriteAggregationSelect(string @operator, Action writeOperand)
        {
            OrderBy.Clear();
            Select.Append(@operator);
            Select.Append("(");
            writeOperand();
            Select.Append(")");
        }

        public string GetSourceName()
        {
            return SourceName;
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

                if (Select.Length == 0)
                {
                    stringBuilder.Append(SourceName); //SELECT TEntity
                }
                else
                {
                    stringBuilder.Append(Select); //SELECT AVG(a.b.c)
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