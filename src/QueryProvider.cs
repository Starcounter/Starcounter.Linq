using System.Linq;
using System.Linq.Expressions;

namespace Starcounter.Linq
{
    public class QueryProvider : IQueryProvider
    {
        private readonly IQueryContext _queryContext;

        public QueryProvider(IQueryContext queryContext)
        {
            _queryContext = queryContext;
        }

        //Multi-method magic, this is faster than `MakeGenericType` + Activator.CreateInstance
        public virtual IQueryable CreateQuery(Expression expression) => CreateQuery<dynamic>(expression);

        public virtual IQueryable<T> CreateQuery<T>(Expression expression) => new Queryable<T>(this, expression);

        object IQueryProvider.Execute(Expression expression) => _queryContext.Execute(expression);

        T IQueryProvider.Execute<T>(Expression expression) => (T) _queryContext.Execute<T>(expression);
    }
}