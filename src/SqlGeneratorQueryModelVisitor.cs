using System;
using System.Linq;
using System.Linq.Expressions;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;

namespace PoS.Infra
{
    public class SqlGeneratorQueryModelVisitor : QueryModelVisitorBase
    {
        public static CommandData GenerateHqlQuery(QueryModel queryModel)
        {
            var visitor = new SqlGeneratorQueryModelVisitor();
            visitor.VisitQueryModel(queryModel);
            return visitor.GetHqlCommand();
        }

        // Instead of generating an HQL string, we could also use a NHibernate ASTFactory to generate IASTNodes.
        private readonly QueryPartsAggregator _queryParts = new QueryPartsAggregator();
        private readonly QueryVariables _variables = new QueryVariables();

        public CommandData GetHqlCommand() => new CommandData(_queryParts.BuildSqlString(), _variables.GetParameters());

        public override void VisitQueryModel(QueryModel queryModel)
        {
            queryModel.SelectClause.Accept(this, queryModel);
            queryModel.MainFromClause.Accept(this, queryModel);
            VisitBodyClauses(queryModel.BodyClauses, queryModel);
            VisitResultOperators(queryModel.ResultOperators, queryModel);
        }

        public override void VisitResultOperator(ResultOperatorBase resultOperator, QueryModel queryModel, int index)
        {
            switch (resultOperator)
            {
                case FirstResultOperator _:
                    _queryParts.FetchPart = "FETCH ?";
                    _variables.AddVariable(1);
                    break;
                case CountResultOperator _:
                    _queryParts.SelectPart = string.Format($"cast(count({_queryParts.SelectPart}) as int)");
                    break;
                case SumResultOperator sum:
                    _queryParts.SelectPart = string.Format($"SUM({_queryParts.SelectPart})");
                    break;
                default:
                    throw new NotSupportedException("Only Count() result operator is showcased in this sample. Adding Sum, Min, Max is left to the reader.");
            }

            base.VisitResultOperator(resultOperator, queryModel, index);
        }

        public override void VisitMainFromClause(MainFromClause fromClause, QueryModel queryModel)
        {
            _queryParts.AddFromPart(fromClause);

            base.VisitMainFromClause(fromClause, queryModel);
        }

        public override void VisitSelectClause(SelectClause selectClause, QueryModel queryModel)
        {
            _queryParts.SelectPart = GetSqlExpression(selectClause.Selector);

            base.VisitSelectClause(selectClause, queryModel);
        }

        public override void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index)
        {
            _queryParts.AddWherePart(GetSqlExpression(whereClause.Predicate));

            base.VisitWhereClause(whereClause, queryModel, index);
        }

        public override void VisitOrderByClause(OrderByClause orderByClause, QueryModel queryModel, int index)
        {
            _queryParts.AddOrderByPart(orderByClause.Orderings.Select(o => GetSqlExpression(o.Expression)));

            base.VisitOrderByClause(orderByClause, queryModel, index);
        }

        public override void VisitJoinClause(JoinClause joinClause, QueryModel queryModel, int index)
        {
            // HQL joins work differently, need to simulate using a cross join with a where condition

            _queryParts.AddFromPart(joinClause);
            _queryParts.AddWherePart("({0} = {1})",
                GetSqlExpression(joinClause.OuterKeySelector),
                GetSqlExpression(joinClause.InnerKeySelector));

            base.VisitJoinClause(joinClause, queryModel, index);
        }

        public override void VisitAdditionalFromClause(AdditionalFromClause fromClause, QueryModel queryModel, int index)
        {
            _queryParts.AddFromPart(fromClause);

            base.VisitAdditionalFromClause(fromClause, queryModel, index);
        }

        public override void VisitGroupJoinClause(GroupJoinClause groupJoinClause, QueryModel queryModel, int index)
        {
            throw new NotSupportedException("Adding a join ... into ... implementation to the query provider is left to the reader for extra points.");
        }

        private string GetSqlExpression(Expression expression) => SqlGeneratorExpressionTreeVisitor.GetSqlExpression(expression, _variables);
    }
}
