using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PoLoAnalysisBusiness.Core.Services;
using PoLoAnalysisBusiness.Repository;
using PoLoAnalysisBusiness.Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(IPoLoExcelServices), typeof(PoLoExcelServices));
builder.Services.AddScoped(typeof(IAppFileServices), typeof(AppFileServices));
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"), options =>
    {
        options.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
        options.EnableRetryOnFailure();
    });
});
var app = builder.Build();

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