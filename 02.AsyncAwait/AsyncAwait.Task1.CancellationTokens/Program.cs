﻿/*
 * Изучите код данного приложения для расчета суммы целых чисел от 0 до N, а затем
 * измените код приложения таким образом, чтобы выполнялись следующие требования:
 * 1. Расчет должен производиться асинхронно.
 * 2. N задается пользователем из консоли. Пользователь вправе внести новую границу в процессе вычислений,
 * что должно привести к перезапуску расчета.
 * 3. При перезапуске расчета приложение должно продолжить работу без каких-либо сбоев.
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens
{
    class Program
    {
        private static CancellationTokenSource _cts;

        /// <summary>
        /// The Main method should not be changed at all.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("Mentoring program L2. Async/await.V1. Task 1");
            Console.WriteLine("Calculating the sum of integers from 0 to N.");
            Console.WriteLine("Use 'q' key to exit...");
            Console.WriteLine();

            Console.WriteLine("Enter N: ");

            string input = Console.ReadLine();
            while (input.Trim().ToUpper() != "Q")
            {
                if (int.TryParse(input, out int n))
                {
                    CalculateSum(n);
                }
                else
                {
                    Console.WriteLine($"Invalid integer: '{input}'. Please try again.");
                    Console.WriteLine("Enter N: ");
                }

                input = Console.ReadLine();
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }

        private static async void CalculateSum(int n)
        {
            _cts?.Cancel();

            try
            {
                _cts = new CancellationTokenSource();
                Task<long> sumTask = Calculator.Calculate(n, _cts.Token);
                Console.WriteLine($"\nThe task for {n} started... Enter N to cancel the request:");

                long sum = await sumTask;
                Console.WriteLine($"Sum for {n} = {sum}.");
                Console.WriteLine();
                Console.WriteLine("Enter N: ");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"\n[Info - {ex.Message}]: Sum for {n} cancelled...");
            }
            finally
            {
                _cts?.Dispose();
                _cts = null;
            }
        }
    }
}