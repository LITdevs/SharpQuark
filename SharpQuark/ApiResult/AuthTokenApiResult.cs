using Newtonsoft.Json;

namespace SharpQuark.ApiResult;

public class AuthTokenApiResult : BaseApiResult
{
    [JsonProperty("response")]
    public new required AuthTokenResponse Response;
}

public class AuthTokenResponse : Response
{
    [JsonProperty("token_type")]
    public required string TokenType;
    [JsonProperty("access_token")]
    public required string AccessToken;
    [JsonProperty("refresh_token")]
    public required string RefreshToken;
}