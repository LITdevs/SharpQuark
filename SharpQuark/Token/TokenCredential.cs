using System.Diagnostics;

namespace SharpQuark.Token;

public class TokenCredential
{
    public AccessToken AccessToken;
    public readonly RefreshToken RefreshToken;

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

    public TokenCredential(AccessToken accessToken, RefreshToken refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
    
    public static async Task<TokenCredential> Login(string email, string password, NetworkInformation networkInformation)
    {
        // Create temporary Lightquark instance
        var tempLq = new Lightquark(new TokenCredential(new AccessToken(), new RefreshToken()), networkInformation);
        var res = await tempLq.AuthToken(email, password);
        var accessToken = (AccessToken)Token.From(res.Response.AccessToken);
        var refreshToken = (RefreshToken)Token.From(res.Response.RefreshToken);
        return new TokenCredential(accessToken, refreshToken);
    }
}