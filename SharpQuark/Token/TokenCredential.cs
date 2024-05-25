using SharpQuark.ApiResult;

namespace SharpQuark.Token;

public class TokenCredential(AccessToken accessToken, RefreshToken refreshToken)
{
    public AccessToken AccessToken = accessToken;
    public readonly RefreshToken RefreshToken = refreshToken;

    public bool Expired => AccessToken.Expired;
    
    public bool Refresh(Lightquark lq)
    {
        try
        {
            AccessToken = AccessToken.Refresh(lq);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public static async Task<TokenCredential> Login(string email, string password, NetworkInformation networkInformation)
    {
        // Create temporary Lightquark instance
        AuthTokenApiResult res;
        using (var tempLq = new Lightquark(new TokenCredential(new AccessToken(), new RefreshToken()), networkInformation, null, "v4", true, false))
        {
            res = await tempLq.AuthToken(email, password);
        }

        var accessToken = (AccessToken)Token.From(res.Response.AccessToken);
        var refreshToken = (RefreshToken)Token.From(res.Response.RefreshToken);
        
        return new TokenCredential(accessToken, refreshToken);
    }
    
    public static async Task<TokenCredential> Register(string email, string password, string username, NetworkInformation networkInformation)
    {
        // Create temporary Lightquark instance
        AuthTokenApiResult res;
        using (var tempLq = new Lightquark(new TokenCredential(new AccessToken(), new RefreshToken()), networkInformation, null, "v4", true))
        {
            res = await tempLq.AuthRegister(email, password, username);
        }

        var accessToken = (AccessToken)Token.From(res.Response.AccessToken);
        var refreshToken = (RefreshToken)Token.From(res.Response.RefreshToken);
        return new TokenCredential(accessToken, refreshToken);
    }
}