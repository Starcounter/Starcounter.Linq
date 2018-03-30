using System.Linq;
using Starcounter.Nova;
using Xunit;
using static Starcounter.Linq.DbLinq;

namespace Starcounter.Linq.QueryTests
{
    [Collection("Data tests")]
    public class PagingTests : IClassFixture<DataTestFixture>
    {
        private readonly DataTestFixture _fixture;

        public PagingTests(DataTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void Take(Mode mode)
        {
            Db.Transact(() =>
            {
                var persons = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Take(1))().ToList()
                    : Objects<Person>().Take(1).ToList();
                Assert.Single(persons);
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void Skip(Mode mode)
        {
            Db.Transact(() =>
            {
                var persons = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Skip(1))().ToList()
                    : Objects<Person>().Skip(1).ToList();
                Assert.Single(persons);
            });
        }
    }
}