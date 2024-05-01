using System.Reflection;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PoLoAnalysisBusiness.Core.Repositories;
using PoLoAnalysisBusiness.Core.Services;
using PoLoAnalysisBusiness.Repository;
using PoLoAnalysisBusiness.Repository.Repositories;
using PoLoAnalysisBusiness.Repository.UnitOfWorks;
using PoLoAnalysisBusiness.Services.Services;
using PoLoAnalysisBusinessAPI.AuthRequirements;
using PoLoAnalysisBusinessAPI.RequirementHandlers;
using SharedLibrary.Configurations;
using IUnitOfWork = PoLoAnalysisBusiness.Core.UnitOfWorks.IUnitOfWork;
using UserService = PoLoAnalysisBusiness.Services.Services.UserService;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<AppTokenOptions>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy  =>
        {
            policy.WithOrigins("https://localhost:7298");
        });
});
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IResultRepository), typeof(ResultRepository));
builder.Services.AddScoped(typeof(IResultService), typeof(ResultService));

builder.Services.AddScoped(typeof(IAppFileRepository), typeof(AppFileRepository));
builder.Services.AddScoped(typeof(IAppFileServices), typeof(AppFileService));

builder.Services.AddScoped(typeof(IResultRepository), typeof(ResultRepository));
builder.Services.AddScoped(typeof(IResultService), typeof(ResultService));

builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
builder.Services.AddScoped(typeof(IUserService), typeof(UserService));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<>), typeof(PoLoAnalysisBusiness.Services.Services.GenericService<>));

builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService, CourseService>();

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"), options =>
    {
        options.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
        options.EnableRetryOnFailure();
    });
});
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)),
        ValidAudience = tokenOptions.Audience[0],
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        RoleClaimType = ClaimTypes.Role,
    };
});
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
    
});
builder.Services.AddSingleton<IAuthorizationHandler, ClientIdRequirementHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, AdminClientIdBypassRequirementHandler>();

var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseRouting(); // Add this line
app.UseAuthorization(); // Add this line if authorization is required
app.MapControllers();

app.Run();