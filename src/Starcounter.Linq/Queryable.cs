using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Starcounter.Linq
{
    public class Queryable<T> : IOrderedQueryable<T>
    {
        public Type ElementType => typeof(T);

        public Expression Expression { get; }

        public IQueryProvider Provider { get; }

        public Queryable(IQueryProvider provider) : this(provider, null)
        {
        }

        public Queryable(IQueryProvider provider, Expression expression)
        {
            if (expression != null && !typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
                throw new ArgumentException($"Not assignable from {expression.Type}", nameof(expression));

            Provider = provider ?? throw new ArgumentNullException(nameof(provider));
            Expression = expression ?? Expression.Constant(this);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Provider.Execute<IEnumerable<T>>(Expression)?.GetEnumerator() ?? Enumerable.Empty<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => Provider.Execute<IEnumerable>(Expression).GetEnumerator();

        //Why an instance impl of FirstOrDefault instead of default extension method?
        //The extension method creates a new expression based on the old expression + a call to first or default
        //This costs a lot of cycles, we simply bypass this and translate the base expression here
        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            var res = Provider.Execute<IEnumerable<T>>(predicate);
            if (res == null)
            {
                return default(T);
            }
            return res.FirstOrDefault();
        }

        public T FirstOrDefault()
        {
            var res = Provider.Execute<IEnumerable<T>>(Expression.Empty());
            if (res == null)
            {
                return default(T);
            }
            return res.FirstOrDefault();
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return InternalAny(predicate);
        }

        public bool Any()
        {
            return InternalAny(Expression.Empty());
        }

        private bool InternalAny(Expression predicate)
        {
            var res = Provider.Execute<IEnumerable<T>>(predicate);
            return res != null && res.Any();
        }
    }
}