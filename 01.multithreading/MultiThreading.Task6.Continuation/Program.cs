/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            ShowMenu();

            Console.ReadLine();
        }

        private static void ShowMenu()
        {
            ConsoleKeyInfo cki;
            do
            {
                Console.WriteLine("\n******** MENU ********");
                Console.WriteLine("a) - Continuation task should be executed regardless of the result of the parent task.");
                Console.WriteLine("b) - Continuation task should be executed when the parent task finished without success.");
                Console.WriteLine("c) - Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
                Console.WriteLine("d) - Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
                Console.WriteLine("ESC - Exit");
                cki = Console.ReadKey(false);
                Console.Clear();

                switch (cki.KeyChar.ToString())
                {
                    case "a":
                        TaskRunner.RunA();
                        break;
                    case "b":
                        TaskRunner.RunB();
                        break;
                    case "c":
                        TaskRunner.RunC();
                        break;
                    case "d":
                        TaskRunner.RunD();
                        break;
                }
            } while (cki.Key != ConsoleKey.Escape);
        }
    }
}
