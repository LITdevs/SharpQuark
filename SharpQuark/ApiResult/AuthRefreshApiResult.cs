using Newtonsoft.Json;

namespace SharpQuark.ApiResult;

public class AuthRefreshApiResult : BaseApiResult
{
    [JsonProperty("response")]
    public new required AuthRefreshResponse Response;
}

public class AuthRefreshResponse : Response
{
    [JsonProperty("accessToken")]
    public required string AccessToken;
    [JsonProperty("expiresInSec")]
    public required string ExpiresInSeconds;
}