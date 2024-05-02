using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PoLoAnalysisAuthServer.Core.DTOs;
using PoLoAnalysisAuthServer.Core.DTOs.Client;
using PoLoAnalysisAuthSever.Service.Services;
using SharedLibrary.Configurations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<AppTokenOptions>(builder.Configuration.GetSection("TokenOptions"));
builder.Services.Configure<List<ClientLoginDto>>(builder.Configuration.GetSection("Clients"));

var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<AppTokenOptions>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer( opt =>
{
    opt.RequireHttpsMetadata = false;
    opt.SaveToken = true;
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = tokenOptions.Issuer,
        ValidateIssuer = true,
        IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),
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

            context.Response.Redirect("/login");
            return Task.CompletedTask;
        },

        OnMessageReceived = context =>
        {
            if (!context.Request.Cookies.ContainsKey("session")) 
                return Task.CompletedTask;
            context.Token = context.Request.Cookies["session"];
            //context.Request.Headers.TryAdd("Bearer", context.Token);

            return Task.CompletedTask;
        },

    };
}).AddCookie( options =>
{
    options.Cookie.Name = "session";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; 
    options.ExpireTimeSpan = TimeSpan.FromMinutes(tokenOptions.AccessTokenExpiration);
    options.LoginPath = "/login";
    options.AccessDeniedPath = "/login";
    
    
});
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();