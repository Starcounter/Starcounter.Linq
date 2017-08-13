using System.Linq.Expressions;

namespace Starcounter.Linq.Raw
{
    public interface IQueryContext
    {
        object Execute(Expression expression);
        string GetQuery(Expression expression);
    }

    public class ScQueryContext<T> : IQueryContext
    {
        public object Execute(Expression expression)
        {
            var query = new QueryBuilder<T>();

            RootVisitor<T>.Instance.Visit(expression, query);
            var sql = query.BuildSqlString();
            var variables = query.GetVariables();
            return Db.SQL<T>(sql, variables); ;
        }


        public string GetQuery(Expression expression)
        {
            var query = new QueryBuilder<T>();

            RootVisitor<T>.Instance.Visit(expression, query);
            var sql = query.BuildSqlString();
            return sql;
        }
    }

    public class QueryContext<T> : IQueryContext
    {
        public object Execute(Expression expression)
        {
            var query = new QueryBuilder<T>();

            RootVisitor<T>.Instance.Visit(expression,query);
            var sql = query.BuildSqlString();
            var vars = query.GetVariables();
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
