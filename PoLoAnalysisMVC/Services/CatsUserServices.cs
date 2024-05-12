using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedLibrary.DTOs.Client;
using SharedLibrary.DTOs.Tokens;
using SharedLibrary.DTOs.User;

namespace PoLoAnalysisMVC.Services;

public static class CatsUserServices
{
    private const string CreateTokenUrl = 
        SharedLibrary.ApiConstants.AuthServerIP + "/api/CatsLogin/Login";

    private const string CreateTokenByClientUrl =
        SharedLibrary.ApiConstants.AuthServerIP + "/api/auth/CreateTokenByClient";

    private const string RevokeRefreshTokenUrl =
        SharedLibrary.ApiConstants.AuthServerIP + "/api/auth/revokerefreshtoken";

    private const string CreateTokenByRefreshTokenUrl =
        SharedLibrary.ApiConstants.AuthServerIP + "/api/auth/CreateTokenByRefreshToken";

    public static ClientTokenDto ClientToken = new ClientTokenDto();
    public static async Task<TokenDto?> LoginUserAsync(CatsUserLogin loginDto)
    {
        using var client = new HttpClient();
        var loginUserJsonData = JsonConvert.SerializeObject(loginDto);

        var content = new StringContent(loginUserJsonData, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(CreateTokenUrl, content);

        if (!response.IsSuccessStatusCode) 
            return new TokenDto();
        var jsonResult = JObject.Parse(await response.Content.ReadAsStringAsync());
        return GetTokenInfo(jsonResult);
    }


    private static TokenDto? GetTokenInfo(JObject jsonResult)
    {
        return new TokenDto()
        {
            AccessToken = jsonResult["data"]["accessToken"].ToString(),
            AccessTokenExpiration = DateTime.TryParse(jsonResult["data"]["accessTokenExpiration"]!.ToString(), out var expirationDate) ? expirationDate : DateTime.MinValue,
            RefreshToken = jsonResult["data"]["refreshToken"].ToString(),
            RefreshTokenExpiration = DateTime.TryParse(jsonResult["data"]["refreshTokenExpiration"].ToString(), out var ex) ? ex : DateTime.MinValue,


        };
    }
    public static async Task<Task> CreateTokenByClientAsync(ClientLoginDto loginDto)
    {
        using var client = new HttpClient();
        var tokenJsonData = JsonConvert.SerializeObject(loginDto);

        var content = new StringContent(tokenJsonData, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(CreateTokenByClientUrl, content);

        if (!response.IsSuccessStatusCode)
            return Task.CompletedTask;
        var jsonResult = JObject.Parse(await response.Content.ReadAsStringAsync());
        ClientToken = jsonResult["data"].ToObject<ClientTokenDto>();
        return Task.CompletedTask;

    }

    public static async Task<TokenDto?> CreateTokenByRefreshTokenAsync(string token)
    {
        var dto = new CreateTokenByRefreshTokenDto()
        {
            RefreshToken = token
        };
        
        using var client = new HttpClient();

        
        if (string.IsNullOrEmpty(ClientToken.AccesToken) || DateTime.Compare(ClientToken.AccesTokenExpiration,DateTime.Now)<=0)
        {
            await CreateTokenByClientAsync( new ClientLoginDto()
            {
                Id = "MVC",
                Secret = "MVClientSecretKey"
            });
        }

        var jsonData = JsonConvert.SerializeObject(dto);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",ClientToken.AccesToken);


        var response = await client.PostAsync(CreateTokenByRefreshTokenUrl,content);

        if (!response.IsSuccessStatusCode) 
            return null;
        
        var jsonResult = JObject.Parse(await response.Content.ReadAsStringAsync());
        return GetTokenInfo(jsonResult);

    }


}