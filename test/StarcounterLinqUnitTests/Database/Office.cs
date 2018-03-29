using Starcounter.Nova;

namespace StarcounterLinqUnitTests
{
    [Database]
    public abstract class Office
    {
        public abstract string City { get; set; }
    }
}