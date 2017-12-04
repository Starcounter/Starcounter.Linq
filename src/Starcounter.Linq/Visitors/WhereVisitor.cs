using System;
using System.Collections;
using System.Linq.Expressions;
using Starcounter.Linq.Helpers;

namespace Starcounter.Linq.Visitors
{
    internal class WhereVisitor<TEntity> : StatelessVisitor<QueryBuilder<TEntity>>
    {
        public static WhereVisitor<TEntity> Instance = new WhereVisitor<TEntity>();

        public override void VisitBinary(BinaryExpression node, QueryBuilder<TEntity> state)
        {
            state.WriteWhere("(");
            switch (node.NodeType)
            {
                case ExpressionType.GreaterThan:
                    Visit(node.Left, state);
                    state.WriteWhere(" > ");
                    Visit(node.Right, state);
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    Visit(node.Left, state);
                    state.WriteWhere(" >= ");
                    Visit(node.Right, state);
                    break;
                case ExpressionType.LessThan:
                    Visit(node.Left, state);
                    state.WriteWhere(" < ");
                    Visit(node.Right, state);
                    break;
                case ExpressionType.LessThanOrEqual:
                    Visit(node.Left, state);
                    state.WriteWhere(" <= ");
                    Visit(node.Right, state);
                    break;
                case ExpressionType.Equal:
                    VisitBinaryEquality(node, state, true);
                    break;
                case ExpressionType.NotEqual:
                    VisitBinaryEquality(node, state, false);
                    break;
                case ExpressionType.AndAlso:
                case ExpressionType.And:
                    Visit(node.Left, state);
                    state.WriteWhere(" AND ");
                    Visit(node.Right, state);
                    break;

                case ExpressionType.OrElse:
                case ExpressionType.Or:
                    Visit(node.Left, state);
                    state.WriteWhere(" OR ");
                    Visit(node.Right, state);
                    break;

                case ExpressionType.Add:
                    Visit(node.Left, state);
                    state.WriteWhere(" + ");
                    Visit(node.Right, state);
                    break;

                case ExpressionType.Subtract:
                    Visit(node.Left, state);
                    state.WriteWhere(" - ");
                    Visit(node.Right, state);
                    break;

                case ExpressionType.Multiply:
                    Visit(node.Left, state);
                    state.WriteWhere(" * ");
                    Visit(node.Right, state);
                    break;

                case ExpressionType.Divide:
                    Visit(node.Left, state);
                    state.WriteWhere(" / ");
                    Visit(node.Right, state);
                    break;

                default:
                    throw new NotSupportedException();
            }

            state.WriteWhere(")");
        }

        private void VisitBinaryEquality(BinaryExpression node, QueryBuilder<TEntity> state, bool isEqualsSign)
        {
            if (node.Left is MemberExpression memberExpression && memberExpression.Type == typeof(bool))
            {
                if (memberExpression.Expression is ParameterExpression parameterExpression)
                {
                    state.WriteWhere(parameterExpression.Type.SourceName());
                    state.WriteWhere("." + SqlHelper.EscapeSingleIdentifier(memberExpression.Member.Name));
                }
                else
                {
                    VisitMember(memberExpression, state, false);
                }
            }
            else
            {
                Visit(node.Left, state);
            }

            if (node.Right is ConstantExpression constantExpression && constantExpression.Value == null ||
                node.Right is MemberExpression rightMemberExpression && rightMemberExpression.RetrieveValue() == null)
            {
                state.WriteWhere(isEqualsSign ? " IS NULL" : " IS NOT NULL");
            }
            else
            {
                state.WriteWhere(isEqualsSign ? " = " : " <> ");
                Visit(node.Right, state);
            }
        }

        public override void VisitBlock(BlockExpression node, QueryBuilder<TEntity> state)
        {
            base.VisitBlock(node, state);
        }

        public override void VisitConditional(ConditionalExpression node, QueryBuilder<TEntity> state)
        {
            base.VisitConditional(node, state);
        }

        public override void VisitConstant(ConstantExpression node, QueryBuilder<TEntity> state)
        {
            state.WriteWhere("?");
            state.AddVariable(node.Value);
        }

        public override void VisitDebugInfo(DebugInfoExpression node, QueryBuilder<TEntity> state)
        {
            base.VisitDebugInfo(node, state);
        }

        public override void VisitDynamic(DynamicExpression node, QueryBuilder<TEntity> state)
        {
            base.VisitDynamic(node, state);
        }

        public override void VisitDefault(DefaultExpression node, QueryBuilder<TEntity> state)
        {
            base.VisitDefault(node, state);
        }

        public override void VisitInvocation(InvocationExpression node, QueryBuilder<TEntity> state)
        {
            base.VisitInvocation(node, state);
        }

        public override void VisitLambda(LambdaExpression node, QueryBuilder<TEntity> state)
        {
            base.VisitLambda(node, state);
        }

        public override void VisitMember(MemberExpression node, QueryBuilder<TEntity> state)
        {
            VisitMember(node, state, true);
        }

        public void VisitMember(MemberExpression node, QueryBuilder<TEntity> state, bool specifyValueForBoolean)
        {
            var isBoolean = node.Type == typeof(bool);
            if (node.Expression is ParameterExpression param)
            {
                if (isBoolean)
                {
                    if (specifyValueForBoolean)
                    {
                        state.WriteWhere("(");
                    }

                    state.WriteWhere(param.Type.SourceName());
                    state.WriteWhere("." + SqlHelper.EscapeSingleIdentifier(node.Member.Name));

                    if (specifyValueForBoolean)
                    {
                        state.WriteWhere(" = True)");
                    }
                }
                else
                {
                    state.WriteWhere(param.Type.SourceName());
                    state.WriteWhere("." + SqlHelper.EscapeSingleIdentifier(node.Member.Name));
                }
            }
            else
            {
                var subNode = node;
                while (subNode.Expression is MemberExpression memberNode)
                {
                    subNode = memberNode;
                }
                if (subNode.Expression is ConstantExpression)
                {
                    var memberValue = node.RetrieveValue();
                    state.WriteWhere("?");
                    state.AddVariable(memberValue);
                }
                else if (subNode.Expression is ParameterExpression)
                {
                    if (isBoolean && specifyValueForBoolean)
                    {
                        state.WriteWhere("(");
                    }

                    Visit(node.Expression, state);
                    state.WriteWhere("." + SqlHelper.EscapeSingleIdentifier(node.Member.Name));

                    if (isBoolean && specifyValueForBoolean)
                    {
                        state.WriteWhere(" = True)");
                    }
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }

        public override void VisitIndex(IndexExpression node, QueryBuilder<TEntity> state)
        {
            base.VisitIndex(node, state);
        }

        public override void VisitMethodCall(MethodCallExpression node, QueryBuilder<TEntity> state)
        {
            ConstantExpression constNode = null;
            if (node.Method == KnownMethods.StringContains)
            {
                state.WriteWhere("(");
                Visit(node.Object, state);
                constNode = node.Arguments[0] as ConstantExpression;
                state.WriteWhere(" LIKE '%' || ? || '%')");
            }
            else if (node.Method == KnownMethods.StringStartsWith)
            {
                state.WriteWhere("(");
                Visit(node.Object, state);
                constNode = node.Arguments[0] as ConstantExpression;
                state.WriteWhere(" LIKE ? || '%')");
            }
            else if (node.Method == KnownMethods.StringEndsWith)
            {
                state.WriteWhere("(");
                Visit(node.Object, state);
                constNode = node.Arguments[0] as ConstantExpression;
                state.WriteWhere(" LIKE '%' || ?)");
            }
            else if (node.Method.IsGenericMethod &&
                node.Method.GetGenericMethodDefinition() == KnownMethods.EnumerableContains &&
                node.Arguments[0] is MemberExpression memberExpression)
            {
                state.WriteWhere("(");
                var items = (IEnumerable)memberExpression.RetrieveValue();
                int i = 0;

                foreach (var item in items)
                {
                    if (i++ > 0)
                    {
                        state.WriteWhere(" OR ");
                    }
                    state.WriteWhere("(");
                    Visit(node.Arguments[1], state);
                    state.WriteWhere(" = ?");
                    state.AddVariable(item);
                    state.WriteWhere(")");
                }
                state.WriteWhere(")");
            }
            else
            {
                throw new NotSupportedException("Method call is not supported");
            }

            if (constNode != null)
            {
                state.AddVariable(constNode.Value);
            }
        }

        public override void VisitNewArray(NewArrayExpression node, QueryBuilder<TEntity> state)
        {
            base.VisitNewArray(node, state);
        }

        public override void VisitNew(NewExpression node, QueryBuilder<TEntity> state)
        {
            base.VisitNew(node, state);
        }

        public override void VisitParameter(ParameterExpression node, QueryBuilder<TEntity> state)
        {
            state.WriteWhere("?");
        }

        public override void VisitRuntimeVariables(RuntimeVariablesExpression node, QueryBuilder<TEntity> state)
        {
            base.VisitRuntimeVariables(node, state);
        }

        public override void VisitUnary(UnaryExpression node, QueryBuilder<TEntity> state)
        {
            if (node.NodeType == ExpressionType.Not)
            {
                state.WriteWhere("NOT ");
                Visit(node.Operand, state);
            }
            else if (node.Operand is LambdaExpression lambda)
            {
                Visit(lambda.Body, state);
            }
            else
            {
                Visit(node.Operand, state);
            }
        }

        public override void VisitTypeBinary(TypeBinaryExpression node, QueryBuilder<TEntity> state)
        {
            state.WriteWhere("(");
            if (node.Expression is ParameterExpression parm)
            {
                state.WriteWhere(state.GetSourceName());
            }
            else
            {
                Visit(node.Expression, state);
            }
            state.WriteWhere($" IS {SqlHelper.EscapeIdentifiers(node.TypeOperand.FullName)})");
        }
    }
}