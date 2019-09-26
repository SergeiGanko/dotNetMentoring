/*
 * 1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.
 * Each Task should iterate from 1 to 1000 and print into the console the following string:
 * “Task #0 – {iteration number}”.
 */
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MultiThreading.Task1._100Tasks
{
    public class Program
    {
        private const int TaskAmount = 100;
        private const int MaxIterationsCount = 1000;

        public static async Task Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. Multi threading V1.");
            Console.WriteLine("1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.");
            Console.WriteLine("Each Task should iterate from 1 to 1000 and print into the console the following string:");
            Console.WriteLine("“Task #0 – {iteration number}”.");
            Console.WriteLine();

            var sw = new Stopwatch();

            sw.Start();
            await HundredTasks();
            sw.Stop();
            var ordinaryForLoopResult = sw.Elapsed;

            sw.Reset();

            sw.Start();
            await HundredTasksUsingParallelFor();
            sw.Stop();
            var parallelForResult = sw.Elapsed;

            Console.WriteLine($"Total elapsed time: {ordinaryForLoopResult} - for loop");
            Console.WriteLine($"Total elapsed time: {parallelForResult} - Parallel.For");

            Console.ReadLine();
        }

        private static async Task HundredTasks()
        {
            var hundredTasks = new Task[TaskAmount];

            for (var i = 0; i < hundredTasks.Length; i++)
            {
                var taskNumber = i;
                hundredTasks[i] = Task.Run(() =>
                {
                    for (var iterationNumber = 1; iterationNumber <= MaxIterationsCount; iterationNumber++)
                    {
                        Output(taskNumber, iterationNumber);
                    }
                });
            }

            await Task.WhenAll(hundredTasks);
        }

        private static async Task HundredTasksUsingParallelFor()
        {
            var hundredTasks = new Task[TaskAmount];

            for (var i = 0; i < hundredTasks.Length; i++)
            {
                var taskNumber = i;
                hundredTasks[i] = Task.Run(() =>
                {
                    Parallel.For(1, MaxIterationsCount,
                        iterationNumber => { Output(taskNumber, iterationNumber); });
                });
            };

            await Task.WhenAll(hundredTasks);
        }

        private static void Output(int taskNumber, int iterationNumber)
        {
            Console.WriteLine($"Task #{taskNumber} – {iterationNumber}");
        }
    }
}
