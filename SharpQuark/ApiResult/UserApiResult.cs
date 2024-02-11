using Newtonsoft.Json;
using SharpQuark.Objects;

namespace SharpQuark.ApiResult;

public class UserMeApiResult : BaseApiResult
{
    [JsonProperty("response")]
    public new required UserMeResponse Response;
}

public class UserApiResult : BaseApiResult
{
    [JsonProperty("response")]
    public new required UserResponse Response;
}

public class UserMeResponse : Response
{
    [JsonProperty("jwtData")]
    public required User User;
}

public class UserResponse : Response
{
    [JsonProperty("user")]
    public required User User;
}