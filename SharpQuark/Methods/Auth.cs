using Newtonsoft.Json;
using SharpQuark.ApiResult;

namespace SharpQuark;

public partial class Lightquark
{
    // /auth/token
    public async Task<AuthTokenApiResult> AuthToken(string email, string password)
    {
        var rawApiResult = await Call("/auth/token", "POST", new ApiCallOptions
        {
            SkipAuth = true,
            Body = new
            {
                email, password
            }
        });

        var parsedApiResult = JsonConvert.DeserializeObject<AuthTokenApiResult>(rawApiResult);
        
        return parsedApiResult ?? throw new Exception("/auth/token API Result is null");
    }
    
    // /auth/refresh
    public async Task<AuthRefreshApiResult> AuthRefresh()
    {
        var rawApiResult = await Call("/auth/refresh", "POST", new ApiCallOptions
        {
            SkipAuth = true,
            Body = new
            {
                accessToken = _tokenCredential.AccessToken.ToString(),
                refreshToken = _tokenCredential.RefreshToken.ToString()
            }
        });
        var parsedApiResult = JsonConvert.DeserializeObject<AuthRefreshApiResult>(rawApiResult);
        
        return parsedApiResult ?? throw new Exception("/auth/refresh API Result is null");
    }
}