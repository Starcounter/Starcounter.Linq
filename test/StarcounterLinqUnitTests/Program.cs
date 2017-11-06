using Starcounter.Xunit.Runner;

namespace StarcounterLinqUnitTests
{
    class Program
    {
        static void Main()
        {
            var testRunner = new StarcounterXunitRunner();
            testRunner.Start();
        }
    }
}