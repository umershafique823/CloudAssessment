using System.Net;
using Moq;
using Moq.Protected;
using Xunit;
using SimeonCloudSoftware.API;
using SimeonCloudSoftware.Authentication;

namespace UnitTests
{
	public class GraphApiClientTests : Credentials
	{
		private Mock<HttpMessageHandler> mockHttpMessageHandler;
		private HttpClient httpClient;

		public GraphApiClientTests() : base()
		{
			mockHttpMessageHandler = new Mock<HttpMessageHandler>();
			httpClient = new HttpClient(mockHttpMessageHandler.Object);
		}

		/// <summary>
		/// This test case checks if the GetGroupsAsync method successfully returns groups when provided with a valid 
		/// access token.
		/// It first obtains an access token using AzureAuthentication, then creates an instance of GraphApiClient 
		/// with the obtained token.
		/// </summary>
		[Fact]
		public async Task GetGroupsAsync_ReturnsGroups_OnSuccess()
		{
			string client_ID = "7f41e0e6-ce16-4e12-8d4a-9bf78f4ff8bb";
			string secret_ID = "zBm8Q~7X.eMvQhF3OU6Hcr~UANweAXM6mv-c2au.";
			string tenant_ID = "6262fbf5-b11d-4b1f-a4dd-b295e270e076";

			var azureAuth = new AzureAuthentication(tenant_ID, client_ID, secret_ID);

			var token = await azureAuth.GetAccessTokenAsync();
			var graphClient = new GraphApiClient(token);

			var groups = await graphClient.GetGroupsAsync();

			Assert.True(groups.Count() > 0);

		}


		/// <summary>
		/// This test case checks if the GetGroupsAsync method throws an HttpRequestException when an API failure occurs.
		/// It obtains an access token using AzureAuthentication and sets up a mock HTTP message handler to simulate a 
		/// server error (Internal Server Error).
		/// Then, it creates an instance of GraphApiClient with a dummy access token ("test_access_token") and calls 
		/// the GetGroupsAsync method.
		/// It asserts that an HttpRequestException is thrown with the expected message indicating a response status
		/// code that does not indicate success.
		/// </summary>
		[Fact]
		public async Task GetGroupsAsync_ThrowsException_OnApiFailure()
		{
			string client_ID = "7f41e0e6-ce16-4e12-8d4a-9bf78f4ff8bb";
			string secret_ID = "zBm8Q~7X.eMvQhF3OU6Hcr~UANweAXM6mv-c2au.";
			string tenant_ID = "6262fbf5-b11d-4b1f-a4dd-b295e270e076";

			var azureAuth = new AzureAuthentication(tenant_ID, client_ID, secret_ID);

			var token = await azureAuth.GetAccessTokenAsync();

			mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>()
				)
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError)); // Simulate a server error

			var graphClient = new GraphApiClient("test_access_token"); // Use the existing constructor

			var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await graphClient.GetGroupsAsync());
			Assert.Equal("Response status code does not indicate success: 401 (Unauthorized).", exception.Message);
		}
	}
}