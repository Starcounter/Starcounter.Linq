using System.Linq;
using Xunit;
using static Starcounter.Linq.DbLinq;
using static Starcounter.Linq.SqlTests.Utils;

namespace Starcounter.Linq.SqlTests
{
    public class SelectTests
    {
        [Fact]
        public void SelectObjectDirectly()
        {
            Assert.Equal("SELECT D FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Department\" D",
                Sql(() => Objects<Department>().Select(x => x)));
        }

        [Fact]
        public void SelectObjectProperty()
        {
            Assert.Equal("SELECT D.\"Company\" FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Department\" D",
                Sql(() => Objects<Department>().Select(x => x.Company)));
        }

        [Fact]
        public void SelectStringProperty()
        {
            Assert.Equal("SELECT D.\"Name\" FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Department\" D",
                Sql(() => Objects<Department>().Select(x => x.Name)));
        }

        [Fact]
        public void SelectEmbeddedProperty()
        {
            Assert.Equal("SELECT D.\"Company\".\"Name\" FROM \"Starcounter\".\"Linq\".\"SqlTests\".\"Department\" D",
                Sql(() => Objects<Department>().Select(x => x.Company.Name)));
        }
    }
}
