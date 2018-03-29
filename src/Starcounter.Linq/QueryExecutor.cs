using System;
using System.Collections;
using System.Linq;
using Starcounter.Nova;

namespace Starcounter.Linq
{
    public class QueryExecutor<T> : IQueryExecutor
    {
        public object Execute<TResult>(string sql, object[] variables, QueryResultMethod queryResultMethod)
        {
            if (queryResultMethod == QueryResultMethod.Delete)
            {
                Db.SlowSQL(sql, variables);
                return null;
            }

            if (typeof(IEnumerable).IsAssignableFrom(typeof(TResult)))
            {
                return Db.SlowSQL<T>(sql, variables);
            }

            var result = GetQueryResult(sql, variables, queryResultMethod);
            if (result == null)
            {
                return default(TResult);
            }

            result = DeliftQueryResult(result, typeof(TResult));

            return (TResult)result;
        }

        public object GetQueryResult(string sql, object[] variables, QueryResultMethod queryResultMethod)
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

        /// <remarks>
        /// SC lifts underlying types to a bigger ones in some cases.
        /// Look at the issue https://github.com/Starcounter/Home/issues/209 for getting more info.
        /// </remarks>
        public object DeliftQueryResult(object result, Type expectedType)
        {
            var resultType = result.GetType();
            if (resultType != expectedType && !resultType.IsSubclassOf(expectedType))
            {
                if (expectedType == typeof(int))
                {
                    return Convert.ToInt32(result);
                }
                if (expectedType == typeof(long))
                {
                    return Convert.ToInt64(result);
                }
                if (expectedType == typeof(decimal))
                {
                    return Convert.ToDecimal(result);
                }
                if (expectedType == typeof(double))
                {
                    return Convert.ToDouble(result);
                }
                if (expectedType == typeof(uint))
                {
                    return Convert.ToUInt32(result);
                }
                if (expectedType == typeof(ulong))
                {
                    return Convert.ToUInt64(result);
                }
                if (expectedType == typeof(bool))
                {
                    return Convert.ToBoolean(result);
                }
                if (expectedType == typeof(string))
                {
                    return Convert.ToString(result);
                }
                if (expectedType == typeof(float))
                {
                    return Convert.ToSingle(result);
                }
            }
            return result;
        }
    }
}