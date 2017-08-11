using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ResultOperators;
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
            var name = expression.ReferencedQuerySource.ItemName.Replace("<", "").Replace(">", "");
            _sqlExpression.Append(name);
            return expression;
        }

        protected override Expression VisitUnary(UnaryExpression expression)
        {
            Write("(");
            Write("NOT ");
            Visit(expression.Operand);
            Write(")");
            return expression;
        }

        protected override Expression VisitBinary(BinaryExpression expression)
        {
            Write("(");
            
            // In production code, handle this via lookup tables.
            switch (expression.NodeType)
            {
                case ExpressionType.GreaterThan:
                    Visit(expression.Left);
                    Write(" > ");
                    Visit(expression.Right);
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    Visit(expression.Left);
                    Write(" >= ");
                    Visit(expression.Right);
                    break;
                case ExpressionType.LessThan:
                    Visit(expression.Left);
                    Write(" < ");
                    Visit(expression.Right);
                    break;
                case ExpressionType.LessThanOrEqual:
                    Visit(expression.Left);
                    Write(" <= ");
                    Visit(expression.Right);
                    break;
                case ExpressionType.Equal:
                    Visit(expression.Left);
                    Write(" = ");
                    Visit(expression.Right);
                    break;
                case ExpressionType.NotEqual:
                    Write("NOT ");
                    Visit(expression.Left);
                    Write(" = ");
                    Visit(expression.Right);
                    break;

                case ExpressionType.AndAlso:
                case ExpressionType.And:
                    Visit(expression.Left);
                    Write(" AND ");
                    Visit(expression.Right);
                    break;

                case ExpressionType.OrElse:
                case ExpressionType.Or:
                    Visit(expression.Left);
                    Write(" OR ");
                    Visit(expression.Right);
                    break;

                case ExpressionType.Add:
                    Visit(expression.Left);
                    Write(" + ");
                    Visit(expression.Right);
                    break;

                case ExpressionType.Subtract:
                    Visit(expression.Left);
                    Write(" - ");
                    Visit(expression.Right);
                    break;

                case ExpressionType.Multiply:
                    Visit(expression.Left);
                    Write(" * ");
                    Visit(expression.Right);
                    break;

                case ExpressionType.Divide:
                    Visit(expression.Left);
                    Write(" / ");
                    Visit(expression.Right);
                    break;

                default:
                    Visit(expression.Left);
                    base.VisitBinary(expression);
                    break;
            }

            _sqlExpression.Append(")");

            return expression;
        }

        private void Write(string text)
        {
            _sqlExpression.Append(text);
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
                Write("(");
                Visit(expression.Object);
                Write(" like ?||");
                _variables.AddVariable("%");
                Visit(expression.Arguments[0]);
                Write("||?)");
                _variables.AddVariable("%");
                return expression;
            }

            if (expression.Method.Equals(MethodTranslator.StringStartsWith))
            {
                Write("(");
                Visit(expression.Object);
                Write(" like ");
                Visit(expression.Arguments[0]);
                Write("||?)");
                _variables.AddVariable("%");
                return expression;
            }

            if (expression.Method.Equals(MethodTranslator.StringEndsWith))
            {
                Write("(");
                Visit(expression.Object);
                Write(" like ?||");
                _variables.AddVariable("%");
                Visit(expression.Arguments[0]);
                Write(")");
                return expression;
            }


            return base.VisitMethodCall(expression); // throws
        }

        protected override Expression VisitSubQuery(SubQueryExpression expression)
        {
            var c = expression?.QueryModel?.MainFromClause?.FromExpression as ConstantExpression;
            if (c != null)
            {
                Write("(");
                var values = ((IEnumerable) c.Value).Cast<object>().ToList();
                var o = expression?.QueryModel?.ResultOperators.First() as ContainsResultOperator;
                bool first = true;
                foreach (var v in values)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        Write(" OR ");
                    }
                    Visit(o.Item);
                    Write(" = ?");
                    _variables.AddVariable(v);
                }
                Write(")");
                return expression;
            }

            return base.VisitSubQuery(expression);
        }

        // Called when a LINQ expression type is not handled above.
        protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod)
        {
            string itemText = unhandledItem.ToString();
            var message = $"The expression '{itemText}' via '{visitMethod}' (type: {typeof(T)}) is not supported by this LINQ provider.";
            return new NotSupportedException(message);
        }
    }
}
