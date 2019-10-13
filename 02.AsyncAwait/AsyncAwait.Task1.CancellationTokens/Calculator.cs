using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens
{
    public static class Calculator
    {
        public static async Task<long> Calculate(int n, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            long sum = 0;

            for (int i = 0; i < n; i++)
            {
                if (token.IsCancellationRequested)
                {
                    throw new TaskCanceledException("Task was canceled");
                }

                // i + 1 is to allow 2147483647 (Max(Int32)) 
                sum = sum + (i + 1);
                await Task.Delay(10, token);
            }

            return sum;
        }
    }
}
