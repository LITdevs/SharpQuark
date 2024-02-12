using Newtonsoft.Json;
using SharpQuark.Objects;

namespace SharpQuark.ApiResult;


public class ChannelMessagesApiResult : BaseApiResult
{
    [JsonProperty("response")]
    public new required ChannelMessagesResponse Response;
}

public class ChannelMessagesResponse : Response
{
    [JsonProperty("messages")]
    public required Message[] Messages;
}
