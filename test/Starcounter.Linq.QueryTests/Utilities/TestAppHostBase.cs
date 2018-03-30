using System;
using System.Diagnostics;
using System.Threading;

namespace Starcounter.Linq.QueryTests.Utilities
{
    public class TestAppHostBase
    {
        static TestAppHostBase()
        {
            PromptAttachDebuggerIfNotAttached();
        }

        public static void PromptAttachIfConfigured()
        {
            var x = Environment.GetEnvironmentVariable("SCCORE_DEBUG_TESTS");
            if (!string.IsNullOrEmpty(x))
            {
                int wait = 60;
                int pid = Process.GetCurrentProcess().Id;
                while (wait > 0)
                {
                    if (Debugger.IsAttached)
                        break;
                    if (wait % 10 == 0)
                        Console.WriteLine("Waiting {0}s for debugger to attach to PID {1}.", wait, pid);
                    Thread.Sleep(1000);
                    wait--;
                }
                Console.WriteLine("Resuming execution.");
            }
        }

        public static void PromptAttachDebuggerIfNotAttached()
        {
            if (!Debugger.IsAttached)
            {
                PromptAttachIfConfigured();
            }
        }
    }
}