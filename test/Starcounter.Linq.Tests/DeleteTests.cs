using System.Linq;
using Xunit;
using static Starcounter.Linq.DbLinq;
using static Starcounter.Linq.Tests.Utils;

namespace Starcounter.Linq.Tests
{
    public class DeleteTests
    {
        [Fact]
        public void Delete_IntegerComparison()
        {
            Assert.Equal("DELETE FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" WHERE ((\"Age\" > ?))",
                Sql<Person>(() => Objects<Person>().Delete(x => x.Age > 0)));
        }

        [Fact]
        public void Delete_ObjectIsNull()
        {
            Assert.Equal("DELETE FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" WHERE ((\"Department\" IS NULL))",
                Sql<Employee>(() => Objects<Employee>().Delete(p => p.Department == null)));
        }

        [Fact]
        public void Delete_ObjectIsNotNullVariable()
        {
            Department department = null;
            Assert.Equal("DELETE FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" WHERE ((\"Department\" IS NOT NULL))",
                Sql<Employee>(() => Objects<Employee>().Delete(p => p.Department != department)));
        }

        [Fact]
        public void Delete_NestedValueComparison()
        {
            Assert.Equal("DELETE FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" WHERE ((\"Department\".\"Company\".\"Index\" = ?))",
                Sql<Employee>(() => Objects<Employee>().Delete(p => p.Department.Company.Index == 0)));
        }

        [Fact]
        public void Delete_EnumerableContains()
        {
            var ages = new[] { 41, 42, 43 };
            Assert.Equal("DELETE FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" WHERE (((\"Age\" = ?) OR (\"Age\" = ?) OR (\"Age\" = ?)))",
                Sql<Employee>(() => Objects<Employee>().Delete(p => ages.Contains(p.Age))));
        }

        [Fact]
        public void DeleteAll()
        {
            Assert.Equal("DELETE FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\"",
                Sql<Person>(() => Objects<Person>().DeleteAll()));
        }
    }
}