using System.Text;
using static Microsoft.Graph.Constants;

namespace M356MigrationAPI.Utils
{
    public class FlowClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _environnmentsURL = "https://prod-32.westeurope.logic.azure.com:443/workflows/629cfc55fe9f497a85a78c924bdae2a0/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=TQ4V0ktWiA2BwZZCksnbttFxAha59fYhEb6mStktASk";
        private readonly string _flowsURL = "https://prod-192.westeurope.logic.azure.com:443/workflows/dd38bb7ec34f434e94e054b5f652e5d2/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=RxTy0n4rTv8DXb9MyOOZZLlNxjnuyTqHscAEL2XWtNw";

        public FlowClient()
        {
            _httpClient = new HttpClient ();
        }

        public async Task<string> GetEnvironmentsAsync()
        {
            var response = await _httpClient.GetStringAsync(_environnmentsURL);
            return response;
        }

        public async Task<string> GetFlowsAsync(string name)
        {
            Console.WriteLine(name);
            var content = new StringContent($"{{\"name\":\"{name}\"}}", Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_flowsURL, content);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
    }
}
