using Newtonsoft.Json;
using SharpQuark.ApiResult;

namespace SharpQuark;

public partial class Lightquark
{
    // /user/me
    public async Task<UserMeApiResult> UserMe()
    {
        var rawApiResult = await Call("/user/me");

        var parsedApiResult = JsonConvert.DeserializeObject<UserMeApiResult>(rawApiResult);
        
        return parsedApiResult ?? throw new Exception("/user/me API Result is null");
    }
    
    // /user/:userId
    public async Task<UserApiResult> UserById(string userId)
    {
        var rawApiResult = await Call($"/user/{userId}");

        var parsedApiResult = JsonConvert.DeserializeObject<UserApiResult>(rawApiResult);
        
        return parsedApiResult ?? throw new Exception("/user/{userId} API Result is null");
    }
}