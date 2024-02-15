using Newtonsoft.Json;
using SharpQuark.ApiResult;
using SharpQuark.Objects;
using SharpQuark.Objects.Id;

namespace SharpQuark;

public partial class Lightquark
{
    public async Task<Channel> ChannelById(ChannelId channelId)
    {
        var channel = await ChannelByIdRaw(channelId);
        return channel.Response.Channel;
    }
    
    public async Task<ChannelApiResult> ChannelByIdRaw(ChannelId channelId)
    {
        var rawApiResult = await Call($"/channel/{channelId}");
        
        var parsedApiResult = JsonConvert.DeserializeObject<ChannelApiResult>(rawApiResult);

        if (parsedApiResult == null) return parsedApiResult ?? throw new Exception("/channel/{channelId} API Result is null");
        
        parsedApiResult.Response.Channel.Lq = this;
        await parsedApiResult.Response.Channel.InitialLoad();
        
        return parsedApiResult;
    }

    internal async Task<ChannelMessagesApiResult> ChannelMessages(Channel channel)
    {
        
        var rawApiResult = await Call($"/channel/{channel.Id}/messages");
        
        var parsedApiResult = JsonConvert.DeserializeObject<ChannelMessagesApiResult>(rawApiResult);
        
        if (parsedApiResult == null) return parsedApiResult ?? throw new Exception("/channel/{channelId}/messages API Result is null");

        foreach (var message in parsedApiResult.Response.Messages)
        {
            message.Channel = channel;
        }
        
        return parsedApiResult;
    }
    
}