using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Reflection;
using Newtonsoft.Json;
using SharpQuark.Objects;
using SharpQuark.Token;
using Websocket.Client;

namespace SharpQuark;

public class UnsupportedVersionException(string e) : Exception(e);

public partial class Lightquark : IDisposable
{
    private static int _sid;
    private readonly int _id;
    private readonly HttpClient _http = new();
    private static readonly string[] UnsupportedVersions = ["v1", "v2", "v3"];
    private readonly string _version;
    private readonly TokenCredential _tokenCredential;
    private readonly string _agent;
    private WebsocketClient? _client;
    public readonly NetworkInformation Network;
    private Uri BaseUri => new (Network.BaseUrl ?? throw new Exception("Invalid network"));
    private Timer? _heartbeatTimer;
    
    public Lightquark(TokenCredential credential, NetworkInformation networkInformation, string? agent = null, string version = "v4", bool suppressStartupMessage = false, bool connectWebsocket = true)
    {
        _id = _sid;
        _sid++;
        _tokenCredential = credential;
        _version = version;
        _agent = agent ?? $"SharpQuark {Assembly.GetExecutingAssembly().GetName().Version}";
        Network = networkInformation;
        if (!suppressStartupMessage)
        {
            Console.WriteLine($"[{_id}] Running {_agent}");
        }

        // Gateway
        if (!connectWebsocket) return;
        _connectGateway();

    }

    private void _connectGateway()
    {
        if (Network.Gateway == null) throw new Exception("Network information missing gateway uri when trying to connect gateway.");
        var clientFactory = new Func<ClientWebSocket>(() =>
        {
            var client = new ClientWebSocket();
            return client;
        });
        _client = new WebsocketClient(new Uri(Network.Gateway), clientFactory);
        
        // Reconnections
        _client.ReconnectTimeout = TimeSpan.FromSeconds(30);
        _client.ReconnectionHappened.Subscribe(info =>
            Debug.WriteLine($"[{_id}] Reconnection happened, type: {info.Type}"));

        _client.DisconnectionHappened.Subscribe(info =>
        {
            Debug.WriteLine($"[{_id}] Disconnection happened, type: {info.Type}");
            if (info.Exception != null) Console.Error.WriteLine(info.Exception);
        });
        
        // Start
        _client.MessageReceived.Subscribe(GatewayMessage);
        _client.StartOrFail();
        
        SendGateway(new AuthenticateGatewayMessage("authenticate", _tokenCredential.AccessToken));
        
        _heartbeatTimer = new Timer(
            _ =>
            {
                SendGateway(new GatewayMessage("heartbeat", "alive"));
                Debug.WriteLine($"[{_id}] Sent heartbeat message.");
            },
            null,
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(15));
    }

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
            throw new UnsupportedVersionException("SharpQuark only supports version 4.");

        // Allow using both "/user/me" and "user/me"
        if (endpoint.StartsWith('/'))
        {
            endpoint = endpoint.Remove(0, 1);
        }
        
        // Use request specific version if available
        var requestUri = new Uri(BaseUri, $"{options?.Version ?? _version}/{endpoint}");

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

    public void Dispose()
    {
        Debug.WriteLine($"[{_id}] Goodbye :(");
        _http.Dispose();
        _client?.Dispose();
        _heartbeatTimer?.Dispose();
        GC.SuppressFinalize(this);
    }
}

public class ApiCallOptions
{
    public string? Version;
    public List<KeyValuePair<string, string>>? Headers;
    public object? Body;
    public bool SkipAuth = false;
}