using System;

namespace Starcounter.Linq
{
    public static class Extensions
    {
        //This returns the source alias for a type.
        //e.g FROM Person P
        public static string SourceName(this Type self) => self.Name[0].ToString();
    }
}
