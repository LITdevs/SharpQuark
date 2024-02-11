## C# API Wrapper for the Lightquark API

> [!WARNING]
> Supports API Versions V3 and up. V1 and V2 **ARE NOT** and **WILL NOT** be supported

### Usage

```csharp
using SharpQuark;
using SharpQuark.Token;

// Get network information, this contains various metadata for the network which can be used in your client to
// show information about the network before the user logs in
var netInfo = await NetworkInformation.GetNetwork("https://equinox.lightquark.network");

// To log in or make any other API calls we need to get the access and refresh tokens, which can be done in two
// different ways. For example with email and password:
var tokens = await TokenCredential.Login("email", "password", netInfo);
// Or alternatively stored tokens can be used to get the TokenCredential directly, for example from strings:
var tokens2 = new TokenCredential((AccessToken)Token.From("access token"), (RefreshToken)Token.From("refresh token"));

// With TokenCredential and NetworkInformation we can actually create the Lightquark instance
var lq = new Lightquark(tokens, netInfo);

// To get data about the logged in user 
var userMe = await lq.UserMe();
Console.WriteLine(userMe.Response.User.Username); // Test_User

// To get data about a specific user
var userById = await lq.UserById("62b3515989cdb45c9e06e010");
Console.WriteLine($"{userById.Response.User.Status?.Type} {userById.Response.User.Status?.Content}"); // Playing Stardew Valley


```