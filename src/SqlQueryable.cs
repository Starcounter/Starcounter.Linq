using System.Linq;
using System.Linq.Expressions;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;

namespace Starcounter.Linq
{
    public class SqlQueryable<T> : QueryableBase<T>
    {
        // This constructor is called by our users, create a new IQueryExecutor.
        public SqlQueryable() : base(QueryParser.CreateDefault(), new SqlQueryExecutor())
        {
        }


        // This constructor is called indirectly by LINQ's query methods, just pass to base.
        public SqlQueryable(IQueryProvider provider, Expression expression) : base(provider, expression)
        {
        }
    }
}