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

        public static MethodInfo GetEnumerableMultiTargetsCastMethod(Type itemsType)
        {
            return typeof(ReflectionHelper).GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Single(x => x.Name == nameof(CastEnumerableMultiTargets))
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

        public static IEnumerable<T> CastEnumerableMultiTargets<T>(IEnumerable source, ConstructorInfo ctor)
        {
            PropertyInfo[] properties = null;

            foreach (var obj in source)
            {
                // TODO : make field values retrieving for all QP implementations
                if (properties == null)
                {
                    properties = obj.GetType().GetProperties();
                }

                var row = new object[properties.Length];
                for (var i = 0; i < properties.Length; i++)
                {
                    PropertyInfo propertyInfo = properties[i];
                    row[i++] = propertyInfo.GetMethod.Invoke(obj, null);
                }
                yield return (T)ctor.Invoke(null, row);
            }
        }
    }
}
