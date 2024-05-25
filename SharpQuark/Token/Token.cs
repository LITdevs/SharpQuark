// Ported from https://github.com/LITdevs/Lightquark/blob/dev/src/classes/Token/Token.ts

using System.Numerics;

namespace SharpQuark.Token;

public enum TokenType
{
    Access = 0,
    Refresh = 1
}

public class TokenException(string? exception = null) : Exception(exception);

public class Token
{
    private string? _token;
    private TokenType _type;
    private DateTime _expiresAt;
    public bool Expired
    {
        get
        {
            if (_type == TokenType.Refresh) return false;
            return DateTime.Now > _expiresAt;
        }
    }


    // Constructor not implemented as we are not able to generate new tokens

    // Invalidate not implemented as we are not able to invalidate tokens (Though maybe this should be an API call?)

    // TODO: IsActive() (Check if token is valid for API)
    
    /**
     * Validate token string and return Token object
     */
    public static Token From(string token)
    {
        var tokenParts = token.Split(".");
        Console.WriteLine(token);
        if (tokenParts.Length != 5)
            throw new TokenException($"Wrong token part count: expected 5, got {tokenParts.Length}");
        var prefixPart = tokenParts[0];
        var typePart = tokenParts[1];
        // randomPart can be ignored, it has no useful data
        // creationTimePart can also be ignored, turns out we don't care when the token was made
        var expirationTimePart = tokenParts[4];

        if (prefixPart != "LQ") throw new TokenException($"Wrong token prefix: expected 'LQ', got {prefixPart}");
        var tokenType = typePart switch
        {
            "AC" => TokenType.Access,
            "RE" => TokenType.Refresh,
            _ => throw new TokenException($"Invalid token type: expected one of 'RE', 'AC', got {typePart}")
        };

        DateTime expiryDate;
        try
        {
            var expiryTime = Base36Converter.ConvertFrom(expirationTimePart);
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            expiryDate = origin.AddMilliseconds(expiryTime);
        }
        catch (Exception e)
        {
            throw new TokenException(e.ToString());
        }

        if (tokenType == TokenType.Access)
        {
            return new AccessToken
            {
                _token = token,
                _type = tokenType,
                _expiresAt = expiryDate
            };
        }

        return new RefreshToken
        {
            _token = token,
            _type = tokenType,
            _expiresAt = expiryDate
        };
    }

    public override string? ToString()
    {
        return _token;
    }
}

internal static class Base36Converter
{
    private const string Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    // https://github.com/bogdanbujdea/csharpbase36/blob/master/Base36Encoder/Base36.cs
    public static long ConvertFrom(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Empty value.");
        value = value.ToUpper();
        var negative = false;
        if (value[0] == '-')
        {
            negative = true;
            value = value.Substring(1, value.Length - 1);
        }
        if (value.Any(c => !Chars.Contains(c)))
            throw new ArgumentException("Invalid value: \"" + value + "\".");
        var decoded = value.Select((t, i) => Chars.IndexOf(t) * (long)BigInteger.Pow(Chars.Length, value.Length - i - 1)).Sum();
        return negative ? decoded * -1 : decoded;
    }
}