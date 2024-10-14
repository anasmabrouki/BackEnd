using Azure.Core;
using System.Net.Http.Headers;
using System.Text;


namespace M356MigrationAPI.Utils
{
    public class AppsClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _powerAppsURL = "https://prod-96.westeurope.logic.azure.com:443/workflows/ecccab3bbc414656886eb46faa6edad2/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=uVa00voYhX8y2D6l8DsamXYRYdXV73IpUO3FATwpCK4";

        public AppsClient()
        {
            _httpClient = new HttpClient ();
        }

        public async Task<string> GetPowerAppsAsync(string name)
        {
            var content = new StringContent($"{{\"name\":\"{name}\"}}", Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_powerAppsURL, content);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

        public async Task ExportPowerAppsAsync(string jwt, string appId)
        {
            Console.WriteLine("Exporting.....");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            var url = $"https://api.powerapps.com/providers/Microsoft.PowerApps/apps/{appId}?api-version=2016-11-01";

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            await System.IO.File.WriteAllTextAsync($"temp/{appId}.json", content);
        }

        public async Task ImportPowerAppAsync(string jwt, string appId)
        {
            Console.WriteLine("Importing.....");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            var url = "https://api.powerapps.com/providers/Microsoft.PowerApps/apps?api-version=2016-11-01";

            var json = await System.IO.File.ReadAllTextAsync($"temp/{appId}.json");
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);
        }

    }
}
