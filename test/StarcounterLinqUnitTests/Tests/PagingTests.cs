using System.Linq;
using Xunit;
using static Starcounter.Linq.DbLinq;

namespace StarcounterLinqUnitTests.Tests
{
    public class PagingTests : BaseTest, IClassFixture<BaseTestsFixture>
    {
        public PagingTests(BaseTestsFixture fixture)
        {
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void Take(Mode mode)
        {
            Run(() =>
            {
                var persons = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Take(1))().ToList()
                    : Objects<Person>().Take(1).ToList();
                Assert.Equal(1, persons.Count);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void Skip(Mode mode)
        {
            Run(() =>
            {
                var persons = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Skip(1))().ToList()
                    : Objects<Person>().Skip(1).ToList();
                Assert.Equal(1, persons.Count);
            });
        }
    }
}