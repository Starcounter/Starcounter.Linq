namespace Starcounter.Linq.SqlTests
{
    public class Employee : Person
    {
        public Department Department { get; set; }
        public bool Disabled { get; set; }
    }
}