using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace AsyncAwait.CodeReviewChallenge.Models
{
    public class GrandStatistics
    {
        private static ConcurrentDictionary<string, long> _statistics
            = new ConcurrentDictionary<string, long>();

        /// <summary>
        /// Registers url visit in a Grand Statistic Storage cloud.
        /// </summary>
        /// <param name="url">The visited url.</param>
        public static async Task RegisterVisitAsync(string url)
        {
            await Task.Delay(3000); // emulation of long-running operation is here
          
            _statistics.AddOrUpdate(url, 1, (key, value) => value + 1);
        }

        /// <summary>
        /// Gets the amount of visits from Grand Statistic Storage cloud.
        /// </summary>
        /// <param name="url">The visited url.</param>
        /// <returns>The amount of registered visits.</returns>
        public static async Task<long> GetVisitsCountAsync(string url)
        {
            await Task.Delay(100); // emulation of long-running operation is here

            long visits;
            _statistics.TryGetValue(url, out visits);
            return visits;
        }
    }
}
