using System.Linq;
using System.Linq.Expressions;
using Remotion.Linq;
using Remotion.Linq.Parsing.ExpressionVisitors.Transformation;
using Remotion.Linq.Parsing.ExpressionVisitors.Transformation.PredefinedTransformations;
using Remotion.Linq.Parsing.Structure;
using Remotion.Linq.Parsing.Structure.ExpressionTreeProcessors;

namespace Starcounter.Linq
{
    internal static class Shared
    {
        internal static readonly QueryParser Parser = CreateParser();
        internal static readonly SqlQueryExecutor Executor = new SqlQueryExecutor();
        internal static readonly EmptyQueryExecutor EmptyExecutor = new EmptyQueryExecutor();

        private static QueryParser CreateParser()
        {
            var transformerRegistry = new ExpressionTransformerRegistry();
            transformerRegistry.Register(new InvocationOfLambdaExpressionTransformer());
            transformerRegistry.Register(new NullableValueTransformer());

            var processor = new TransformingExpressionTreeProcessor(transformerRegistry);
            var e = new ExpressionTreeParser(ExpressionTreeParser.CreateDefaultNodeTypeProvider(), processor);
            var p = new QueryParser(e);
            return p;
        }
    }

    public class SqlQueryable<T> : QueryableBase<T>
    {
        public SqlQueryable(SqlQueryExecutor executor) : base(Shared.Parser, executor)
        {
        }

        // This constructor is called indirectly by LINQ's query methods, just pass to base.
        public SqlQueryable(IQueryProvider provider, Expression expression) : base(provider, expression)
        {
        }
    }
}