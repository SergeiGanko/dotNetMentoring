using System.Threading.Tasks;

namespace AsyncAwait.CodeReviewChallenge.Models.Help
{
    public class FaqAssistent : IAssistent
    {
        public async Task<string> RequestAssistanceAsync(string requestorInfo)
        {
            return await Task.Run(() => "The FAQ section is empty. Be first who asks the question: contactus@exampleservices.xyz");
        }
    }
}
