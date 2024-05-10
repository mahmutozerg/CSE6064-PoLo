using Microsoft.AspNetCore.Http.Extensions;
using PoLoAnalysisMVC.Services;
using SharedLibrary;
using SharedLibrary.DTOs.Tokens;

namespace PoLoAnalysisMVC.Middleware;

public class RefreshTokenMiddleware
{
    private readonly RequestDelegate _next;

    public RefreshTokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var refreshToken = context.Request.Cookies[ApiConstants.RefreshCookieName];
        var accessToken = context.Request.Cookies[ApiConstants.SessionCookieName];
        var requestPath = context.Request.Path;


        if (!string.IsNullOrEmpty(refreshToken) && string.IsNullOrEmpty(accessToken) && (!requestPath.StartsWithSegments("/login") || context.Request.Method == "GET"))
        {
            var tokenDto = await CatsUserServices.CreateTokenByRefreshToken(refreshToken);
            if(string.IsNullOrEmpty(tokenDto.AccessToken))
                await _next(context);
            var sessionCookieOptions = new CookieOptions()
            {
                SameSite = SameSiteMode.Strict,
                Expires = new DateTimeOffset(tokenDto.AccessTokenExpiration),
                Secure = true,
            
            };
            var refreshCookieOptions = new CookieOptions()
            {
                SameSite = SameSiteMode.Strict,
                Expires = new DateTimeOffset(tokenDto.RefreshTokenExpiration),
                Secure = true,
            };
            context.Response.Cookies.Append(ApiConstants.SessionCookieName,  tokenDto.AccessToken,sessionCookieOptions);
            context.Response.Cookies.Append(ApiConstants.RefreshCookieName,  tokenDto.RefreshToken,refreshCookieOptions);
        }

        await _next(context);
    }
}
public static class RefreshTokenMiddlewareExtensions
{
    public static IApplicationBuilder UseRefreshTokenMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RefreshTokenMiddleware>();
    }
}