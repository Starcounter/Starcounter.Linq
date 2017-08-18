using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Starcounter.Linq
{
    public class OrderByVisitor<T> : StatelessVisitor<QueryBuilder<T>>
    {
        public static OrderByVisitor<T> Instance = new OrderByVisitor<T>();
        public override void Visit(Expression node, QueryBuilder<T> state)
        {
            base.Visit(node, state);
        }

        public override void VisitBinary(BinaryExpression node, QueryBuilder<T> state)
        {
            base.VisitBinary(node, state);
        }

        public override void VisitBlock(BlockExpression node, QueryBuilder<T> state)
        {
            base.VisitBlock(node, state);
        }

        public override void VisitConditional(ConditionalExpression node, QueryBuilder<T> state)
        {
            base.VisitConditional(node, state);
        }

        public override void VisitConstant(ConstantExpression node, QueryBuilder<T> state)
        {
            base.VisitConstant(node, state);
        }

        public override void VisitDebugInfo(DebugInfoExpression node, QueryBuilder<T> state)
        {
            base.VisitDebugInfo(node, state);
        }

        public override void VisitDynamic(DynamicExpression node, QueryBuilder<T> state)
        {
            base.VisitDynamic(node, state);
        }

        public override void VisitDefault(DefaultExpression node, QueryBuilder<T> state)
        {
            base.VisitDefault(node, state);
        }

        public override void VisitInvocation(InvocationExpression node, QueryBuilder<T> state)
        {
            base.VisitInvocation(node, state);
        }

        public override void VisitLambda(LambdaExpression node, QueryBuilder<T> state)
        {
            base.VisitLambda(node, state);
        }

        public override void VisitMember(MemberExpression node, QueryBuilder<T> state)
        {
            if (node.Expression is ParameterExpression param)
            {
                state.WriteOrderBy(param.Type.SourceName());
            }
            else
            {
                Visit(node.Expression, state);
            }
            state.WriteOrderBy("." + node.Member.Name);
        }

        public override void VisitIndex(IndexExpression node, QueryBuilder<T> state)
        {
            base.VisitIndex(node, state);
        }

        public override void VisitMethodCall(MethodCallExpression node, QueryBuilder<T> state)
        {
            base.VisitMethodCall(node, state);
        }

        public override void VisitNewArray(NewArrayExpression node, QueryBuilder<T> state)
        {
            base.VisitNewArray(node, state);
        }

        public override void VisitNew(NewExpression node, QueryBuilder<T> state)
        {
            base.VisitNew(node, state);
        }

        public override void VisitParameter(ParameterExpression node, QueryBuilder<T> state)
        {
            base.VisitParameter(node, state);
        }

        public override void VisitRuntimeVariables(RuntimeVariablesExpression node, QueryBuilder<T> state)
        {
            base.VisitRuntimeVariables(node, state);
        }

        public override void VisitUnary(UnaryExpression node, QueryBuilder<T> state)
        {
            base.VisitUnary(node, state);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
