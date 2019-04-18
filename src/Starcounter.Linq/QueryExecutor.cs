using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Starcounter.Linq.Helpers;

namespace Starcounter.Linq
{
    public class QueryExecutor<T> : IQueryExecutor
    {
        public object Execute<TResult>(string sql, object[] variables, QueryResultMethod queryResultMethod, AggregationOperation? aggregation)
        {
            if (queryResultMethod == QueryResultMethod.Delete)
            {
                Db.SlowSQL(sql, variables);
                return null;
            }
            Type resultType = typeof(TResult);

            if (resultType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(resultType))
            {
                Type resultItemType = resultType.GetGenericArguments().FirstOrDefault();
                if (resultItemType != null)
                {
                    IEnumerable<object> queryResult = Db.SlowSQL(sql, variables);
                    MethodInfo castItemsMethod = ReflectionHelper.GetEnumerableCastMethod(resultItemType);
                    return (TResult)castItemsMethod.Invoke(null, new object[] { queryResult });
                }

                return Db.SlowSQL<T>(sql, variables);
            }

            var result = Query(sql, variables, queryResultMethod);
            if (result == null)
            {
                return default(TResult);
            }

            return (TResult)CastHelper.Convert(result, resultType);
        }

        public object Query(string sql, object[] variables, QueryResultMethod queryResultMethod)
        {
            var queryResult = Db.SlowSQL(sql, variables);
            switch (queryResultMethod)
            {
                case QueryResultMethod.FirstOrDefault:
                    return queryResult.FirstOrDefault();
                case QueryResultMethod.First:
                    return queryResult.First();
                case QueryResultMethod.SingleOrDefault:
                    return queryResult.SingleOrDefault();
                case QueryResultMethod.Single:
                    return queryResult.Single();
                case QueryResultMethod.Any:
                    return queryResult.Any();
                case QueryResultMethod.All:
                    throw new NotSupportedException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(queryResultMethod), queryResultMethod, null);
            }
        }
    }
}
