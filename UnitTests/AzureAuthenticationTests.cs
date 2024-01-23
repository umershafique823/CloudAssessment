using System.Net;
using Moq;
using Moq.Protected;
using Xunit;
using SimeonCloudSoftware.Authentication;
using UnitTests;

namespace UnitTests
{

	public class AzureAuthenticationTests : Credentials
	{
		private Mock<HttpMessageHandler> mockHttpMessageHandler;
		private HttpClient httpClient;

		public AzureAuthenticationTests() : base()
		{
			mockHttpMessageHandler = new Mock<HttpMessageHandler>();
			httpClient = new HttpClient(mockHttpMessageHandler.Object);
		}
		/// <summary>
		/// This test case checks if the GetAccessTokenAsync method successfully returns a non-empty token when valid credentials are provided.
		/// It creates an instance of AzureAuthentication with valid credentials and asserts that the returned token is not empty.
		/// </summary>
		[Fact]
		public async Task GetAccessTokenAsync_ReturnsToken_OnSuccess()
		{
			var azureAuth = new AzureAuthentication(tenant_ID, client_ID, secret_ID); // Adjusted

			// Act
			var token = await azureAuth.GetAccessTokenAsync();

			// Assert
			Assert.NotEmpty(token);
		}

		/// <summary>
		/// This test case checks if the GetAccessTokenAsync method throws an HttpRequestException when invalid 
		/// credentials are provided.
		/// It sets up a mock HTTP message handler to simulate a response with a BadRequest status code, ensuring 
		/// that the HTTP request fails due to invalid credentials.
		/// It then asserts that calling the GetAccessTokenAsync method with invalid credentials results in an 
		/// exception being thrown.
		/// </summary>
		[Fact]
		public async Task GetAccessTokenAsync_ThrowsException_OnInvalidCredentials()
		{
			// Arrange
			mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>()
				)
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest));

			var azureAuth = new AzureAuthentication("invalidTenant", "invalidClient", "invalidSecret");

			// Act & Assert
			await Assert.ThrowsAsync<HttpRequestException>(async () => await azureAuth.GetAccessTokenAsync());
		}

		/// <summary>
		/// This test case checks if the GetAccessTokenAsync method throws an HttpRequestException when an HTTP client 
		/// failure occurs.
		/// It sets up a mock HTTP message handler to simulate an exception being thrown when sending the HTTP request, 
		/// simulating a network error.
		/// It then asserts that calling the GetAccessTokenAsync method in such a scenario results in an exception being 
		/// thrown.
		/// </summary>
		[Fact]
		public async Task GetAccessTokenAsync_ThrowsException_OnHttpClientFailure()
		{
			// Arrange
			mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>()
				)
				.ThrowsAsync(new HttpRequestException("Network error"));

			var azureAuth = new AzureAuthentication("tenantId", "clientId", "clientSecret");

			// Act & Assert
			await Assert.ThrowsAsync<HttpRequestException>(async () => await azureAuth.GetAccessTokenAsync());
		}
	}
}