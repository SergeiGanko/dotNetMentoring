using AsyncAwait.CodeReviewChallenge.Middleware;
using Microsoft.AspNetCore.Builder;

namespace AsyncAwait.CodeReviewChallenge.Extensions
{
    public static class LoggingRequestMiddlewareExtensions
    {
        public static IApplicationBuilder UseStatistic(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<StatisticMiddleware>();
        }
    }
}
