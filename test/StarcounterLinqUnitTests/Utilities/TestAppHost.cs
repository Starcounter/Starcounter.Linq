namespace StarcounterLinqUnitTests.Utilities
{
    public class TestAppHost : TempAppHost
    {
        public TestAppHost() : base(0)
        {
            Start();
        }
    }
}
