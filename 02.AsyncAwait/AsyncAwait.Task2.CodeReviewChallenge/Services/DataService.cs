using System.Net.Http;
using System.Threading.Tasks;

namespace AsyncAwait.CodeReviewChallenge.Services
{
    public class DataService
    {
        private static HttpClient _httpClient = new HttpClient();

        private const string BaseApiUrl = "https://yourcustomerssite.example-api.com";

        #region public methods

        public static async Task<string> GetPrivacyDataAsync()
        {
            try
            {
                return await _httpClient.GetStringAsync($"{BaseApiUrl}/v1/privacy-2019");
            }
            catch (HttpRequestException)
            {
                return "Privacy policy: async/await is cool!";
            }
        }

        public static async void RegisterAssistanceRequestAsync(string userName)
        {
            using (HttpContent httpContent = new StringContent(userName))
            {
                await _httpClient.PutAsync($"{BaseApiUrl}/v1/assistance/reg", httpContent).ConfigureAwait(false);
            }
        }

        public static async Task<string> GetAvailableAssistenseAsync()
        {
            return await _httpClient.GetStringAsync($"{BaseApiUrl}/v1/assistantse?available=true");
        }

        #endregion
    }
}