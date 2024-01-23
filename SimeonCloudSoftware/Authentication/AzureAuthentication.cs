using Newtonsoft.Json;

namespace SimeonCloudSoftware.Authentication
{
    public class AzureAuthentication
    {
        private readonly string tenantId;
        private readonly string clientId;
        private readonly string clientSecret;

        /// <summary>
        /// Authentication contstructor
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        public AzureAuthentication(string tenantId, string clientId, string clientSecret)
        {
            this.tenantId = tenantId;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }
        /// <summary>
        /// Retrieves an access token from Azure Active Directory (Azure AD) using the client credentials flow.
        /// This method performs an OAuth2 client credentials authentication to Azure AD, obtaining an access token
        /// that can be used to access protected resources, such as Microsoft Graph APIs.
        /// </summary>
        /// <returns>The access token as a string. This token is required for making authenticated requests to Azure AD-secured APIs.</returns>

        public async Task<string> GetAccessTokenAsync()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token");

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials",
                ["client_id"] = clientId,
                ["client_secret"] = clientSecret,
                ["scope"] = "https://graph.microsoft.com/.default"
            });

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            dynamic jsonResponse = JsonConvert.DeserializeObject(responseBody);
            return jsonResponse.access_token;
        }
    }
}
