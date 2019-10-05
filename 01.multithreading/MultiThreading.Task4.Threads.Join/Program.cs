/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        private static Semaphore _pool;
        private static int _numberOfThreads = 10;

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            ConsoleKeyInfo cki;
            do
            {
                Console.WriteLine("******** MENU ********");
                Console.WriteLine("a) - Use Thread class for this task and Join for waiting threads.");
                Console.WriteLine("b) - ThreadPool class for this task and Semaphore for waiting threads.");
                Console.WriteLine("ESC - Exit");
                cki = Console.ReadKey(false);
                switch (cki.KeyChar.ToString())
                {
                    case "a":
                        Console.Clear();
                        CreateThreads(_numberOfThreads);
                        break;
                    case "b":
                        Console.Clear();
                        _pool = new Semaphore(3, 3);
                        CreateThreadsUsingThreadPool(_numberOfThreads);
                        break;
                }
            } while (cki.Key != ConsoleKey.Escape);

            Console.ReadLine();
        }

        private static void CreateThreads(int n)
        {
            if (n == 0)
            {
                return;
            }

            Thread thread = new Thread(() =>
            {
                Console.WriteLine($"Thread #{n} is doing some work");
                Interlocked.Decrement(ref n);
            });
            
            thread.Start();

            thread.Join();

            CreateThreads(n);
        }

        private static void CreateThreadsUsingThreadPool(int n)
        {
            if (n == 0)
            {
                return;
            }

            ThreadPool.QueueUserWorkItem(ThreadPoolCallback, n);
            Interlocked.Decrement(ref n);
            CreateThreadsUsingThreadPool(n);
        }

        public static void ThreadPoolCallback(object threadContext)
        {
            int threadIndex = (int)threadContext;
            Console.WriteLine($"Thread #{threadIndex} begins and waits for the semaphore");
            _pool.WaitOne();
            Console.WriteLine($"Thread #{threadIndex} enters the semaphore");
            Thread.Sleep(3000);
            Console.WriteLine($"Thread #{threadIndex} releases the semaphore. Previous semaphore count: {_pool.Release()}");
        }
    }
}
