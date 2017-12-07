using System.Linq;
using Starcounter;
using Xunit;
using static Starcounter.Linq.DbLinq;

namespace StarcounterLinqUnitTests.Tests
{
    public class CustomMethodsTests : IClassFixture<BaseTestsFixture>
    {
        private readonly BaseTestsFixture Fixture;

        public CustomMethodsTests(BaseTestsFixture fixture)
        {
            Fixture = fixture;
        }

        [Fact]
        public void Delete_Predicate()
        {
            Scheduling.RunTask(() =>
            {
                Db.Transact(() =>
                {
                    Objects<Person>().Delete(x => x.Age > 40);
                });
                var cnt = Objects<Person>().Count();
                Assert.Equal(1, cnt);
                Fixture.RecreateData();
            }).Wait();
        }

        [Fact]
        public void DeleteAll()
        {
            Scheduling.RunTask(() =>
            {
                Db.Transact(() =>
                {
                    Objects<Person>().DeleteAll();
                });
                Assert.False(Objects<Person>().Any());
                Fixture.RecreateData();
            }).Wait();
        }
    }
}