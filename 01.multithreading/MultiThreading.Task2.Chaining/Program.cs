/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    public class Program
    {
        private static readonly Random Random = new Random();
        private const int ArrayLength = 10;

        private static async Task Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            await Task.Factory.StartNew(CreateRandomNumbers)
                .ContinueWith(antecedent => MultiplyRamdomNumbers(antecedent.Result))
                .ContinueWith(antecedent => SortRandomNumbersByAscending(antecedent.Result))
                .ContinueWith(antecedent => CalculateAverage(antecedent.Result));

            Console.ReadLine();
        }

        private static int[] CreateRandomNumbers()
        {
            var randomNumbers = new int[ArrayLength];
            for (var i = 0; i < randomNumbers.Length; i++)
            {
                randomNumbers[i] = GetRandom();
            }

            Console.WriteLine("Array of 10 random numbers:");
            PrintArray(randomNumbers);

            return randomNumbers;

        }

        private static int[] MultiplyRamdomNumbers(int[] randomNumbers)
        {
            int multiplier = GetRandom();
            
            for (var i = 0; i < randomNumbers.Length; i++)
            {
                randomNumbers[i] *= multiplier;
            }

            Console.WriteLine($"Array of 10 random numbers after multiplying by {multiplier}");
            PrintArray(randomNumbers);

            return randomNumbers;
        }

        private static int[] SortRandomNumbersByAscending(int[] randomNumbers)
        {
            Array.Sort(randomNumbers);

            Console.WriteLine("Array sorted by ascending");
            PrintArray(randomNumbers);

            return randomNumbers;
        }

        private static double CalculateAverage(int[] randomNumbers)
        {
            var average = randomNumbers.Average();

            Console.WriteLine($"Average value: {average}");

            return average;
        }

        private static int GetRandom()
        {
            return Random.Next(1, 10);
        }

        private static void PrintArray(int[] randomNumbers)
        {
            foreach (var number in randomNumbers)
            {
                Console.Write($"{number}, ");
            }

            Console.WriteLine();
        }
    }
}
