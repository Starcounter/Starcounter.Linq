using Starcounter.Xunit.Runner;

namespace StarcounterLinqUnitTests
{
    class Program
    {
        static void Main()
        {
            StarcounterXunitRunner runner = new StarcounterXunitRunner();
            runner.Start();
        }
    }
}