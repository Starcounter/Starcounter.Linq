using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Starcounter.Linq.Helpers
{
    internal static class ReflectionHelper
    {
        private static MethodInfo _dbSlowSqlMethodBase;

        public static MethodInfo DbSlowSqlMethodBase =>
            _dbSlowSqlMethodBase ??
            (_dbSlowSqlMethodBase = typeof(Db).GetMethods(BindingFlags.Public | BindingFlags.Static)
                                              .Where(x => x.Name == nameof(Db.SlowSQL))
                                              .Single(x => x.IsGenericMethod));

        public static MethodInfo GetEnumerableCastMethod(Type itemsType)
        {
            return typeof(ReflectionHelper).GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Single(x => x.Name == nameof(CastEnumerableItems))
                .MakeGenericMethod(itemsType);
        }

        public static IEnumerable<T> CastEnumerableItems<T>(IEnumerable source)
        {
            foreach(object current in source)
            {
                var tt = (dynamic) current;
                yield return (T)tt;
            }
        }
    }
}
