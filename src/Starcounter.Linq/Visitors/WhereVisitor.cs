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
                    VisitAndOperand(node.Left, state);
                    state.WriteWhere(" AND ");
                    VisitAndOperand(node.Right, state);
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
        }

        private void VisitAndOperand(Expression node, QueryBuilder<TEntity> state)
        {
            if (node is BinaryExpression binExpr &&
                (binExpr.NodeType == ExpressionType.Or || binExpr.NodeType == ExpressionType.OrElse))
            {
                state.OpenWhereParentheses();
                Visit(node, state);
                state.CloseWhereParentheses();
            }
            else
            {
                Visit(node, state);
            }
        }

        private void VisitBinaryEquality(BinaryExpression node, QueryBuilder<TEntity> state, bool isEqualsSign)
        {
            this.VisitBinaryEquality(node.Left, node.Right, state, isEqualsSign);
        }

        private void VisitBinaryEquality(Expression left, Expression right, QueryBuilder<TEntity> state, bool isEqualsSign)
        {
            if (left is MemberExpression memberExpression && memberExpression.Type == typeof(bool))
            {
                if (memberExpression.Expression is ParameterExpression)
                {
                    if (state.ResultMethod == QueryResultMethod.Delete)
                    {
                        state.WriteWhere(SqlHelper.EscapeSingleIdentifier(memberExpression.Member.Name));
                    }
                    else
                    {
                        state.WriteWhere(state.GetSource());
                        state.WriteWhere("." + SqlHelper.EscapeSingleIdentifier(memberExpression.Member.Name));
                    }
                }
                else
                {
                    VisitMember(memberExpression, state, false);
                }
            }
            else
            {
                VisitPartOfBinaryExpression(left, state);
            }

            if (right is ConstantExpression constantExpression && constantExpression.Value == null ||
                right is MemberExpression rightMemberExpression && rightMemberExpression.RetrieveMemberValue() == null)
            {
                state.WriteWhere(isEqualsSign ? " IS NULL" : " IS NOT NULL");
            }
            else
            {
                state.WriteWhere(isEqualsSign ? " = " : " <> ");
                Visit(right, state);
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

        private void VisitMemberParameterExpression(string paramName, string memberName, bool isBoolean, QueryBuilder<TEntity> state, bool specifyValueForBoolean)
        {
            if (isBoolean)
            {
                if (state.ResultMethod == QueryResultMethod.Delete)
                {
                    state.WriteWhere(SqlHelper.EscapeSingleIdentifier(memberName));
                }
                else
                {
                    state.WriteWhere(paramName);
                    state.WriteWhere("." + SqlHelper.EscapeSingleIdentifier(memberName));
                }

                if (specifyValueForBoolean)
                {
                    state.WriteWhere(" = True");
                }
            }
            else
            {
                if (state.ResultMethod == QueryResultMethod.Delete)
                {
                    state.WriteWhere(SqlHelper.EscapeSingleIdentifier(memberName));
                }
                else
                {
                    state.WriteWhere(paramName);
                    state.WriteWhere("." + SqlHelper.EscapeSingleIdentifier(memberName));
                }
            }
        }

        public void VisitMember(MemberExpression node, QueryBuilder<TEntity> state, bool specifyValueForBoolean)
        {
            var isBoolean = node.Type == typeof(bool);
            if (node.Expression is ParameterExpression)
            {
                VisitMemberParameterExpression(state.GetSource(), node.Member.Name, isBoolean, state, specifyValueForBoolean);
            }
            else if (node.Expression is UnaryExpression)
            {
                VisitMemberParameterExpression(state.GetSource(), node.Member.Name, isBoolean, state, specifyValueForBoolean);
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
                    var memberValue = node.RetrieveMemberValue();
                    if (memberValue is LambdaExpression lambdaExpression)
                    {
                        Visit(lambdaExpression.Body, state);
                    }
                    else
                    {
                        state.WriteWhere("?");
                        state.AddVariable(memberValue);
                    }
                }
                else if (subNode.Expression is ParameterExpression || subNode.Expression is UnaryExpression)
                {
                    Visit(node.Expression, state);
                    state.WriteWhere("." + SqlHelper.EscapeSingleIdentifier(node.Member.Name));

                    if (isBoolean && specifyValueForBoolean)
                    {
                        state.WriteWhere(" = True");
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
            if (node.Method == KnownMethods.ObjectEquals)
            {
                VisitBinaryEquality(node.Object, node.Arguments[0], state, true);
            }
            else if (node.Method == KnownMethods.StringContains)
            {
                Visit(node.Object, state);
                AddConstantOrMemberNodeValue(node, state);
                state.WriteWhere(" LIKE '%' || ? || '%'");
            }
            else if (node.Method == KnownMethods.StringStartsWith)
            {
                Visit(node.Object, state);
                AddConstantOrMemberNodeValue(node, state);
                state.WriteWhere(" LIKE ? || '%'");
            }
            else if (node.Method == KnownMethods.StringEndsWith)
            {
                Visit(node.Object, state);
                AddConstantOrMemberNodeValue(node, state);
                state.WriteWhere(" LIKE '%' || ?");
            }
            else if (node.Method == KnownMethods.GetOid)
            {
                state.WriteWhereObjectNo(state.ResultMethod != QueryResultMethod.Delete);
            }
            else if (node.Method.IsGenericMethod &&
                node.Method.GetGenericMethodDefinition() == KnownMethods.EnumerableContains &&
                node.Arguments[0] is MemberExpression memberExpression)
            {
                var items = (IEnumerable)memberExpression.RetrieveMemberValue();
                int i = 0;
                bool parenthesesOpened = false;

                foreach (var item in items)
                {
                    if (i <= 0)
                    {
                        parenthesesOpened = state.OpenWhereParenthesesIfFirst();
                    }
                    if (i > 0)
                    {
                        state.WriteWhere(" OR ");
                    }
                    i++;

                    Visit(node.Arguments[1], state);
                    state.WriteWhere(" = ?");
                    state.AddVariable(item);
                }
                if (parenthesesOpened)
                {
                    state.CloseWhereParentheses();
                }
                if (i <= 0) // empty collection
                {
                    VisitPartOfBinaryExpression(node.Arguments[1], state);
                    state.WriteWhere(" <> ");
                    VisitPartOfBinaryExpression(node.Arguments[1], state);
                }
            }
            else if (node.Object != null)
            {
                object callValue = node.RetrieveMethodCallValue();
                state.WriteWhere("?");
                state.AddVariable(callValue);
            }
            else
            {
                throw new NotSupportedException("Method call is not supported");
            }
        }

        private void AddConstantOrMemberNodeValue(MethodCallExpression node, QueryBuilder<TEntity> state)
        {
            object variable;
            if (node.Arguments[0] is ConstantExpression constNode)
            {
                variable = constNode.Value;
            }
            else if (node.Arguments[0] is MemberExpression memberNode)
            {
                variable = memberNode.RetrieveMemberValue();
            }
            else
            {
                return;
            }
            state.AddVariable(variable);
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
                VisitNot(node.Operand, state);
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
            if (node.Expression is ParameterExpression)
            {
                state.WriteWhere(state.GetSource());
            }
            else
            {
                Visit(node.Expression, state);
            }
            state.WriteWhere($" IS {SqlHelper.EscapeIdentifiers(node.TypeOperand.FullName)}");
        }

        private void VisitNot(Expression operand, QueryBuilder<TEntity> state)
        {
            state.WriteWhere("NOT ");
            state.OpenWhereParentheses();
            Visit(operand, state);
            state.CloseWhereParentheses();
        }

        private void VisitPartOfBinaryExpression(Expression expr, QueryBuilder<TEntity> state)
        {
            if (expr is ParameterExpression)
            {
                state.WriteWhere(state.GetSource());
            }
            else
            {
                Visit(expr, state);
            }
        }
    }
}
