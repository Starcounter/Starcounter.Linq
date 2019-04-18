using System;
using System.Collections.Generic;

namespace Starcounter.Linq.Helpers
{
    public static class CastHelper
    {
        private static readonly Dictionary<Type, Func<object, object>> converters = new Dictionary<Type, Func<object, object>>
        {
            {typeof(bool), x => System.Convert.ToBoolean(x)},
            {typeof(sbyte), x => System.Convert.ToSByte(x)},
            {typeof(short), x => System.Convert.ToInt16(x)},
            {typeof(int), x => System.Convert.ToInt32(x)},
            {typeof(long), x => System.Convert.ToInt64(x)},
            {typeof(byte), x => System.Convert.ToByte(x)},
            {typeof(ushort), x => System.Convert.ToUInt16(x)},
            {typeof(uint), x => System.Convert.ToUInt32(x)},
            {typeof(ulong), x => System.Convert.ToUInt64(x)},
            {typeof(float), x => System.Convert.ToSingle(x)},
            {typeof(double), x => System.Convert.ToDouble(x)},
            {typeof(decimal), x => System.Convert.ToDecimal(x)},
            {typeof(string), System.Convert.ToString}
        };

        /// <remarks>
        /// SC lifts underlying types to a bigger ones in some cases.
        /// Look at the issue https://github.com/Starcounter/Home/issues/209 for getting more info.
        /// </remarks>
        public static object Convert(object value, Type destinationType)
        {
            if (value == null)
                return null;

            var resultType = value.GetType();
            if (resultType != destinationType && !resultType.IsSubclassOf(destinationType) &&
                converters.TryGetValue(destinationType, out var convert))
            {
                return convert(value);
            }
            return value;
        }
    }
}
