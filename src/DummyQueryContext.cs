using System.Linq.Expressions;
using Starcounter.Linq.Visitors;

namespace Starcounter.Linq
{
    public interface IQueryContext
    {
        object Execute(Expression expression);
        string GetQuery(Expression expression);
    }

    public class QueryContext<T> : IQueryContext
    {
        public object Execute(Expression expression)
        {
            var query = new QueryBuilder<T>();

            RootVisitor<T>.Instance.Visit(expression, query);
            var sql = query.BuildSqlString();
            var variables = query.GetVariables();
            return Db.SlowSQL<T>(sql, variables); ;
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
        public object Execute(Expression expression)
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
