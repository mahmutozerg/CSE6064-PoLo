using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PoLoAnalysisAuthServer.Core.DTOs;
using PoLoAnalysisAuthServer.Core.DTOs.Client;
using PoLoAnalysisAuthServer.Core.Models;
using PoLoAnalysisAuthServer.Core.Repositories;
using PoLoAnalysisAuthServer.Core.Services;
using PoLoAnalysisAuthServer.Repository;
using PoLoAnalysisAuthServer.Repository.Repositories;
using PoLoAnalysisAuthSever.Service.Services;
using SharedLibrary.AuthRequirements;
using SharedLibrary.Configurations;
using SharedLibrary.RequirementHandlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AppTokenOptions>(builder.Configuration.GetSection("TokenOptions"));
builder.Services.Configure<List<ClientLoginDto>>(builder.Configuration.GetSection("Clients"));
var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<AppTokenOptions>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"), options =>
    {
        options.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
        options.EnableRetryOnFailure();
    });
});
builder.Services.AddIdentity<User, AppRole>(opt =>
    {
        opt.Password.RequireDigit = true;
        opt.Password.RequireNonAlphanumeric = false; 
        opt.Password.RequireUppercase = false; 
        opt.Password.RequiredLength = 4;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
{
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
});
var cl = new List<string> { "authserver", "jsclient" };
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AuthServerPolicy", policy =>
        policy.Requirements.Add(new ClientIdRequirement("authserver")));    
    
    options.AddPolicy("AdminBypassAuthServerPolicy", policy =>
        policy.Requirements.Add(new AdminClientIdBypassRequirement("authserver")));
    
    options.AddPolicy("JSClientPolicy", policy =>
        policy.Requirements.Add(new ClientIdRequirement("jsclient")));
    
    options.AddPolicy("AdminBypassJSClientPolicy", policy =>
        policy.Requirements.Add(new AdminClientIdBypassRequirement("jsclient")));

    options.AddPolicy("ClientsWithAdminByPassPolicy", policy =>
        policy.Requirements.Add(new AdminClientsRequirementBypass(cl)));

});
builder.Services.AddSingleton<IAuthorizationHandler, ClientIdRequirementHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, AdminClientIdBypassRequirementHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, AdminClientsRequirementHandler>();

var app = builder.Build();

 if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
