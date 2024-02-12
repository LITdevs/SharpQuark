using Newtonsoft.Json;
using SharpQuark.ApiResult;
using SharpQuark.Objects;
using SharpQuark.Objects.Id;

namespace SharpQuark;

public partial class Lightquark
{
    public async Task<ChannelApiResult> ChannelById(ChannelId channelId)
    {
        var rawApiResult = await Call($"/channel/{channelId}");
        
        var parsedApiResult = JsonConvert.DeserializeObject<ChannelApiResult>(rawApiResult);

        if (parsedApiResult != null) parsedApiResult.Response.Channel.Lq = this;

        return parsedApiResult ?? throw new Exception("/channel/{channelId} API Result is null");
    }

    public async Task<ChannelMessagesApiResult> ChannelMessages(ChannelId channelId)
    {
        
        var rawApiResult = await Call($"/channel/{channelId}/messages");
        
        var parsedApiResult = JsonConvert.DeserializeObject<ChannelMessagesApiResult>(rawApiResult);

        return parsedApiResult ?? throw new Exception("/channel/{channelId}/messages API Result is null");
    }
    
}