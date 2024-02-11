using Newtonsoft.Json;

namespace SharpQuark.ApiResult;

public class BaseApiResult
{
    [JsonProperty("request")]
    public required Request Request;
    [JsonProperty("response")]
    public Response? Response;
}

public class Request
{
    [JsonProperty("status_code")]
    public required int StatusCode;
    [JsonProperty("success")]
    public required bool Success;
    [JsonProperty("cat")]
    public required string Cat;
}

public class Response
{
    [JsonProperty("message")]
    public required string Message;
}