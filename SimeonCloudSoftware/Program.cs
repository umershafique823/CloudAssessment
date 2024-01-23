using SimeonCloudSoftware.Authentication;
using SimeonCloudSoftware.API;
using SimeonCloudSoftware.Writter;
using Microsoft.Extensions.Configuration;
using SimeonCloudSoftware;

try
{
    IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    var section = configuration.GetSection("AzureAuthentication");

    string client_ID = Convert.ToString(section["ClientId"]);
    string secret_ID = Convert.ToString(section["ClientSecret"]);
    string tenant_ID = Convert.ToString(section["TenantId"]);
    Console.WriteLine("*********************************");
    Console.WriteLine("Simeon Cloud Software Engineering");
    Console.WriteLine("*********************************");
    if (string.IsNullOrEmpty(client_ID) || string.IsNullOrEmpty(secret_ID) || string.IsNullOrEmpty(tenant_ID))
    {
        Console.WriteLine("Tenant Credentials are not valid.");
        return;
    }

    var azureAuth = new AzureAuthentication(tenant_ID, client_ID, secret_ID);
    string accessToken = await azureAuth.GetAccessTokenAsync();
    Console.WriteLine("Successfully obtained access token.");
    Console.WriteLine("*********************************");

    var graphClient = new GraphApiClient(accessToken);
    var groups = await graphClient.GetGroupsAsync();
    Console.WriteLine("Successfully fetched groups.");
    Console.WriteLine("*********************************");

    var jsonFileWriter = new JsonFileWriter();

    jsonFileWriter.WriteGroupsToJson(groups);

    string currentDirectory = Directory.GetCurrentDirectory();
    string directoryPath = Path.Combine(currentDirectory, Constant.filePath);

    Console.WriteLine($"Groups stored in: {directoryPath}");
    Console.WriteLine($"Total groups stored: {groups.Count()}");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
