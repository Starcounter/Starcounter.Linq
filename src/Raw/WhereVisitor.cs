using System;
using System.Linq.Expressions;

namespace Starcounter.Linq.Raw
{
    public class WhereVisitor<T> : StatelessVisitor<QueryBuilder<T>>
    {
        public static WhereVisitor<T> Instance = new WhereVisitor<T>();

        public override void VisitBinary(BinaryExpression node, QueryBuilder<T> state)
        {
            state.AddWherePart("(");
            switch (node.NodeType)
            {
                case ExpressionType.GreaterThan:
                    Visit(node.Left, state);
                    state.AddWherePart(" > ");
                    Visit(node.Right, state);
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    Visit(node.Left, state);
                    state.AddWherePart(" >= ");
                    Visit(node.Right, state);
                    break;
                case ExpressionType.LessThan:
                    Visit(node.Left, state);
                    state.AddWherePart(" < ");
                    Visit(node.Right, state);
                    break;
                case ExpressionType.LessThanOrEqual:
                    Visit(node.Left, state);
                    state.AddWherePart(" <= ");
                    Visit(node.Right, state);
                    break;
                case ExpressionType.Equal:
                    Visit(node.Left, state);
                    state.AddWherePart(" = ");
                    Visit(node.Right, state);
                    break;
                case ExpressionType.NotEqual:
                    state.AddWherePart("NOT ");
                    Visit(node.Left, state);
                    state.AddWherePart(" = ");
                    Visit(node.Right, state);
                    break;

                case ExpressionType.AndAlso:
                case ExpressionType.And:
                    Visit(node.Left, state);
                    state.AddWherePart(" AND ");
                    Visit(node.Right, state);
                    break;

                case ExpressionType.OrElse:
                case ExpressionType.Or:
                    Visit(node.Left, state);
                    state.AddWherePart(" OR ");
                    Visit(node.Right, state);
                    break;

                case ExpressionType.Add:
                    Visit(node.Left, state);
                    state.AddWherePart(" + ");
                    Visit(node.Right, state);
                    break;

                case ExpressionType.Subtract:
                    Visit(node.Left, state);
                    state.AddWherePart(" - ");
                    Visit(node.Right, state);
                    break;

                case ExpressionType.Multiply:
                    Visit(node.Left, state);
                    state.AddWherePart(" * ");
                    Visit(node.Right, state);
                    break;

                case ExpressionType.Divide:
                    Visit(node.Left, state);
                    state.AddWherePart(" / ");
                    Visit(node.Right, state);
                    break;

                default:
                    throw new NotSupportedException();
            }

            state.AddWherePart(")");
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
            state.AddWherePart("?");
            state.AddVariable(node.Value);
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
                state.AddWherePart(param.Type.SourceName());
            }
            else
            {
                Visit(node.Expression, state);
            }
            state.AddWherePart("." + node.Member.Name);
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
            state.AddWherePart("?");
        }

        public override void VisitRuntimeVariables(RuntimeVariablesExpression node, QueryBuilder<T> state)
        {
            base.VisitRuntimeVariables(node, state);
        }

        public override void VisitUnary(UnaryExpression node, QueryBuilder<T> state)
        {
            var body = (node.Operand as LambdaExpression).Body;
            Visit(body,state);
         //   base.VisitUnary(node, state);
        }
    }
}