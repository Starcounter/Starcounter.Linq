using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Starcounter.Linq.Helpers
{
    internal static class ReflectionHelper
    {
        public static MethodInfo GetEnumerableCastMethod(Type itemsType)
        {
            return typeof(ReflectionHelper).GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Single(x => x.Name == nameof(CastEnumerableItems))
                .MakeGenericMethod(itemsType);
        }

        public static IEnumerable<T> CastEnumerableItems<T>(IEnumerable source)
        {
            var type = typeof(T);
            foreach (object item in source)
            {
                yield return (T)CastHelper.Convert(item, type);
            }
        }
    }
}
