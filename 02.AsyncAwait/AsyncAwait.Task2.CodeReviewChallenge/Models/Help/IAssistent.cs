using System.Threading.Tasks;

namespace AsyncAwait.CodeReviewChallenge.Models.Help
{
    interface IAssistent
    {
        Task<string> RequestAssistanceAsync(string requestorInfo);
    }
}
