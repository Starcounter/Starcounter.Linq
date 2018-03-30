using System;

namespace Starcounter.Linq.QueryTests
{
    public class DataTestFixture : BaseTestsFixture, IDisposable
    {
        public DataTestFixture() : base()
        {
            DataHelper.ResetData();
            DataHelper.CreateEmployees();
        }

        public void RecreateData()
        {
            DataHelper.ResetData();
            DataHelper.CreateEmployees();
        }

        public void Dispose()
        {
            DataHelper.ResetData();
        }
    }
}