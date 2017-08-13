using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Starcounter.Linq.Raw
{
    public class Queryable<T> : IOrderedQueryable<T>
    {
        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            var res =  Provider.Execute<IEnumerable<T>>(predicate);
            if (res == null)
                return default(T);
            return res.FirstOrDefault();
        }

        public Queryable(IQueryContext queryContext)
        {
            Initialize(new QueryProvider(queryContext), null);
        }

        public Queryable(IQueryProvider provider)
        {
            Initialize(provider, null);
        }

        internal Queryable(IQueryProvider provider, Expression expression)
        {
            Initialize(provider, expression);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Provider.Execute<IEnumerable<T>>(Expression)?.GetEnumerator() ??
                   Enumerable.Empty<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Provider.Execute<IEnumerable>(Expression).GetEnumerator();
        }

        public Type ElementType => typeof(T);

        public Expression Expression { get; private set; }
        public IQueryProvider Provider { get; private set; }

        private void Initialize(IQueryProvider provider, Expression expression)
        {
            if (expression != null && !typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
                throw new ArgumentException(
                    $"Not assignable from {expression.Type}", nameof(expression));

            Provider = provider ?? throw new ArgumentNullException(nameof(provider));
            Expression = expression ?? Expression.Constant(this);
        }
    }
}