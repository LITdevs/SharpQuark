namespace SharpQuark.Token;

public class AccessToken : Token
{
    public AccessToken Refresh(Lightquark lq)
    {
        var apiResult = lq.AuthRefresh();
        var stringToken = apiResult.Result.Response.AccessToken;
        return (AccessToken)From(stringToken);
    }
}