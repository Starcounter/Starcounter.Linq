using System;
using System.Linq;
using System.Linq.Expressions;
using Starcounter.Linq.Visitors;

namespace Starcounter.Linq
{
    public interface IQueryContext
    {
        object Execute(Expression expression);
        object Execute<TResult>(Expression expression);
        string GetQuery(Expression expression);
    }

    public class QueryContext<T> : IQueryContext
    {
        public object Execute(Expression expression) => Execute<T>(expression);

        public object Execute<TResult>(Expression expression)
        {
            var query = new QueryBuilder<T>();

            RootVisitor<T>.Instance.Visit(expression, query);
            var sql = query.BuildSqlString();
            var variables = query.GetVariables();

            if (typeof(TResult).IsGenericType)
            {
                return Db.SlowSQL<T>(sql, variables);
            }
            var result = Db.SlowSQL(sql, variables).FirstOrDefault();
            var resultType = result.GetType();
            
            // SC lifts underlying types to a bigger ones in some cases.
            // Look at the issue https://github.com/Starcounter/Home/issues/209 for getting more info.
            if (resultType != typeof(TResult) && !resultType.IsSubclassOf(typeof(TResult)))
            {
                var expectedType = typeof(TResult);
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
            return (TResult) result;
        }


        public string GetQuery(Expression expression)
        {
            var query = new QueryBuilder<T>();

            RootVisitor<T>.Instance.Visit(expression, query);
            var sql = query.BuildSqlString();
            return sql;
        }
    }

    public class DummyQueryContext<T> : IQueryContext
    {
        public object Execute(Expression expression) => Execute<T>(expression);

        public object Execute<TResult>(Expression expression)
        {
            var query = new QueryBuilder<T>();

            RootVisitor<T>.Instance.Visit(expression,query);
            query.BuildSqlString();
            query.GetVariables();
            return null;
        }

        public string GetQuery(Expression expression)
        {
            var query = new QueryBuilder<T>();

            RootVisitor<T>.Instance.Visit(expression, query);
            var sql = query.BuildSqlString();
            return sql;
        }
    }
}
