using System.Collections.Generic;
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

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void OrderBy(Mode mode)
        {
            Db.Transact(() =>
            {
                List<Person> people = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().OrderBy(x => x.Age))().ToList()
                    : Objects<Person>().OrderBy(x => x.Age).ToList();
                Assert.Equal(2, people.Count);
                Assert.True(Db.GetOid(people[0]) < Db.GetOid(people[1]));
            });
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void OrderBy_Oid(Mode mode)
        {
            Db.Transact(() =>
            {
                List<Person> people = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().OrderByDescending(x => Db.GetOid(x)))().ToList()
                    : Objects<Person>().OrderByDescending(x => Db.GetOid(x)).ToList();
                Assert.Equal(2, people.Count);
                Assert.True(Db.GetOid(people[0]) > Db.GetOid(people[1]));
            });
        }
    }
}