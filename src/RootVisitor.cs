using System;
using System.Linq.Expressions;

namespace Starcounter.Linq
{
    public class RootVisitor<T> : StatelessVisitor<QueryBuilder<T>>
    {
        public static readonly RootVisitor<T> Instance = new RootVisitor<T>();
        public override void VisitMethodCall(MethodCallExpression node, QueryBuilder<T> state)
        {
            var method = node.Method;
            if (method == KnownMethods<T>.QueryableFirstOrDefaultPred)
            {
                state.Fetch(1);
                var expression = node.Arguments[0];
                VisitWhere(state, expression);
            }
            else if (method == KnownMethods<T>.IQueryableFirstOrDefaultPred)
            {
                state.Fetch(1);
                var expression = node.Arguments[1];
                VisitWhere(state, expression);
            }
            else if (method == KnownMethods<T>.QueryableWhere)
            {
                var expression = node.Arguments[1];
                VisitWhere(state, expression);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private static void VisitWhere(QueryBuilder<T> state, Expression expression)
        {
            state.BeginWhereSection(); 
            WhereVisitor<T>.Instance.Visit(expression, state);
            state.EndWhereSection();
        }

        public override void VisitLambda(LambdaExpression node, QueryBuilder<T> state)
        {
            var arg = node.Parameters[0];

            WhereVisitor<T>.Instance.Visit(node.Body, state);
        }
    }
}
