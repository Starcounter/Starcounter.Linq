using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Starcounter.Linq.Raw
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
        public QueryBuilder()
        {
            
        }

        private StringBuilder WhereParts { get; } = new StringBuilder();
        private List<string> OrderByParts { get; } = new List<string>();

        private List<object> Variables { get; } = new List<object>();


        public string FetchPart { get; set; }

        public void Fetch(int count)
        {
            FetchPart = $" FETCH {count}";
        }



        //public void AddFromPart(IQuerySource querySource)
        //{
        //    FromPart.Add($"{GetEntityName(querySource)} {querySource.ItemName()}");
        //}

        public void AddWherePart(string formatString)
        {
            WhereParts.Append(formatString);
        }

        public void AddWherePart(string formatString, params object[] args)
        {
            WhereParts.AppendFormat(formatString, args);
        }


        public void AddOrderByPart(IEnumerable<string> orderings)
        {
            OrderByParts.Insert(0, SeparatedStringBuilder.Build(", ", orderings));
        }

        public string BuildSqlString()
        {
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
                stringBuilder.Append($" ORDER BY {SeparatedStringBuilder.Build(", ", OrderByParts)}");

            //if (FetchPart != null)
            //{
            //    stringBuilder.Append(FetchPart);
            //}

            return stringBuilder.ToString();
        }

        public void AddVariable(object value)
        {
            Variables.Add(value);
        }

        public object[] GetVariables()
        {
            return Variables.ToArray();
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