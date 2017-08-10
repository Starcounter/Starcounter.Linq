using System.Collections.Generic;
using System.Linq;
using Remotion.Linq;
using Starcounter;

namespace PoS.Infra
{
    public class SCQueryExecutor : IQueryExecutor
    {
        public SCQueryExecutor()
        {
        }

        // Executes a query with a scalar result, i.e. a query that ends with a result operator such as Count, Sum, or Average.
        public T ExecuteScalar<T>(QueryModel queryModel)
        {
            return ExecuteCollection<T>(queryModel).Single();
        }

        // Executes a query with a single result object, i.e. a query that ends with a result operator such as First, Last, Single, Min, or Max.
        public T ExecuteSingle<T>(QueryModel queryModel, bool returnDefaultWhenEmpty)
        {
            return returnDefaultWhenEmpty ? ExecuteCollection<T>(queryModel).SingleOrDefault() : ExecuteCollection<T>(queryModel).Single();
        }

        // Executes a query with a collection result.
        public IEnumerable<T> ExecuteCollection<T>(QueryModel queryModel)
        {
            var commandData = SqlGeneratorQueryModelVisitor.GenerateHqlQuery(queryModel);
            var args = commandData.NamedParameters.Select(n => n.Value).ToArray();
            var res = Db.SQL<T>(commandData.Statement, args);
            return res;
        }
    }
}