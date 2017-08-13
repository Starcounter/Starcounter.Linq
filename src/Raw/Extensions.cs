using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starcounter.Linq.Raw
{
    public static class Extensions
    {
        public static string SourceName(this Type self)
        {
            return self.Name[0].ToString();
        }
    }
}
