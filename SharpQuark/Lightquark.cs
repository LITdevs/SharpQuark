using System.Net.Http.Headers;
using System.Reflection;
using Newtonsoft.Json;
using SharpQuark.Token;

namespace SharpQuark;

public class UnsupportedVersionException(string e) : Exception(e);

public partial class Lightquark
{
    private readonly HttpClient _http = new();
    private readonly string _version;
    private readonly Uri _baseUri;
    private readonly TokenCredential _tokenCredential;
    private readonly string _agent;
    
    public Lightquark(TokenCredential credential, NetworkInformation networkInformation, string? agent = null, string version = "v3", bool suppressStartupMessage = false)
    {
        _tokenCredential = credential;
        _version = version;
        _agent = agent ?? $"SharpQuark {Assembly.GetExecutingAssembly().GetName().Version}";
        if (networkInformation.BaseUrl == null) throw new Exception("Invalid network");
        _baseUri = new Uri(networkInformation.BaseUrl);
        if (!suppressStartupMessage)
        {
            Console.WriteLine($"Running {_agent}");
        }
    }

    private static readonly string[] UnsupportedVersions = ["v1", "v2"];
    // ReSharper disable once MemberCanBePrivate.Global
    public async Task<string> Call(string endpoint, string method = "GET", ApiCallOptions? options = null)
    {
        if (options?.SkipAuth is not true)
        {
            if (_tokenCredential.Expired && !_tokenCredential.Refresh(this))
            {
                throw new Exception("Failed to refresh expired token");
            }
        }
        // Prevent falling over for predictable reasons
        if (UnsupportedVersions.Any(v => v == (options?.Version ?? _version)))
            throw new UnsupportedVersionException("SharpQuark does not support API versions 1 and 2 due to authentication differences");

        // Allow using both "/user/me" and "user/me"
        if (endpoint.StartsWith('/'))
        {
            endpoint = endpoint.Remove(0, 1);
        }
        
        // Use request specific version if available
        var requestUri = new Uri(_baseUri, $"{options?.Version ?? _version}/{endpoint}");

        var request = new HttpRequestMessage
        {
            RequestUri = requestUri,
            Method = new HttpMethod(method)
        };
        
        // Add default headers
        request.Headers.Add("Authorization", $"Bearer {_tokenCredential.AccessToken}");
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("lq_agent", _agent);
        
        // Add custom headers
        if (options?.Headers != null)
        {
            foreach (var header in options.Headers)
            {
                request.Headers.Remove(header.Key); // If header was already present, remove it
                request.Headers.Add(header.Key, header.Value);
            }
        }

        if (method != "GET")
        {
            request.Content = new StringContent(JsonConvert.SerializeObject(options?.Body), new MediaTypeHeaderValue("application/json"));
        }

        var result = await _http.SendAsync(request);
        var resultString = await result.Content.ReadAsStringAsync();

        return resultString;
    }
}

public class ApiCallOptions
{
    public string? Version;
    public List<KeyValuePair<string, string>>? Headers;
    public object? Body;
    public bool SkipAuth = false;
}