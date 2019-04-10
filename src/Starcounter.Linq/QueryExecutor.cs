using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Starcounter.Linq.Helpers;

namespace Starcounter.Linq
{
    public class QueryExecutor<T> : IQueryExecutor
    {
        private readonly Type contextType = typeof(T);

        public object Execute<TResult>(string sql, object[] variables, QueryResultMethod queryResultMethod, AggregationOperation? aggregation)
        {
            if (queryResultMethod == QueryResultMethod.Delete)
            {
                Db.SlowSQL(sql, variables);
                return null;
            }

            Type resultType = typeof(TResult);

            if (typeof(IEnumerable).IsAssignableFrom(resultType))
            {
                Type resultItemType = resultType.GetGenericArguments().FirstOrDefault();
                if (resultItemType != null)
                {
                    Type qpExpectedType;
                    switch (aggregation)
                    {
                        case AggregationOperation.Count:
                            qpExpectedType = typeof(long);
                            break;
                        case AggregationOperation.Max:
                        case AggregationOperation.Min:
                        case AggregationOperation.Sum:
                            if (resultItemType == typeof(sbyte) || resultItemType == typeof(short) || resultItemType == typeof(int))
                                qpExpectedType = typeof(long);
                            else if (resultItemType == typeof(float))
                                qpExpectedType = typeof(double);
                            else if (resultItemType == typeof(byte) || resultItemType == typeof(ushort) || resultItemType == typeof(uint))
                                qpExpectedType = typeof(ulong);
                            else
                                qpExpectedType = resultItemType;
                            break;
                        case AggregationOperation.Average:
                            qpExpectedType = typeof(decimal);
                            break;
                        case null:
                            qpExpectedType = resultItemType;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(aggregation), aggregation, null);
                    }
                    MethodInfo dbSqlMethod = ReflectionHelper.DbSlowSqlMethodBase.MakeGenericMethod(qpExpectedType);
                    object queryResult = dbSqlMethod.Invoke(null, new object[] { sql, variables });
                    MethodInfo castItemsMethod = ReflectionHelper.GetEnumerableCastMethod(resultItemType);
                    return (TResult)castItemsMethod.Invoke(null, new[] { queryResult });
                }
                return Db.SlowSQL<T>(sql, variables);
            }

            var result = GetQueryResult(sql, variables, queryResultMethod);
            if (result == null)
            {
                return default(TResult);
            }

            result = DeliftQueryResult(result, resultType);

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