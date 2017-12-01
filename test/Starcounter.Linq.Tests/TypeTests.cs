using System.Linq;
using Xunit;
using static Starcounter.Linq.DbLinq;
using static Starcounter.Linq.Tests.Utils;

namespace Starcounter.Linq.Tests
{
    public class TypeTests
    {
        [Fact]
        public void TypeIs()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE ((P IS \"Starcounter\".\"Linq\".\"Tests\".\"Employee\"))",
                Sql(() => Objects<Person>().Where(p => p is Employee)));
        }

#pragma warning disable CS0184 // 'is' expression's given expression is never of the provided type
        [Fact]
        public void TypeIs_Nested()
        {
            Assert.Equal("SELECT D FROM \"Starcounter\".\"Linq\".\"Tests\".\"Department\" D WHERE ((D.\"Company\" IS \"Starcounter\".\"Linq\".\"Tests\".\"Company\"))",
                Sql(() => Objects<Department>().Where(d => d.Company is Company)));
        }

        [Fact]
        public void TypeIs_NestedTwice()
        {
            Assert.Equal("SELECT E FROM \"Starcounter\".\"Linq\".\"Tests\".\"Employee\" E WHERE ((E.\"Department\".\"Company\" IS \"Starcounter\".\"Linq\".\"Tests\".\"Company\"))",
                Sql(() => Objects<Employee>().Where(d => d.Department.Company is Company)));
        }

        [Fact]
        public void TypeIs_ReservedWordType()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE ((P IS \"Starcounter\".\"Linq\".\"Tests\".\"Unknown\"))",
                Sql(() => Objects<Person>().Where(p => p is Unknown)));
        }
#pragma warning restore CS0184 // 'is' expression's given expression is never of the provided type

        [Fact]
        public void TypeIsNot()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE (NOT (P IS \"Starcounter\".\"Linq\".\"Tests\".\"Employee\"))",
                Sql(() => Objects<Person>().Where(p => !(p is Employee))));
        }
    }
}