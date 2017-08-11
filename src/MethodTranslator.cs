using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace PoS.Infra
{
    public class MethodTranslator
    {
        private static MethodInfo MethodFromExample<T>(Expression<Func<T>> fun)
        {
            var call = fun.Body as MethodCallExpression;
            var method = call.Method;
            if (method.IsGenericMethod)
            {
                method = method.GetGenericMethodDefinition();
            }
            return method;
        }


        public static readonly MethodInfo EnumerableContains = MethodFromExample(() => Enumerable.Contains(null, 0));
        public static readonly MethodInfo StringContains = MethodFromExample(() => "".Contains(""));
        public static readonly MethodInfo StringStartsWith = MethodFromExample(() => "".StartsWith(""));
        public static readonly MethodInfo StringEndsWith = MethodFromExample(() => "".EndsWith(""));
    }
}