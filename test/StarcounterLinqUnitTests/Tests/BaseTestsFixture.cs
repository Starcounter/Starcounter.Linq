using StarcounterLinqUnitTests.Helpers;
using StarcounterLinqUnitTests.Utilities;

namespace StarcounterLinqUnitTests.Tests
{
    public class BaseTestsFixture : TestAppHost
    {
        public BaseTestsFixture() : base()
        {
            DataHelper.ResetData();
            DataHelper.CreateEmployees();
        }

        //public void RecreateData()
        //{
        //    DataHelper.ResetData();
        //    DataHelper.CreateEmployees();
        //}

        //public void Dispose()
        //{
        //    DataHelper.ResetData();
        //}
    }
}