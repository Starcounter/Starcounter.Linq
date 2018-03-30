

// ReSharper disable once CheckNamespace
namespace Starcounter.Linq.QueryTests
{
    [Database]
    public class Employee : Person
    {
        public Department Department { get; set; }
        public bool Disabled { get; set; }
    }
}