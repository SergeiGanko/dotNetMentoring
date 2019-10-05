using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    public static class TaskRunner
    {
        private static readonly CancellationTokenSource CancelSource = new CancellationTokenSource();

        public static void RunA()
        {
            Console.WriteLine("\nA is running...");
            string message = "Hello World";
            Task.Run(() =>
            {
                Console.WriteLine("DoWork is running...");
                return DoWork(message);
            }).ContinueWith(antecedent =>
            {
                Console.WriteLine(
                    antecedent.IsFaulted
                        ? $"Continuation is executed despite the antecedent was faulted with unhandled exception: {antecedent.Exception}"
                        : $"Continuation is executed despite the antecedent was completed successfully with Result: {antecedent.Result}");
            }).Wait();
        }

        public static void RunB()
        {
            Console.WriteLine("\nB is running...");
            string message = null;
            var token = CancelSource.Token;
            var task = Task.Run(() =>
            {
                Console.WriteLine("DoWork is running...");
                return DoWork(message);
            }, token);

            task.ContinueWith(antecedent =>
            {
                Console.WriteLine($"Continuation is executed because the parent task finished without success (The antecedent TaskStatus: {antecedent.Status})");
            }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.OnlyOnCanceled);

            CancelSource.Cancel();

            try
            {
                task.Wait();
            }
            catch (AggregateException)
            {
                Console.WriteLine("Task canceled");
            }
        }

        public static void RunC()
        {
            Console.WriteLine("\nC is running...");
            string message = null;
            var task = Task.Run(() =>
            {
                Console.WriteLine("DoWork is running...");
                return DoWork(message);
            }).ContinueWith(antecedent =>
            {
                Console.WriteLine(
                    $"Continuation task is executed because the parent task is finished with fail and parent task thread is reused for continuation");
            }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

            task.Wait();
        }

        public static void RunD()
        {
            Console.WriteLine("\nD is running...");
            string message = "Hello World";
            var token = CancelSource.Token;
            var task = Task.Run(() =>
            {
                Console.WriteLine("DoWork is running...");
                DoWork(message);

            }, token).ContinueWith(antecedent =>
            {
                Console.WriteLine("Continuation task is executed outside of the thread pool when the parent task was cancelled.");
                Console.WriteLine($"IsThreadPoolThread: {Thread.CurrentThread.IsThreadPoolThread}");
            }, TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning);

            CancelSource.Cancel();

            try
            {
                task.Wait();
            }
            catch (AggregateException ex)
            {
                Console.WriteLine("Task canceled");
            }
        }

        private static string DoWork(object obj)
        {
            if (!(obj is string result))
            {
                throw new InvalidCastException("Cannot cast object to string");
            }

            return result;
        }
    }
}
