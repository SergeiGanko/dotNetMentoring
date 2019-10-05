using AsyncAwait.CodeReviewChallenge.Services;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.CodeReviewChallenge.Models.Help
{
    public class ManualAssistent : IAssistent
    {
        public async Task<string> RequestAssistanceAsync(string requestorInfo)
        {
            try
            {
                DataService.RegisterAssistanceRequestAsync(requestorInfo);
                Thread.Sleep(7000); // this is just to be sure that the request is registered
                string availableAssistentsInfo = await DataService.GetAvailableAssistenseAsync().ConfigureAwait(false);

                var sb = new StringBuilder();
                sb.AppendLine($"Dear user, the assistance request is registered.");
                sb.AppendLine($"Please find the list of available assistents below: ");
                
                return sb.ToString();
            }
            catch (HttpRequestException ex)
            {
                return $"Failed to register assistance request. Please try later. {ex.Message}";
            }
        }
    }
}
