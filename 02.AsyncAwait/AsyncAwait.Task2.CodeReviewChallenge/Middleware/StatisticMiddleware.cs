using AsyncAwait.CodeReviewChallenge.Headers;
using AsyncAwait.CodeReviewChallenge.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AsyncAwait.CodeReviewChallenge.Middleware
{
    public class StatisticMiddleware
    {
        private readonly RequestDelegate _next;

        public StatisticMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {   
            string path = context.Request.Path;
            GrandStatistics.RegisterVisitAsync(path);

            context.Response.Headers.Add(
            CustomHttpHeaders.TotalPageVisits,
            GrandStatistics.GetVisitsCountAsync(path).GetAwaiter().GetResult().ToString());            

            await _next(context);
        }
    }
}
