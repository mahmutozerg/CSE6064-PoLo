using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedLibrary.DTOs.User;
using SharedLibrary.DTOs.Tokens;


namespace PoLoAnalysisMVC.Services
{
    public static class CatsUserServices
    {
        private const string CreateTokenUrl = SharedLibrary.APIConstants.AuthServerIP + "/api/CatsLogin/Login";
        public static async Task<JObject> LoginUser(CatsUserLogin loginDto)
        {
            using (var client = new HttpClient())
            {

                var createUserJsonData = JsonConvert.SerializeObject(loginDto);

                var content = new StringContent(createUserJsonData, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(CreateTokenUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResult = JObject.Parse(await response.Content.ReadAsStringAsync());
                    return jsonResult;
                }

                return new JObject();
            }
        }


        public static TokenDto GetTokenInfo(JObject jsonResult)
        {
            return new TokenDto()
            {
                AccessToken = jsonResult["data"]["accessToken"].ToString(),
                AccessTokenExpiration = DateTime.TryParse(jsonResult["data"]["accessTokenExpiration"]!.ToString(), out var expirationDate) ? expirationDate : DateTime.MinValue,
                RefreshToken = jsonResult["data"]["refreshToken"].ToString(),
                RefreshTokenExpiration = DateTime.TryParse(jsonResult["data"]["refreshTokenExpiration"].ToString(), out var ex) ? ex : DateTime.MinValue,


            };
        }
    }
}
