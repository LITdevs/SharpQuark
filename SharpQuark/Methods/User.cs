using Newtonsoft.Json;
using SharpQuark.ApiResult;
using SharpQuark.Objects;
using SharpQuark.Objects.Id;

namespace SharpQuark;

public partial class Lightquark
{
    public async Task<User> UserMe()
    {
        var user = await UserMeRaw();
        return user.Response.User;
    }
    
    // /user/me
    public async Task<UserMeApiResult> UserMeRaw()
    {
        var rawApiResult = await Call("/user/me");

        var parsedApiResult = JsonConvert.DeserializeObject<UserMeApiResult>(rawApiResult);
        
        return parsedApiResult ?? throw new Exception("/user/me API Result is null");
    }
    
    public async Task<User> UserById(UserId userId)
    {
        var user = await UserByIdRaw(userId);
        return user.Response.User;
    }
    
    // /user/:userId
    public async Task<UserApiResult> UserByIdRaw(UserId userId)
    {
        var rawApiResult = await Call($"/user/{userId}");

        var parsedApiResult = JsonConvert.DeserializeObject<UserApiResult>(rawApiResult);
        
        return parsedApiResult ?? throw new Exception("/user/{userId} API Result is null");
    }
}