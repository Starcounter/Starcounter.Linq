using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Starcounter.Linq.Cache
{
    public sealed class ExpressionEqualityComparer : IEqualityComparer<Expression>
    {
        #region Private fields

        private int _hashCode;

        #endregion

        #region Hash code

        private void Visit(Expression expression)
        {


            _hashCode ^= (int) expression.NodeType ^ expression.Type.GetHashCode();

            switch (expression.NodeType)
            {
                case ExpressionType.ArrayLength:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.Negate:
                case ExpressionType.UnaryPlus:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                    VisitUnary((UnaryExpression) expression);
                    break;

                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.ArrayIndex:
                case ExpressionType.Coalesce:
                case ExpressionType.Divide:
                case ExpressionType.Equal:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LeftShift:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.Modulo:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.NotEqual:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.Power:
                case ExpressionType.RightShift:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    VisitBinary((BinaryExpression) expression);
                    break;

                case ExpressionType.Call:
                    VisitMethodCall((MethodCallExpression) expression);
                    break;

                case ExpressionType.Conditional:
                    VisitConditional((ConditionalExpression) expression);
                    break;

                case ExpressionType.Constant:
                    VisitConstant((ConstantExpression) expression);
                    break;

                case ExpressionType.Invoke:
                    VisitInvocation((InvocationExpression) expression);
                    break;

                case ExpressionType.Lambda:
                    VisitLambda((LambdaExpression) expression);
                    break;

                case ExpressionType.ListInit:
                    VisitListInit((ListInitExpression) expression);
                    break;

                case ExpressionType.MemberAccess:
                    VisitMemberAccess((MemberExpression) expression);
                    break;

                case ExpressionType.MemberInit:
                    VisitMemberInit((MemberInitExpression) expression);
                    break;

                case ExpressionType.New:
                    VisitNew((NewExpression) expression);
                    break;

                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    VisitNewArray((NewArrayExpression) expression);
                    break;

                case ExpressionType.Parameter:
                    VisitParameter((ParameterExpression) expression);
                    break;

                case ExpressionType.TypeIs:
                    VisitTypeIs((TypeBinaryExpression) expression);
                    break;

                default:
                    throw new ArgumentException("Unhandled expression type");
            }
        }

        private void VisitUnary(UnaryExpression expression)
        {
            if (expression.Method != null)
                _hashCode ^= expression.Method.GetHashCode();

            Visit(expression.Operand);
        }

        private void VisitBinary(BinaryExpression expression)
        {
            if (expression.Method != null)
                _hashCode ^= expression.Method.GetHashCode();

            Visit(expression.Left);
            Visit(expression.Right);
            Visit(expression.Conversion);
        }

        private void VisitMethodCall(MethodCallExpression expression)
        {
            _hashCode ^= expression.Method.GetHashCode();

            Visit(expression.Object);
            VisitExpressionList(expression.Arguments);
        }

        private void VisitConditional(ConditionalExpression expression)
        {
            Visit(expression.Test);
            Visit(expression.IfTrue);
            Visit(expression.IfFalse);
        }

        private void VisitConstant(ConstantExpression expression)
        {
            if (expression.Value != null)
                _hashCode ^= expression.Value.GetHashCode();
        }

        private void VisitInvocation(InvocationExpression expression)
        {
            Visit(expression.Expression);
            VisitExpressionList(expression.Arguments);
        }

        private void VisitLambda(LambdaExpression expression)
        {
            if (expression.Name != null)
                _hashCode ^= expression.Name.GetHashCode();

            Visit(expression.Body);
            VisitParameterList(expression.Parameters);
        }

        private void VisitListInit(ListInitExpression expression)
        {
            VisitNew(expression.NewExpression);
            VisitElementInitializerList(expression.Initializers);
        }

        private void VisitMemberAccess(MemberExpression expression)
        {
            _hashCode ^= expression.Member.GetHashCode();
            Visit(expression.Expression);
        }

        private void VisitMemberInit(MemberInitExpression expression)
        {
            Visit(expression.NewExpression);
            VisitBindingList(expression.Bindings);
        }

        private void VisitNew(NewExpression expression)
        {
            _hashCode ^= expression.Constructor.GetHashCode();

            VisitMemberList(expression.Members);
            VisitExpressionList(expression.Arguments);
        }

        private void VisitNewArray(NewArrayExpression expression)
        {
            VisitExpressionList(expression.Expressions);
        }

        private void VisitParameter(ParameterExpression expression)
        {
            _hashCode ^= expression.Name.GetHashCode();
        }

        private void VisitTypeIs(TypeBinaryExpression expression)
        {
            _hashCode ^= expression.TypeOperand.GetHashCode();
            Visit(expression.Expression);
        }

        private void VisitBinding(MemberBinding binding)
        {
            _hashCode ^= (int) binding.BindingType ^ binding.Member.GetHashCode();

            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment:
                    VisitMemberAssignment((MemberAssignment) binding);
                    break;

                case MemberBindingType.MemberBinding:
                    VisitMemberMemberBinding((MemberMemberBinding) binding);
                    break;

                case MemberBindingType.ListBinding:
                    VisitMemberListBinding((MemberListBinding) binding);
                    break;

                default:
                    throw new ArgumentException("Unhandled binding type");
            }
        }

        private void VisitMemberAssignment(MemberAssignment assignment)
        {
            Visit(assignment.Expression);
        }

        private void VisitMemberMemberBinding(MemberMemberBinding binding)
        {
            VisitBindingList(binding.Bindings);
        }

        private void VisitMemberListBinding(MemberListBinding binding)
        {
            VisitElementInitializerList(binding.Initializers);
        }

        private void VisitElementInitializer(ElementInit initializer)
        {
            _hashCode ^= initializer.AddMethod.GetHashCode();

            VisitExpressionList(initializer.Arguments);
        }

        private void VisitExpressionList(ReadOnlyCollection<Expression> list)
        {
            if (list != null)
                foreach (Expression t in list)
                    Visit(t);
        }

        private void VisitParameterList(IReadOnlyList<ParameterExpression> list)
        {
            if (list != null)
                foreach (ParameterExpression t in list)
                    Visit(t);
        }

        private void VisitBindingList(IReadOnlyList<MemberBinding> list)
        {
            if (list != null)
                foreach (MemberBinding t in list)
                    VisitBinding(t);
        }

        private void VisitElementInitializerList(ReadOnlyCollection<ElementInit> list)
        {
            if (list != null)
                foreach (ElementInit t in list)
                    VisitElementInitializer(t);
        }

        private void VisitMemberList(ReadOnlyCollection<MemberInfo> list)
        {
            if (list != null)
                foreach (MemberInfo t in list)
                    _hashCode ^= t.GetHashCode();
        }

        #endregion

        #region Equality

        private bool Visit(Expression x, Expression y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x == null || y == null)
                return false;

            if (x.NodeType != y.NodeType || x.Type != y.Type)
                return false;

            switch (x.NodeType)
            {
                case ExpressionType.ArrayLength:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.Negate:
                case ExpressionType.UnaryPlus:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                    return VisitUnary((UnaryExpression) x, (UnaryExpression) y);

                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.ArrayIndex:
                case ExpressionType.Coalesce:
                case ExpressionType.Divide:
                case ExpressionType.Equal:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LeftShift:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.Modulo:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.NotEqual:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.Power:
                case ExpressionType.RightShift:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return VisitBinary((BinaryExpression) x, (BinaryExpression) y);

                case ExpressionType.Call:
                    return VisitMethodCall((MethodCallExpression) x, (MethodCallExpression) y);

                case ExpressionType.Conditional:
                    return VisitConditional((ConditionalExpression) x, (ConditionalExpression) y);

                case ExpressionType.Constant:
                    return VisitConstant((ConstantExpression) x, (ConstantExpression) y);

                case ExpressionType.Invoke:
                    return VisitInvocation((InvocationExpression) x, (InvocationExpression) y);

                case ExpressionType.Lambda:
                    return VisitLambda((LambdaExpression) x, (LambdaExpression) y);

                case ExpressionType.ListInit:
                    return VisitListInit((ListInitExpression) x, (ListInitExpression) y);

                case ExpressionType.MemberAccess:
                    return VisitMemberAccess((MemberExpression) x, (MemberExpression) y);

                case ExpressionType.MemberInit:
                    return VisitMemberInit((MemberInitExpression) x, (MemberInitExpression) y);

                case ExpressionType.New:
                    return VisitNew((NewExpression) x, (NewExpression) y);

                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    return VisitNewArray((NewArrayExpression) x, (NewArrayExpression) y);

                case ExpressionType.Parameter:
                    return VisitParameter((ParameterExpression) x, (ParameterExpression) y);

                case ExpressionType.TypeIs:
                    return VisitTypeIs((TypeBinaryExpression) x, (TypeBinaryExpression) y);

                default:
                    throw new ArgumentException("Unhandled expression type");
            }
        }

        private bool VisitUnary(UnaryExpression x, UnaryExpression y)
        {
            return x.Method == y.Method &&
                   Visit(x.Operand, y.Operand);
        }

        private bool VisitBinary(BinaryExpression x, BinaryExpression y)
        {
            return x.Method == y.Method &&
                   Visit(x.Left, y.Left) &&
                   Visit(x.Right, y.Right) &&
                   Visit(x.Conversion, y.Conversion);
        }

        private bool VisitMethodCall(MethodCallExpression x, MethodCallExpression y)
        {
            return x.Method == y.Method &&
                   Visit(x.Object, y.Object) &&
                   VisitExpressionList(x.Arguments, y.Arguments);
        }

        private bool VisitConditional(ConditionalExpression x, ConditionalExpression y)
        {
            return Visit(x.Test, y.Test) &&
                   Visit(x.IfTrue, y.IfTrue) &&
                   Visit(x.IfFalse, y.IfFalse);
        }

        private bool VisitConstant(ConstantExpression x, ConstantExpression y)
        {
            return Equals(x.Value, y.Value);
        }

        private bool VisitInvocation(InvocationExpression x, InvocationExpression y)
        {
            return Visit(x.Expression, y.Expression) &&
                   VisitExpressionList(x.Arguments, x.Arguments);
        }

        private bool VisitLambda(LambdaExpression x, LambdaExpression y)
        {
            return Visit(x.Body, y.Body) &&
                   VisitParameterList(x.Parameters, y.Parameters);
        }

        private bool VisitListInit(ListInitExpression x, ListInitExpression y)
        {
            return VisitNew(x.NewExpression, y.NewExpression) &&
                   VisitElementInitializerList(x.Initializers, y.Initializers);
        }

        private bool VisitMemberAccess(MemberExpression x, MemberExpression y)
        {
            return x.Member == y.Member &&
                   Visit(x.Expression, y.Expression);
        }

        private bool VisitMemberInit(MemberInitExpression x, MemberInitExpression y)
        {
            return Visit(x.NewExpression, y.NewExpression) &&
                   VisitBindingList(x.Bindings, y.Bindings);
        }

        private bool VisitNew(NewExpression x, NewExpression y)
        {
            return x.Constructor == y.Constructor &&
                   VisitMemberList(x.Members, y.Members) &&
                   VisitExpressionList(x.Arguments, y.Arguments);
        }

        private bool VisitNewArray(NewArrayExpression x, NewArrayExpression y)
        {
            return VisitExpressionList(x.Expressions, y.Expressions);
        }

        private bool VisitParameter(ParameterExpression x, ParameterExpression y)
        {
            return x.Type == y.Type && x.IsByRef == y.IsByRef;
        }

        private bool VisitTypeIs(TypeBinaryExpression x, TypeBinaryExpression y)
        {
            return x.TypeOperand == y.TypeOperand &&
                   Visit(x.Expression, y.Expression);
        }

        private bool VisitBinding(MemberBinding x, MemberBinding y)
        {
            if (x.BindingType != y.BindingType || x.Member != y.Member)
                return false;

            switch (x.BindingType)
            {
                case MemberBindingType.Assignment:
                    return VisitMemberAssignment((MemberAssignment) x, (MemberAssignment) y);

                case MemberBindingType.MemberBinding:
                    return VisitMemberMemberBinding((MemberMemberBinding) x, (MemberMemberBinding) y);

                case MemberBindingType.ListBinding:
                    return VisitMemberListBinding((MemberListBinding) x, (MemberListBinding) y);

                default:
                    throw new ArgumentException("Unhandled binding type");
            }
        }

        private bool VisitMemberAssignment(MemberAssignment x, MemberAssignment y)
        {
            return Visit(x.Expression, y.Expression);
        }

        private bool VisitMemberMemberBinding(MemberMemberBinding x, MemberMemberBinding y)
        {
            return VisitBindingList(x.Bindings, y.Bindings);
        }

        private bool VisitMemberListBinding(MemberListBinding x, MemberListBinding y)
        {
            return VisitElementInitializerList(x.Initializers, y.Initializers);
        }

        private bool VisitElementInitializer(ElementInit x, ElementInit y)
        {
            return x.AddMethod == y.AddMethod &&
                   VisitExpressionList(x.Arguments, y.Arguments);
        }

        private bool VisitExpressionList(ReadOnlyCollection<Expression> x, ReadOnlyCollection<Expression> y)
        {
            if (x == y)
                return true;

            if (x != null && y != null && x.Count == y.Count)
            {
                for (var i = 0; i < x.Count; i++)
                    if (Visit(x[i], y[i]) == false)
                        return false;

                return true;
            }

            return false;
        }

        private bool VisitParameterList(ReadOnlyCollection<ParameterExpression> x,
            ReadOnlyCollection<ParameterExpression> y)
        {
            if (x == y)
                return true;

            if (x != null && y != null && x.Count == y.Count)
            {
                for (var i = 0; i < x.Count; i++)
                    if (Visit(x[i], y[i]) == false)
                        return false;

                return true;
            }

            return false;
        }

        private bool VisitBindingList(ReadOnlyCollection<MemberBinding> x, ReadOnlyCollection<MemberBinding> y)
        {
            if (x == y)
                return true;

            if (x != null && y != null && x.Count == y.Count)
            {
                for (var i = 0; i < x.Count; i++)
                    if (VisitBinding(x[i], y[i]) == false)
                        return false;

                return true;
            }

            return false;
        }

        private bool VisitElementInitializerList(ReadOnlyCollection<ElementInit> x,
            ReadOnlyCollection<ElementInit> y)
        {
            if (x == y)
                return true;

            if (x != null && y != null && x.Count == y.Count)
            {
                for (var i = 0; i < x.Count; i++)
                    if (VisitElementInitializer(x[i], y[i]) == false)
                        return false;

                return true;
            }

            return false;
        }

        private bool VisitMemberList(ReadOnlyCollection<MemberInfo> x, ReadOnlyCollection<MemberInfo> y)
        {
            if (x == y)
                return true;

            if (x != null && y != null && x.Count == y.Count)
            {
                for (var i = 0; i < x.Count; i++)
                    if (x[i] != y[i])
                        return false;

                return true;
            }

            return false;
        }

        #endregion

        #region IEqualityComparer<Expression> Members

        public bool Equals(Expression x, Expression y)
        {
            return Visit(x, y);
        }

        public int GetHashCode(Expression expression)
        {
            _hashCode = 0;

            Visit(expression);

            return _hashCode;
        }

        #endregion
    }
}