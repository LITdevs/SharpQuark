using Newtonsoft.Json;
using SharpQuark.Objects;

namespace SharpQuark.ApiResult;


public class ChannelApiResult : BaseApiResult
{
    [JsonProperty("response")]
    public new required ChannelResponse Response;
}

public class ChannelResponse : Response
{
    [JsonProperty("channel")]
    public required Channel Channel;
}
