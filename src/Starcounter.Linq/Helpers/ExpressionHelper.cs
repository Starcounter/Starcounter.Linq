using System;
using System.Linq.Expressions;

namespace Starcounter.Linq.Helpers
{
    internal static class ExpressionHelper
    {
        public static object RetrieveValue(this MemberExpression node)
        {
            var objectMember = Expression.Convert(node, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            var getter = getterLambda.Compile();
            return getter();
        }
    }
}