using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Starcounter.Linq.Raw
{
    public class RootVisitor<T> : StatelessVisitor<QueryBuilder<T>>
    {
        public static readonly RootVisitor<T> Instance = new RootVisitor<T>();
        public override void VisitMethodCall(MethodCallExpression node, QueryBuilder<T> state)
        {
            var method = node.Method;
            if (method == MethodTranslator<T>.QueryableFirstOrDefaultPred)
            {
                state.Fetch(1);
                WhereVisitor<T>.Instance.Visit(node.Arguments[0], state);
            }
            if (method == MethodTranslator<T>.IQueryableFirstOrDefaultPred)
            {
                state.Fetch(1);
                WhereVisitor<T>.Instance.Visit(node.Arguments[1], state);
            }
            if (method == MethodTranslator<T>.QueryableWhere)
            {
                var args = node.Arguments.ToArray();


                WhereVisitor<T>.Instance.Visit(args[1], state);
            }
            if (node.Method.IsGenericMethod)
            {
                var generic = node.Method.GetGenericMethodDefinition();
                
            }
        }

        public override void VisitLambda(LambdaExpression node, QueryBuilder<T> state)
        {
            var arg = node.Parameters[0];

            WhereVisitor<T>.Instance.Visit(node.Body, state);
        }
    }
}
