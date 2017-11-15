using Starcounter;

// ReSharper disable once CheckNamespace
namespace StarcounterLinqUnitTests
{
    [Database]
    public class Employee : Person
    {
        public Department Department { get; set; }
    }
}