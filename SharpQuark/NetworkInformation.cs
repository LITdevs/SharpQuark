using Newtonsoft.Json;

namespace SharpQuark;

public class NetworkInformation
{
    private static readonly HttpClient Http = new();
    [JsonProperty("name")]
    public string? Name;
    [JsonProperty("description")]
    public string? Description;
    [JsonProperty("environment")]
    public string? Environment;
    [JsonProperty("maintainer")]
    public string? Maintainer;
    [JsonProperty("linkBase")]
    public string? LinkBase;
    [JsonProperty("baseUrl")]
    public string? BaseUrl;
    [JsonProperty("iconUrl")]
    public string? IconUrl;
    // authenticationSourceRegisterUrl & authenticationSourceName are no longer used with LQ v3 API (0.1.0 onwards?)
    [JsonProperty("gateway")]
    public string? Gateway;
    [JsonProperty("privacyPolicy")]
    public string? PrivacyPolicy;
    [JsonProperty("termsOfService")]
    public string? TermsOfService;
    [JsonProperty("version")]
    public string? Version;
    [JsonProperty("capabilities")]
    public object? Capabilities;

    public static async Task<NetworkInformation> GetNetwork(string baseUrl)
    {
        var baseUri = new Uri(baseUrl);
        var res = await Http.GetAsync(new Uri(baseUri, "/v3/network"));
        var rawApiResult = await res.Content.ReadAsStringAsync();
        var parsedApiResult = JsonConvert.DeserializeObject<NetworkInformation>(rawApiResult);
        return parsedApiResult ?? throw new InvalidOperationException();
    }
}