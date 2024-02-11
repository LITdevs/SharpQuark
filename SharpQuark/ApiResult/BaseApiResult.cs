using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SharpQuark.ApiResult;

public class BaseApiResult
{
    [JsonProperty("request")]
    public Request Request;
    [JsonProperty("response")]
    public Response Response;
}

public class Request
{
    [JsonProperty("status_code")]
    public int StatusCode;
    [JsonProperty("success")]
    public bool Success;
    [JsonProperty("cat")]
    public string Cat;
}

public class Response
{
    [JsonProperty("message")]
    public string Message;
    public JObject RawData;
}