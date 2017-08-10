using System.Linq;
using System.Linq.Expressions;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;

namespace PoS.Infra
{
    public class SCQueryable<T> : QueryableBase<T>
    {
        // This constructor is called by our users, create a new IQueryExecutor.
        public SCQueryable() : base(QueryParser.CreateDefault(), new SCQueryExecutor())
        {
        }
       

        // This constructor is called indirectly by LINQ's query methods, just pass to base.
        public SCQueryable(IQueryProvider provider, Expression expression) : base(provider, expression)
        {
        }
    }
}