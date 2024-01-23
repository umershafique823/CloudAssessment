using System.Net.Http.Headers;
using Newtonsoft.Json;
using SimeonCloudSoftware.Model;

namespace SimeonCloudSoftware.API
{
    /// <summary>
    /// Graph api client class
    /// </summary>
    public class GraphApiClient
    {
        private readonly string accessToken;

        public GraphApiClient(string accessToken)
        {
            this.accessToken = accessToken;
        }
        /// <summary>
        /// Retrieves a list of groups from Microsoft Graph using the provided access token.
        /// This method sends an authenticated GET request to the Microsoft Graph API endpoint
        /// to fetch a collection of groups that the authenticated user has access to.
        /// </summary>
        /// <returns>
        /// An enumerable collection of Group objects representing the groups retrieved from Microsoft Graph.
        /// </returns>
        public async Task<IEnumerable<Group>> GetGroupsAsync()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync("https://graph.microsoft.com/v1.0/groups");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            dynamic jsonResponse = JsonConvert.DeserializeObject(responseBody);
            return jsonResponse.value.ToObject<IEnumerable<Group>>();
        }
    }
}
