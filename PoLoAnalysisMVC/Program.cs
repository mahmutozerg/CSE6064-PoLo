using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PoLoAnalysisMVC.Middleware;
using SharedLibrary;
using SharedLibrary.Configurations;


var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<AppTokenOptions>(builder.Configuration.GetSection("TokenOptions"));

var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<AppTokenOptions>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "APIConstants.SessionCookieName";
    options.DefaultSignInScheme = "APIConstants.SessionCookieName";
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.RequireHttpsMetadata = false;
    opt.SaveToken = true;
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = tokenOptions.Issuer,
        ValidateIssuer = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)),
        ValidAudience = tokenOptions.Audience[0],
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        RoleClaimType = ClaimTypes.Role
    };
    opt.Events = new JwtBearerEvents()
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Request.Cookies.ContainsKey(ApiConstants.SessionCookieName))
                context.Response.Cookies.Delete(ApiConstants.SessionCookieName);

            if (context.Request.Cookies.ContainsKey(ApiConstants.RefreshCookieName))
                context.Response.Cookies.Delete(ApiConstants.RefreshCookieName);
            context.Response.Redirect("/login");
            return Task.CompletedTask;
        },

        OnMessageReceived = context =>
        {
            if (!context.Request.Cookies.ContainsKey(ApiConstants.SessionCookieName))
                return Task.CompletedTask;
            context.Token = context.Request.Cookies[ApiConstants.SessionCookieName];
            //context.Request.Headers.TryAdd("Bearer", context.Token);

            return Task.CompletedTask;
        },

    };
}).AddCookie(ApiConstants.SessionCookieName, options =>
{
    options.Cookie.Name = ApiConstants.SessionCookieName;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(tokenOptions.AccessTokenExpiration);
    options.LoginPath = "/login";
    options.AccessDeniedPath = "/login";


}).AddCookie(ApiConstants.RefreshCookieName, options =>
{
    options.Cookie.Name = ApiConstants.SessionCookieName;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(tokenOptions.RefreshTokenExpiration);
    options.LoginPath = "/login";
    options.AccessDeniedPath = "/login";


});
builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseRefreshTokenMiddleware();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();