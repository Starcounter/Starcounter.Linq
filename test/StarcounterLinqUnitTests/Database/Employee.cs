using Starcounter.Nova;

// ReSharper disable once CheckNamespace
namespace StarcounterLinqUnitTests
{
    [Database]
    public abstract class Employee : Person
    {
        public abstract Department Department { get; set; }
        public abstract bool Disabled { get; set; }
    }
}