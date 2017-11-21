using System;
using Starcounter;

namespace StarcounterLinqUnitTests.Tests
{
    public class BaseTest
    {
        public void Run(Action action)
        {
            Exception exception = null;
            Scheduling.ScheduleTask(() =>
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    exception = e;
                }
            }, waitForCompletion: true);
            if (exception != null)
            {
                throw exception;    // throwing exactly out of Scheduler, otherwise it breaks the process
            }
        }
    }
}