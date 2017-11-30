using System.Linq;
using Xunit;
using static Starcounter.Linq.DbLinq;
using static Starcounter.Linq.Tests.Utils;
namespace Starcounter.Linq.Tests
{
    public class WhereTests
    {

        [Fact]
        public void WhereEquals()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE ((P.\"Name\" = ?))",
                Sql(() => Objects<Person>().Where(p => p.Name == "XXX")));
        }

        [Fact]
        public void MultipleWhereEquals()
        {
            var name = "XXX";
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE ((P.\"Name\" = ?)) AND ((P.\"Age\" = ?))",
                Sql(() => Objects<Person>().Where(p => p.Name == name).Where(p => p.Age == 1)));
        }

        [Fact]
        public void WhereOrEquals()
        {
            var name = "XXX";
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE (((P.\"Name\" = ?) OR (P.\"Age\" = ?)))",
                Sql(() => Objects<Person>().Where(p => p.Name == name || p.Age == 1)));
        }

        [Fact]
        public void WhereAndEquals()
        {
            var name = "XXX";
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE (((P.\"Name\" = ?) AND (P.\"Age\" = ?)))",
                Sql(() => Objects<Person>().Where(p => p.Name == name && p.Age == 1)));
        }
    }
}
