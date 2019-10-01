using System;
using System.Collections.Generic;
using System.Threading;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    public static class TwoWaySignaling
    {
        private static readonly List<int> _shared = new List<int>();
        private static readonly EventWaitHandle _fill = new AutoResetEvent(false);
        private static readonly EventWaitHandle _print = new AutoResetEvent(false);

        public static void Run()
        {
            var firstThread = new Thread(FillCollection);
            var secondThread = new Thread(PrintCollection);
            firstThread.Start();
            secondThread.Start();

            firstThread.Join();
            secondThread.Join();
        }

        private static void FillCollection()
        {
            for (int i = 0; i < 10; i++)
            {
                _print.WaitOne(); // Wait for signal that printer is ready to print
                Console.WriteLine($"Adding {i}...");
                Thread.Sleep(TimeSpan.FromSeconds(1));
                _shared.Add(i);
                _fill.Set(); // Signal to printer that item is added
                _print.WaitOne(); // Wait for signal from printer
            }
        }

        private static void PrintCollection()
        {
            while (true)
            {
                _print.Set(); // Signal that printer is ready
                _fill.WaitOne(); // Wait for signal from filler
                foreach (var i in _shared)
                {
                    Console.Write($"{i}, ");
                }
                Console.WriteLine("...Printed!\n");

                if (_shared.Count == 10)
                {
                    return;
                }
                _print.Set(); // Signal that print is done
            }
        }
    }
}
