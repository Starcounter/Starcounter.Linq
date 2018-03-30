using System.Linq;
using Xunit;
using static Starcounter.Linq.DbLinq;

namespace Starcounter.Linq.QueryTests
{
    public class CustomMethodsTests : IClassFixture<BaseTestsFixture>
    {
        private readonly BaseTestsFixture _fixture;

        public CustomMethodsTests(BaseTestsFixture fixture)
        {
            _fixture = fixture;
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
                _fixture.RecreateData();
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
                _fixture.RecreateData();
            }).Wait();
        }
    }
}