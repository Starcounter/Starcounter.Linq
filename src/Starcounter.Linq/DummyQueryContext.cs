using System.Linq.Expressions;
using Starcounter.Linq.Visitors;

namespace Starcounter.Linq
{
    internal class DummyQueryContext<T> : IQueryContext
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

        public TranslatedQuery GetQuery(Expression expression)
        {
            var query = new QueryBuilder<T>();

            RootVisitor<T>.Instance.Visit(expression, query);
            var sql = query.BuildSqlString();
            return new TranslatedQuery
            {
                SqlStatement = sql,
                ResultMethod = query.ResultMethod
            };
        }
    }
}
