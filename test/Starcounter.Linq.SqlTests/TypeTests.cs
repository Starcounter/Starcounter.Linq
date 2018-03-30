using System.Linq;
using Xunit;
using static Starcounter.Linq.DbLinq;
using static Starcounter.Linq.SqlTests.Utils;

namespace Starcounter.Linq.SqlTests
{
    public class TypeTests
    {
        [Fact]
        public void TypeIs()
        {
            Assert.Equal((string)"SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE ((P IS \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\"))",
                (string)Sql(() => Objects<Person>().Where(p => p is Employee)));
        }

#pragma warning disable CS0184 // 'is' expression's given expression is never of the provided type
        [Fact]
        public void TypeIs_Nested()
        {
            Assert.Equal((string)"SELECT D FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Department\" D WHERE ((D.\"Company\" IS \"Starcounter\".\"Linq\".\"SqlTests\".\"Company\"))",
                (string)Sql(() => Objects<Department>().Where(d => d.Company is Company)));
        }

        [Fact]
        public void TypeIs_NestedTwice()
        {
            Assert.Equal((string)"SELECT E FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\" E WHERE ((E.\"Department\".\"Company\" IS \"Starcounter\".\"Linq\".\"SqlTests\".\"Company\"))",
                (string)Sql(() => Objects<Employee>().Where(d => d.Department.Company is Company)));
        }

        [Fact]
        public void TypeIs_ReservedWordType()
        {
            Assert.Equal((string)"SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE ((P IS \"Starcounter\".\"Linq\".\"SqlTests\".\"Unknown\"))",
                (string)Sql(() => Objects<Person>().Where(p => p is Unknown)));
        }
#pragma warning restore CS0184 // 'is' expression's given expression is never of the provided type

        [Fact]
        public void TypeIsNot()
        {
            Assert.Equal((string)"SELECT P FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Person\" P WHERE (NOT (P IS \"Starcounter\".\"Linq\".\"SqlTests\".\"Employee\"))",
                (string)Sql(() => Objects<Person>().Where(p => !(p is Employee))));
        }
    }
}