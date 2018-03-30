namespace Starcounter.Linq.SqlTests
{
    public class Department
    {
        public Company Company { get; set; }
        public string Name { get; set; }
        public bool Global { get; set; }
    }
}