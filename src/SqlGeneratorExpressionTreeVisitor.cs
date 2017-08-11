using System;
using System.Linq.Expressions;
using System.Text;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing;

namespace PoS.Infra
{
    public class SqlGeneratorExpressionTreeVisitor : ThrowingExpressionVisitor
    {
        private readonly StringBuilder _sqlExpression = new StringBuilder();
        private readonly QueryVariables _variables;

        public static string GetSqlExpression(Expression linqExpression, QueryVariables variables)
        {
            var visitor = new SqlGeneratorExpressionTreeVisitor(variables);
            visitor.Visit(linqExpression);
            return visitor.GetSqlExpression();
        }

        private SqlGeneratorExpressionTreeVisitor(QueryVariables variables)
        {
            _variables = variables;
        }

        public string GetSqlExpression() => _sqlExpression.ToString();

        protected override Expression VisitQuerySourceReference(QuerySourceReferenceExpression expression)
        {
            _sqlExpression.Append(expression.ReferencedQuerySource.ItemName);
            return expression;
        }

        protected override Expression VisitBinary(BinaryExpression expression)
        {
            _sqlExpression.Append("(");

            Visit(expression.Left);

            // In production code, handle this via lookup tables.
            switch (expression.NodeType)
            {
                case ExpressionType.GreaterThan:
                    _sqlExpression.Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    _sqlExpression.Append(" >= ");
                    break;
                case ExpressionType.LessThan:
                    _sqlExpression.Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    _sqlExpression.Append(" <= ");
                    break;
                case ExpressionType.Equal:
                    _sqlExpression.Append(" = ");
                    break;

                case ExpressionType.AndAlso:
                case ExpressionType.And:
                    _sqlExpression.Append(" and ");
                    break;

                case ExpressionType.OrElse:
                case ExpressionType.Or:
                    _sqlExpression.Append(" or ");
                    break;

                case ExpressionType.Add:
                    _sqlExpression.Append(" + ");
                    break;

                case ExpressionType.Subtract:
                    _sqlExpression.Append(" - ");
                    break;

                case ExpressionType.Multiply:
                    _sqlExpression.Append(" * ");
                    break;

                case ExpressionType.Divide:
                    _sqlExpression.Append(" / ");
                    break;

                default:
                    base.VisitBinary(expression);
                    break;
            }

            Visit(expression.Right);
            _sqlExpression.Append(")");

            return expression;
        }

        protected override Expression VisitMember(MemberExpression expression)
        {
            Visit(expression.Expression);
            _sqlExpression.Append($".{expression.Member.Name}");

            return expression;
        }

        protected override Expression VisitConstant(ConstantExpression expression)
        {
            var namedParameter = _variables.AddVariable(expression.Value);

            //TODO: ordering or named arguments?
            _sqlExpression.Append($"{namedParameter.Name}");

            return expression;
        }


        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            if (expression.Method.Equals(MethodTranslator.StringContains))
            {
                _sqlExpression.Append("(");
                Visit(expression.Object);
                _sqlExpression.Append(" like ?||");
                _variables.AddVariable("%");
                Visit(expression.Arguments[0]);
                _sqlExpression.Append("||?)");
                _variables.AddVariable("%");
                return expression;
            }

            if (expression.Method.Equals(MethodTranslator.StringStartsWith))
            {
                _sqlExpression.Append("(");
                Visit(expression.Object);
                _sqlExpression.Append(" like ");
                Visit(expression.Arguments[0]);
                _sqlExpression.Append("||?)");
                _variables.AddVariable("%");
                return expression;
            }

            if (expression.Method.Equals(MethodTranslator.StringEndsWith))
            {
                _sqlExpression.Append("(");
                Visit(expression.Object);
                _sqlExpression.Append(" like ?||");
                _variables.AddVariable("%");
                Visit(expression.Arguments[0]);
                _sqlExpression.Append(")");
                return expression;
            }


            return base.VisitMethodCall(expression); // throws
        }

        // Called when a LINQ expression type is not handled above.
        protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod)
        {
            const string itemText = "Dunnoo";
            var message = $"The expression '{itemText}' (type: {typeof(T)}) is not supported by this LINQ provider.";
            return new NotSupportedException(message);
        }
    }
}
