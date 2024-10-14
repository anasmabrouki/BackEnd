using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using M356MigrationAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class GraphClient
{
    private readonly HttpClient _httpClient;
    private readonly string _migrationURL = "https://prod-22.westeurope.logic.azure.com:443/workflows/92aa023111334d6da3083c03385afba1/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=4SDtQZofjXymJeGhxcZdNyyeTFCEh3o3x4pzwzTYk6A";
    public GraphClient()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> GetSharepointSites(string jwt)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/sites?search=");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetSharepointLists(string jwt, string siteId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://graph.microsoft.com/v1.0/sites/{siteId}/lists");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    public async Task MigrateListAsync(string sourceJwt, string targetJwt, string sourceSiteUrl, string targetSiteUrl, List<ListGuid> lists)
    {
        var requestBody = new
        {
            sourceJwt = sourceJwt,
            targetJwt = targetJwt,
            sourceSiteId = sourceSiteUrl,
            targetSiteId = targetSiteUrl,
            listGuids = lists.Select(list => new { url = list.url, name = list.name }).ToList()
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

        // Send the POST request
        var response = await _httpClient.PostAsync(_migrationURL, jsonContent);

        // Ensure the request was successful
        response.EnsureSuccessStatusCode();


    }
}
