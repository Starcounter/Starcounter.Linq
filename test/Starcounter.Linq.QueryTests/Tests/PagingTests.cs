using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Starcounter.Linq.DbLinq;

namespace Starcounter.Linq.QueryTests
{
    public class PagingTests : IClassFixture<BaseTestsFixture>
    {
        private readonly BaseTestsFixture _fixture;

        public PagingTests(BaseTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void Take(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var persons = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Take(1))().ToList()
                    : Objects<Person>().Take(1).ToList();
                Assert.Equal(1, persons.Count);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void Skip(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                var persons = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().Skip(1))().ToList()
                    : Objects<Person>().Skip(1).ToList();
                Assert.Equal(1, persons.Count);
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void OrderBy(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                List<Person> people = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().OrderBy(x => x.Age))().ToList()
                    : Objects<Person>().OrderBy(x => x.Age).ToList();
                Assert.Equal(2, people.Count);
                Assert.True(people[0].GetObjectNo() < people[1].GetObjectNo());
            }).Wait();
        }

        [Theory]
        [InlineData(Mode.AdHoc)]
        [InlineData(Mode.CompiledQuery)]
        public void OrderBy_ObjectNo(Mode mode)
        {
            Scheduling.RunTask(() =>
            {
                List<Person> people = mode == Mode.CompiledQuery
                    ? CompileQuery(() => Objects<Person>().OrderByDescending(x => x.GetObjectNo()))().ToList()
                    : Objects<Person>().OrderByDescending(x => x.GetObjectNo()).ToList();
                Assert.Equal(2, people.Count);
                Assert.True(people[0].GetObjectNo() > people[1].GetObjectNo());
            }).Wait();
        }
    }
}