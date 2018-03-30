using Starcounter.Nova;

namespace Starcounter.Linq.QueryTests
{
    [Database]
    public abstract class Office
    {
        public abstract string City { get; set; }
    }
}