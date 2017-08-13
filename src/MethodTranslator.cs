using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Starcounter.Linq.Raw;

namespace Starcounter.Linq
{
    public static class MethodTranslator<T>
    {
        private static readonly IQueryable<T> IQueryable = null;
        private static readonly Queryable<T> Queryable = null;

        public static readonly MethodInfo QueryableFirstOrDefault = MethodFromExample(() => Queryable.FirstOrDefault());
        public static readonly MethodInfo QueryableFirstOrDefaultPred = MethodFromExample(() => Queryable.FirstOrDefault(i => true));
        public static readonly MethodInfo IQueryableFirstOrDefault = MethodFromExample(() => IQueryable.FirstOrDefault());
        public static readonly MethodInfo IQueryableFirstOrDefaultPred = MethodFromExample(() => IQueryable.FirstOrDefault(i => true));
        public static readonly MethodInfo QueryableWhere = MethodFromExample(() => IQueryable.Where(i => true));

        private static MethodInfo MethodFromExample<T>(Expression<Func<T>> fun)
        {

            var call = fun.Body as MethodCallExpression;
            var method = call.Method;
            if (method.IsGenericMethod)
                method = method.GetGenericMethodDefinition();
            return method;
        }
    }
    public class MethodTranslator
    {
        private static readonly int[] Enumerable = null;

        public static readonly MethodInfo EnumerableContains = MethodFromExample(() => Enumerable.Contains(0));
        public static readonly MethodInfo StringContains = MethodFromExample(() => "".Contains(""));
        public static readonly MethodInfo StringStartsWith = MethodFromExample(() => "".StartsWith(""));
        public static readonly MethodInfo StringEndsWith = MethodFromExample(() => "".EndsWith(""));

        private static MethodInfo MethodFromExample<T>(Expression<Func<T>> fun)
        {
            
            var call = fun.Body as MethodCallExpression;
            var method = call.Method;
            if (method.IsGenericMethod)
                method = method.GetGenericMethodDefinition();
            return method;
        }
    }
}