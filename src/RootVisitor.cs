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
                VisitWhere(expression, state);
            }
            else if (method == KnownMethods<T>.IQueryableFirstOrDefaultPred)
            {
                state.Fetch(1);
                var expression = node.Arguments[1];
                VisitWhere(expression, state);
            }
            else if (method == KnownMethods<T>.QueryableWhere)
            {
                var expression = node.Arguments[1];
                VisitWhere(expression, state);
            }
            else if (method.IsGenericMethod)
            {
                var gen = method.GetGenericMethodDefinition();


                //Why cannot this be done like the above?
                //Because OrderBy use Func<T,TKeySelector> where TKeySelector is unknown to us here
                //That is, it is the return type of the property
                if (gen == KnownMethods<T>.IQueryableOrderBy)
                {
                    VisitOrderBy(node, state, true);
                }
                else if (gen == KnownMethods<T>.IQueryableOrderByDesc)
                {
                    VisitOrderBy(node, state, false);
                }
                else if (gen == KnownMethods<T>.IQueryableThenBy)
                {
                    VisitOrderBy(node, state, true);
                }
                else if (gen == KnownMethods<T>.IQueryableThenByDesc)
                {
                    VisitOrderBy(node, state, false);
                }
                else if (gen == KnownMethods<T>.IQueryableTake)
                {
                    var value = (int)(node.Arguments[1] as ConstantExpression).Value;
                    state.Fetch(value);
                }
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private void VisitOrderBy(MethodCallExpression node, QueryBuilder<T> state, bool asc)
        {
            var left = node.Arguments[0];
            Visit(left, state);
            state.BeginOrderBySection();
            var arg = node.Arguments[1];
            OrderByVisitor<T>.Instance.Visit(arg, state);
            state.EndOrderBySection(asc);
        }


        private static void VisitWhere(Expression expression, QueryBuilder<T> state)
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
