using System.Linq;
using Starcounter.Nova;
using Xunit;
using static Starcounter.Linq.DbLinq;

namespace Starcounter.Linq.QueryTests
{
    [Collection("Data tests")]
    public class CustomMethodsTests : IClassFixture<DataTestFixture>
    {
        private readonly DataTestFixture _fixture;

        public CustomMethodsTests(DataTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Delete_Predicate()
        {
            Db.Transact(() =>
            {
                Objects<Person>().Delete(x => x.Age > 40);
            });
            Db.Transact(() =>
            {
                var cnt = Objects<Person>().Count();
                Assert.Equal(1, cnt);
            });
            _fixture.RecreateData();
        }

        [Fact]
        public void Delete_ByObjectNo()
        {
            Scheduling.RunTask(() =>
            {
                Db.Transact(() =>
                {
                    Objects<Person>().Delete(x => x.GetObjectNo() > 0);
                });
                var cnt = Objects<Person>().Count();
                Assert.Equal(0, cnt);
                _fixture.RecreateData();
            }).Wait();
        }

        [Fact]
        public void DeleteAll()
        {
            Db.Transact(() =>
            {
                Objects<Person>().DeleteAll();
            });
            Db.Transact(() =>
            {
                Assert.False(Objects<Person>().Any());
            });
            _fixture.RecreateData();
        }
    }
}